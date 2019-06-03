using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class UserModelOut : Model<User, UserModelOut>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public virtual List<IndicatorConfigurationModel> IndicatorConfigurations { get; set; }

        public override User ToEntity() => new User()
        {
            Id = this.Id,
            Name = this.Name,
            LastName = this.LastName,
            UserName = this.UserName,
            Email = this.Email,
            Role = this.Role
        };

        protected override UserModelOut SetModel(User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            LastName = entity.LastName;
            UserName = entity.UserName;
            Email = entity.Email;
            Role = entity.Role;
            IndicatorConfigurations = entity.IndicatorConfigurations.ConvertAll( m=> new IndicatorConfigurationModel(m));
            return this;
        }
    }
}