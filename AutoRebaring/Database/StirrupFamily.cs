namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StirrupFamily")]
    public partial class StirrupFamily
    {
        public long ID { get; set; }

        public int ProjectID { get; set; }

        [Required]
        [StringLength(15)]
        public string Mark { get; set; }

        [Required]
        [StringLength(20)]
        public string FamiyStirrup { get; set; }

        [Required]
        [StringLength(20)]
        public string StirrupType { get; set; }
    }
}
