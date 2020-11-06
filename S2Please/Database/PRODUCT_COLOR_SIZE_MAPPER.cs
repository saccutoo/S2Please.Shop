namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_COLOR_SIZE_MAPPER
    {
        public long ID { get; set; }

        public long? COLOR_ID { get; set; }

        public long? SIZE_ID { get; set; }

        public long? PRODUCT_ID { get; set; }

        public long? AMOUNT { get; set; }

        public double? PRICE { get; set; }

        public long? RATE { get; set; }

        public bool? IS_MAIN { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
