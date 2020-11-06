namespace S2Please.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MULTI_LANGUAGE
    {
        public long ID { get; set; }

        public long LANGUAGE_ID { get; set; }

        [Required]
        [StringLength(500)]
        public string KEY { get; set; }

        [Required]
        [StringLength(500)]
        public string VALUE { get; set; }
    }
}
