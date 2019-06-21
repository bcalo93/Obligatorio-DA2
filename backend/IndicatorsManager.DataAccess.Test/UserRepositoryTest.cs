using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class UserRepositoryTest : BaseTest
    {

        [TestMethod]
        public void CreateUserOkTest()
        {
            IRepository<User> repositoryCreate = new UserRepository(CreateContext(dbName));
            User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            repositoryCreate.Add(create);
            repositoryCreate.Save();

            IRepository<User> repositoryGet = new UserRepository(CreateContext(dbName));
            User result = repositoryGet.Get(create.Id);
            UsersAssertAreEquals(create, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUserNullTest()
        {
            IRepository<User> repository = new UserRepository(Context);
            repository.Add(null);
        }

        [ExpectedException(typeof(IdExistException))]
        [TestMethod]
        public void CreateUserSameIdTest()
        {
            IRepository<User> repository = new UserRepository(Context);
            Guid id = Guid.NewGuid();
            User firstUser = new User
            {
                Id = id,
                Name = "Test1 Name",
                LastName = "Test1 LastName",
                Username = "Username Test1",
                Password = "Password Test1",
                Email = "test1@email.com",
                Role = Role.Manager
            };

            repository.Add(firstUser);

            User secondUser = new User
            {
                Id = id,
                Name = "Test2 Name",
                LastName = "Test2 LastName",
                Username = "Username Test2",
                Password = "Password Test2",
                Email = "test2@email.com",
                Role = Role.Manager
            };

            repository.Add(secondUser);
            
        }

        [TestMethod]
        public void GetAllUserOkTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            IEnumerable<User> data = CreateUserData(20);

            IEnumerable<User> result = repository.GetAll();
            
            Assert.AreEqual(20, result.Count());
            foreach(User expected in data)
            {
                Assert.IsTrue(result.Any(
                    u => u.Name == expected.Name && 
                    u.LastName == expected.LastName &&
                    u.Username == expected.Username &&
                    u.Password == expected.Password &&
                    u.Email == expected.Email &&
                    u.Role == expected.Role &&
                    u.IsDeleted == expected.IsDeleted));
            }
        }

        [TestMethod]
        public void GetUserByIdOkTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            User expected = CreateUserData(20).ElementAt(11);
            
            User result = repository.Get(expected.Id);
            UsersAssertAreEquals(expected, result);
        }

        [TestMethod]
        public void GetUserByIdNotExistTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            CreateUserData(10);
            
            User result = repository.Get(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserByUsernameOkTest()
        {
            UserRepository repository = new UserRepository(CreateContext(dbName));
            User expected = CreateUserData(20).ElementAt(19);

            User result = repository.GetByUsername(expected.Username);
            UsersAssertAreEquals(expected, result); 
        }

        [TestMethod]
        public void GetUserByUsernameIsDeletedTest()
        {
            User deleted = new User
            {
                Name = "Name Deleted",
                LastName = "LastName Deleted",
                Username = "Username Deleted",
                Password = "PasswordDeleted",
                Email = "deleted@email.com",
                Role = Role.Admin,
                IsDeleted = true
            };
            UserRepository repositoryCreate = new UserRepository(CreateContext(dbName));
            repositoryCreate.Add(deleted);
            repositoryCreate.Save();

            UserRepository repositoryGet = new UserRepository(CreateContext(dbName));
            User result = repositoryGet.GetByUsername(deleted.Username);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserByUsernameNotExistTest()
        {
            UserRepository repository = new UserRepository(CreateContext(dbName));
            CreateUserData(20);

            User result = repository.GetByUsername("USERNAMENOTEXIST");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ModifyUserOkTest()
        {
            IRepository<User> repositoryUpdate = new UserRepository(CreateContext(dbName));
            User created = CreateUserData(10).ElementAt(5);
            
            User update = new User {
                Name = "Name Changed",
                LastName = "LastName Changed",
                Username = "Username Changed",
                Password = "PasswordChanged",
                Email = "changed@email.com",
                Role = Role.Manager
            };
            
            created.Update(update);
            repositoryUpdate.Update(created);
            repositoryUpdate.Save();

            IRepository<User> repositoryGet = new UserRepository(CreateContext(dbName));
            Assert.AreEqual(10, repositoryGet.GetAll().Count());
            User result = repositoryGet.Get(created.Id);
            UsersAssertAreEquals(update, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ModifyUserNotExistTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            CreateUserData(25);

            User update = new User {
                Id = Guid.NewGuid(),
                Name = "Name Changed",
                LastName = "LastName Changed",
                Username = "Username Changed",
                Password = "PasswordChanged",
                Email = "changed@email.com",
                Role = Role.Manager
            };

            repository.Update(update);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ModifyUserNullTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            repository.Update(null);
        }

        [TestMethod]
        public void DeleteUserOkTest()
        {
            IRepository<User> repositoryDelete = new UserRepository(CreateContext(dbName));
            User toDelete = CreateUserData(30).ElementAt(20);

            repositoryDelete.Remove(toDelete);
            repositoryDelete.Save();

            IRepository<User> repositoryGet = new UserRepository(CreateContext(dbName));
            Assert.AreEqual(29, repositoryGet.GetAll().Count());
            User result = repositoryGet.Get(toDelete.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void DeleteUserNotExistTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            User delete = new User {
                Id = Guid.NewGuid(),
                Name = "Name Changed",
                LastName = "LastName Changed",
                Username = "Username Changed",
                Password = "PasswordChanged",
                Email = "changed@email.com",
                Role = Role.Manager
            };

            repository.Remove(delete);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteUserNullTest()
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            repository.Remove(null);
        }

        private void UsersAssertAreEquals(User expected, User actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Role, actual.Role);
            Assert.AreEqual(expected.IsDeleted, actual.IsDeleted);
        }

        private IEnumerable<User> CreateUserData(int amount)
        {
            IRepository<User> repository = new UserRepository(CreateContext(dbName));
            List<User> result = new List<User>();
            for(int i = 0; i < amount; i++)
            {
                User create = new User
                {
                    Name = "Name " + i,
                    LastName = "LastName " + i,
                    Username = "Username " + i,
                    Password = "Password " + i,
                    Email = "test" + i + "@email.com",
                    Role = i % 2 == 0 ? Role.Manager : Role.Admin
                };
                repository.Add(create);
                result.Add(create);
            }
            repository.Save();
            return result;
        }
    }
}