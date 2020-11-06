namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CUSTOMER")]
    public partial class CUSTOMER
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string FULL_NAME { get; set; }

        [StringLength(500)]
        public string PHONE { get; set; }

        [StringLength(500)]
        public string EMAIL { get; set; }

        public long? GENDER { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATE_OF_BIRTH { get; set; }

        [StringLength(500)]
        public string ADRESS_SPECIFIC { get; set; }

        public long? PROVIN_CITY { get; set; }

        public long? DISTRICT { get; set; }

        public long? WARD_COMMUNITY { get; set; }

        [StringLength(500)]
        public string IMAGE { get; set; }

        public bool? IS_DELETED { get; set; }
    }
}
