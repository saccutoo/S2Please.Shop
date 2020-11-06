namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MENU")]
    public partial class MENU
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string MENU_NAME { get; set; }

        public bool? IS_DELETED { get; set; }

        public long? PARENT_ID { get; set; }

        [StringLength(500)]
        public string LINK { get; set; }

        [StringLength(500)]
        public string LINK_VIEW { get; set; }

        public int? ORDER_MENU { get; set; }

        public long PRODUCT_GROUP_ID { get; set; }

    }
}
