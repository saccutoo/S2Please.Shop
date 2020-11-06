namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_IMG
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string PRODUCT_IMAGE { get; set; }

        public long? PRODUCT_ID { get; set; }

        [StringLength(500)]
        public string TITLE { get; set; }

        public string DECRIPTION { get; set; }

        public bool? IS_MAIN { get; set; }

        public bool? IS_SHOW_COLOR { get; set; }

        public bool? IS_SHOW_SLIDE { get; set; }

        public long? PRODUCT_DETAIL_ID { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
