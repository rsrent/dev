using System;

namespace Rent.DTOs.TimePlanningDTO
{

    public class AbsenceDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AbsenceReasonID { get; set; }
        public string Comment { get; set; }
        public string  Description { get; set; }

        //Can/should the user this DTO is send to reply to the absence state
        public bool CanRespondToApprovalState { get; set; }

        public string CreatorName { get; set; }
        public ApprovalState ApprovalState { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsRequest { get; set; }
    }
}