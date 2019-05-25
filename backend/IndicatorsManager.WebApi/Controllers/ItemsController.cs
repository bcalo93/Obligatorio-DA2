using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ItemsController : ControllerBase
    {
        private IIndicatorItemLogic itemLogic;

        public ItemsController(IIndicatorItemLogic itemLogic) : base()
        {
            this.itemLogic = itemLogic;
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                IndicatorItemResultModel model = new IndicatorItemResultModel(this.itemLogic.Get(id));
                return Ok(model);
            }
            catch(EntityNotExistException en)
            {
                return NotFound(en.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Remove(Guid id)
        {
            try
            {
                this.itemLogic.Remove(id);
                return NoContent();
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "E; servicio no esta disponible");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] IndicatorItemPersistModel model)
        {
            try
            {
                IndicatorItem result = this.itemLogic.Update(id, model.ToEntity());
                return Ok(new IndicatorItemGetModel(result));
            }
            catch(EntityNotExistException eex)
            {
                return NotFound(eex.Message);
            }
            catch(InvalidEntityException ie)
            {
                return BadRequest(ie.Message);
            }
            catch(EntityExistException ee)
            {
                return Conflict(ee.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "E; servicio no esta disponible");
            }
        }
    }
}