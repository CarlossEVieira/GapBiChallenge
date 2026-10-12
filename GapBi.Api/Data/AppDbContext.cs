
using GapBi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GapBi.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<PedidoItem> PedidoItens { get; set; }

        public DbSet<Sincronizacao> Sincronizacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento entre Cliente e Pedido
            modelBuilder.Entity<Pedido>()
                .HasOne(pedido => pedido.Cliente)
                .WithMany(cliente => cliente.Pedidos)
                .HasForeignKey(pedido => pedido.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento entre Pedido e PedidoItem
            modelBuilder.Entity<PedidoItem>()
                .HasOne(item => item.Pedido)
                .WithMany(pedido => pedido.Itens)
                .HasForeignKey(item => item.PedidoId);

            // Relacionamento entre Produto e PedidoItem
            modelBuilder.Entity<PedidoItem>()
                .HasOne(item => item.Produto)
                .WithMany(produto => produto.PedidoItens)
                .HasForeignKey(item => item.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração dos valores monetários
            modelBuilder.Entity<Produto>()
                .Property(produto => produto.Preco)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
                .Property(pedido => pedido.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
                .Property(pedido => pedido.TotalComDesconto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoItem>()
                .Property(item => item.Preco)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoItem>()
                .Property(item => item.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoItem>()
                .Property(item => item.TotalComDesconto)
                .HasPrecision(18, 2);
        }
    }
}
