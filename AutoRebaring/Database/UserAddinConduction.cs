namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAddinConduction")]
    public partial class UserAddinConduction
    {
        public long ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AddinName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string MacAddress { get; set; }

        public bool IsEnabled { get; set; }
    }
}
