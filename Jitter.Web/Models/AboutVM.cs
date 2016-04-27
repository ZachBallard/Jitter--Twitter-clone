using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jitter.Web.Models
{
    public class AboutVM
    {
        public ICollection<TweakVM> Tweaks { get; set; } = new List<TweakVM>();
        public string PhotoURL { get; set; }
        public int TweaksCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public IDictionary<string, string> Following { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> Followers { get; set; } = new Dictionary<string, string>();
        public bool CanDelete { get; set; }
        public string UserId { get; set; }
    }

    public class TweakVM
    {
        public DateTime TweakDate { get; set; }
        public string UserHandle { get; set; }
        public string Body { get; set; }
        public int TweakId { get; set; }
    }
}
