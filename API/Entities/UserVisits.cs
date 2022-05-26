using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserVisits
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser VisitedUser { get; set; }
        public int VisitedUserId { get; set; }
        public DateTime LastVisit { get; set; } = DateTime.UtcNow;
    }
}