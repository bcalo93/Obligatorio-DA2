using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.Logger.Interface;
using IndicatorsManager.Logger.Interface.Exceptions;
using IndicatorsManager.WebApi.Filters;
using IndicatorsManager.WebApi.Models;

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
                IEnumerable<User> result = this.report.GetMostLoggedInManagers(limit);
                return Ok(result.Select(u => new UserGetModel(u)));
            }
            catch(LoggerException le)
            {
                return StatusCode(503, le.Message);
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

        [ProtectFilter(Role.Admin)]
        [HttpGet("systemactions")]
        public IActionResult GetSystemActions([FromQuery]DateTime start, [FromQuery]DateTime end)
        {
            try
            {
                IEnumerable<Log> result = this.report.GetSystemActivity(start, end);
                return Ok(result.Select(l => new LogModel(l)));
            }
            catch(LoggerException le)
            {
                return StatusCode(503, le.Message);
            }
        }
    }
    
}