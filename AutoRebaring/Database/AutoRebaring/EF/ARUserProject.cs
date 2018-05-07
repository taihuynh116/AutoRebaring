namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARUserProject")]
    public partial class ARUserProject
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        public long IDUserType { get; set; }

        public long IDUser { get; set; }

        public long IDMacAddress { get; set; }

        public long IDWindowsName { get; set; }

        public DateTime LastLogin { get; set; }

        public bool IsActive { get; set; }

        public virtual ARMacAddress ARMacAddress { get; set; }

        public virtual ARProject ARProject { get; set; }

        public virtual ARUser ARUser { get; set; }

        public virtual ARUserType ARUserType { get; set; }

        public virtual ARWindowsName ARWindowsName { get; set; }
    }
}
