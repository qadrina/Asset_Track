using System;
using System.Collections.Generic;

namespace Asset_Track.Shared.Models
{
    public partial class Asset
    {
        public string Asset_Id { get; set; } = null!;
        public string? Asset_Title { get; set; }
        public string? Spot_Id { get; set; }
        public string? Spot_Ex { get; set; }
        public DateTime? Time_Stamp { get; set; }
    }
}
