using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class TaskCompletedRepository
    {
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleRepo;

        public TaskCompletedRepository(RentContext context, IRoleAuthenticationRepository roleRepo)
        {
            _context = context;
            _roleRepo = roleRepo;
        }

        public async Task<int> Create(int requester, int taskId, string comment)
        {
            return await Create(requester, taskId, new CleaningTaskCompleted
            {
                CompletedDate = Helpers.DateTimeHelpers.GmtPlusOneDateTime(),
                Comment = comment,
            });

        }

        public async Task<int> Create(int requester, int taskId, CleaningTaskCompleted taskCompleted)
        {
            if (_roleRepo.IsAdminOrManager(requester) || _roleRepo.IsUser(requester))
            {
                taskCompleted.CleaningTaskID = taskId;
                taskCompleted.CompletedByUserID = requester;
                taskCompleted.Confirmed = true;
                _context.CleaningTaskCompleted.Add(taskCompleted);
                await _context.SaveChangesAsync();
                return taskCompleted.ID;
            }
            throw new UnauthorizedAccessException();
        }

        public async Task Update(int requester, int taskCompletedId, CleaningTaskCompleted taskCompleted)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var taskCompletedToUpdate = _context.CleaningTaskCompleted.Find(taskCompletedId);
                if (taskCompletedToUpdate == null) throw new NotFoundException();

                taskCompletedToUpdate.Comment = taskCompleted.Comment;
                taskCompletedToUpdate.CompletedDate = taskCompleted.CompletedDate;

                _context.CleaningTaskCompleted.Update(taskCompletedToUpdate);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public dynamic GetDTO(int requester, int taskCompletedId)
        {
            var requesterRole = _roleRepo.GetRole(requester);
            var taskCompleted = _context.CleaningTaskCompleted.Find(taskCompletedId);
            return CleaningTaskCompleted.BasicDTO(requester, requesterRole).Compile()(taskCompleted);
        }

        public IEnumerable<dynamic> GetOfTaskDTO(int requester, int taskId)
        {
            var requesterRole = _roleRepo.GetRole(requester);
            return _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == taskId)
                .Include(ctc => ctc.CompletedByUser).Select(CleaningTaskCompleted.BasicDTO(requester, requesterRole));
        }
    }
}
