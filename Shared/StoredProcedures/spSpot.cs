using Asset_Track.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Asset_Track.Shared.StoredProcedures
{
    public class spSpot
    {
        public string? Spot_Id { get; set; }
        public string? Spot_Title { get; set; }
        [JsonIgnore, NotMapped]
        public List<spAsset>? xpAssets { get; set; } = new List<spAsset>();
        public int Asset_Count { get; set; } = 0;
    }
}
