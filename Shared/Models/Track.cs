using System;
using System.Collections.Generic;

namespace Asset_Track.Shared.Models
{
    public partial class Track
    {
        public string? Asset_Id { get; set; }
        public string? Spot_Id { get; set; }
        public DateTime? Time_Stamp { get; set; }
    }
}
