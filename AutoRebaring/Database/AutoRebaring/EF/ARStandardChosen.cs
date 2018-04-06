namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStandardChosen")]
    public partial class ARStandardChosen
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        public double Lmax { get; set; }

        public double Lmin { get; set; }

        [Required]
        [StringLength(10)]
        public string Step { get; set; }

        [Required]
        [StringLength(10)]
        public string LImplantMax { get; set; }

        public virtual ARProject ARProject { get; set; }
    }
}
