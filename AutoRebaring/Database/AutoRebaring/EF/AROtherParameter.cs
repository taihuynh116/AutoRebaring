namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AROtherParameter")]
    public partial class AROtherParameter
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDMark { get; set; }

        public long IDView3d { get; set; }

        public bool View3dInclude { get; set; }

        public int PartCount { get; set; }

        public virtual ARMark ARMark { get; set; }

        public virtual ARView3d ARView3d { get; set; }
    }
}
