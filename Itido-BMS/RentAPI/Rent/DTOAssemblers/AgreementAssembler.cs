using Rent.Repositories.TimePlanning;
using Rent.DTOs.TimePlanningDTO;
using Rent.Models.TimePlanning;
using Rent.Models;
using Rent.Data;
using System.Collections.Generic;
using System;

namespace Rent.DTOAssemblers
{
    public class AgreementAssembler
    {

        public System.Linq.Expressions.Expression<Func<Agreement, Object>> AgreementDTO = agreement => new 
        {
            ID = agreement.ID,
            Name = agreement.Name
        };

    }
}