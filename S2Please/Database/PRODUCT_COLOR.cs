namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_COLOR
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string COLOR { get; set; }

        [StringLength(500)]
        public string IMG { get; set; }

        public long? IS_DELETED { get; set; }
    }
}
