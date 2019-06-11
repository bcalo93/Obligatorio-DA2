using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Filters;
using IndicatorsManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndicatorsManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private IReportLogic report;

        public ReportsController(IReportLogic report) : base()
        {
            this.report = report;
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("topusers")]
        public IActionResult GetTopUsers([FromQuery]int limit)
        {
            try
            {
                IEnumerable<User> result = this.report.GetUsersMostLogs(limit);
                return Ok(result.Select(u => new UserGetModel(u)));
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("tophiddenindicators")]
        public IActionResult GetTopHiddenIndicators([FromQuery]int limit)
        {
            try
            {
                IEnumerable<Indicator> result = this.report.GetMostHiddenIndicators(limit);
                return Ok(result.Select(i => new IndicatorOnlyModel(i)));
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }
    }
    
}