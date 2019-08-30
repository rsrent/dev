using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class MessageDTOFactory
    {
        public MessageDTO MakeMessageDTO(Rent.Models.Message message)
        {
            if (message.Type.Equals("text"))
            {
                return new MessageTextDTO(message);
            }
            throw new NotImplementedException();
        }


    }
}
