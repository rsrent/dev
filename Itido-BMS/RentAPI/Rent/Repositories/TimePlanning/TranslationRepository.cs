using System;
using System.Collections.Generic;
using Rent.Data;
using Rent.Models.TimePlanning;
using Rent.Repositories.Language;

/*
namespace Rent.Repositories.TimePlanning
{
    public class TranslationRepository
    {
        private readonly RentContext _context;

        private Dictionary<string, Translations> _translations;


        public TranslationRepository(RentContext context)
        {
            this._context = context;

            _translations = new Dictionary<string, Translations>()
            {
                { "da", new TranslationsDanish() },
                { "en", new TranslationsEnglish() },
            };
        }

        public void SetNotiTitleAndBody(Noti noti)
        {
            var receiver = _context.User.Find(noti.ReceiverID);
            var language = receiver.LanguageCode ?? "en";
            var translations = Translations.GetTranslations(language);
            var sender = _context.User.Find(noti.SenderID);

            if (noti.NotiType.StartsWith("Absence_", StringComparison.Ordinal))
            {
                if (noti.NotiType.Equals(Noti.AbsenceRequested))
                {
                    noti.Title = translations.NotiTitleAbsenceRequested();
                    noti.Body = translations.NotiBodyAbsenceRequested(sender);
                }
                if (noti.NotiType.Equals(Noti.AbsenceCreated))
                {
                    noti.Title = translations.NotiTitleAbsenceCreated();
                    noti.Body = translations.NotiBodyAbsenceCreated(sender);
                }
                if (noti.NotiType.Equals(Noti.AbsenceApproved))
                {
                    noti.Title = translations.NotiTitleAbsenceApproved();
                    noti.Body = translations.NotiBodyAbsenceApproved(sender);
                }
                if (noti.NotiType.Equals(Noti.AbsenceDeclined))
                {
                    noti.Title = translations.NotiTitleAbsenceDeclined();
                    noti.Body = translations.NotiBodyAbsenceDeclined(sender);
                }
            }
            if (noti.NotiType.StartsWith(Noti.Post, StringComparison.Ordinal))
            {
                noti.Title = translations.NotiTitlePostReceived(sender);
                noti.Body = "Informationen";
            }
        }
    }
}

*/
