namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARElementTypeProject")]
    public partial class ARElementTypeProject
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDMark { get; set; }

        public long IDElementType { get; set; }

        public virtual ARElementType ARElementType { get; set; }

        public virtual ARMark ARMark { get; set; }
    }
}
