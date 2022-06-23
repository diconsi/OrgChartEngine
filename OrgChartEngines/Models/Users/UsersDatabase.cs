using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.Users
{
    public partial class UsersDatabase
    {
        public int Id { get; set; }
        public string? IdUser { get; set; }
        public int? IdDatabase { get; set; }

        public virtual DatabaseCatalog? IdDatabaseNavigation { get; set; }
        public virtual AspNetUser? IdUserNavigation { get; set; }
    }
}
