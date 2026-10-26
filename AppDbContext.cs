using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;

namespace UnstoreProjectADS;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<DescontoProduto> DescontosProdutos { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<ProdutoCompra> ProdutosCompra { get; set; }
    public DbSet<Encomenda> Encomendas { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<AvaliacaoProduto> AvaliacoesProdutos { get; set; }
    public DbSet<Reclamacao> Reclamacoes { get; set; }
    public DbSet<MotivoReclamacao> MotivosReclamacoes { get; set; }
    public DbSet<Conversa> Conversas { get; set; }
    public DbSet<Mensagem> Mensagens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().Property(u => u.Id).UseAutoincrement();
        modelBuilder.Entity<Cargo>().Property(c => c.Id).UseAutoincrement();
        modelBuilder.Entity<Produto>().Property(p => p.Id).UseAutoincrement();
        modelBuilder.Entity<DescontoProduto>().Property(d => d.Id).UseAutoincrement();
        modelBuilder.Entity<Compra>().Property(c => c.Id).UseAutoincrement();
        modelBuilder.Entity<Encomenda>().Property(e => e.Id).UseAutoincrement();
        modelBuilder.Entity<Endereco>().Property(e => e.Id).UseAutoincrement();
        modelBuilder.Entity<AvaliacaoProduto>().Property(a => a.Id).UseAutoincrement();
        modelBuilder.Entity<Reclamacao>().Property(r => r.Id).UseAutoincrement();
        modelBuilder.Entity<MotivoReclamacao>().Property(m => m.Id).UseAutoincrement();
        modelBuilder.Entity<Conversa>().Property(c => c.Id).UseAutoincrement();
        modelBuilder.Entity<Mensagem>().Property(m => m.Id).UseAutoincrement();
        
        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.Cargo)
            .WithMany(c => c.Funcionarios)
            .HasForeignKey(f => f.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProdutoCompra>()
            .HasKey(pc => new { pc.ProdutoId, pc.CompraId });

        modelBuilder.Entity<ProdutoCompra>()
            .HasOne(pc => pc.Produto)
            .WithMany(p => p.ProdutosCompra)
            .HasForeignKey(pc => pc.ProdutoId);

        modelBuilder.Entity<ProdutoCompra>()
            .HasOne(pc => pc.Compra)
            .WithMany(c => c.ProdutosCompra)
            .HasForeignKey(pc => pc.CompraId);

        modelBuilder.Entity<Encomenda>()
            .HasOne(e => e.Compra)
            .WithOne(c => c.Encomenda)
            .HasForeignKey<Encomenda>(e => e.CompraId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}