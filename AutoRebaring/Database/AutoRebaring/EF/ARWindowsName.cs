namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARWindowsName")]
    public partial class ARWindowsName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARWindowsName()
        {
            ARUserProjects = new HashSet<ARUserProject>();
        }

        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARUserProject> ARUserProjects { get; set; }
    }
}
