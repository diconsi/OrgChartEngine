using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.OrgChart
{
    public partial class OrgChartPosition
    {
        public OrgChartPosition()
        {
            PositionUsers = new HashSet<PositionUser>();
        }

        public int IdPosition { get; set; }
        public string? Title { get; set; }
        public string? IdRol { get; set; }
        public string? IdUser { get; set; }
        public int? IdDepartment { get; set; }
        public int? PositionLevel { get; set; }
        public int? SuperiorPosition { get; set; }
        public int? IdLine { get; set; }
        public int? IdLineSub { get; set; }
        public string? Assemblies { get; set; }

        public virtual ICollection<PositionUser> PositionUsers { get; set; }
    }
}
