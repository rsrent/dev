using System;
using Newtonsoft.Json;

namespace Rent.Models.TimePlanning
{
    public class Noti
    {
        public int ID { get; set; }
        public string NotiType { get; set; }
        public int SubjectID { get; set; }
        public int UserID { get; set; }

        [JsonConverter(typeof(DartDateTimeConverter))]
        [JsonProperty("sendTime")]
        public DateTime SendTime { get; set; }
        public int? SenderID { get; set; }
        public virtual User Sender { get; set; }

        public int ReceiverID { get; set; }
        public virtual User Receiver { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool Seen { get; set; }

        public static string AccidentReport = "AccidentReport_";
        public static string Absence = "Absence_";
        public static string Shift = "Shift_";

        public static string Work = "Work_";
        public static string Post = "Post_";
        public static string WorkContract = "WorkContract_";

        public static string WorkRegistration = "WorkRegistration";
        public static string Invitation = "Invitation";
        public static string AbsenceRequested = Absence + "Requested";
        public static string AbsenceCreated = Absence + "Created";
        public static string AbsenceApproved = Absence + "Approved";
        public static string AbsenceDeclined = Absence + "Declined";
        public static string ShiftInvitationReceived = Shift + "InvitationReceived";
        public static string ShiftInvitationApproved = Shift + "InvitationApproved";
        public static string ShiftInvitationDeclined = Shift + "InvitationDeclined";

        public static string AccidentReportRequested = AccidentReport + "Requested";
        public static string AccidentReportApproved = AccidentReport + "Approved";
        public static string AccidentReportDeclined = AccidentReport + "Declined";
        public static string WorkContractUserAdded = WorkContract + "UserAdded";
        public static string WorkContractUserRemoved = WorkContract + "UserRemoved";

        public static string WorkRegistrationApproved = WorkRegistration + "Approved";
        public static string WorkRegistrationDeclined = WorkRegistration + "Declined";

        public static string WorkReplacerAdded = Work + "ReplacerAdded";

        public static string InvitationSend = Invitation + "Send";
    }
}
