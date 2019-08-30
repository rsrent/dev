using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;
using Rent.DTOs;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/QualityReports")]
    public class QualityReportsController : ControllerExecutor
    {
        private readonly QualityReportRepository _qualityReportRepository;

        public QualityReportsController(QualityReportRepository qualityReportRepository)
        {
            _qualityReportRepository = qualityReportRepository;
        }

        [HttpGet("GetPlanWithFloors/{id}")]
        public IActionResult GetPlanWithFloors([FromRoute] int id)
        => Executor(() => _qualityReportRepository.GetPlanWithFloors(Requester, id));

        [HttpGet("UpdateCustomerComment/{id}/{comment}")]
        public async Task<IActionResult> UpdateCustomerComment([FromRoute] int id, [FromRoute] string comment)
        => await Executor(async () => await _qualityReportRepository.UpdateCustomerComment(Requester, id, comment));

        [HttpGet("GetForLocation/{id}")]
        public IActionResult GetForLocation([FromRoute] int id)
        => Executor(() => _qualityReportRepository.GetQualityReportsForLocation(Requester, id));

        [HttpPut("AddItem/{id}")]
        public async Task<IActionResult> PutQualityReport([FromRoute] int id, [FromBody] QualityReportItem qualityReportItem)
        => await Executor(async () => await _qualityReportRepository.AddItem(Requester, id, qualityReportItem));

        [HttpPut("UpdateItem/{id}")]
        public async Task<IActionResult> UpdateQualityReportItem([FromRoute] int id, [FromBody] QualityReportItem qualityReportItem)
        => await Executor(async () => await _qualityReportRepository.UpdateItem(Requester, qualityReportItem));

        [HttpPost("Create/{locationID}")]
        public async Task<IActionResult> PostQualityReport([FromRoute] int locationID)
        => await Executor(async () => await _qualityReportRepository.CreateReport(Requester, locationID));
            
        [HttpPost("CompleteQualityReport/{qualityReportID}/{ratingValue}")]
        public async Task<IActionResult> CompleteQualityReport([FromRoute] int qualityReportID, [FromBody] DateTime nextMeetingDate, [FromRoute] int ratingValue) 
        => await Executor(async () => await _qualityReportRepository.CompleteReport(Requester, qualityReportID, nextMeetingDate, ratingValue));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQualityReport([FromRoute] int id)
        => await Executor(async () => await _qualityReportRepository.DeleteQualityReport(Requester, id));
    }
}