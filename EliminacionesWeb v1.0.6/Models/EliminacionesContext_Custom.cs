using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EliminacionesWeb.ModelsDTO;

namespace EliminacionesWeb.Models
{
    public partial class EliminacionesContext_Custom : EliminacionesContext
    {
        public EliminacionesContext_Custom(DbContextOptions<EliminacionesContext_Custom> options)
            : base(ChangeOptionsType<EliminacionesContext>(options))
        {
        }

        protected static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);

            if (sqlExt == null)
                throw (new Exception("Failed to retrieve SQL connection string for base Context"));

            return new DbContextOptionsBuilder<T>()
                        .UseSqlServer(((SqlServerOptionsExtension)sqlExt).ConnectionString)
                        .Options;
        }

        public virtual DbSet<txtOutputDTO> txtOutputDTO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<txtOutputDTO>(entity =>
            //{
            //    entity.HasKey(e => new
            //    {
            //        e.empresa
            //        //e.intercompany,
            //        //e.rubro,
            //        //e.año,
            //        //e.periodo
            //    })
            //        .HasName("PK_txtOutputDTO");
            //});


            modelBuilder.Query<AjustesDTO>();
            modelBuilder.Query<DatosPlanillaDTO>();
            modelBuilder.Query<EliminacionesDTO>();
            modelBuilder.Query<txtOutputDTO>();

        }


      

    }
}
