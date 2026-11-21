using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services.Compartilhado;
using UnstoreProjectADS.Dados;
using UnstoreProjectADS.Pages.Pedidos;
using UnstoreProjectADS.Pages.Requests;

namespace UnstoreProjectADS.Core.Services;

public class ServicoConta
{
    private readonly ContextoDados _contexto;
    public ServicoConta(ContextoDados context)
    {
        _contexto = context;
    }

    public async Task<ClaimsPrincipal?> IniciarSessao(string email, string password)
    {
        Usuario? usuario = await _contexto.Clientes.FirstOrDefaultAsync(cliente => cliente.Email == email);
        
        if (usuario == null)
            usuario = await _contexto.Funcionarios
                .Include(funcionario => funcionario.Cargo)
                .FirstOrDefaultAsync(funcionario => funcionario.Email == email);

        if (usuario == null)
            return null;

        if (usuario.Senha != password)
            return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, usuario.Nome),
        };

        var nomeCargo = "Cliente";

        if (usuario is Funcionario funcionario)
            nomeCargo = funcionario.Cargo.Nome;
        
        claims.Add(new Claim(ClaimTypes.Role, nomeCargo));

        var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        return new ClaimsPrincipal(identidade);
    }

    public async Task<RespostaServico> Registrar(PedidoDeRegistro pedidoRegistro, bool aplicarValidacaoExtra = false)
    {
        RespostaServico respostaServico = new();

        if (aplicarValidacaoExtra)
        {
            var validacaoPedidoRegistro = ValidarPedidoDeRegistro(pedidoRegistro);

            if (!validacaoPedidoRegistro.Ok)
                return validacaoPedidoRegistro;
        }
        
        var cliente = AlimentarDadosCliente(pedidoRegistro);

        await _contexto.Clientes.AddAsync(cliente);
        await _contexto.SaveChangesAsync();
        
        respostaServico.AdicionarMensagem("Cadastrado com sucesso!");
        
        return respostaServico;
    }

    private Cliente AlimentarDadosCliente(PedidoDeRegistro pedidoRegistro)
    {
        Cliente cliente = new();
        
        cliente.Nome = pedidoRegistro.Nome;
        cliente.Email = pedidoRegistro.Email;
        cliente.Senha = pedidoRegistro.Senha;

        foreach (var enderecoFormulario in pedidoRegistro.Enderecos)
        {
            var endereco = new Endereco
            {
                Rua = enderecoFormulario.Rua,
                CEP = enderecoFormulario.CEP,
                Complemento = enderecoFormulario.Complemento,
                Estado = enderecoFormulario.Estado,
                Municipio = enderecoFormulario.Municipio,
                Numero = enderecoFormulario.Numero
            };
            cliente.Enderecos.Add(endereco);
        }
        
        cliente.Carrinho = new Carrinho();

        return cliente;
    }

    private RespostaServico ValidarPedidoDeRegistro(PedidoDeRegistro pedidoRegistro)
    {
        RespostaServico respostaServico = new();

        if (string.IsNullOrEmpty(pedidoRegistro?.Nome))
            respostaServico.AdicionarErro("Nome não pode ser vazio.");
        if (string.IsNullOrEmpty(pedidoRegistro?.Senha))
            respostaServico.AdicionarErro("Senha não pode ser vazia.");
        if (pedidoRegistro?.Senha?.Length < 8)
            respostaServico.AdicionarErro("Senha deve ter no mínimo 8 caracteres.");
        if (pedidoRegistro?.Enderecos.Count < 1)
            respostaServico.AdicionarErro("Ao menos um endereço deve ser cadastrado");

        var validacaoEndereco = ValidarEnderecosDePedidoDeRegistro(pedidoRegistro);
        
        if (!validacaoEndereco.Ok)
            respostaServico.AdicionarVariosErros(validacaoEndereco.MensagensErro);
        
        return respostaServico;
    }

    private RespostaServico ValidarEnderecosDePedidoDeRegistro(PedidoDeRegistro pedidoRegistro)
    {
        RespostaServico resposta = new();
        foreach (var endereco in pedidoRegistro.Enderecos)
        {
            if (string.IsNullOrEmpty(endereco?.Estado?.Trim()))
                resposta.AdicionarErro("O estado deve estar preenchido.");
            
            if (string.IsNullOrEmpty(endereco?.CEP?.Trim()))
                resposta.AdicionarErro("O CEP deve estar preenchido.");
            
            if (endereco?.CEP?.Length != 8)
                resposta.AdicionarErro("O CEP deve possui 8 dígitos.");
            
            if (string.IsNullOrEmpty(endereco?.Municipio?.Trim()))
                resposta.AdicionarErro("O Município deve estar preenchido.");

            if (string.IsNullOrEmpty(endereco?.Rua?.Trim()))
                resposta.AdicionarErro("A rua deve estar deve estar preenchida.");
        }
        return resposta;
    }
}