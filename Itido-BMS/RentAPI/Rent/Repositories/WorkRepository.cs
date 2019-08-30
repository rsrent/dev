using System;
using System.Linq;
using Rent.Data;
using Rent.Models;

namespace Rent.Repositories
{
    /* 
    public class WorkRepository
    {
        RentContext _context;

        public WorkRepository(RentContext context)
        {
            _context = context;
        }

        
        public void StartWork(Work work)
        {
            work.StartTime = DateTime.Now;


        }

        public Work EndWork(Work work)
        {
            var oldWork = _context.Work.Find(work.ID);
            if (oldWork == null) {
                return null;
            }

            work.EndTime = DateTime.Now;
            _context.Work.Update(work);
            _context.SaveChanges();

            return work;
        }

        public void PerformTaskWork(Work work)
        {

        }

        public void GetUserWork(int userID)
        {

        }

        public void GetLocationWork(int locationID)
        {
            _context.Work.Where(w => w.LocationID == locationID);
        }

        public void GetTaskWork(int taskID)
        {
            _context.Work.Where(w => w.CleaningTaskID == taskID);
        }
        
}*/
}
