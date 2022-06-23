using System;
using System.Collections.Generic;

namespace OrgChartEngines.Models.OrgChart
{
    public partial class PositionUser
    {
        public int IdPositionUser { get; set; }
        public int? IdPosition { get; set; }
        public string? IdUser { get; set; }
        public int? Shift { get; set; }

        public virtual OrgChartPosition? IdPositionNavigation { get; set; }
    }
}
