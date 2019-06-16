using Microsoft.AspNetCore.Mvc;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.WebApi.Models;
using IndicatorsManager.WebApi.Filters;

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

        [HttpGet("info")]
        public IActionResult GetImporterParameters()
        {
            return Ok(this.importLogic.GetIndicatorImporters());
        }

        [HttpPost]
        public IActionResult Post([FromBody] ImportModel model)
        {
            return Ok(this.importLogic.ImportIndicators(model.AreaId, model.ImporterName, model.Parameters));
        }
    }
}