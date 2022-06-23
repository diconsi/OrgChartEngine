using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.Users
{
    public partial class DatabaseCatalog
    {
        public DatabaseCatalog()
        {
            UsersDatabases = new HashSet<UsersDatabase>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<UsersDatabase> UsersDatabases { get; set; }
    }
}
