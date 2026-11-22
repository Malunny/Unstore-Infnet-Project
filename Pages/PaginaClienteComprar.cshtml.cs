using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados;

namespace UnstoreProjectADS.Pages;

[Authorize(Roles = "Cliente")]
public class PaginaClienteComprar : PageModel
{
    public Cliente? Cliente { get; set; }
    public decimal Frete { get; } = 20m;
    public decimal TotalProdutos => Cliente?.Carrinho.PegarProdutos()
        .Sum(produto => produto.Valor) ?? 0;
    public List<string> FormasPagamento { get; } = 
        new() { "Pix", "Cartão de crédito", "Boleto" };

    [BindProperty] public int EnderecoId { get; set; }
    [BindProperty] public string FormaPagamento { get; set; } = string.Empty;

    public async Task OnGetAsync([FromServices] ContextoDados contextoDados)
    {
        Cliente = await ObterCliente(contextoDados);
    }

    public async Task<IActionResult> OnPostAsync([FromServices] ContextoDados contextoDados)
    {
        Cliente = await ObterCliente(contextoDados);

        if (Cliente == null)
            return Page();

        var produtos = Cliente.Carrinho.PegarProdutos();

        if (!produtos.Any())
        {
            TempData["ProblemaCompra"] = "Seu carrinho está vazio.";
            return Page();
        }

        var endereco = Cliente.Enderecos.FirstOrDefault(e => e.Id == EnderecoId);

        if (endereco == null)
        {
            TempData["ProblemaCompra"] = "Selecione um endereço de entrega.";
            return Page();
        }

        if (!FormasPagamento.Contains(FormaPagamento))
        {
            TempData["ProblemaCompra"] = "Selecione uma forma de pagamento.";
            return Page();
        }

        if (produtos.Any(produto => produto.QuantidadeEstoque < 1))
        {
            TempData["ProblemaCompra"] = "Há produtos do carrinho sem estoque.";
            return Page();
        }

        var compra = new Compra
        {
            ClienteId = Cliente.Id,
            RealizadaEm = DateTime.Now
        };
        
        contextoDados.Compras.Add(compra);
        await contextoDados.SaveChangesAsync();

        foreach (var produto in produtos)
        {
            var produtoCompra = new ProdutoCompra
            {
                CompraId = compra.Id,
                ProdutoId = produto.Id,
                ValorNoMomento = produto.Valor,
                QuantidadeCompra = 1
            };
            
            await contextoDados.AddAsync(produtoCompra);
            await contextoDados.SaveChangesAsync();

            produto.QuantidadeEstoque -= 1;
        }

        var encomenda = new Encomenda
        {
            CompraId = compra.Id,
            EnderecoId = endereco.Id,
            EstadoEntrega = "Aguardando envio",
            PrevisaoEntrega = DateTime.Now.AddDays(7)
        };
                
        Console.WriteLine("fazendo encomenda");
        
        await contextoDados.Encomendas.AddAsync(encomenda);
        await contextoDados.SaveChangesAsync();
        
        compra.Encomenda = encomenda;

        Cliente.Carrinho.ProdutosCarrinho.Clear();

        await contextoDados.SaveChangesAsync();

        return RedirectToPage("/PaginaCompraRealizada");
    }

    private async Task<Cliente?> ObterCliente(ContextoDados contextoDados)
    {
        var clienteEmail = User.FindFirstValue(ClaimTypes.Email);

        if (clienteEmail == null)
            return null;

        var cliente = await contextoDados.Clientes
            .Include(cliente => cliente.Carrinho)
            .ThenInclude(carrinho => carrinho.ProdutosCarrinho)
            .ThenInclude(pc => pc.Produto)
            .Include(cliente => cliente.Enderecos)
            .FirstOrDefaultAsync(cliente => cliente.Email == clienteEmail);

        return cliente;
    }
}