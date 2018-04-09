namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARDimensionParameterValue")]
    public partial class ARDimensionParameterValue
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDDimensionParameterType { get; set; }

        public long IDProject { get; set; }

        public virtual ARDimensionParameterType ARDimensionParameterType { get; set; }

        public virtual ARProject ARProject { get; set; }
    }
}
