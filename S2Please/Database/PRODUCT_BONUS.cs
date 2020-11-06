namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_BONUS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(500)]
        public string BONUS_NAME { get; set; }

        public long? IS_DELETED { get; set; }

        public long? PRODUCT_ID { get; set; }
    }
}
