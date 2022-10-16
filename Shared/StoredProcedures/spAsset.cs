using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Track.Shared.StoredProcedures
{
    public class spAsset
    {
        public string? Asset_Id { get; set; }
        public string? Asset_Title { get; set; }
        public string? Spot_Id { get; set; }
        public string? Spot_Ex { get; set; }
        public DateTime Time_Stamp { get; set; }
    }
}
