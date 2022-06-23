using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.OrgChart
{
    public partial class RolesDepartment
    {
        public int IdRolDepartment { get; set; }
        public string IdRol { get; set; } = null!;
        public int IdDepartment { get; set; }

        public virtual Departament IdDepartmentNavigation { get; set; } = null!;
    }
}
