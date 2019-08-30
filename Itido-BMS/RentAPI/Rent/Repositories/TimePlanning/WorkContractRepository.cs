using System.Threading.Tasks;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;
using Rent.Data;
using System;
using System.Linq;
using Rent.ContextPoint.Exceptions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Rent.Helpers;
using Rent.Models.TimePlanning.Enums;
using Rent.DTOAssemblers;

namespace Rent.Repositories.TimePlanning

{
    public class WorkContractRepository
    {

        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly RentContext _rentContext;
        private readonly WorkRepository _workRepository;
        private readonly WorkContractListAssembler _workContractAssembler;
        private readonly NotiRepository _notiRepository;
        private readonly AbsenceHelperRepository _absenceRepository;

        public WorkContractRepository(ProjectRoleRepository projectRoleRepository, IRoleAuthenticationRepository roleRepo, RentContext rentContext, WorkRepository workRepository, NotiRepository notiRepository, AbsenceHelperRepository absenceRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _roleRepo = roleRepo;
            _rentContext = rentContext;
            _workRepository = workRepository;
            _notiRepository = notiRepository;
            _absenceRepository = absenceRepository;
        }

        /*
        public IQueryable<dynamic> GetAllWorkContractsForUser(int requester, int contractId)
        {
            return _projectRoleRepository.GetReadableWorkContractsOfUser(requester).Where(wc => wc.ContractID == contractId).Select(WorkContract.StandardDTO());

            var requesterRole = _roleRepo.GetRole(requester);
            return _rentContext.WorkContract
                .Include(wc => wc.WorkDays)
                .Include(wc => wc.WorkHolidays)
                .Include(wc => wc.Contract)
                .ThenInclude(c => c.User).Where(wc => wc.ContractID == contractId).Select(WorkContract.BasicDTO());
        }
        */

        public IQueryable<WorkContract> GetWorkContract(int requester, int workContractId)
        {
            return _projectRoleRepository.GetReadableWorkContractsOfUser(requester).Where(wc => wc.ID == workContractId);

            if (_roleRepo.IsAdminOrManager(requester))
            {
                return _rentContext.WorkContract
                .Include(wc => wc.ProjectItem)
                .Include(wc => wc.WorkDays)
                .Include(wc => wc.WorkHolidays)
                .Include(wc => wc.Contract)
                .ThenInclude(c => c.User)
                .Where(wc => wc.ID == workContractId);
            }
            throw new UnauthorizedAccessException();
        }

        public object GetWorkContractDTO(int requester, int workContractId)
        {
            return GetWorkContract(requester, workContractId).Select(WorkContract.DetailedDTO()).FirstOrDefault();

            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                IQueryable<WorkContract> wc = GetWorkContract(requester, workContractId);
                return wc.Select(WorkContract.DetailedDTO()).FirstOrDefault();
            }
            throw new UnauthorizedAccessException();
        }

        /*
        public IQueryable<Object> GetWorkContractsForLocationDTO(int requester, int locationId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                return _rentContext.WorkContract
                .Include(wc => wc.Contract)
                .ThenInclude(c => c.User)
                .Where(wc => wc.LocationID == locationId).Select(WorkContract.BasicDTO());
            }
            throw new UnauthorizedAccessException();

        }
        */

        public IQueryable<dynamic> GetWorkContractsForUserDTO(int requester, int contractId)
        {
            return _projectRoleRepository.GetReadableWorkContractsOfUser(requester).Where(wc => wc.Contract.ID == contractId).Select(WorkContract.StandardDTO());
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                return _rentContext.WorkContract
                .Include(wc => wc.Contract)
                .ThenInclude(c => c.User)
                .Where(wc => wc.Contract.ID == contractId).Select(WorkContract.BasicDTO());
            }
            throw new UnauthorizedAccessException();

        }

        public async Task UpdateWorkContract(int requester, WorkContract workContract)
        {
            if (workContract == null)
            {
                throw new NullReferenceException();
            }

            // find work contract
            var workContractToUpdate =
                _projectRoleRepository.GetWritableWorkContractsOfUser(requester)
                .Include(wc => wc.WorkDays)
                .Include(wc => wc.WorkHolidays)
                .FirstOrDefault(wc => wc.ID == workContract.ID);

            if (workContractToUpdate == null) throw new NotFoundException();

            //var workContractToUpdate = _rentContext.WorkContract.Include(wc => wc.WorkDays).Include(wc => wc.WorkHolidays).FirstOrDefault(wc => wc.ID == workContract.ID);
            // remove all workdays and workholidays
            _rentContext.WorkDay.RemoveRange(workContractToUpdate.WorkDays);
            _rentContext.WorkHoliday.RemoveRange(workContractToUpdate.WorkHolidays);
            //await _rentContext.SaveChangesAsync();


            // update workContract values
            workContractToUpdate.Note = workContract.Note;
            workContractToUpdate.FromDate = workContract.FromDate;
            workContractToUpdate.ToDate = workContract.ToDate;
            workContractToUpdate.WorkHolidays = workContract.WorkHolidays;
            workContractToUpdate.WorkDays = workContract.WorkDays;

            _rentContext.WorkContract.Update(workContractToUpdate);
            await _rentContext.SaveChangesAsync();

            // find absences of workContract-User - if any

            /*
            ICollection<Absence> absences = new List<Absence>();
            if (workContract.ContractID != null)
            {
                absences = _rentContext.Absence.Include(a => a.User).ThenInclude(u => u.Contracts).Where(a => a.User.Contracts.Any(c => c.ID == workContract.ContractID)).ToList();
            }


            Console.WriteLine("Absences in period list length");
            Console.WriteLine(absences.Count);
            */

            IQueryable<WorkContract> queryNewWorkContract = GetWorkContract(requester, workContractToUpdate.ID);
            var newWorkContract = queryNewWorkContract.FirstOrDefault();

            var oldWork = _rentContext.Work.Where(w => w.WorkContractID == newWorkContract.ID && w.WorkRegistration == null).ToList();
            Console.WriteLine("new work contract from date");
            Console.WriteLine(newWorkContract.FromDate);
            Console.WriteLine("new work contract to date");
            Console.WriteLine(newWorkContract.ToDate);
            var newWork = CreateContractWorkBetweenDates(requester, newWorkContract, newWorkContract.FromDate, newWorkContract.ToDate).ToList();

            Console.WriteLine("Old works");
            oldWork.ForEach(w =>
            {
                Console.WriteLine(w.Date);
            });
            Console.WriteLine("new works");

            newWork.ForEach(w =>
            {
                Console.WriteLine(w.Date);
            });

            //new work - old work - workReplacements
            var worksToAdd = newWork.Where(nw => !oldWork.Any(ow => nw.Date.Equals(ow.Date))).ToList();
            //old work in new work
            var worksToUpdate = oldWork.Where(ow => newWork.Any(nw => nw.Date.Equals(ow.Date))).ToList();
            //old work - new work
            var worksToRemove = oldWork.Where(ow => !newWork.Any(nw => ow.Date.Equals(nw.Date))).ToList();
            Console.WriteLine("Works to remove");
            worksToRemove.ForEach(w =>
            {
                Console.WriteLine(w.Date);
            });
            Console.WriteLine("Works to Add");
            worksToAdd.ForEach(w =>
            {
                Console.WriteLine(w.Date);
            });


            List<WorkReplacement> replacementsToRemove = new List<WorkReplacement>();

            worksToRemove.ForEach(w =>
            {
                if (w.WorkReplacement != null)
                {
                    replacementsToRemove.Add(w.WorkReplacement);
                }
            });

            worksToUpdate.ForEach(w =>
            {

                if (!FlagsHelper.IsSet<int>(w.Modifications, (int)WorkModificationFlags.Registered))
                {
                    int week = DateTimeHelpers.WeekOfYear(w.Date);
                    int day = DateTimeHelpers.GetDayOfWeek(w.Date);
                    var workDay = newWorkContract.WorkDays.FirstOrDefault(wd => ((wd.IsEvenWeek == 0) || (DateTimeHelpers.IsEvenWeek(week) && wd.IsEvenWeek == 1) || (!DateTimeHelpers.IsEvenWeek(week) && wd.IsEvenWeek == 2)) && day == wd.DayOfWeek);

                    if (!FlagsHelper.IsSet<int>(w.Modifications, (int)WorkModificationFlags.Note)) w.Note = workContract.Note;
                    if (!FlagsHelper.IsSet<int>(w.Modifications, (int)WorkModificationFlags.StartTimeMins)) w.StartTimeMins = workDay.StartTimeMins;
                    if (!FlagsHelper.IsSet<int>(w.Modifications, (int)WorkModificationFlags.EndTimeMins)) w.EndTimeMins = workDay.EndTimeMins;
                    if (!FlagsHelper.IsSet<int>(w.Modifications, (int)WorkModificationFlags.BreakMins)) w.BreakMins = workDay.BreakMins;
                    if (!FlagsHelper.IsSet<int>(w.Modifications, (int)WorkModificationFlags.IsVisible)) w.IsVisible = workContract.IsVisible;
                }
            });

            _rentContext.Work.UpdateRange(oldWork);
            _rentContext.Work.AddRange(worksToAdd);
            _rentContext.WorkReplacement.RemoveRange(replacementsToRemove);
            _rentContext.Work.RemoveRange(worksToRemove);

            await _rentContext.SaveChangesAsync();

            List<WorkReplacement> replacementsToAdd = new List<WorkReplacement>();
            worksToAdd.ForEach(w =>
            {
                // if w overlaps with users absence
                //var absence = absences.FirstOrDefault(a => DateTimeHelpers.DoesSingleDateOverlapWithDates(w.Date, a.From, a.To));

                var absence = _absenceRepository.ContractAbsenceOnDate(workContract.ContractID.Value, w.Date);

                if (absence != null)
                {

                    replacementsToAdd.Add(new WorkReplacement()
                    {
                        AbsenceID = absence.ID,
                        WorkID = w.ID,
                    });
                }
            });

            _rentContext.WorkReplacement.AddRange(replacementsToAdd);


            await _rentContext.SaveChangesAsync();
        }

        public async Task AddUserToWorkContract(int requester, int workContractId, int contractId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {

                var workContract = _rentContext.WorkContract.FirstOrDefault(wc => wc.ID == workContractId);
                Console.WriteLine("work contract's contract ID");

                if (workContract != null && workContract.ContractID == null)
                {
                    workContract.ContractID = contractId;
                    await _rentContext.SaveChangesAsync();
                    await AddUserToAllWorksAssociatedWithWorkContract(workContractId, contractId);
                    await _notiRepository.SendUserAddedToWorkContractNoti(requester, workContract);
                    return;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            throw new UnauthorizedAccessException();
        }

        public async Task RemoveUserFromWorkContract(int requester, int workContractId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var workContract = _rentContext.WorkContract.FirstOrDefault(wc => wc.ID == workContractId);
                if (workContract != null && workContract.ContractID != null)
                {

                    var oldContractId = workContract.ContractID.Value;
                    workContract.ContractID = null;
                    _rentContext.WorkContract.Update(workContract);
                    await _rentContext.SaveChangesAsync();
                    await RemoveUserFromAllWorksAssociatedWithWorkContract(workContractId, oldContractId);
                    // await _notiRepository.SendUserRemovedToWorkContractNoti(requester, workContract);
                    return;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            throw new UnauthorizedAccessException();
        }

        public async Task RemoveUserFromAllWorksAssociatedWithWorkContract(int workContractId, int contractId)
        {
            var works = _rentContext.Work.Include(w => w.WorkReplacement)
                .Where(w => w.WorkContractID == workContractId && w.ContractID == contractId && w.WorkRegistration == null)
                .ToList();
            var workReplacements = new List<WorkReplacement>();
            works.ForEach(w =>
            {
                w.ContractID = null;
                if (w.WorkReplacement != null)
                {
                    workReplacements.Add(w.WorkReplacement);
                    w.ContractID = w.WorkReplacement.ContractID;
                }
            });
            _rentContext.Work.UpdateRange(works);
            _rentContext.WorkReplacement.UpdateRange(workReplacements);
            await _rentContext.SaveChangesAsync();
        }

        public async Task AddUserToAllWorksAssociatedWithWorkContract(int workContractId, int contractId)
        {
            // get work of work contract which has no owner
            var work = _rentContext.Work.Where(w => w.WorkContractID == workContractId && w.ContractID == null).ToList();

            // get absences of user with the given contractId
            //var absences = _rentContext.Absence.Where(a => a.User.Contracts.Any(c => c.ID == contractId));

            // list for workReplacements to add for work where the user is absent. 
            var workReplacements = new List<WorkReplacement>();


            work.ForEach(w =>
            {
                // find if any absence overlaps with the work
                //var absence = absences.FirstOrDefault(a => DateTimeHelpers.DoesSingleDateOverlapWithDates(w.Date, a.From, a.To));

                var absence = _absenceRepository.ContractAbsenceOnDate(contractId, w.Date);
                if (absence != null)
                {
                    // create a workReplacement if user is absent 
                    workReplacements.Add(new WorkReplacement
                    {
                        WorkID = w.ID,
                        AbsenceID = absence.ID
                    });
                }

                // update the works contract id to the new owner
                w.ContractID = contractId;
            });

            if (workReplacements.Count > 0)
            {
                _rentContext.WorkReplacement.AddRange(workReplacements);
            }
            _rentContext.Work.UpdateRange(work);
            await _rentContext.SaveChangesAsync();
        }
        /*
        public async Task<int> CreateWorkContract(int requester, WorkContract workContract, int locationId)
        {
            if (workContract == null)
            {
                throw new NullReferenceException();
            }
            if (_roleRepo.IsAdminOrManager(requester))
            {
                //var workDays = workContract.WorkDays.ToList();
                //var workHolidays = workContract.WorkHolidays.ToList();
                workContract.LocationID = locationId;
                _rentContext.WorkContract.Add(workContract);
                await _rentContext.SaveChangesAsync();

                //workDays.ForEach(wd => wd.WorkContractID = workContract.ID);
                //workHolidays.ForEach(wd => wd.WorkContractID = workContract.ID);
                //_rentContext.WorkDay.AddRange(workDays);
                //_rentContext.WorkHoliday.AddRange(workHolidays);
                //await _rentContext.SaveChangesAsync();

                IQueryable<WorkContract> queryWorkContract = GetWorkContract(requester, workContract.ID);
                workContract = queryWorkContract.FirstOrDefault();

                var workToCreate = CreateContractWorkBetweenDates(requester, workContract, workContract.FromDate, workContract.ToDate);
                _rentContext.Work.AddRange(workToCreate);
                await _rentContext.SaveChangesAsync();
                return workContract.ID;
            }
            throw new UnauthorizedAccessException();
        }
        */

        public IEnumerable<Work> CreateContractWorkBetweenDates(int requester, WorkContract workContract, DateTime start, DateTime end)
        {
            var holidayCalc = IHolidayCalculator.CreateHolidayCalculator("DK");
            var holidates = holidayCalc.GetDatesOfHolidays(workContract.WorkHolidays.Select(wh => wh.Holiday).ToList(), start, end);
            var worksToBeCreated = new List<Work>();
            DateTimeHelpers.IterateDates(start, end, date =>
            {
                if (!holidates.Any(d => d.Date.Equals(date.Date)))
                {
                    int week = DateTimeHelpers.WeekOfYear(date);
                    int day = DateTimeHelpers.GetDayOfWeek(date);
                    var workDay = workContract.WorkDays.FirstOrDefault(wd => ((wd.IsEvenWeek == 0) || (DateTimeHelpers.IsEvenWeek(week) && wd.IsEvenWeek == 1) || (!DateTimeHelpers.IsEvenWeek(week) && wd.IsEvenWeek == 2)) && day == wd.DayOfWeek);
                    if (workDay != null)
                    {

                        Work work = new Work
                        {
                            WorkContractID = workContract.ID,
                            ContractID = workContract.ContractID,
                            ProjectItemID = workContract.ProjectItemID,
                            //LocationID = workContract.LocationID,
                            Note = workContract.Note,
                            Date = date,
                            StartTimeMins = workDay.StartTimeMins,
                            EndTimeMins = workDay.EndTimeMins,
                            BreakMins = workDay.BreakMins,
                            //WorkReplacementID = null,
                            //WorkRegistrationID = null,
                            IsVisible = true,
                            Modifications = 0
                        };
                        worksToBeCreated.Add(work);
                    }
                }
            });
            return worksToBeCreated;
            // _rentContext.AddRange(worksToBeCreated);
            //await _rentContext.SaveChangesAsync();


        }



        public async Task<int> CreateForProjectItem(int requester, WorkContract workContract, int projectItemId)
        {
            if (workContract == null)
            {
                throw new NullReferenceException();
            }
            if (_roleRepo.IsAdminOrManager(requester))
            {
                workContract.ProjectItemID = projectItemId;
                _rentContext.WorkContract.Add(workContract);
                await _rentContext.SaveChangesAsync();

                IQueryable<WorkContract> queryWorkContract = GetWorkContract(requester, workContract.ID);
                workContract = queryWorkContract.FirstOrDefault();

                var workToCreate = CreateContractWorkBetweenDates(requester, workContract, workContract.FromDate, workContract.ToDate);
                _rentContext.Work.AddRange(workToCreate);
                await _rentContext.SaveChangesAsync();
                return workContract.ID;
            }
            throw new UnauthorizedAccessException();
        }

        public IQueryable<dynamic> GetOfProjectItem(int requester, int projectItemId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                return _rentContext.WorkContract
                //.Include(wc => wc.Location)
                //.ThenInclude(l => l.Customer)
                .Include(wc => wc.Contract)
                .ThenInclude(c => c.User)
                .Where(wc => wc.ProjectItemID == projectItemId).Select(WorkContract.StandardDTO());
            }
            throw new UnauthorizedAccessException();
        }


    }
}