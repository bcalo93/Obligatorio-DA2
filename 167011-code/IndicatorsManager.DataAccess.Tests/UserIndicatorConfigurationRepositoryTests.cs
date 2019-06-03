
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IndicatorsManager.DataAccess.Tests
{
    [TestClass]
    public class UserIndicatorConfigurationRepositoryTests 
    {
        private const string USER_NAME = "Ramiro Gonzalez";
        private const string USER_USERNAME = "rgonzalez";
        private const string USER_PASSWORD = "rgonzalez1234";
        private const string USER_EMAIL = "rgonzalez@mail.com";

        private const string INDICATOR_NAME_1 = "Indicator Test 1";
        private const string INDICATOR_NAME_2 = "Indicator Test 2";
        private const string INDICATOR_NAME_3 = "Indicator Test 3";
        private User CreateValidUser(string name) => new User()
        {
            Name = name,
            LastName = USER_NAME,
            UserName = USER_USERNAME,
            Password = USER_PASSWORD,
            Email = USER_EMAIL,
            Role = UserRole.MANAGER,
        };
        public List<User> CreateValidUsers(Context context)
        {
             List<User> result = new List<User>(){
                CreateValidUser(USER_NAME),
                CreateValidUser(USER_USERNAME),
                CreateValidUser(USER_EMAIL),
            };

            var userRepository = new UserRepository(context);

            foreach (var user in result)
            {
                userRepository.Add(user);
            }
            return (List<User>)userRepository.GetAll();   

        }
        private Indicator CreateValidIndicator(string name) => new Indicator() { Name = name };
        
        private List<Indicator> CreateValidIndicators(Context context)
        {
            List<Indicator> result = new List<Indicator>(){
                CreateValidIndicator(INDICATOR_NAME_1),
                CreateValidIndicator(INDICATOR_NAME_2),
                CreateValidIndicator(INDICATOR_NAME_3),
            };
            var indicatorRepository =  new IndicatorRepository(context);

            foreach (var indicator in result)
            {
                indicatorRepository.Add(indicator);
            }
            return (List<Indicator>)indicatorRepository.GetAll(); 
        }

        private UserIndicator CreateValidConfiguration_Visible(Indicator indicator) => new UserIndicator()
        {
            IndicatorId = indicator.Id,
            IsVisible = true
        };

        private UserIndicator CreateValidConfiguration_NotVisible(Indicator indicator) => new UserIndicator()
        {
            IndicatorId = indicator.Id,
            IsVisible = false
        };


        public void AddConfigurations(List<User> users, List<Indicator> indicators, Context context)
        {
            List<UserIndicator> configurations_notVisible = new List<UserIndicator>(); 
            foreach (var indicator in indicators)
            {
                configurations_notVisible.Add(CreateValidConfiguration_NotVisible(indicator));
            }

            List<UserIndicator> configurations_visible = new List<UserIndicator>(); 
            foreach (var indicator in indicators)
            {
                configurations_visible.Add(CreateValidConfiguration_Visible(indicator));
            }

            List<UserIndicator> config_one_indicatorNotVisible = new List<UserIndicator>()
            {
                CreateValidConfiguration_Visible(indicators.Find(x=>x.Name == INDICATOR_NAME_1)),
                CreateValidConfiguration_NotVisible(indicators.Find(x=>x.Name == INDICATOR_NAME_2))
            };

            users.Find(x=>x.Name == USER_NAME).IndicatorConfigurations.AddRange(configurations_notVisible);
            users.Find(x=>x.Name == USER_USERNAME).IndicatorConfigurations.AddRange(configurations_visible);
            users.Find(x=>x.Name == USER_EMAIL).IndicatorConfigurations.AddRange(config_one_indicatorNotVisible);

            var userRepository =  new UserRepository(context);

            foreach (var user in users)
            {
                userRepository.Update(user.Id, user);
            }
        }

        [TestMethod]
        public void TestGetHiddenIndicators()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new Context(options);
            context.Database.EnsureCreated();

 
            List<User> users = CreateValidUsers(context);
            List<Indicator> indicators = CreateValidIndicators(context);
            
            AddConfigurations(users, indicators, context);

            var configurationRepository = new UserIndicatorConfigurationRepository(context);

            var result = configurationRepository.GetTopHiddenIndicators();

            int firstPosition = 0;
            Assert.IsTrue(((List<Indicator>)result).FindIndex(x=>x.Name == INDICATOR_NAME_2) == firstPosition);


        }
    }
}