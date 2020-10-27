using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RealEstate.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
           
            userIdentity.AddClaim(new Claim("FirstCustomName", this.Name));
            userIdentity.AddClaim(new Claim("LastCustomName", this.LastName));
            return userIdentity;
            
    }
        //public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public bool ISClient { get; set; }
        public bool IsBlocked { get; set; }
        public string UserRole { get; set; }
        public string AgentEmail { get; set; }
        public string ColorCode { get; set; }
        public bool? EventCalendar { get; set; }
        public bool? StatusTimeline { get; set; }
        public bool? SettlementDate { get; set; }
        public bool? DocumentEmail { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationRole> Roles { get; set; }
        public ApplicationDbContext()
            : base("EFDBContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()

        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);

            //Defining the keys and relations
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        }

        public bool RoleExists(ApplicationRoleManager roleManager, string name)
        {
            return roleManager.RoleExists(name);
        }

        public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        {
            var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        {
            var idResult = _userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        {
            var user = userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.UserRoles);
            foreach (ApplicationUserRole role in currentRoles)
            {
                userManager.RemoveFromRole(userId, role.Role.Name);
            }
        }

        public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        {
            userManager.RemoveFromRole(userId, roleName);
        }

        public void DeleteRole(ApplicationDbContext context, ApplicationUserManager userManager, string roleId)
        {
            var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
            var role = context.Roles.Find(roleId);

            foreach (var user in roleUsers)
            {
                this.RemoveFromRole(userManager, user.Id, role.Name);
            }
            context.Roles.Remove(role);
            context.SaveChanges();
        }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    if (modelBuilder == null)
        //    {
        //        throw new ArgumentNullException("ModelBuilder is NULL");
        //    }

        //    base.OnModelCreating(modelBuilder);

        //    //Defining the keys and relations
        //    modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        //    modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
        //    modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
        //    modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        //}

        //public bool RoleExists(ApplicationRoleManager roleManager, string name)
        //{
        //    return roleManager.RoleExists(name);
        //}

        //public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        //{
        //    var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, description));
        //    return idResult.Succeeded;
        //}

        //public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        //{
        //    var idResult = _userManager.AddToRole(userId, roleName);
        //    return idResult.Succeeded;
        //}

        //public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        //{
        //    var user = userManager.FindById(userId);
        //    var currentRoles = new List<IdentityUserRole>();

        //    currentRoles.AddRange(user.UserRoles);
        //    foreach (ApplicationUserRole role in currentRoles)
        //    {
        //        userManager.RemoveFromRole(userId, role.Role.Name);
        //    }
        //}

        //public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        //{
        //    userManager.RemoveFromRole(userId, roleName);
        //}

        //public void DeleteRole(ApplicationDbContext context, ApplicationUserManager userManager, string roleId)
        //{
        //    var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
        //    var role = context.Roles.Find(roleId);

        //    foreach (var user in roleUsers)
        //    {
        //        this.RemoveFromRole(userManager, user.Id, role.Name);
        //    }
        //    context.Roles.Remove(role);
        //    context.SaveChanges();
        //}
    }
}