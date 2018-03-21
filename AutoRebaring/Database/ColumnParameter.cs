namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ColumnParameter")]
    public partial class ColumnParameter
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        [Required]
        [StringLength(50)]
        public string B1_Param { get; set; }

        [Required]
        [StringLength(50)]
        public string B2_Param { get; set; }
    }
}
