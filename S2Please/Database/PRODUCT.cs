namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCT")]
    public partial class PRODUCT
    {
        public long ID { get; set; }

        [StringLength(10)]
        public string PRODUCT_CODE { get; set; }

        public long? PRODUCT_TYPE_ID { get; set; }

        public long? PRODUCT_GROUP_ID { get; set; }

        [StringLength(500)]
        public string NAME { get; set; }

        public string DECRIPTION { get; set; }

        [StringLength(500)]
        public string PRODUCT_MATERIAL { get; set; }

        [StringLength(500)]
        public string PRODUCT_ORIGIN { get; set; }

        public bool? IS_SHOW { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public long? UPDATED_BY { get; set; }

        public DateTime? UPDATED_DATE { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
