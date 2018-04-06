namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStirrupDesignParameterValue")]
    public partial class ARStirrupDesignParameterValue
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDDesignLevel { get; set; }

        public long IDStirrupDesignParameterType { get; set; }

        public int Value { get; set; }

        public virtual ARDesignLevel ARDesignLevel { get; set; }

        public virtual ARStirrupDesignParameterType ARStirrupDesignParameterType { get; set; }
    }
}
