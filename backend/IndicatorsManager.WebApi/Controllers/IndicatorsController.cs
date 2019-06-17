using System;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Filters;
using IndicatorsManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndicatorsManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorsController : ControllerBase
    {
        private IIndicatorLogic indicatorLogic;
        private ISessionLogic sessionLogic;
        private IIndicatorItemLogic itemLogic;

        public IndicatorsController(IIndicatorLogic indicatorLogic, ISessionLogic sessionLogic, 
            IIndicatorItemLogic itemLogic) : base()
        {
            this.indicatorLogic = indicatorLogic;
            this.sessionLogic = sessionLogic;
            this.itemLogic = itemLogic;
        }

        [IndicatorFilter()]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                IndicatorResultModel model = new IndicatorResultModel(this.indicatorLogic.Get(id));
                return Ok(model);
            }
            catch(EntityNotExistException en)
            {
                return NotFound(en.Message);
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] IndicatorOnlyModel indicator)
        {
            try
            {
                Indicator result = this.indicatorLogic.Update(id, indicator.ToEntity());
                return Ok(new IndicatorOnlyModel(result));
            }
            catch(EntityNotExistException eex)
            {
                return NotFound(eex.Message);
            }
            catch(InvalidEntityException ie)
            {
                return BadRequest(ie.Message);
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                this.indicatorLogic.Remove(id);
                return NoContent();
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPost("{id}/items")]
        public IActionResult AddItem(Guid id, [FromBody] IndicatorItemPersistModel item)
        {
            try
            {
                IndicatorItem result = this.itemLogic.Create(id, item.ToEntity());
                return Ok(new IndicatorItemGetModel(result));
            }
            catch(InvalidEntityException ie)
            {
                return BadRequest(ie.Message);
            }
            catch(EntityNotExistException en)
            {
                return NotFound(en.Message);
            }
            catch(EntityExistException ee)
            {
                return Conflict(ee.Message);
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }
    }
}
