using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.Users
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            UsersDatabases = new HashSet<UsersDatabase>();
            Roles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool UserActive { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; } = null!;
        public string? ConcurrencyStamp { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? NormalizedUserName { get; set; }

        public virtual ICollection<UsersDatabase> UsersDatabases { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
