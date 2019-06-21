using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class UserGetModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public UserGetModel() { }

        public UserGetModel(User user)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.LastName = user.LastName;
            this.Username = user.Username;
            this.Email = user.Email;
            this.Role = user.Role;

        }

        public User ToEntity() => new User
        {
            Name = this.Name,
            LastName = this.LastName,
            Username = this.Username,
            Email = this.Email,
            Role = this.Role
        };

    }
}