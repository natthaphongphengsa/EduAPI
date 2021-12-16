using EduAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace EduAPI.Data.DataInitialize
{
    public class DataInitializer
    {
        public static ApplicationDbContext _dbContext;
        public static void SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            dbContext.Database.Migrate();
            SeedUsers();

        }
        public static void SeedUsers()
        {
            //Hämta fake data från https://fakestoreapi.com/Users/
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var json = webClient.DownloadString(@"https://fakestoreapi.com/Users/");
            List<FakeUser> fakeUsers = JsonConvert.DeserializeObject<List<FakeUser>>(json);

            foreach (var user in fakeUsers)
            {
                if (!_dbContext.Users.Any(u => u.Email == user.email))
                {
                    _dbContext.Users.Add(new User
                    {
                        Id = $"{Guid.NewGuid()}{user.id}",
                        FirstName = user.name.firstname,
                        LastName = user.name.lastname,
                        Email = user.email,
                    });
                    _dbContext.SaveChanges();
                }
            }
        }
        public static void SeedCourse()
        {

        }
    }
}
