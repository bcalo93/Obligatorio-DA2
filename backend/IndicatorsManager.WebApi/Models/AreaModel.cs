using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class AreaModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DataSource { get; set; }

        public List<UserGetModel> Users { get; set; }

        public AreaModel() { }

        public AreaModel(Area area)
        {
            this.Id = area.Id;
            this.Name = area.Name;
            this.DataSource = area.DataSource;
            this.Users = GetUserList(area);
        }

        public Area ToEntity() => new Area
        {
            Name = this.Name,
            DataSource = this.DataSource
        };

        public List<UserGetModel> GetUserList(Area area)
        {
            List<UserGetModel> list = new List<UserGetModel>();
            foreach (var item in area.UserAreas)
            {
                list.Add(new UserGetModel(item.User));
            }
            return list;
        }

    }
}