using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Exceptions;
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
        public IActionResult GetTopUsers()
        {
            try
            {
                IEnumerable<User> result = this.report.GetUsersMostLogs(10);
                if(result == null)
                {
                    return NotFound("No hay usuarios que se hayan logueado.");
                }
                return Ok(result.Select(u => new UserGetModel(u)));
            }
            catch(Exception)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("tophiddenindicators")]
        public IActionResult GetTopHiddenIndicators()
        {
            try
            {
                IEnumerable<Indicator> result = this.report.GetMostHiddenIndicators(10);
                if(result == null)
                {
                    return NotFound("No hay indicadores ocultos.");
                }
                return Ok(result.Select(i => new IndicatorOnlyModel(i)));
            }
            catch(Exception)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }
    }
    
}