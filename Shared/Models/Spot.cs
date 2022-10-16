using Asset_Track.Shared.StoredProcedures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Asset_Track.Shared.Models
{
    public partial class Spot
    {
        public string? Spot_Up { get; set; }
        public string Spot_Id { get; set; } = null!;
        public string? Spot_Title { get; set; }
        [JsonIgnore, NotMapped]
        public List<spAsset>? spAssets { get; set; } = new List<spAsset>();
        public int Asset_Count { get; set; } = 0;
    }
}
