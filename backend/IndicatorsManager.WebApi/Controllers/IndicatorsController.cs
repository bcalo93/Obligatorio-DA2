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
        
        [HttpGet]
        public IActionResult Get()
        {
            try 
            {
                Guid token;
                bool isValid = Guid.TryParse(HttpContext.Request.Headers["Authorization"], out token);
                if(!isValid)
                {
                    return Unauthorized("El token es invalido");
                }
                User user = this.sessionLogic.GetUser(token);
                if(user == null || user.Role != Role.Manager)
                {
                    return NotFound("El usuario no existe");
                }
                
                return Ok(this.indicatorLogic.GetManagerIndicators(user.Id)
                    .Select(i => new IndicatorConfigModel(i, user.Id)).OrderByDescending(i => i.Position.HasValue).ThenBy(i => i.Position));
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible");
            }
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
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
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
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
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
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
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
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
            }
        }

        [ProtectFilter(Role.Manager)]
        [HttpPost("{id}/userindicator")]
        public IActionResult Post(Guid id)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            Guid guidToken = Guid.Parse(token);
            User user = sessionLogic.GetUser(guidToken);
            try
            {
                this.indicatorLogic.AddUserIndicator(id, user.Id);
                return Ok();
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

        [ProtectFilter(Role.Manager)]
        [HttpDelete("{indicatorId}/userindicator/{userId}")]
        public IActionResult Delete(Guid indicatorId, Guid userId)
        {
            try
            {
                this.indicatorLogic.RemoveUserIndicator(indicatorId, userId);
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
    }
}
