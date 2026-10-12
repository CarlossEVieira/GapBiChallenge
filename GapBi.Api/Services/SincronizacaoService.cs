
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

    }
}
