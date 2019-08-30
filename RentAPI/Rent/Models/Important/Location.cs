using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Rent.Repositories;

namespace Rent.Models
{
    public class Location : IDto
    {
        public int ID { get; set; }
        public bool Disabled { get; set; }
        public int? CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public string Phone { get; set; }
        public string ImageLocation { get; set; }
        public int? ProjectNumber { get; set; }
        public int IntervalOfServiceLeaderMeeting { get; set; }

        public int? CustomerContactID { get; set; }
        public int? ServiceLeaderID { get; set; }

        public int? CustomerConversationID { get; set; }
        public int? TeamConversationID { get; set; }

        public int GeneralFolderID { get; set; }
        public int CleaningplanFolderID { get; set; }

        public int? CleaningFolderID { get; set; }
        public int? WindowFolderID { get; set; }
        public int? FanCoilFolderID { get; set; }
        public int? PeriodicFolderID { get; set; }

        public double? Lat { get; set; }
        public double? Lon { get; set; }

        [ForeignKey("LocationHour")]
        public int LocationHourID { get; set; }

        [ForeignKey("LocationEconomy")]
        public int LocationEconomyID { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual User CustomerContact { get; set; }
        public virtual User ServiceLeader { get; set; }

        public virtual Conversation CustomerConversation { get; set; }
        public virtual Conversation TeamConversation { get; set; }

        public virtual DocumentFolder GeneralFolder { get; set; }
        public virtual DocumentFolder CleaningplanFolder { get; set; }

        public virtual DocumentFolder CleaningFolder { get; set; }
        public virtual DocumentFolder WindowFolder { get; set; }
        public virtual DocumentFolder FanCoilFolder { get; set; }
        public virtual DocumentFolder PeriodicFolder { get; set; }

        public virtual ICollection<QualityReport> QualityReports { get; set; }
        public virtual ICollection<LocationUser> LocationUsers { get; set; }
        public virtual ICollection<CleaningTask> CleaningTasks { get; set; }
        public virtual ICollection<LocationLog> LocationLogs { get; set; }
        public virtual ICollection<MoreWork> MoreWork { get; set; }


        public DateTime? FirstQualityReportTime { get; set; }

        [ForeignKey("LatestQualityReport")]
        public int? LatestQualityReportID { get; set; }
        public virtual QualityReport LatestQualityReport { get; set; }


        //public DateTime? FirstScheduledQualityReportTimeSinceUpdate { get; set; }
        public int NumberOfQualityReportsSinceUpdate { get; set; }
        //public int TotalNumberOfQualityReports{ get; set; }

        public virtual LocationHour LocationHour { get; set; }
        public virtual LocationEconomy LocationEconomy { get; set; }

        public override dynamic Detailed()
        {
            return Merger.Merge(new
            {
                CustomerContactID,
                ServiceLeaderID,
                NextQualityReport = QualityReports != null && QualityReports.Any() ? QualityReports.Last().Time.AddDays(IntervalOfServiceLeaderMeeting) : new DateTime(),
                Address,
                Comment,
                Phone,
                ProjectNumber,
                IntervalOfServiceLeaderMeeting,
                Customer = Customer?.Basic(),
                CustomerContact = LocationUsers?.FirstOrDefault(lu => lu.User?.RoleID == 8)?.User.Detailed(),
                ServiceLeader = LocationUsers?.FirstOrDefault(lu => lu.User?.RoleID == 3)?.User.Detailed(),
                CleaningFolderID,
                WindowFolderID,
                FanCoilFolderID,
                PeriodicFolderID,
            }, Basic());
        }
        public override dynamic Basic()
        {
            return new
            {
                ID,
                CustomerID,
                CustomerName = Customer?.Name,
                Disabled,
                Name,
                ImageLocation,
                HoursCompleted = LocationHour != null && LocationHour.Completed(),
            };
        }
    }
}