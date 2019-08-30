using System;
using Rent.Models;
using Rent.Models.TimePlanning;
using Rent.Models.TimePlanning.Enums;

namespace Rent.Repositories.Language
{
    public class TranslationsEnglish : Translations
    {
        public override string NotiTitleAbsenceRequested() =>
            "Absence requested";
        public override string NotiBodyAbsenceRequested(User sender) =>
            "You have received a request about absence from " + sender.GetName();

        public override string NotiTitleAbsenceCreated() =>
            "Absence created";
        public override string NotiBodyAbsenceCreated(User sender) =>
            "Absence has been created by " + sender.GetName();

        public override string NotiTitleAbsenceApproved() =>
            "Absence approved";
        public override string NotiBodyAbsenceApproved(User sender) =>
            "Request for absence has been approved by " + sender.GetName();

        public override string NotiTitleAbsenceDeclined() =>
            "Absence declined";
        public override string NotiBodyAbsenceDeclined(User sender) =>
            "Request for absence has been declined by " + sender.GetName();

        public override string NotiTitlePostReceived(User sender) =>
            "New post from " + sender.GetName();
        public override string NotiBodyPostReceived(User sender, Post post) =>
            post.Title + ": " + post.Body;
        
        
        public override string NotiTitleWorkContractUserAdded(User sender) =>
            sender.GetName() + " has added you to a work contract";

         public override string NotiBodyWorkContractUserAdded(User sender, WorkContract workContract) =>
            sender.GetName() + " has added you to a work contract on ???";


        public override string NotiTitleWorkContractUserRemoved(User sender) =>
            sender.GetName() + " has removed you from a work contract";

         public override string NotiBodyWorkContractUserRemoved(User sender, WorkContract workContract) =>
            sender.GetName() + " has removed you from a work contract on ???";

         public override string NotiTitleWorkReplacerAdded(User sender, Work work) =>
            sender.GetName() + " has added you to replace a shift";

        public override string NotiNodyWorkRepacerAdded(User sender, Work work) =>
            sender.GetName() + " has added you to replace at ??? shift d. " + work.Date;
        public override string NotiTitleAccidentReportRequested(AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " created";
        public override string NotiBodyAccidentReportRequested(User sender, AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " has been created by " + sender.GetName();

        public override string NotiTitleAccidentReportApproved(AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " approved";
        public override string NotiBodyAccidentReportApproved(User sender, AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " has been approved by " + sender.GetName();

        public override string NotiTitleAccidentReportDeclined(AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " declined";
        public override string NotiBodyAccidentReportDeclined(User sender, AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " has been declined by " + sender.GetName();
        public override string NotiBodyRegistrationApproved(User sender, WorkRegistration workRegistration) =>
            "Your registration has been approved";
        public override string NotiBodyRegistrationDeclined(User sender, WorkRegistration workRegistration) =>
            "Your registration has been declined";

        public override string NotiTitleRegistrationApproved(User sender, WorkRegistration workRegistration) =>
            "The registration from the "  + workRegistration.Date.Month + "/" + workRegistration.Date.Day +  " has been approved";
        public override string NotiTitleRegistrationDeclined(User sender, WorkRegistration workRegistration) =>
            "The registration from the "  + workRegistration.Date.Month + "/" + workRegistration.Date.Day +  " has been declined";
        public override string NotiTitleInvitation(User sender, WorkInvitation invitation) => 
            "You have been invited to a shift";

        public override string NotiBodyInvitation(User sender, WorkInvitation invitation) =>
            "You can either accept or decline";

        public override string NotiTitleReplacementInvitation(User sender, WorkInvitation invitation) =>
            "You have been invited to replace a shift";
        public override string NotiBodyReplacementInvitation(User sender, WorkInvitation invitation) =>
            "You can accept or decline this";
        
        public override string NotiTitleInvitationAccept(User sender, WorkInvitation invitation) =>
            "An invitation has been accpeted";
        public override string NotiBodyInvitationAccept(User sender, WorkInvitation invitation) =>
            "An invitation has been accpeted";

        
        public override string NotiTitleInvitationDecline(User sender, WorkInvitation invitation) =>
            "An invitation has declined";
        public override string NotiBodyInvitationDecline(User sender, WorkInvitation invitation) =>
            "An invitation has declined";

            
        string AccidentRapportTypeToString(AccidentReportType type) =>
            type == AccidentReportType.Accident ? "Accident report" : "Near-by-accident report";
    }
}
