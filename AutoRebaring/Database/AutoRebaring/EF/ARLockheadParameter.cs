namespace AutoRebaring.Database.AutoRebaring.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ARLockheadParameter
    {
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        public long IDProject { get; set; }

        public double ShortenLimit { get; set; }

        public int LockheadMutiply { get; set; }

        public double LockheadConcreteCover { get; set; }

        public virtual ARProject ARProject { get; set; }
    }
}
