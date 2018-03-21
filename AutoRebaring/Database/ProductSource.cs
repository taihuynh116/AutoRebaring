namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductSource")]
    public partial class ProductSource
    {
        public int ID { get; set; }

        public long? ProductID { get; set; }

        [Column("ProductSource")]
        [StringLength(50)]
        public string ProductSource1 { get; set; }

        public long? Count { get; set; }
    }
}
