﻿using System;
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
    public class UsersController : ControllerBase
    {
        private ILogic<User> userLogic;
        private IIndicatorLogic indicatorLogic;
        private ISessionLogic sessionLogic;

        public UsersController(ILogic<User> userLogic) : base()
        {
            this.userLogic = userLogic;
        }
        
        [ProtectFilter(Role.Admin)]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(this.userLogic.GetAll().Select(u => new UserGetModel(u)));
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            try
            {
                User result = this.userLogic.Get(id);
                if(result == null)
                {
                    return NotFound("El usuario no existe.");
                }
                return Ok(new UserGetModel(result));
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPost]
        public IActionResult Post([FromBody] UserPersistModel value)
        {
            try
            {
                User user = this.userLogic.Create(value.ToEntity());
                return CreatedAtRoute("Get", new { id = user.Id }, new UserGetModel(user));
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
                return StatusCode(503, "El servicio no esta disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] UserPersistModel user)
        {
            try
            {
                User result = this.userLogic.Update(id, user.ToEntity());
                if(result == null)
                {
                    return NotFound(string.Format("El Usuario {0} no existe.",user.Username));
                }
                return Ok(new UserGetModel(result));
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
                return StatusCode(503, "El servicio no esta disponible.");
            }
        }

        [ProtectFilter(Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                this.userLogic.Remove(id);
                return NoContent();
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible.");
            }
        }

        [HttpGet("indicators")]
        public IActionResult GetManagerIndicators()
        {
            try 
            {
                Guid token;
                bool isValid = Guid.TryParse(HttpContext.Request.Headers["Authorization"], out token);
                if(!isValid)
                {
                    return Unauthorized("El token es invalido.");
                }
                
                return Ok(this.indicatorLogic.GetManagerIndicators(token)
                    .Select(i => new IndicatorConfigModel(i)).OrderByDescending(i => i.Position.HasValue).ThenBy(i => i.Position));
            }
            catch(UnauthorizedException ue)
            {
                return Unauthorized(ue.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible");
            }
        }

        [HttpGet("activeindicators")]
        public IActionResult GetManagerActiveIndicators()
        {
            try 
            {
                Guid token = this.ParseAuthorizationHeader();
                return Ok(this.indicatorLogic.GetManagerActiveIndicators(token).Select(i => new ActiveIndicatorModel(i)));
            }
            catch(UnauthorizedException ue) 
            {
                return Unauthorized(ue.Message);
            }
            catch(DataAccessException)
            {
                return StatusCode(503, "El servicio no esta disponible");
            }
        }

        private Guid ParseAuthorizationHeader()
        {
            Guid token;
            bool isValid = Guid.TryParse(HttpContext.Request.Headers["Authorization"], out token);
            if(!isValid)
            {
                throw new UnauthorizedException("El formato del token es invalido.");
            }
            return token;
        }
    }
}
