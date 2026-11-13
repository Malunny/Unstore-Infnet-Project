using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Pages.Requests;

namespace UnstoreProjectADS.Services;

public class ServicoConta
{
    private readonly AppDbContext _contexto;
    public ServicoConta(AppDbContext context)
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
            new Claim(ClaimTypes.Name, email),
        };

        var nomeCargo = "Cliente";

        if (usuario is Funcionario funcionario)
            nomeCargo = funcionario.Cargo.Nome;
        
        claims.Add(new Claim(ClaimTypes.Role, nomeCargo));

        var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        return new ClaimsPrincipal(identidade);
    }

    public async Task<bool> Registrar(RegisterRequest pedidoRegistro)
    {
        Cliente cliente = new();
        
        cliente.Nome = pedidoRegistro.Nome;
        cliente.Email = pedidoRegistro.Email;
        cliente.Senha = pedidoRegistro.Senha;

        if (cliente.Nome is null || cliente.Email is null || cliente.Senha is null)
        {
            return false;
        }

        var endereco = new Endereco();

        endereco.Rua = pedidoRegistro.Rua;
        endereco.CEP = pedidoRegistro.CEP;
        endereco.Complemento = pedidoRegistro.Complemento;
        endereco.Estado = pedidoRegistro.Estado;
        endereco.Municipio = pedidoRegistro.Municipio;
        endereco.Numero = pedidoRegistro.Numero;
        
        if (endereco.Estado is null || endereco.Municipio is null 
                                    || endereco.Rua is null ||
                                    endereco.Complemento is null ||
                                    endereco.CEP is null ||
                                    endereco.Numero < 1)
        {
            return false;
        }
        
        cliente.Enderecos.Add(endereco);
        cliente.Carrinho = new Carrinho();

        await _contexto.Clientes.AddAsync(cliente);
        await _contexto.SaveChangesAsync();
        
        return true;
    }
}