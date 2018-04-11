namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARRebarDesignType")]
    public partial class ARRebarDesignType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARRebarDesignType()
        {
            ARStandardDesigns = new HashSet<ARStandardDesign>();
            ARStirrupFamilyTypes = new HashSet<ARStirrupFamilyType>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDElementType { get; set; }

        public long IDRebarType { get; set; }

        [Required]
        [StringLength(30)]
        public string Type { get; set; }

        public virtual ARElementType ARElementType { get; set; }

        public virtual ARRebarType ARRebarType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStandardDesign> ARStandardDesigns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARStirrupFamilyType> ARStirrupFamilyTypes { get; set; }
    }
}
