namespace AutoRebaring.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserManagement")]
    public partial class UserManagement
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }

        [StringLength(50)]
        public string MacAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string WebUsername { get; set; }

        [Required]
        [StringLength(500)]
        public string WebPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string LoginType { get; set; }

        [StringLength(50)]
        public string ChangeMacAddress { get; set; }

        public bool IsChangePending { get; set; }
    }
}
