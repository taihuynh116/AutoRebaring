namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARStandardDesignParameterValue")]
    public partial class ARStandardDesignParameterValue
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDDesignLevel { get; set; }

        public long IDStandardDesignParameterType { get; set; }

        public int Value { get; set; }

        public virtual ARDesignLevel ARDesignLevel { get; set; }

        public virtual ARStandardDesignParameterType ARStandardDesignParameterType { get; set; }
    }
}
