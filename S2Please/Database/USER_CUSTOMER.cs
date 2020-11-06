namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_CUSTOMER
    {
        public long ID { get; set; }

        public long? USER_ID { get; set; }

        [StringLength(500)]
        public string USER_NAME { get; set; }

        [StringLength(500)]
        public string PASS_WORD { get; set; }

        public bool? IS_LOCK { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
