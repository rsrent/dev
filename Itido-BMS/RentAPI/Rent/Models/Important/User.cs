using System;
using System.Collections.Generic;
using System.Dynamic;
using Rent.DTOs;
using Rent.Repositories;
using Rent.Models.TimePlanning;
using System.Linq.Expressions;
using System.Linq;
using Rent.Models.Projects;

namespace Rent.Models
{
    public class User : IDto
    {
        public int ID { get; set; }
        public bool Disabled { get; set; }
        public int LoginID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }

        public string UserRole { get; set; }
        public int? ProjectRoleID { get; set; }

        public int? CustomerID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string LanguageCode { get; set; }

        public virtual ProjectRole ProjectRole { get; set; }
        public virtual Role Role { get; set; }
        public virtual Login Login { get; set; }
        public Customer Customer { get; set; }
        public virtual ICollection<LocationUser> LocationUsers { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Absence> Absences { get; set; }
        public virtual ICollection<Absence> AbsencesCreated { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<ProjectItemUser> ProjectItemUsers { get; set; }

        public int? ClientID { get; set; }
        public virtual Client Client { get; set; }

        static public Expression<Func<User, dynamic>> BasicDTO()
        {
            return u => u != null ?
            new
            {
                u.ID,
                u.FirstName,
                u.LastName,
                u.UserRole,
                u.EmployeeNumber,
                u.ImageLocation,
                u.Disabled,
                IsClientUser = u.ClientID != null,
            } : null;
        }

        static public Expression<Func<User, dynamic>> StandardDTO(DateTime? absenceDate)
        {
            return u => u != null ?
            new
            {
                u.ID,
                u.FirstName,
                u.LastName,
                u.UserRole,
                u.EmployeeNumber,
                u.ImageLocation,
                u.Disabled,
                u.Email,
                u.Phone,
                u.LanguageCode,
                u.ProjectRole,
                Client = u.Client != null ? new { u.Client.ID, u.Client.ProjectItem.Project.Name } : null,
                HasProject = u.ProjectUsers.Any(),
                HasAbsence = absenceDate != null ? (bool?)u.Absences
                    .Any(a =>
                    a.From <= absenceDate && absenceDate <= a.To
                    && (!a.IsRequest
                        || a.Request.ApprovalState == ApprovalState.Approved))
                        : null,
                HasAbsenceRequest = absenceDate != null ? (bool?)u.Absences
                    .Any(a =>
                    a.From <= absenceDate && absenceDate <= a.To
                    && (a.IsRequest
                        && a.Request.ApprovalState == ApprovalState.Pending))
                        : null,
            } : null;
        }



        public override dynamic Detailed()
        {
            return Merger.Merge(new
            {
                RoleID,
                Email,
                Phone,
                Comment,
                ProjectRole,
            }, Basic());
        }

        public override dynamic Basic()
        {
            return new
            {
                ID,
                FirstName,
                LastName,
                EmployeeNumber,
                RoleID,
                RoleName = Role?.Name,
                Title,
                CustomerName = Customer?.Name,
                ImageLocation,
                Disabled,
                CustomerID,
                UserRole,
            };
        }

        public string GetName() => FirstName + " " + LastName + (EmployeeNumber != null ? (" (" + EmployeeNumber + ") ") : "");

    }
}
