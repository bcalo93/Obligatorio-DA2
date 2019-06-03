using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IndicatorsManager.BusinessLogic.Exceptions;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic
{
    public class UserConfigurationLogic : IConfigurationLogic
    {
        private ILogic<User> userService;
        private ILogic<Indicator> indicatorService;
        private IConfigurationRepository configRepository;

        public UserConfigurationLogic(
                ILogic<User> userService,
                ILogic<Indicator> indicatorService, 
                IConfigurationRepository configRepository)
        {
            this.userService = userService;
            this.indicatorService = indicatorService;
            this.configRepository = configRepository;
        }

        public User AddConfiguration(Guid userId, List<UserIndicator> configurations)
        {
            ThrowErrorIfUserNotExists(userId);
            CheckIfAnyIndicatorDoesNotExist(configurations);
            User userModelInWithConfig = User.UserUpdateIndicatorInstance(configurations);
            User userUpdated = userService.Update(userId, userModelInWithConfig);
            return userUpdated;
        }

        private void CheckIfAnyIndicatorDoesNotExist(List<UserIndicator> configurations)
        {
            foreach (var configItem in configurations)
            {
                ThrowErrorIfIndicatorNotExists(configItem.IndicatorId);
            }
        }

        public User UpdateConfiguration(Guid userId, UserIndicator newConfig)
        {
            ThrowErrorIfUserNotExists(userId);
            ThrowErrorIfIndicatorConfigurationNotExists(userId,newConfig.IndicatorId);
            User userToUpdate = userService.Get(userId);
            UserIndicator configTpUpdate = GetIndicatorConfiguration(userToUpdate,newConfig.IndicatorId);
            configTpUpdate.Update(newConfig);
            User updatedUser = userService.Update(userId, userToUpdate);
            return userToUpdate;
        }


        private UserIndicator GetIndicatorConfiguration(User user, Guid indicatorId)
        {
            return user.IndicatorConfigurations.Find(x => x.UserId == user.Id && x.IndicatorId == indicatorId);
        }
        private void ThrowErrorIfUserNotExists(Guid userId)
        {
            bool existUserId = userService.ExistCondition(x => x.Id == userId);
            if(!existUserId) 
                throw new ElementNotFoundException(CustomExceptionMessages.USER_NON_EXISTANT);
        }

        private void ThrowErrorIfIndicatorNotExists(Guid indicatorId)
        {
            bool existIndicatorId = indicatorService.ExistCondition(x => x.Id == indicatorId);
            if(!existIndicatorId) 
                throw new ElementNotFoundException(CustomExceptionMessages.INDICATOR_NON_EXISTANT);
        }

        private void ThrowErrorIfIndicatorConfigurationNotExists(Guid userId, Guid indicatorId)
        {
            bool existUserIndicatorId = 
                configRepository.ExistCondition(x => x.UserId == userId && x.Indicator.Id == indicatorId);
            if(!existUserIndicatorId) 
                throw new ElementNotFoundException(CustomExceptionMessages.CONFIGURATION_NON_EXISTANT);
        }

        private void ThrowErrorIfIndicatorConfigurationNotExists(UserIndicator config)
        {
            if(config == null) throw new ElementNotFoundException(CustomExceptionMessages.CONFIGURATION_NON_EXISTANT);
        }

        public List<Indicator> GetTopHiddenIndicators(int limit = 10)
        {
            return configRepository.GetTopHiddenIndicators(limit);
        }
    }
}
