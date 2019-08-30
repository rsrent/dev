using System;
using System.Collections.Generic;
using Rent.Models;
using Rent.Models.TimePlanning;

namespace Rent.Repositories.Language
{
    public abstract class Translations
    {
        public abstract string NotiTitleAbsenceRequested();
        public abstract string NotiBodyAbsenceRequested(User sender);

        public abstract string NotiTitleAbsenceCreated();
        public abstract string NotiBodyAbsenceCreated(User sender);

        public abstract string NotiTitleAbsenceApproved();
        public abstract string NotiBodyAbsenceApproved(User sender);

        public abstract string NotiTitleAbsenceDeclined();
        public abstract string NotiBodyAbsenceDeclined(User sender);

        public abstract string NotiTitlePostReceived(User sender);
        public abstract string NotiBodyPostReceived(User sender, Post post);

        public abstract string NotiTitleAccidentReportRequested(AccidentReport accidentReport);
        public abstract string NotiBodyAccidentReportRequested(User sender, AccidentReport accidentReport);


        public abstract string NotiTitleAccidentReportApproved(AccidentReport accidentReport);
        public abstract string NotiBodyAccidentReportApproved(User sender, AccidentReport accidentReport);

        public abstract string NotiTitleAccidentReportDeclined(AccidentReport accidentReport);
        public abstract string NotiBodyAccidentReportDeclined(User sender, AccidentReport accidentReport);

        public abstract string NotiTitleWorkContractUserAdded(User sender);

        public abstract string NotiBodyWorkContractUserAdded(User sender, WorkContract workContract);

        public abstract string NotiTitleWorkContractUserRemoved(User sender);
        public abstract string NotiBodyWorkContractUserRemoved(User sender, WorkContract workContract);

        public abstract string NotiTitleWorkReplacerAdded(User sender, Work work);

        public abstract string NotiNodyWorkRepacerAdded(User sender, Work work);

        public abstract string NotiBodyRegistrationApproved(User sender, WorkRegistration workRegistration);
        public abstract string NotiBodyRegistrationDeclined(User sender, WorkRegistration workRegistration);

        public abstract string NotiTitleRegistrationApproved(User sender, WorkRegistration workRegistration);
        public abstract string NotiTitleRegistrationDeclined(User sender, WorkRegistration workRegistration);

        public abstract string NotiTitleInvitation(User sender, WorkInvitation invitation);
        public abstract string NotiBodyInvitation(User sender, WorkInvitation invitation);

        public abstract string NotiTitleReplacementInvitation(User sender, WorkInvitation invitation);
        public abstract string NotiBodyReplacementInvitation(User sender, WorkInvitation invitation);



        public abstract string NotiTitleInvitationAccept(User sender, WorkInvitation invitation);
        public abstract string NotiBodyInvitationAccept(User sender, WorkInvitation invitation);

        
        public abstract string NotiTitleInvitationDecline(User sender, WorkInvitation invitation);
        public abstract string NotiBodyInvitationDecline(User sender, WorkInvitation invitation);





        private static Dictionary<string, Translations> _translations = new Dictionary<string, Translations>();

        public static Translations GetTranslations(string languageCode)
        {
            if (languageCode == null)
                languageCode = "en";

            if (_translations.ContainsKey(languageCode)) return _translations[languageCode];

            Translations newTranslations;
            if (languageCode == "da") newTranslations = new TranslationsDanish();
            else newTranslations = new TranslationsEnglish();

            _translations.Add(languageCode, newTranslations);

            return newTranslations;
        }
    }
}
