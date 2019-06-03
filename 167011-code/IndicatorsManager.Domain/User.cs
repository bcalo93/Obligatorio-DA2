using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public virtual List<UserArea> Areas { get; set; }
        public virtual List<UserIndicator> IndicatorConfigurations { get; set; }

        public User()
        {
            Areas = new List<UserArea>();
            IndicatorConfigurations = new  List<UserIndicator>();
        }

        public static User UserUpdateIndicatorInstance(List<UserIndicator> insicatorConfigurations) 
        => new User(){
            IndicatorConfigurations = insicatorConfigurations,
            Role = UserRole.MANAGER,
            Areas = null
        };

        public bool IsAdmin(){ return Role == UserRole.ADMIN; }

        public User Update(User entity)
        {
            if (!String.IsNullOrEmpty(entity.Name))
                Name = entity.Name;
            if (!String.IsNullOrEmpty(entity.LastName))
                LastName = entity.LastName;
            if (!String.IsNullOrEmpty(entity.UserName))
                UserName = entity.UserName;
            if (!String.IsNullOrEmpty(entity.Password))
                Password = entity.Password;
            if (!String.IsNullOrEmpty(entity.Email))
                Email = entity.Email;
            if (entity.Areas != null)
                Areas = entity.Areas;
            if (entity.IndicatorConfigurations != null){
                IndicatorConfigurations = entity.IndicatorConfigurations;
            }
            return this;
        }
    }
}
