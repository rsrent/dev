using System;
using System.Linq;
using System.Threading.Tasks;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models.Projects;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class CommentRepository
    {
        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleRepo;

        public CommentRepository(ProjectRoleRepository projectRoleRepository, RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _projectRoleRepository = projectRoleRepository;
            _context = rentContext;
            _roleRepo = roleRepo;
        }

        public dynamic Get(int requester, int commentId)
        {
            var comment = _projectRoleRepository.GetReadableCommentsOfUser(requester).Where(p => p.ID == commentId).Select(Comment.BasicDTO()).FirstOrDefault();
            if (comment == null) throw new NotFoundException();
            return comment;
        }

        public async Task Update(int requester, int commentId, Comment comment)
        {
            var commentToUpdate = _projectRoleRepository.GetWritableCommentsOfUser(requester).FirstOrDefault(c => c.ID == commentId);
            if (commentToUpdate != null)
            {
                commentToUpdate.Title = comment.Title;
                commentToUpdate.Body = comment.Body;

                _context.Comment.Update(commentToUpdate);
                await _context.SaveChangesAsync();
            }
            else
                throw new NotFoundException();
        }
    }
}
