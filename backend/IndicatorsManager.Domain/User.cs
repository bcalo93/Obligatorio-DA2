using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IndicatorsManager.Domain
{
    public enum Role
    {
        Admin, Manager
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<UserArea> UserAreas { get; set; }
        public virtual List<UserIndicator> UserIndicators { get; set; }

        public User()
        {
            this.UserAreas = new List<UserArea>();
            this.UserIndicators = new List<UserIndicator>();
        }

        public bool IsValid() 
        {
            bool validStrings = !String.IsNullOrEmpty(this.Name) && this.Name.Trim() != "" && !String.IsNullOrEmpty(this.LastName) && this.LastName.Trim() != "" &&
                !String.IsNullOrEmpty(this.Username) && this.Username.Trim() != "" && !String.IsNullOrEmpty(this.Password) && this.Password.Trim() != "" &&
                !String.IsNullOrEmpty(this.Email) && this.Email.Trim() != "";
            if(!validStrings)
            {
                return false;
            }

            Regex regExp = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            Match match = regExp.Match(this.Email);
            return match.Success;
        }

        public User Update(User user)
        {
            this.Name = user.Name;
            this.LastName = user.LastName;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Role = user.Role;
            return this;
        }

        public void AddArea(UserArea userArea)
        {
            this.UserAreas.Add(userArea);
        }

        public void RemoveArea(UserArea userArea)
        {
            UserArea ua = UserAreas.Where(a => a.AreaId == userArea.AreaId && a.UserId == userArea.UserId).FirstOrDefault();
            this.UserAreas.Remove(ua);
        }

        public void AddIndicator(UserIndicator userIndicator)
        {
            this.UserIndicators.Add(userIndicator);
        }

        public void RemoveIndicator(UserIndicator userIndicator)
        {
            UserIndicator ui = UserIndicators
                    .Where(a => a.IndicatorId == userIndicator.IndicatorId && a.UserId == userIndicator.UserId).FirstOrDefault();
            this.UserIndicators.Remove(ui);
        }


    }
}
