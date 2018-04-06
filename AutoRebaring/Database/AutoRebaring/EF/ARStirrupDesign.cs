namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStirrupDesign")]
    public partial class ARStirrupDesign
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDStirrupFamilyType { get; set; }

        public long IDRebarDesignType { get; set; }

        public long IDRebarBarType { get; set; }

        public long IDDesignLevel { get; set; }

        public virtual ARDesignLevel ARDesignLevel { get; set; }

        public virtual ARRebarBarType ARRebarBarType { get; set; }

        public virtual ARRebarDesignType ARRebarDesignType { get; set; }

        public virtual ARStirrupFamilyType ARStirrupFamilyType { get; set; }
    }
}
