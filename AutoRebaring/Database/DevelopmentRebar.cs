namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DevelopmentRebar")]
    public partial class DevelopmentRebar
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        public double BottomOffset { get; set; }

        public double TopOffset { get; set; }

        public bool OffsetInclude { get; set; }

        public double BottomOffsetRatio { get; set; }

        public double TopOffsetRatio { get; set; }

        public bool OffsetRatioInclude { get; set; }

        public double BottomStirrupOffset { get; set; }

        public double TopStirrupOffset { get; set; }

        public bool StirrupOffsetInclude { get; set; }

        public double BottomStirrupOffsetRatio { get; set; }

        public double TopStirrupOffsetRatio { get; set; }

        public bool StirrupOffsetRatioInclude { get; set; }

        public bool IsInsideBeam { get; set; }

        public bool IsStirrupInsideBeam { get; set; }
    }
}
