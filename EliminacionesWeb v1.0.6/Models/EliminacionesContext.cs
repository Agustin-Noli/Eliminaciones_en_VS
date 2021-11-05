using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EliminacionesWeb.Models
{
    public partial class EliminacionesContext : DbContext
    {
        public EliminacionesContext()
        {
        }

        public EliminacionesContext(DbContextOptions<EliminacionesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ajustes> Ajustes { get; set; }
        public virtual DbSet<AuxDatosPlanilla> AuxDatosPlanilla { get; set; }
        public virtual DbSet<DatosPlanilla> DatosPlanilla { get; set; }
        public virtual DbSet<Eliminaciones> Eliminaciones { get; set; }
        public virtual DbSet<EmpresaRubro> EmpresaRubro { get; set; }
        public virtual DbSet<Empresas> Empresas { get; set; }
        public virtual DbSet<GrandesRubros> GrandesRubros { get; set; }
        public virtual DbSet<GrupoEmpresas> GrupoEmpresas { get; set; }
        public virtual DbSet<Grupos> Grupos { get; set; }
        public virtual DbSet<Historial> Historial { get; set; }
        public virtual DbSet<ImportPlanillas> ImportPlanillas { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Perfiles> Perfiles { get; set; }
        public virtual DbSet<RelacionRubros> RelacionRubros { get; set; }
        public virtual DbSet<Rubros> Rubros { get; set; }
        public virtual DbSet<RubrosCabecera> RubrosCabecera { get; set; }
        public virtual DbSet<Sectores> Sectores { get; set; }
        public virtual DbSet<SubRubros> SubRubros { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=SQLAD_CMRM_Eliminaciones,4000;Initial Catalog=Eliminaciones;User ID=usr_eliminaciones;Password=zug236ls1i");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ajustes>(entity =>
            {
                entity.HasKey(e => new { e.EmpCodigo, e.Periodo, e.RubCodigo, e.EmpCodigoContraparte, e.SecCodigo })
                    .IsClustered(false);

                entity.ToTable("AJUSTES");

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.Periodo)
                    .HasColumnName("PERIODO")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RubCodigo)
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EmpCodigoContraparte).HasColumnName("EMP_CODIGO_CONTRAPARTE");

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AjuSaldo)
                    .HasColumnName("AJU_SALDO")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.AjuSaldoPromedio)
                    .HasColumnName("AJU_SALDO_PROMEDIO")
                    .HasColumnType("numeric(23, 2)");
            });

            modelBuilder.Entity<AuxDatosPlanilla>(entity =>
            {
                entity.HasKey(e => e.AuxId)
                    .IsClustered(false);

                entity.ToTable("AUX_DATOS_PLANILLA");

                entity.Property(e => e.AuxId).HasColumnName("AUX_ID");

                entity.Property(e => e.Concepto)
                    .HasColumnName("CONCEPTO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.EmpCodigoContraparte).HasColumnName("EMP_CODIGO_CONTRAPARTE");

                entity.Property(e => e.Exposicion)
                    .HasColumnName("EXPOSICION")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ind).HasColumnName("IND");

                entity.Property(e => e.MonCodigo).HasColumnName("MON_CODIGO");

                entity.Property(e => e.Periodo)
                    .IsRequired()
                    .HasColumnName("PERIODO")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Pond).HasColumnName("POND");

                entity.Property(e => e.RubCodigo)
                    .IsRequired()
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Saldo)
                    .HasColumnName("SALDO")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.SaldoPromedio)
                    .HasColumnName("SALDO_PROMEDIO")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.SecCodigo).HasColumnName("SEC_CODIGO");
            });

            modelBuilder.Entity<DatosPlanilla>(entity =>
            {
                entity.HasKey(e => new { e.EmpCodigo, e.Periodo, e.RubCodigo, e.EmpCodigoContraparte, e.SecCodigo })
                    .IsClustered(false);

                entity.ToTable("DATOS_PLANILLA");

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.Periodo)
                    .HasColumnName("PERIODO")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RubCodigo)
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EmpCodigoContraparte).HasColumnName("EMP_CODIGO_CONTRAPARTE");

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Concepto)
                    .HasColumnName("CONCEPTO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Exposicion)
                    .HasColumnName("EXPOSICION")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ind).HasColumnName("IND");

                entity.Property(e => e.MonCodigo).HasColumnName("MON_CODIGO");

                entity.Property(e => e.Pond).HasColumnName("POND");

                entity.Property(e => e.Saldo)
                    .HasColumnName("SALDO")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.SaldoPromedio)
                    .HasColumnName("SALDO_PROMEDIO")
                    .HasColumnType("numeric(23, 2)");
            });

            modelBuilder.Entity<Eliminaciones>(entity =>
            {
                entity.HasKey(e => new { e.EmpCodigo, e.Periodo, e.EmpCodigoContraparte, e.RubCodigo, e.SecCodigo })
                    .IsClustered(false);

                entity.ToTable("ELIMINACIONES");

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.Periodo)
                    .HasColumnName("PERIODO")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EmpCodigoContraparte).HasColumnName("EMP_CODIGO_CONTRAPARTE");

                entity.Property(e => e.RubCodigo)
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EliSaldo)
                    .HasColumnName("ELI_SALDO")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.EliSaldoPromedio)
                    .HasColumnName("ELI_SALDO_PROMEDIO")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.EmpPorcentaje)
                    .HasColumnName("EMP_PORCENTAJE")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.EsAjuste)
                    .IsRequired()
                    .HasColumnName("ES_AJUSTE")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.MonCodigo).HasColumnName("MON_CODIGO");
            });

            modelBuilder.Entity<EmpresaRubro>(entity =>
            {
                entity.HasKey(e => new { e.EmpCodigo, e.RubCodigo, e.SecCodigo })
                    .IsClustered(false);

                entity.ToTable("EMPRESA_RUBRO");

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.RubCodigo)
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Empresas>(entity =>
            {
                entity.HasKey(e => new { e.EmpCodigo, e.SecCodigo })
                    .HasName("PK_EMP_CODIGO")
                    .IsClustered(false);

                entity.ToTable("EMPRESAS");

                entity.Property(e => e.EmpCodigo)
                    .HasColumnName("EMP_CODIGO")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("ACTIVO")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('S')");

                entity.Property(e => e.EmpDescripcion)
                    .IsRequired()
                    .HasColumnName("EMP_DESCRIPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpIntercompany)
                    .IsRequired()
                    .HasColumnName("EMP_INTERCOMPANY")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpPorcentaje).HasColumnName("EMP_PORCENTAJE");
            });

            modelBuilder.Entity<GrandesRubros>(entity =>
            {
                entity.HasKey(e => e.GranrubCodigo);

                entity.ToTable("GRANDES_RUBROS");

                entity.Property(e => e.GranrubCodigo)
                    .HasColumnName("GRANRUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GranrubDescripcion)
                    .HasColumnName("GRANRUB_DESCRIPCION")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.RubcabCodigo)
                    .IsRequired()
                    .HasColumnName("RUBCAB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<GrupoEmpresas>(entity =>
            {
                entity.HasKey(e => new { e.GrupoId, e.EmpCodigo, e.SecCodigo })
                    .IsClustered(false);

                entity.ToTable("GRUPO_EMPRESAS");

                entity.Property(e => e.GrupoId).HasColumnName("GRUPO_ID");

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Grupos>(entity =>
            {
                entity.HasKey(e => e.GrupoId)
                    .HasName("PK__GRUPOS__9FE3F59E382C011B");

                entity.ToTable("GRUPOS");

                entity.Property(e => e.GrupoId).HasColumnName("GRUPO_ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("ACTIVO")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('S')");

                entity.Property(e => e.GrupoDescripcion)
                    .IsRequired()
                    .HasColumnName("GRUPO_DESCRIPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoNombre)
                    .IsRequired()
                    .HasColumnName("GRUPO_NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Historial>(entity =>
            {
                entity.HasKey(e => e.HistId)
                    .HasName("PK_Historial");

                entity.ToTable("HISTORIAL");

                entity.Property(e => e.HistId).HasColumnName("HIST_ID");

                entity.Property(e => e.Accion)
                    .HasColumnName("ACCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaHora)
                    .HasColumnName("FECHA_HORA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mensaje)
                    .HasColumnName("MENSAJE")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Periodo)
                    .HasColumnName("PERIODO")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SecCodigo).HasColumnName("SEC_CODIGO");

                entity.Property(e => e.UsuLegajo)
                    .IsRequired()
                    .HasColumnName("USU_LEGAJO")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ImportPlanillas>(entity =>
            {
                entity.HasKey(e => new { e.EmpCodigo, e.Periodo, e.SecCodigo })
                    .IsClustered(false);

                entity.ToTable("IMPORT_PLANILLAS");

                entity.Property(e => e.EmpCodigo).HasColumnName("EMP_CODIGO");

                entity.Property(e => e.Periodo)
                    .HasColumnName("PERIODO")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Importacion)
                    .IsRequired()
                    .HasColumnName("IMPORTACION")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('S')");

                entity.Property(e => e.MonCodigo)
                    .HasColumnName("MON_CODIGO")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NombrePlanilla)
                    .IsRequired()
                    .HasColumnName("NOMBRE_PLANILLA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Porcentaje)
                    .HasColumnName("PORCENTAJE")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasKey(e => e.MonCodigo)
                    .HasName("PK__MONEDA__C047136693422F4A");

                entity.ToTable("MONEDA");

                entity.Property(e => e.MonCodigo)
                    .HasColumnName("MON_CODIGO")
                    .ValueGeneratedNever();

                entity.Property(e => e.MonCotizacion)
                    .HasColumnName("MON_COTIZACION")
                    .HasColumnType("numeric(23, 2)");

                entity.Property(e => e.MonDescripcion)
                    .HasColumnName("MON_DESCRIPCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Perfiles>(entity =>
            {
                entity.HasKey(e => e.PerCodigo)
                    .HasName("PK__PERFILES__AA2E668DAD75060E");

                entity.ToTable("PERFILES");

                entity.Property(e => e.PerCodigo).HasColumnName("PER_CODIGO");

                entity.Property(e => e.PerDescripcion)
                    .HasColumnName("PER_DESCRIPCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RelacionRubros>(entity =>
            {
                entity.HasKey(e => new { e.RubcabCodigo, e.GranrubCodigo, e.SubrubId, e.RubCodigo });

                entity.ToTable("RELACION_RUBROS");

                entity.Property(e => e.RubcabCodigo)
                    .HasColumnName("RUBCAB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GranrubCodigo)
                    .HasColumnName("GRANRUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SubrubId).HasColumnName("SUBRUB_ID");

                entity.Property(e => e.RubCodigo)
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("ACTIVO")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('S')");
            });

            modelBuilder.Entity<Rubros>(entity =>
            {
                entity.HasKey(e => e.RubCodigo);

                entity.Property(e => e.RubCodigo)
                    .HasColumnName("RUB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EsBcra)
                    .HasColumnName("ES_BCRA")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RubDescripcion)
                    .HasColumnName("RUB_DESCRIPCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RubNivel).HasColumnName("RUB_NIVEL");

                entity.Property(e => e.RubOrden)
                    .IsRequired()
                    .HasColumnName("RUB_ORDEN")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RubSigno)
                    .HasColumnName("RUB_SIGNO")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RubrosCabecera>(entity =>
            {
                entity.HasKey(e => e.RubcabCodigo)
                    .HasName("PK_Rubros_Cabecera");

                entity.ToTable("RUBROS_CABECERA");

                entity.Property(e => e.RubcabCodigo)
                    .HasColumnName("RUBCAB_CODIGO")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RubcabDescripcion)
                    .HasColumnName("RUBCAB_DESCRIPCION")
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sectores>(entity =>
            {
                entity.HasKey(e => e.SecCodigo)
                    .HasName("PK__SECTORES__880DDF25A3F7412D");

                entity.ToTable("SECTORES");

                entity.Property(e => e.SecCodigo).HasColumnName("SEC_CODIGO");

                entity.Property(e => e.SecDescripcion)
                    .HasColumnName("SEC_DESCRIPCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubRubros>(entity =>
            {
                entity.HasKey(e => e.SubrubId);

                entity.ToTable("SUB_RUBROS");

                entity.Property(e => e.SubrubId).HasColumnName("SUBRUB_ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("ACTIVO")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('S')");

                entity.Property(e => e.SubrubDescripcion)
                    .HasColumnName("SUBRUB_DESCRIPCION")
                    .HasMaxLength(110)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.UsuLegajo)
                    .HasName("PK_Usuarios");

                entity.ToTable("USUARIOS");

                entity.Property(e => e.UsuLegajo)
                    .HasColumnName("USU_LEGAJO")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("ACTIVO")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('S')");

                entity.Property(e => e.PerCodigo)
                    .HasColumnName("PER_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SecCodigo)
                    .HasColumnName("SEC_CODIGO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UsuFecalta)
                    .HasColumnName("USU_FECALTA")
                    .HasColumnType("datetime");

                entity.Property(e => e.UsuNombre)
                    .HasColumnName("USU_NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
