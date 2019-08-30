using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rent.Helpers;
using Rent.Models.Projects;

namespace Rent.Models.TimePlanning
{

    public class WorkContract
    {
        public int ID { get; set; }
        public int? ContractID { get; set; }

        public string Note { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsVisible { get; set; }

        public virtual Contract Contract { get; set; }


        public virtual ICollection<WorkHoliday> WorkHolidays { get; set; }
        public virtual ICollection<WorkDay> WorkDays { get; set; }

        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }

        public static Expression<Func<WorkContract, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Note,
                v.ToDate,
                v.FromDate,
                v.IsVisible,
            } : null;
        }

        public static Expression<Func<WorkContract, dynamic>> StandardDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Note,
                v.ToDate,
                v.FromDate,
                v.IsVisible,

                // project 
                ProjectItem = ProjectItem.BasicDTO().Compile()(v.ProjectItem),
                ProjectItem_project = v.ProjectItem != null ? Project.BasicDTO().Compile()(v.ProjectItem.Project) : null,

                // contract
                Contract = Contract.BasicDTO().Compile()(v.Contract),
                Contract_user = v.Contract != null ? User.BasicDTO().Compile()(v.Contract.User) : null,
            } : null;
        }

        public static Expression<Func<WorkContract, dynamic>> DetailedDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Note,
                v.ToDate,
                v.FromDate,
                v.IsVisible,

                // project 
                ProjectItem = ProjectItem.BasicDTO().Compile()(v.ProjectItem),
                ProjectItem_project = v.ProjectItem != null ? Project.BasicDTO().Compile()(v.ProjectItem.Project) : null,

                // contract
                Contract = Contract.BasicDTO().Compile()(v.Contract),
                Contract_user = v.Contract != null ? User.BasicDTO().Compile()(v.Contract.User) : null,

                v.WorkDays,
                v.WorkHolidays,
            } : null;
        }
    }
}
