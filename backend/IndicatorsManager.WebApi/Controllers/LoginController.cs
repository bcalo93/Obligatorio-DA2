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
        public IActionResult Login([FromBody] LoginModel model) {
            var token = session.CreateToken(model.Username, model.Password);
            if (token == null) 
            {
                return BadRequest("Usuario/contraseña inválidos");
            }
            return Ok(token.Token);
        }
    }

}