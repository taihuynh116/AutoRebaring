namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RebarDesign")]
    public partial class RebarDesign
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        [Required]
        [StringLength(20)]
        public string Mark { get; set; }

        [Required]
        [StringLength(50)]
        public string StartLevel { get; set; }

        public double Elevation { get; set; }

        [Required]
        [StringLength(50)]
        public string DesignLevel { get; set; }

        [Required]
        [StringLength(10)]
        public string RebarType { get; set; }

        public long RebarB1 { get; set; }

        public long RebarB2 { get; set; }

        [Required]
        [StringLength(10)]
        public string StirrupType1 { get; set; }

        public double StirrupTBSpacing1 { get; set; }

        public double StirrupMSpacing1 { get; set; }

        [Required]
        [StringLength(10)]
        public string StirrupType2 { get; set; }

        public double StirrupTBSpacing2 { get; set; }

        public double StirrupMSpacing2 { get; set; }
    }
}
