namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeneralParameterInput")]
    public partial class GeneralParameterInput
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        public double ConcreteCover { get; set; }

        public double DevelopmentMultiply { get; set; }

        public double DevelopmentLengthsDistance { get; set; }

        public bool ReinforcementStirrupInclude { get; set; }

        public double DeltaDevelopmentError { get; set; }

        public int NumberDevelopmentError { get; set; }

        public bool DevelopmentErrorInclude { get; set; }

        public double DevelopmentLevelOffsetAllowed { get; set; }

        public bool DevelopmentLevelOffsetInclude { get; set; }

        public double ShortenLimit { get; set; }

        public double AnchorMultiply { get; set; }

        public double LockheadMultiply { get; set; }

        public double ConcreteTopCover { get; set; }

        public double RatioLH { get; set; }

        public double CoverTopSmall { get; set; }
    }
}
