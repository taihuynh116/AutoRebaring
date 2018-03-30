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

        public int ProjectID { get; set; }

        [Required]
        [StringLength(15)]
        public string Mark { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(15)]
        public string B1_Param { get; set; }

        [Required]
        [StringLength(15)]
        public string B2_Param { get; set; }

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

        public double LockheadConcreteCover { get; set; }

        public double RatioLH { get; set; }

        public double ConcreteSmallCover { get; set; }

        public double ConcreteCover { get; set; }

        [Required]
        [StringLength(20)]
        public string ElementType { get; set; }

        public double EdgeDimension { get; set; }

        public bool EdgeDimensionInclude { get; set; }

        public double EdgeRatio { get; set; }

        public bool EdgeRatioInclude { get; set; }

        public double BottomOffset { get; set; }

        public double BottomOffsetRatio { get; set; }

        public double TopOffset { get; set; }

        public double TopOffsetRatio { get; set; }

        public bool OffsetInclude { get; set; }

        public bool OffsetRatioInclude { get; set; }

        public double BottomOffsetStirrup { get; set; }

        public double BottomOffsetStirrupRatio { get; set; }

        public double TopOffsetStirrup { get; set; }

        public double TopOffsetStirrupRatio { get; set; }

        public bool StirrupOffsetInclude { get; set; }

        public bool StirrupOffsetRatioInclude { get; set; }

        public bool IsInsideBeam { get; set; }

        public bool IsStirrupInsideBeam { get; set; }

        public double Lmax { get; set; }

        public double Lmin { get; set; }

        public double Step { get; set; }

        public double LImplantMax { get; set; }

        [Required]
        [StringLength(50)]
        public string CheckLevel { get; set; }

        [Required]
        [StringLength(50)]
        public string View3d { get; set; }

        public bool View3dInclude { get; set; }

        public int PartCount { get; set; }
    }
}
