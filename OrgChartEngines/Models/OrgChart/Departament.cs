using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.OrgChart
{
    public partial class Departament
    {
        public Departament()
        {
            RolesDepartments = new HashSet<RolesDepartment>();
        }

        public int Id { get; set; }
        public string? DepartamentName { get; set; }

        public virtual ICollection<RolesDepartment> RolesDepartments { get; set; }
    }
}
