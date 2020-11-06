namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_TYPE
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string NAME { get; set; }

        public bool? IS_SHOW_VIEW { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public long? UPDATED_BY { get; set; }

        public DateTime? UPDATED_DATE { get; set; }

        public bool? IS_DELETED { get; set; }

        public long? MENU_ID { get; set; }
    }
}
