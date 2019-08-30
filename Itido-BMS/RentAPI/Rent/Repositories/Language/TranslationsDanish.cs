using System;
using Rent.Models;
using Rent.Models.TimePlanning;
using Rent.Models.TimePlanning.Enums;

namespace Rent.Repositories.Language
{
    public class TranslationsDanish : Translations
    {
        public override string NotiTitleAbsenceRequested() =>
            "Fravær anmodet";
        public override string NotiBodyAbsenceRequested(User sender) =>
            "Du har fået en anmodning om fravær fra " + sender.GetName();

        public override string NotiTitleAbsenceCreated() =>
            "Fravær oprettet";
        public override string NotiBodyAbsenceCreated(User sender) =>
            "Fravær er blevet oprettet af " + sender.GetName();

        public override string NotiTitleAbsenceApproved() =>
            "Fravær godkendt";
        public override string NotiBodyAbsenceApproved(User sender) =>
            "Fravær er blevet godkendt af " + sender.GetName();

        public override string NotiTitleAbsenceDeclined() =>
            "Fravær afvist";
        public override string NotiBodyAbsenceDeclined(User sender) =>
            "Fravær er blevet afvist af " + sender.GetName();

        public override string NotiTitlePostReceived(User sender) =>
            "Ny post fra " + sender.GetName();
        public override string NotiBodyPostReceived(User sender, Post post) =>
            post.Title + ": " + post.Body;

        public override string NotiTitleAccidentReportRequested(AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " oprettet";
        public override string NotiBodyAccidentReportRequested(User sender, AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " er blevet oprettet af " + sender.GetName();

        public override string NotiTitleAccidentReportApproved(AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " godkendt";
        public override string NotiBodyAccidentReportApproved(User sender, AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " er blevet godkendt af " + sender.GetName();

        public override string NotiTitleAccidentReportDeclined(AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " afvist";
        public override string NotiBodyAccidentReportDeclined(User sender, AccidentReport accidentReport) =>
            AccidentRapportTypeToString(accidentReport.AccidentReportType) + " er blevet afvist af " + sender.GetName();

        public override string NotiTitleWorkContractUserAdded(User sender) =>
            sender.GetName() + " har tilføjet dig til en arbejdskontrakt";

        public override string NotiBodyWorkContractUserAdded(User sender, WorkContract workContract) =>
            sender.GetName() + " har tilføjet dig til en arbejdskontrakt på ???";



        public override string NotiTitleWorkContractUserRemoved(User sender) =>
            sender.GetName() + " har fjernet dig fra en arbejdskontrakt";

        public override string NotiBodyWorkContractUserRemoved(User sender, WorkContract workContract) =>
            sender.GetName() + " har fjernet dig fra en arbejdskontrakt på ???";

        public override string NotiTitleWorkReplacerAdded(User sender, Work work) =>
            sender.GetName() + " har tilføjet dig til at erstatte en vagt";

        public override string NotiNodyWorkRepacerAdded(User sender, Work work) =>
            sender.GetName() + " har tilføjet dig til at erstatte en vagt på ??? d. " + work.Date;

        public override string NotiBodyRegistrationApproved(User sender, WorkRegistration workRegistration) =>
            "Din registrering er blevet godkendt";
        public override string NotiBodyRegistrationDeclined(User sender, WorkRegistration workRegistration) =>
            "Din registrering er blevet afvist";

        public override string NotiTitleRegistrationApproved(User sender, WorkRegistration workRegistration) =>
            "Din registrering d. " + workRegistration.Date.Day + "/" + workRegistration.Date.Month +  " er blevet godkendt";
        public override string NotiTitleRegistrationDeclined(User sender, WorkRegistration workRegistration) =>
            "Din registrering d. " + workRegistration.Date.Day + "/" + workRegistration.Date.Month +  " er blevet afvist";

        public override string NotiTitleInvitation(User sender, WorkInvitation invitation) =>
            "Du er blevet inviteret til en vagt";

        public override string NotiBodyInvitation(User sender, WorkInvitation invitation) =>
            "Du kan svare ja eller nej til din nye vagt";

        public override string NotiTitleReplacementInvitation(User sender, WorkInvitation invitation) =>
            "Du er blevet inviteret til at erstatte en vagt";
        public override string NotiBodyReplacementInvitation(User sender, WorkInvitation invitation) =>
            "Du kan svare ja eller nej til dette";

        public override string NotiTitleInvitationAccept(User sender, WorkInvitation invitation) =>
            "En invitation er blevet accepteret";
        public override string NotiBodyInvitationAccept(User sender, WorkInvitation invitation) =>
            "En invitation er blevet accepteret";


        public override string NotiTitleInvitationDecline(User sender, WorkInvitation invitation) =>
            "En invitation er blevet afvist";
        public override string NotiBodyInvitationDecline(User sender, WorkInvitation invitation) =>
            "En invitation er blevet afvist";

        string AccidentRapportTypeToString(AccidentReportType type) =>
            type == AccidentReportType.Accident ? "Ulykkesrapport" : "Nær-ved-ulykkesrapport";
    }
}
