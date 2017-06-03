namespace Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.ElectronicStoreDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.ElectronicStoreDbContext context)
        {
            CreateContactDetail(context);
        }

        private void CreateApplicationUserSample(ElectronicStoreDbContext context)
        {
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ElectronicStoreDbContext()));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ElectronicStoreDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "tedu",
            //    Email = "tedu.international@gmail.com",
            //    EmailConfirmed = true,
            //    BirthDay = DateTime.Now,
            //    FullName = "Technology Education"
            //};

            //manager.Create(user, "123654$");

            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            //var adminUser = manager.FindByEmail("tedu.international@gmail.com");

            //manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }

        private void CreateContactDetail(ElectronicStoreDbContext context)
        {
            if (context.ContactDetails.Count() == 0)
            {
                var contactDetail = new ContactDetail
                {
                    Name = "Electronic Store",
                    Address = "Số 7 lô 84, Phường Dư Hàng, Quận Lê Chân, TP Hải Phòng",
                    Email = "tedu@gmail.com",
                    Lat = 20.846205,
                    Lng = 106.677471,
                    Phone = "0936613439",
                    Website = "http://tedu.com.vn",
                    Other = "",
                    Status = true

                };
                context.ContactDetails.Add(contactDetail);
                context.SaveChanges();
            }
        }
    }
}
