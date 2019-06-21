using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndicatorsManager.WebApi.Controllers
{

    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private ISessionLogic session;
        

        public LoginController(ISessionLogic session) : base()
        {
            this.session = session;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model) 
        {
            try
            {
                var authenticationToken = session.CreateToken(model.Username, model.Password);
                if (authenticationToken == null) 
                {
                    return BadRequest("User/password invalid.");
                }
                return Ok(new LoginModelOut(authenticationToken));
            }
            catch(UnauthorizedException ue)
            {
                return Unauthorized(ue.Message);
            }
            catch(DataAccessException de)
            {
                return StatusCode(503, de.Message);
            }
        }
    }

}