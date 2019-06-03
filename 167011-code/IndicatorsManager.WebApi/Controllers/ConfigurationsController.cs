using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Models;
using IndicatorsManager.WebApi.Filters;
using IndicatorsManager.BusinessLogic;
using IndicatorsManager.BusinessLogic.Exceptions;

namespace IndicatorsManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private const String GET_USER_ROUTE = "GetUser";
        private IConfigurationLogic userConfigurationService;

        public ConfigurationsController(IConfigurationLogic userConfigurationService)
        {
            this.userConfigurationService = userConfigurationService;
        }


        [HttpPost("{userId}")]
        public IActionResult AddUserConfigurations(Guid userId, [FromBody]List<IndicatorConfigurationModel> configurationsModelIn )
        {
            try
            {
                List<UserIndicator> configurationsToAdd = IndicatorConfigurationModel.ToEntity(configurationsModelIn).ToList();
                User user = userConfigurationService.AddConfiguration(userId, configurationsToAdd);
                return CreatedAtRoute(GET_USER_ROUTE, new { id = user.Id }, UserModelOut.ToModel(user));           
            }
            catch(ElementNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{userId}")]
        public IActionResult UpdateUserConfigurations(Guid userId, [FromBody] IndicatorConfigurationModel configurationModelIn )
        {
            try
            {
                UserIndicator configurationsToUpdate = IndicatorConfigurationModel.ToEntity(configurationModelIn);
                User user = userConfigurationService.UpdateConfiguration(userId, configurationsToUpdate);
                return Ok(UserModelOut.ToModel(user));           
            }
            catch(ElementNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch(InvalidElementException e) 
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("indicators/hidden")]
        public IActionResult GetTopHiddenIndicators([FromQuery]int limit)
        {
            return Ok(IndicatorModel.ToModel(userConfigurationService.GetTopHiddenIndicators(limit)));           
        }
        
    }
}