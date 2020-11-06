namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LANGUAGE")]
    public partial class LANGUAGE
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string LANGUAGE_NAME { get; set; }

        [StringLength(500)]
        public string LANGUAGE_IMAGE { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
