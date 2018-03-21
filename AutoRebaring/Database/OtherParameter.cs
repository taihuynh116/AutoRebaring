namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OtherParameter
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        [Required]
        [StringLength(50)]
        public string CheckLevel { get; set; }

        public bool IsReversed { get; set; }

        public bool View3dInclude { get; set; }

        [Required]
        [StringLength(20)]
        public string View3d { get; set; }

        public int PartCount { get; set; }

        [Required]
        [StringLength(20)]
        public string Mark { get; set; }
    }
}
