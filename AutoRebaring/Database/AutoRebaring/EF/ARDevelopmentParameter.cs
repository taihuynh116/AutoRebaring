namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ARDevelopmentParameter
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        public int DevelopmentMultiply { get; set; }

        public double DevelopmentLengthsDistance { get; set; }

        public double DeltaDevelopmentError { get; set; }

        public int NumberDevelopmentError { get; set; }

        public bool DevelopmentErrorInclude { get; set; }

        public double DevelopmentLevelOffsetAllowed { get; set; }

        public bool DevelopmentLevelOffsetInclude { get; set; }

        public bool ReinforcementStirrupInclude { get; set; }

        public virtual ARProject ARProject { get; set; }
    }
}
