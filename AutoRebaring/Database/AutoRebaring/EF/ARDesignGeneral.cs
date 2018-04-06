namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARDesignGeneral")]
    public partial class ARDesignGeneral
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARDesignGeneral()
        {
            ARStandardStartZs = new HashSet<ARStandardStartZ>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDMark { get; set; }

        public long IDStartLevel { get; set; }

        public long IDEndLevel { get; set; }

        public long IDStandardEndType { get; set; }

        public virtual ARLevel ARLevel { get; set; }

        public virtual ARLevel ARLevel1 { get; set; }

        public virtual ARMark ARMark { get; set; }

        public virtual ARStandardEndType ARStandardEndType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStandardStartZ> ARStandardStartZs { get; set; }
    }
}
