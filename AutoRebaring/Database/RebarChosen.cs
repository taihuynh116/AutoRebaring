namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RebarChosen")]
    public partial class RebarChosen
    {
        public long ID { get; set; }

        public int ProjectID { get; set; }

        public double LStandard { get; set; }

        [Required]
        [StringLength(50)]
        public string LType { get; set; }
    }
}
