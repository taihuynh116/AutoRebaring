namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AddinConduction")]
    public partial class AddinConduction
    {
        public long ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AddinName { get; set; }

        public double Version { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
