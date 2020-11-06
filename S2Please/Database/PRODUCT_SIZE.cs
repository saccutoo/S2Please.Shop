namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_SIZE
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string SIZE_NAME { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
