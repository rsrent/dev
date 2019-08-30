using System;
using System.Linq.Expressions;

namespace Rent.Models.TimePlanning
{
    public class DTOHelper
    {

        public static Expression<Func<DTOHelper, dynamic>> BasicDTO(int requester, string requesterRole)
        {
            return v => v != null ?
            new
            {

            } : null;
        }

        public static Expression<Func<DTOHelper, dynamic>> StandardDTO(int requester, string requesterRole)
        {
            return v => v != null ?
            new
            {

            } : null;
        }

        public static Expression<Func<DTOHelper, dynamic>> DetailedDTO(int requester, string requesterRole)
        {
            return v => v != null ?
            new
            {

            } : null;
        }
    }
}
