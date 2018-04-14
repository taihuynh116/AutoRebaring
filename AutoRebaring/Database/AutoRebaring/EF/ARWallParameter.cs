namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARWallParameter")]
    public partial class ARWallParameter
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDMark { get; set; }

        public double EdgeWidth { get; set; }

        public bool EdgeWidthInclude { get; set; }

        public double EdgeRatio { get; set; }

        public bool EdgeRatioInclude { get; set; }

        public virtual ARMark ARMark { get; set; }
    }
}
