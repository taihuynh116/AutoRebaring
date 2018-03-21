namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RebarChosenGeneral")]
    public partial class RebarChosenGeneral
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        public double Lmax { get; set; }

        public double Lmin { get; set; }

        public double Step { get; set; }

        public double LImplantMax { get; set; }

        [Required]
        [StringLength(50)]
        public string FamilyStirrup1 { get; set; }

        [Required]
        [StringLength(50)]
        public string FamilyStirrup2 { get; set; }
    }
}
