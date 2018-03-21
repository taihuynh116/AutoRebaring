namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mark")]
    public partial class Mark
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        [Column("Mark")]
        [Required]
        [StringLength(20)]
        public string Mark1 { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
