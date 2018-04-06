namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStandardStartZ")]
    public partial class ARStandardStartZ
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDStandardStartZType { get; set; }

        public long IDStandardDesignGeneral { get; set; }

        public double Z1 { get; set; }

        public double Z2 { get; set; }

        public virtual ARDesignGeneral ARDesignGeneral { get; set; }

        public virtual ARStandardStartZType ARStandardStartZType { get; set; }
    }
}
