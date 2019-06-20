using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class UserPersistModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public UserPersistModel() { }

        public UserPersistModel(User user)
        {
            this.Name = user.Name;
            this.LastName = user.LastName;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Role = user.Role;

        }

        public User ToEntity() => new User
        {
            Name = this.Name,
            LastName = this.LastName,
            Username = this.Username,
            Password = this.Password,
            Email = this.Email,
            Role = this.Role
        };

    }
}