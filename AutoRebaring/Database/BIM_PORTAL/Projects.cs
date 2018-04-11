namespace AutoRebaring.Database.BIM_PORTAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Projects")]
    public partial class Project
    {
        public int SYS_ID { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public Project(int sys_id, string code, string value)
        {
            SYS_ID = sys_id;
            Code = code;
            Value = value;
        }
    }
}
