using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Enums;

namespace UnstoreProjectADS.Dados;

public class ContextoDados : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }
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
    public DbSet<Imagem> Imagens { get; set; }

    public ContextoDados(DbContextOptions<ContextoDados> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db").LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        modelBuilder.Entity<Imagem>().Property(i => i.Id).UseAutoincrement();
        
        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.Cargo)
            .WithMany(c => c.Funcionarios)
            .HasForeignKey(f => f.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>()
            .HasDiscriminator<string>("TipoUsuario")
            .HasValue<Usuario>("Usuario")
                .HasValue<Funcionario>("Funcionario")
                .HasValue<Cliente>("Cliente");
        
        modelBuilder.Entity<Cliente>()
            .HasOne(cliente => cliente.Carrinho)
            .WithOne(carrinho => carrinho.Cliente)
            .HasForeignKey<Cliente>(cliente => cliente.CarrinhoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProdutoCarrinho>()
            .HasOne(p => p.Carrinho)
            .WithMany(c => c.ProdutosCarrinho)
            .HasForeignKey(f => f.CarrinhoId);
        
        modelBuilder.Entity<ProdutoCarrinho>()
            .HasOne(p => p.Produto)
            .WithMany()
            .HasForeignKey(p => p.ProdutoId);

        modelBuilder.Entity<ProdutoCarrinho>()
            .HasKey(p => new  { p.ProdutoId, p.CarrinhoId } );

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

        modelBuilder.Entity<Funcionario>()
            .HasMany(f => f.PedidosDeReposicao)
            .WithOne(p => p.CriadorDoPedido)
            .HasForeignKey(p => p.CriadorDoPedidoId);

        modelBuilder.Entity<PedidoDeReposicao>()
            .HasMany(p => p.ProdutosApontados)
            .WithMany(p => p.PedidosDeReposicao);

        modelBuilder.Entity<Conversa>().HasOne(c => c.Cliente).WithMany(u => u.Conversas)
            .HasForeignKey(c => c.ClienteId);
        modelBuilder.Entity<Conversa>().HasOne(c => c.Funcionario).WithMany(u => u.Conversas)
            .HasForeignKey(c => c.FuncionarioId);

        modelBuilder.Entity<Conversa>().HasMany(c => c.Mensagens).WithOne(m => m.Conversa)
            .HasForeignKey(m => m.ConversaId);

    }
}