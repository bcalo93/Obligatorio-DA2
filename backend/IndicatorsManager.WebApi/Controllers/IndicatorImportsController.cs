using Microsoft.AspNetCore.Mvc;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.WebApi.Models;
using IndicatorsManager.WebApi.Filters;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorImportsController : ControllerBase
    {
        private IIndicatorImportLogic importLogic;

        public IndicatorImportsController(IIndicatorImportLogic importLogic) : base()
        {
            this.importLogic = importLogic;
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("info")]
        public IActionResult GetImporterParameters()
        {
            return Ok(this.importLogic.GetIndicatorImporters());
        }

        [ProtectFilter(Role.Admin)]
        [HttpPost]
        public IActionResult Post([FromBody] ImportModel model)
        {
            try
            {
                return Ok(this.importLogic.ImportIndicators(model.AreaId, model.ImporterName, model.Parameters));
            }
            catch(EntityNotExistException ee)
            {
                return NotFound(ee.Message);
            }
            catch(ImportException ie)
            {
                return BadRequest(ie.Message);
            }
        }
    }
}