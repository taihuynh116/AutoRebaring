namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ARDimensionParameter
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDElementType { get; set; }

        [Required]
        [StringLength(15)]
        public string B1_Param { get; set; }

        [Required]
        [StringLength(15)]
        public string B2_Param { get; set; }

        public virtual ARElementType ARElementType { get; set; }
    }
}
