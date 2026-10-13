
using GapBi.Api.Data;
using GapBi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GapBi.Api.Services
{
    public class SincronizacaoService
    {
        private readonly AppDbContext contextoBancoDados;
        private readonly DummyJsonService dummyJsonService;

        public SincronizacaoService(
            AppDbContext contextoBancoDados,
            DummyJsonService dummyJsonService)
        {
            this.contextoBancoDados = contextoBancoDados;
            this.dummyJsonService = dummyJsonService;
        }

        // Busca os produtos da DummyJSON e salva no SQL Server
        public async Task<int> SincronizarProdutosAsync()
        {
            // Consulta todos os produtos da API externa
            var dados = await dummyJsonService.BuscarDadosAsync("products");

            var produtos = dados.GetProperty("products");

            var totalEsperado = dados.GetProperty("total").GetInt32();

            // Confere se a API retornou todos os registros
            if (produtos.GetArrayLength() != totalEsperado)
            {
                throw new Exception(
                    "A API externa não retornou todos os produtos."
                );
            }

            int quantidadeProcessada = 0;

            // Percorre os produtos recebidos
            foreach (var produtoJson in produtos.EnumerateArray())
            {
                int idExterno = produtoJson.GetProperty("id").GetInt32();

                string nome = produtoJson.GetProperty("title").GetString()
                    ?? string.Empty;

                string categoria = produtoJson.GetProperty("category").GetString()
                    ?? string.Empty;

                decimal preco = produtoJson.GetProperty("price").GetDecimal();

                // Verifica se o produto já existe no banco
                var produtoExistente = await contextoBancoDados.Produtos
                    .FirstOrDefaultAsync(
                        produto => produto.IdExterno == idExterno
                    );

                if (produtoExistente == null)
                {
                    // Cadastra um produto novo
                    var novoProduto = new Produto
                    {
                        IdExterno = idExterno,
                        Nome = nome,
                        Categoria = categoria,
                        Preco = preco
                    };

                    contextoBancoDados.Produtos.Add(novoProduto);
                }
                else
                {
                    // Atualiza os dados do produto existente
                    produtoExistente.Nome = nome;
                    produtoExistente.Categoria = categoria;
                    produtoExistente.Preco = preco;
                }

                quantidadeProcessada++;
            }

            // Salva as alterações no SQL Server
            await contextoBancoDados.SaveChangesAsync();

            return quantidadeProcessada;
        }
        
        // Busca os clientes da DummyJSON e salva no SQL Server
        public async Task<int> SincronizarClientesAsync()
        {
            // Consulta todos os clientes da API externa
            var dados = await dummyJsonService.BuscarDadosAsync("users");

            var clientes = dados.GetProperty("users");

            var totalEsperado = dados.GetProperty("total").GetInt32();

            // Confere se todos os clientes foram retornados
            if (clientes.GetArrayLength() != totalEsperado)
            {
                throw new Exception(
                    "A API externa não retornou todos os clientes."
                );
            }

            int quantidadeProcessada = 0;

            // Percorre os clientes recebidos
            foreach (var clienteJson in clientes.EnumerateArray())
            {
                int idExterno = clienteJson.GetProperty("id").GetInt32();

                string nome = clienteJson.GetProperty("firstName").GetString()
                    ?? string.Empty;

                string sobrenome = clienteJson.GetProperty("lastName").GetString()
                    ?? string.Empty;

                string email = clienteJson.GetProperty("email").GetString()
                    ?? string.Empty;

                // Verifica se o cliente já existe no banco
                var clienteExistente = await contextoBancoDados.Clientes
                    .FirstOrDefaultAsync(
                        cliente => cliente.IdExterno == idExterno
                    );

                if (clienteExistente == null)
                {
                    // Cadastra um novo cliente
                    var novoCliente = new Cliente
                    {
                        IdExterno = idExterno,
                        Nome = nome,
                        Sobrenome = sobrenome,
                        Email = email
                    };

                    contextoBancoDados.Clientes.Add(novoCliente);
                }
                else
                {
                    // Atualiza os dados do cliente existente
                    clienteExistente.Nome = nome;
                    clienteExistente.Sobrenome = sobrenome;
                    clienteExistente.Email = email;
                }

                quantidadeProcessada++;
            }

            // Salva as alterações no SQL Server
            await contextoBancoDados.SaveChangesAsync();

            return quantidadeProcessada;
        }
        
        // Busca os pedidos da DummyJSON e salva no SQL Server
        public async Task<int> SincronizarPedidosAsync()
        {
            // Consulta todos os carrinhos da API externa
            var dados = await dummyJsonService.BuscarDadosAsync("carts");

            var pedidos = dados.GetProperty("carts");

            int totalEsperado = dados.GetProperty("total").GetInt32();

            // Confere se todos os pedidos foram retornados
            if (pedidos.GetArrayLength() != totalEsperado)
            {
                throw new Exception(
                    "A API externa não retornou todos os pedidos."
                );
            }

            // Carrega os clientes e produtos cadastrados
            var clientes = await contextoBancoDados.Clientes.ToListAsync();

            var produtos = await contextoBancoDados.Produtos.ToListAsync();


            // Utiliza a transação existente ou cria uma para pedidos isolados
            bool possuiTransacaoExterna =
                contextoBancoDados.Database.CurrentTransaction != null;

            var transacao = possuiTransacaoExterna
                ? null
                : await contextoBancoDados.Database.BeginTransactionAsync();

                int quantidadeProcessada = 0;

            foreach (var pedidoJson in pedidos.EnumerateArray())
            {
                int idExterno = pedidoJson.GetProperty("id").GetInt32();

                int clienteIdExterno = pedidoJson.GetProperty("userId").GetInt32();

                // Localiza o cliente pelo identificador externo
                var cliente = clientes.FirstOrDefault(
                    cliente => cliente.IdExterno == clienteIdExterno
                );

                if (cliente == null)
                {
                    throw new Exception(
                        $"Cliente externo {clienteIdExterno} não encontrado."
                    );
                }

                decimal total = pedidoJson.GetProperty("total").GetDecimal();

                decimal totalComDesconto = pedidoJson
                    .GetProperty("discountedTotal").GetDecimal();

                // Verifica se o pedido já existe no banco
                var pedidoExistente = await contextoBancoDados.Pedidos
                    .Include(pedido => pedido.Itens)
                    .FirstOrDefaultAsync(
                        pedido => pedido.IdExterno == idExterno
                    );

                Pedido pedidoAtual;

                if (pedidoExistente == null)
                {
                    // Cria um novo pedido
                    pedidoAtual = new Pedido
                    {
                        IdExterno = idExterno,
                        ClienteId = cliente.Id,
                        Total = total,
                        TotalComDesconto = totalComDesconto
                    };

                    contextoBancoDados.Pedidos.Add(pedidoAtual);
                }
                else
                {
                    // Atualiza o pedido existente
                    pedidoAtual = pedidoExistente;

                    pedidoAtual.ClienteId = cliente.Id;
                    pedidoAtual.Total = total;
                    pedidoAtual.TotalComDesconto = totalComDesconto;

                    // Remove os itens antigos para evitar duplicações
                    contextoBancoDados.PedidoItens.RemoveRange(
                        pedidoAtual.Itens
                    );
                }

                // Percorre os produtos do pedido
                var itensJson = pedidoJson.GetProperty("products");

                foreach (var itemJson in itensJson.EnumerateArray())
                {
                    int produtoIdExterno = itemJson
                        .GetProperty("id").GetInt32();

                    // Localiza o produto pelo identificador externo
                    var produto = produtos.FirstOrDefault(
                        produto => produto.IdExterno == produtoIdExterno
                    );

                    if (produto == null)
                    {
                        throw new Exception(
                            $"Produto externo {produtoIdExterno} não encontrado."
                        );
                    }

                    // Cria o item relacionado ao pedido e produto
                    var novoItem = new PedidoItem
                    {
                        Pedido = pedidoAtual,
                        ProdutoId = produto.Id,
                        Quantidade = itemJson
                            .GetProperty("quantity").GetInt32(),
                        Preco = itemJson
                            .GetProperty("price").GetDecimal(),
                        Total = itemJson
                            .GetProperty("total").GetDecimal(),
                        TotalComDesconto = itemJson
                            .GetProperty("discountedTotal").GetDecimal()
                    };

                    contextoBancoDados.PedidoItens.Add(novoItem);
                }

                quantidadeProcessada++;
            }

            // Salva os pedidos e seus itens
            await contextoBancoDados.SaveChangesAsync();

            // Confirma somente a transação criada por este método
            if (transacao != null)
            {
                await transacao.CommitAsync();
                await transacao.DisposeAsync();
            }

            return quantidadeProcessada;
        }
        
        // Executa a sincronização completa dos dados
        public async Task<object> SincronizarTudoAsync()
        {
            // Inicia uma transação para proteger as alterações no banco
            await using var transacao = await contextoBancoDados
                .Database.BeginTransactionAsync();

            try
            {
                // Sincroniza os dados na ordem dos relacionamentos
                int totalProdutos = await SincronizarProdutosAsync();

                int totalClientes = await SincronizarClientesAsync();

                int totalPedidos = await SincronizarPedidosAsync();

                // Registra a data e hora da sincronização
                var sincronizacao = new Sincronizacao
                {
                    DataHora = DateTime.UtcNow,
                    Sucesso = true,
                    Mensagem = "Sincronização completa realizada com sucesso."
                };

                contextoBancoDados.Sincronizacoes.Add(sincronizacao);

                await contextoBancoDados.SaveChangesAsync();

                // Confirma todas as alterações no banco
                await transacao.CommitAsync();

                return new
                {
                    mensagem = sincronizacao.Mensagem,
                    quantidadeProdutos = totalProdutos,
                    quantidadeClientes = totalClientes,
                    quantidadePedidos = totalPedidos,
                    ultimaSincronizacao = sincronizacao.DataHora
                };
            }
            catch
            {
                // Desfaz as alterações se ocorrer algum erro
                await transacao.RollbackAsync();

                throw;
            }
        }
        
        // Consulta a data e hora da última sincronização realizada com sucesso
        public async Task<DateTime?> ObterUltimaSincronizacaoAsync()
        {
            // Busca o registro mais recente de sincronização bem-sucedida
            var ultimaSincronizacao = await contextoBancoDados.Sincronizacoes
                .AsNoTracking()
                .Where(sincronizacao => sincronizacao.Sucesso)
                .OrderByDescending(sincronizacao => sincronizacao.DataHora)
                .FirstOrDefaultAsync();

            // Retorna null caso nenhuma sincronização tenha sido realizada
            if (ultimaSincronizacao == null)
            {
                return null;
            }

            // Identifica explicitamente que a data armazenada está em UTC
            return DateTime.SpecifyKind(
                ultimaSincronizacao.DataHora,
                DateTimeKind.Utc
            );
        }
    }
}
