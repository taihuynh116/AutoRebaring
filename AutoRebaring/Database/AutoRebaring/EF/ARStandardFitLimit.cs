namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStandardFitLimit")]
    public partial class ARStandardFitLimit
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDStandardFitType { get; set; }

        public long IDProject { get; set; }

        public int Limit { get; set; }

        public virtual ARProject ARProject { get; set; }

        public virtual ARStandardFitType ARStandardFitType { get; set; }
    }
}
