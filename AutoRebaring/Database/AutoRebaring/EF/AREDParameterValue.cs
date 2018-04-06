namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AREDParameterValue")]
    public partial class AREDParameterValue
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDParameterType { get; set; }

        public long? IDMark { get; set; }

        public double Value { get; set; }

        public virtual AREDParameterType AREDParameterType { get; set; }

        public virtual ARMark ARMark { get; set; }
    }
}
