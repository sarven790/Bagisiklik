//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BagisiklikTakipSistemi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbBagisiklikEntities : DbContext
    {
        public DbBagisiklikEntities()
            : base("name=DbBagisiklikEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PostTablosu> PostTablosu { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TuketimTablosu> TuketimTablosu { get; set; }
        public virtual DbSet<UyelerTablosu> UyelerTablosu { get; set; }
        public virtual DbSet<YorumlarTablosu> YorumlarTablosu { get; set; }
    }
}
