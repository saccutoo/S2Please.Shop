namespace S2Please.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ado : DbContext
    {
        public ado()
            : base("name=ado")
        {
        }

        public virtual DbSet<CUSTOMER> CUSTOMER { get; set; }
        public virtual DbSet<LANGUAGE> LANGUAGE { get; set; }
        public virtual DbSet<MENU> MENU { get; set; }
        public virtual DbSet<MULTI_LANGUAGE> MULTI_LANGUAGE { get; set; }
        public virtual DbSet<PRODUCT> PRODUCT { get; set; }
        public virtual DbSet<PRODUCT_BONUS> PRODUCT_BONUS { get; set; }
        public virtual DbSet<PRODUCT_COLOR> PRODUCT_COLOR { get; set; }
        public virtual DbSet<PRODUCT_COLOR_SIZE_MAPPER> PRODUCT_COLOR_SIZE_MAPPER { get; set; }
        public virtual DbSet<PRODUCT_DETAIL> PRODUCT_DETAIL { get; set; }
        public virtual DbSet<PRODUCT_DISCOUNT> PRODUCT_DISCOUNT { get; set; }
        public virtual DbSet<PRODUCT_GROUP> PRODUCT_GROUP { get; set; }
        public virtual DbSet<PRODUCT_IMG> PRODUCT_IMG { get; set; }
        public virtual DbSet<PRODUCT_SIZE> PRODUCT_SIZE { get; set; }
        public virtual DbSet<PRODUCT_TYPE> PRODUCT_TYPE { get; set; }
        public virtual DbSet<USER_CUSTOMER> USER_CUSTOMER { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.STATUS)
                .IsFixedLength();
        }
    }
}
