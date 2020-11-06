namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_DISCOUNT
    {
        public long ID { get; set; }

        public long? DISCOUNT_RATE { get; set; }

        public long? PRODUCT_DETAIL_ID { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
