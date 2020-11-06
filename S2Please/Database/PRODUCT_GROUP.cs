namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_GROUP
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string GROUP_NAME { get; set; }

        public bool? IS_SHOW { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
