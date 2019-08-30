using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibrary.Shared.Email;

namespace ModuleLibraryShared.Email
{
    public abstract class IEmail
    {
        public IEmailSettings _settings;
        public IEmail(IEmailSettings settings)
        {
            _settings = settings;
        }

        public abstract Task<bool> Send(string receiverMail, string receiverName, string header, string body, ICollection<EmailAttachment> attachments = null);
    }
}
