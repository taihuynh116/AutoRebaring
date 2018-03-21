namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AluformSchedule")]
    public partial class AluformSchedule
    {
        public int ID { get; set; }

        public long? ProductID { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public long? Count { get; set; }

        public long? Storage { get; set; }

        public long? Delta { get; set; }
    }
}
