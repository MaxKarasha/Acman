using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Integration.OData;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcMan.Server.Test
{
    public class BaseTest
    {
        private DbContextOptions<AcManContext> _options;
        public DbContextOptions<AcManContext> Options
        {
            get
            {
                if (_options is null)
                {
                    _options = new DbContextOptionsBuilder<AcManContext>()
                        .UseInMemoryDatabase(databaseName: "AcmanTest")
                        .Options;
                }
                return _options;
            }
        }
        public DbContextOptions<AcManContext> OptionsRandom
        {
            get
            {
                return new DbContextOptionsBuilder<AcManContext>()
                        .UseInMemoryDatabase(databaseName: "AcmanTest" + Guid.NewGuid().ToString())
                        .Options;
            }
        }

        public DbContextOptions<AcManContext> MsSql2016Options {
            get {
                return new DbContextOptionsBuilder<AcManContext>()
                        .UseSqlServer(
                            "Data Source=Service-MS\\MSSQL2016; Initial Catalog=Acman; User ID=Acman; Password=Acman;", 
                            providerOptions => providerOptions.CommandTimeout(60)
                        ).Options;
            }
        }

        public ODBase MsSqlMKODBase {
            get {
                string url = "http://m-karasha/Bandle_7120_Demo_D";
                string login = "Supervisor";
                string password = "Supervisor";
                return new ODBase(url, login, password);
            }
        }

        public ODBase DemoTS_ODBase {
            get {
                string url = "https://031533-crm-bundle.bpmonline.com";
                string login = "Karasha Maksym";
                string password = "123456";
                return new ODBase(url, login, password);
            }
        }

        public BaseTest ()
        {
            var userId = Guid.NewGuid();
            using (var context = new AcManContext(Options))
            {
                var userRepository = new UserRepository(context);
                var user = new User { Id = userId, Name = "Test current user " + userId.ToString() };
                userRepository.Add(user);
                CurrentConnection.CurrentUser = user;
            }
        }        
    }
}
