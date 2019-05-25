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
    public class AreasController : ControllerBase
    {
        private ILogic<Area> areaLogic;
        private IIndicatorLogic indicatorLogic;
        private IUserAreaLogic uaLogic;

        public AreasController(ILogic<Area> areaLogic, IUserAreaLogic uaLogic, IIndicatorLogic indicatorLogic) : base()
        {
            this.areaLogic = areaLogic;
            this.uaLogic = uaLogic;
            this.indicatorLogic = indicatorLogic;
        }
        
        [ProtectFilter(Role.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(this.areaLogic.GetAll().Select(a => new AreaModel(a)));
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                Area result = this.areaLogic.Get(id);
                if(result == null)
                {
                    return NotFound("El área no existe.");
                }
                return Ok(new AreaModel(result));
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPost]
        public IActionResult Post([FromBody] AreaModel value)
        {
            try
            {
                Area area = this.areaLogic.Create(value.ToEntity());
                return CreatedAtRoute("Get", new { id = area.Id }, new AreaModel(area));
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
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] AreaModel area)
        {
            try
            {
                Area result = this.areaLogic.Update(id, area.ToEntity());
                if(result == null)
                {
                    return NotFound(string.Format("El área no existe."));
                }
                return Ok(new AreaModel(result));
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
                return StatusCode(503, "El servicio no está disponible.");
            }
        }        

        [ProtectFilter(Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                this.areaLogic.Remove(id);
                return NoContent();
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPost("{id}/userarea")]
        public IActionResult Post(Guid id, [FromBody] Guid userId)
        {
            try
            {
                this.uaLogic.AddAreaManager(id, userId);
                return Ok();
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
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpDelete("{areaId}/userarea/{userId}")]
        public IActionResult Delete(Guid areaId, Guid userId)
        {
            try
            {
                uaLogic.RemoveAreaManager(areaId, userId);
                return NoContent();
            }
            catch(InvalidEntityException ie)
            {
                return BadRequest(ie.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPost("{id}/indicators")]
        public IActionResult AddIndicator(Guid id, [FromBody] IndicatorCreateModel model)
        {
            try
            {
                Indicator result = this.indicatorLogic.Create(id, model.ToEntity());
                return Ok(new IndicatorGetModel(result));
            }
            catch(ComponentException ce)
            {
                return BadRequest(ce.Message);
            }
            catch(InvalidEntityException ie)
            {
                return BadRequest(ie.Message);
            }
            catch(EntityNotExistException en)
            {
                return NotFound(en.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("{id}/indicators")]
        public IActionResult GetIndicatorsPerArea(Guid id)
        {
            try
            {
                return Ok(this.indicatorLogic.GetAll(id).Select(i => new IndicatorGetModel(i))); 
            }
            catch(EntityNotExistException ee)
            {
                return BadRequest(ee.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no está disponible.");
            }
        }
        
    }
}