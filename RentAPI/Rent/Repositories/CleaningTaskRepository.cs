using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;

namespace Rent.Repositories
{
    public class CleaningTaskRepository
    {
        private readonly CleaningTaskContext _cleaningTaskContext;
        private readonly QualityReportItemContext _qualityReportItemContext;

        private readonly RentContext _context;
        private readonly PermissionRepository _permissionRepository;
        private readonly PropCondition _propCondition;

        public CleaningTaskRepository(CleaningTaskContext cleaningTaskContext, QualityReportItemContext qualityReportItemContext, RentContext context, PermissionRepository permissionRepository, PropCondition propCondition)
        {
            _context = context;
            _cleaningTaskContext = cleaningTaskContext;
            _qualityReportItemContext = qualityReportItemContext;
            _permissionRepository = permissionRepository;
            _propCondition = propCondition;
        }

        private IEnumerable<CleaningTask> GetTasks(int requester)
        {
            return _cleaningTaskContext.Database(requester, null, "Area", "Floor", "Area.CleaningPlan", "CompletedTasks");
        }

        /*
        public List<CleaningPlanDTO> PlansForLocation(int locationID, int userID) {
            var planTypes = new List<CleaningPlanDTO>();
            foreach (var task in GetTasks(locationID, userID).ToHashSet())
            {
                if (!planTypes.Any(pt => pt.CleaningPlan.ID == task.Area.CleaningPlanID))
                    planTypes.Add(new CleaningPlanDTO { CleaningPlan = task.Area.CleaningPlan, Floors = new List<CleaningPlanFloorDTO>(), Areas = new List<CleaningPlanAreaDTO>() });
                var planType = planTypes.FirstOrDefault(pt => pt.CleaningPlan.ID == task.Area.CleaningPlanID);

                if(task.Floor != null) {
                    if (!planType.Floors.Any(f => f.Floor.ID == task.Floor.ID))
                        planType.Floors.Add(new CleaningPlanFloorDTO { Floor = task.Floor, Areas = new List<CleaningPlanAreaDTO>() });
                    var floor = planType.Floors.FirstOrDefault(f => f.Floor.ID == task.Floor.ID);

                    if (!floor.Areas.Any(a => a.Area.ID == task.Area.ID))
                        floor.Areas.Add(new CleaningPlanAreaDTO { Area = task.Area, Tasks = new List<dynamic>() });
                    floor.Areas.FirstOrDefault(a => a.Area.ID == task.Area.ID).Tasks.Add(task.Standard());

                } else {
                    if (!planType.Areas.Any(a => a.Area.ID == task.Area.ID))
                        planType.Areas.Add(new CleaningPlanAreaDTO { Area = task.Area, Tasks = new List<dynamic>() });
                    planType.Areas.FirstOrDefault(a => a.Area.ID == task.Area.ID).Tasks.Add(task.Standard());
                }
            }
            return planTypes;
        }

        public dynamic PlanTasks(int requester, int locationID)
        {
            return GetTasks(requester).Where(c => c.LocationID == locationID)
                    .ToHashSet()
                    .GroupBy(t => t.Area.CleaningPlan)
                    .Select(p =>
                             new
                             {
                                 CleaningPlan = p.Key,
                                 Floors = FloorTasks(p.Where(t => t.Floor != null)),
                                 Areas = AreaTasks(p.Where(t => t.Floor == null)),
                             });
        }
        */


        private dynamic PlanTasks(int requester, IEnumerable<CleaningTask> tasks)
        {
            return tasks.GroupBy(t => t.Area.CleaningPlan).Select(p =>
                             new
                             {
                                 CleaningPlan = p.Key.Basic(),
                                 Floors = FloorTasks(requester,p.Where(t => t.Floor != null)),
                                 Areas = AreaTasks(requester,p.Where(t => t.Floor == null)),
                             });
        }

        private dynamic FloorTasks(int requester, IEnumerable<CleaningTask> tasks)
        {
            return tasks.GroupBy(t => t.Floor).Select(a => new { Floor = a.Key.Basic(), Areas = AreaTasks(requester, a) });
        }
        
        private dynamic AreaTasks(int requester, IEnumerable<CleaningTask> tasks)
        {
            return tasks.GroupBy(g => g.Area).Select(g => new { Area = g.Key.Basic(), Tasks = g.ToList().Select(t => t.Detailed()) });
        }


        public dynamic Plans(int requester, int locationID)
        {
            return PlanTasks(requester, GetTasks(requester).Where(c => c.LocationID == locationID)
                            .ToHashSet());
        }

        public dynamic Floors(int requester, int locationID, int planID)
        {
            return FloorTasks(requester, GetTasks(requester)
                             .Where(c => c.LocationID == locationID)
                             .Where(t => t.Area.CleaningPlanID == planID)
                             .ToHashSet());
        }

        public dynamic Tasks(int requester, int locationID, int planID)
        {
            return AreaTasks(requester, GetTasks(requester)
                             .Where(c => c.LocationID == locationID)
                             .Where(t => t.Area.CleaningPlanID == planID)
                             .ToHashSet());
        }

        public dynamic Tasks(int requester, int locationID, int planID, int floorID)
        {
            return AreaTasks(requester, GetTasks(requester)
                             .Where(c => c.LocationID == locationID)
                             .Where(t => t.Area.CleaningPlanID == planID && t.Floor.ID == floorID)
                             .ToHashSet());
        }









        /*
        public List<CleaningPlanAreaDTO> TaskFromFloor(int requester, int locationID, int planID, int floorID)
        {
            return ToCleaningPlanAreaDTO(GetTasks(locationID, requester).Where(t => t.Area.CleaningPlanID == planID && t.Floor.ID == floorID).ToHashSet());
        }

        public List<CleaningPlanAreaDTO> TaskFromPlan(int requester, int locationID, int planID)
        {
            return ToCleaningPlanAreaDTO(GetTasks(locationID, requester).Where(t => t.Area.CleaningPlanID == planID).ToHashSet());
        }

        private List<CleaningPlanAreaDTO> ToCleaningPlanAreaDTO(IEnumerable<CleaningTask> tasks)
        {
            var areas = new List<CleaningPlanAreaDTO>();
            foreach (var task in tasks)
            {
                if (!areas.Any(a => a.Area.ID == task.Area.ID))
                    areas.Add(new CleaningPlanAreaDTO { Area = task.Area, Tasks = new List<dynamic>() });
                areas.FirstOrDefault(a => a.Area.ID == task.Area.ID).Tasks.Add(task.Standard());
            }
            return areas;
        }
        */

        public async Task<dynamic> Add(int requester, int locationID, CleaningTask CleaningTask) 
        {
            var newTask = await _cleaningTaskContext.Create(requester, new CleaningTask
            {
                AreaID = CleaningTask.Area.ID,
                FloorID = CleaningTask.Floor?.ID,
                Comment = CleaningTask.Comment,
                Frequency = CleaningTask.Frequency,
                LocationID = locationID,
                SquareMeters = CleaningTask.SquareMeters,
                Active = true,
                TimesOfYear = CleaningTask.TimesOfYear,
            });

            if (CleaningTask.Area.CleaningPlanID == 1)
            {
                var newQualityReportItems = _context.QualityReport.Where(q => q.LocationID == locationID && q.RatingID == null).Select(q => new QualityReportItem { CleaningTaskID = newTask.ID, QualityReportID = q.ID }).ToList();
                await _qualityReportItemContext.Create(requester, newQualityReportItems);
            }
            return newTask.Detailed();
        }

        public async Task Delete(int requester, int id)
        {
            await _cleaningTaskContext.Update(requester, c => c.ID == id, c => {
                c.Active = false;
            });

            await _qualityReportItemContext.Delete(requester, q => q.CleaningTaskID == id && q.QualityReport.RatingID == null, "QualityReport");
        }
    }
}
