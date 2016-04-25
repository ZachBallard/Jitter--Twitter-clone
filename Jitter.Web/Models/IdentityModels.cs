using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jitter.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class JitterUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<JitterUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Handle { get; set; }
        public string PhotoURL { get; set; } = "http://www.herosforheros.com/images/user_profile/noUserPhoto.jpg";

        [ForeignKey("FollowerId")]
        public virtual ICollection<JitterFollowers> Followers { get; set; } = new List<JitterFollowers>();
        [ForeignKey("FollowingId")]
        public virtual ICollection<JitterFollowers> Following { get; set; } = new List<JitterFollowers>();

        public virtual ICollection<Tweak> Tweaks { get; set; } = new List<Tweak>();

    }

    public class Tweak
    {
        public int Id { get; set; }
        public DateTime TweakDate { get; set; } = DateTime.Now;
        [MaxLength(250)]
        public string Body { get; set; }

        public virtual JitterUser JitterUser { get; set; }
    }

    public class JitterFollowers
    {
        public int Id { get; set;}

        [ForeignKey("FollowerId")]
        public virtual JitterUser Follower { get; set; }
        public string FollowerId { get; set; }

        [ForeignKey("FollowingId")]
        public virtual JitterUser Following{ get; set; }
        public string FollowingId { get; set; }

        public bool IsBlocked { get; set; }
    }

    public class DbContext : IdentityDbContext<JitterUser>
    {
        public DbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static DbContext Create()
        {
            return new DbContext();
        }


        public System.Data.Entity.DbSet<Jitter.Web.Models.Tweak> Tweaks { get; set; }
    }
}