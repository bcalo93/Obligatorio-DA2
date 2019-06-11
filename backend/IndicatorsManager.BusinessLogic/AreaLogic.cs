using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic
{
    public class AreaLogic : ILogic<Area>, IUserAreaLogic
    {
        private IRepository<Area> repository;

        private IRepository<User> userRepo;

        private IAreaQuery query;

        public AreaLogic(IRepository<Area> repository, IRepository<User> userRepo, IAreaQuery query)
        {
            this.repository = repository;
            this.userRepo = userRepo;
            this.query = query;
        }

        public Area Create(Area area)
        {
            ThrowErrorIfInvalid(area);
            CheckForEqualName(area);            
            try 
            {
                this.repository.Add(area);
                this.repository.Save();
            }
            catch(IdExistException) 
            {
                throw new EntityExistException("The area already exist.");
            }
            return area;
        }

        public Area Get(Guid id)
        {
            return this.repository.Get(id);
        }

        public IEnumerable<Area> GetAll()
        {
            return this.repository.GetAll();
        }

        public void Remove(Guid id)
        {
            Area area = repository.Get(id);
            if(area != null)
            {
                repository.Remove(area);
                repository.Save();
            }
        }

        public Area Update(Guid id, Area newArea)
        {
            ThrowErrorIfInvalid(newArea);
            CheckForEqualName(newArea);
            Area area = repository.Get(id);
            ThrowErrorIfInvalid(area);
            area.Update(newArea);
            repository.Update(area);
            repository.Save();
            return area;
        }

        public void AddAreaManager(Guid areaId, Guid userId)
        {
            User user = this.userRepo.Get(userId);
            if (user == null || user.IsDeleted || user.Role != Role.Manager)
            {
                throw new InvalidEntityException("The user doesn't exist or is invalid.");
            }
            Area area = this.repository.Get(areaId);
            if (area == null)
            {
                throw new InvalidEntityException("The area doesn't exist.");
            }
            if(user.UserAreas.Any(u => u.AreaId == area.Id))
            {
                throw new EntityExistException("The user is part of the area.");
            }
            area.AddUser(new UserArea{ User = user });
            repository.Save();
        }

        public void RemoveAreaManager(Guid areaId, Guid userId)
        {
            User user = this.userRepo.Get(userId);
            if (user == null || user.IsDeleted)
            {
                throw new InvalidEntityException("The user doesn't exist.");
            }
            Area area = this.repository.Get(areaId);
            if (area == null)
            {
                throw new InvalidEntityException("The area doesn't exist.");
            }
            UserArea userArea = new UserArea(user, area);
            area.RemoveUser(userArea);
            user.RemoveArea(userArea);
            repository.Update(area);
            userRepo.Update(user);
            repository.Save();
            userRepo.Save();
        }


        private void ThrowErrorIfInvalid(Area area) 
        {
            if (area == null || !area.IsValid()) 
            {
                throw new InvalidEntityException("Area's data is invalid.");
            }
        }

        private void CheckForEqualName(Area area)
        {
            Area areaNameCheck = this.query.GetByName(area.Name);
            if(areaNameCheck != null && areaNameCheck.Id != area.Id)
            {
                throw new EntityExistException("Area name already exist.");
            }
        } 
        
    }

}