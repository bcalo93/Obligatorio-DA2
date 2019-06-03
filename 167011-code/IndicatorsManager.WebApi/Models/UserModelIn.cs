using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class UserModelIn : Model<User, UserModelIn>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public override User ToEntity() => new User()
        {
            Name = this.Name,
            LastName = this.LastName,
            UserName = this.UserName,
            Password = this.Password,
            Email = this.Email,
        };

        protected override UserModelIn SetModel(User entity)
        {
            Name = entity.Name;
            LastName = entity.LastName;
            UserName = entity.UserName;
            Password = entity.Password;
            Email = entity.Email;
            return this;
        }
    }
}
