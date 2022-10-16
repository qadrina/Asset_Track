using Asset_Track.Shared.Models;
using System;

namespace Asset_Track.Shared.StoredProcedures
{
    public class spSpot_Parameter
    {
        public string? sender { get; set; } = String.Empty;
        public string? receiver { get; set; } = String.Empty;
        public string? connectionId { get; set; } = String.Empty;
        public string? message { get; set; } = String.Empty;
        public Int16? increment { get; set; } = 0;
        public spAsset? spAsset { get; set; } = new spAsset();
    }
}
