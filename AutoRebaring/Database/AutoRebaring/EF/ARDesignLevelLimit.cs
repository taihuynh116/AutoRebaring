namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARDesignLevelLimit")]
    public partial class ARDesignLevelLimit
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDMark { get; set; }

        public int Limit { get; set; }

        public virtual ARMark ARMark { get; set; }
    }
}
