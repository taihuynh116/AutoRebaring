namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStirrupFamilyType")]
    public partial class ARStirrupFamilyType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARStirrupFamilyType()
        {
            ARStirrupDesigns = new HashSet<ARStirrupDesign>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        public long IDRebarDesignType { get; set; }

        public long IDStirrupFamilyName { get; set; }

        public virtual ARProject ARProject { get; set; }

        public virtual ARRebarDesignType ARRebarDesignType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStirrupDesign> ARStirrupDesigns { get; set; }

        public virtual ARStirrupFamilyName ARStirrupFamilyName { get; set; }
    }
}
