namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_DETAIL
    {
        public long ID { get; set; }

        public long? PRODUCT_ID { get; set; }

        public string DECRIPTION { get; set; }

        public bool? IS_MAIN_SHOW { get; set; }

        public long? PRODUCT_SIZE_ID { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
