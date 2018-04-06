namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ARRebarVerticalParameter
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDRebarType { get; set; }

        public long IDProject { get; set; }

        public double BottomOffset { get; set; }

        public double BottomOffsetRatio { get; set; }

        public double TopOffset { get; set; }

        public double TopOffsetRatio { get; set; }

        public bool OffsetInclude { get; set; }

        public bool OffsetRatioInclude { get; set; }

        public bool IsInsideBeam { get; set; }

        public virtual ARProject ARProject { get; set; }

        public virtual ARRebarType ARRebarType { get; set; }
    }
}
