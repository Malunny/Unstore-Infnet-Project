using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Enums;
using UnstoreProjectADS.Core.Extensoes;
using UnstoreProjectADS.Core.Services.Compartilhado;
using UnstoreProjectADS.Dados;
using UnstoreProjectADS.Pages.Pedidos;
using UnstoreProjectADS.Pages.Pedidos.Registrar;

namespace UnstoreProjectADS.Core.Services;

public class ServicoConta
{
    private readonly ContextoDados _contexto;
    public ServicoConta(ContextoDados context)
    {
        _contexto = context;
    }

    public async Task<RespostaServicoComDados<ClaimsPrincipal?>> IniciarSessao(string email, string password)
    {
        var resposta = new RespostaServicoComDados<ClaimsPrincipal?>();
        var erroEmailOuSenha = "Email ou senha incorretos.";
        
        Usuario? usuario = await _contexto.Clientes.FirstOrDefaultAsync(cliente => cliente.Email == email);
        
        if (usuario == null)
            usuario = await _contexto.Funcionarios
                .Include(funcionario => funcionario.Cargo)
                .FirstOrDefaultAsync(funcionario => funcionario.Email == email);

        if (usuario == null)
        {
            resposta.AdicionarErro(erroEmailOuSenha);
            return resposta;
        }

        if (usuario.Senha != password)
        {
            resposta.AdicionarErro(erroEmailOuSenha);
            return resposta;
        }

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
        
        resposta.Dados = new ClaimsPrincipal(identidade);

        return resposta;
    }

    public async Task<RespostaServico> RegistrarClienteAsync(PedidoDeRegistroCliente pedidoRegistro)
    {
        RespostaServico respostaServico = new();
        
        var validacaoPedidoRegistro = await ValidarPedidoDeRegistro(pedidoRegistro);
        var validacaoEndereco = ValidarEnderecosDePedidoDeRegistro(pedidoRegistro);

        if (!validacaoEndereco.Ok || !validacaoEndereco.Ok)
        {
            respostaServico.AdicionarVariosErros(validacaoEndereco.MensagensErro);
            respostaServico.AdicionarVariosErros(validacaoPedidoRegistro.MensagensErro);

            return respostaServico;
        }        
        
        var usuarioJaExiste = await _contexto.Usuarios.AnyAsync(u => u.Email == pedidoRegistro.Email);

        if (usuarioJaExiste)
        {
            respostaServico.AdicionarErro("Já existe usuário com esse email.");
            return respostaServico;
        }
        
        var cliente = AlimentarDadosCliente(pedidoRegistro);

        await _contexto.Clientes.AddAsync(cliente);
        await _contexto.SaveChangesAsync();
        
        respostaServico.AdicionarMensagem("Cadastrado com sucesso!");
        
        return respostaServico;
    }

    public async Task<RespostaServico> RegistrarFuncionarioAsync(PedidoRegistroFuncionario pedidoRegistro)
    {
        RespostaServico respostaServico = new();

        var validacaoPedidoRegistro = await ValidarPedidoDeRegistro(pedidoRegistro);
        var validacaoCargos = await ValidarCargosDePedidoDeRegistroAsync(pedidoRegistro);

        if (!validacaoCargos.Ok || !validacaoCargos.Ok)
        {
            respostaServico.AdicionarVariosErros(validacaoPedidoRegistro.MensagensErro);
            respostaServico.AdicionarVariosErros(validacaoCargos.MensagensErro);

            return respostaServico;
        }

        var funcionario = pedidoRegistro.ConverterParaFuncionario();
        
        await _contexto.Funcionarios.AddAsync(funcionario);
        await _contexto.SaveChangesAsync();

        return respostaServico;
    }

    private Cliente AlimentarDadosCliente(PedidoDeRegistroCliente pedidoRegistro)
    {
        Cliente cliente = new();
        
        cliente.Nome = pedidoRegistro.Nome;
        cliente.Email = pedidoRegistro.Email.ToLower();
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

    private async Task<RespostaServico> ValidarPedidoDeRegistro(PedidoRegistro pedidoRegistro)
    {
        RespostaServico respostaServico = new();

        bool nomeVazio = string.IsNullOrEmpty(pedidoRegistro?.Nome);
        bool emailVazio = string.IsNullOrEmpty(pedidoRegistro?.Email);
        bool senhaVazia = string.IsNullOrEmpty(pedidoRegistro?.Senha);
        bool senhaMenorQueIdeal = pedidoRegistro?.Senha?.Length < 8;
        
        if (nomeVazio)
            respostaServico.AdicionarErro("Nome não pode ser vazio.");
        
        if (emailVazio)
            respostaServico.AdicionarErro("Email não pode ser vazio.");
        
        if (senhaVazia)
            respostaServico.AdicionarErro("Senha não pode ser vazia.");
        
        if (senhaMenorQueIdeal)
            respostaServico.AdicionarErro("Senha deve ter no mínimo 8 caracteres.");

        if (!emailVazio)
        {
            var usuarioComMesmoEmailExiste = await _contexto.Usuarios
                .AnyAsync(usuario => usuario.Email == pedidoRegistro.Email);
            
            if (usuarioComMesmoEmailExiste)
                respostaServico.AdicionarErro("Esse e-mail está em uso.");
        }
        
        return respostaServico;
    }

    public async Task<RespostaServico> ValidarCargosDePedidoDeRegistroAsync(PedidoRegistroFuncionario pedidoRegistro)
    {
        RespostaServico respostaServico = new();
        
        var cargoId = pedidoRegistro.CargoId;
        
        var cargo = await _contexto.Cargos.FirstOrDefaultAsync(cargo => cargo.Id == cargoId);

        if (cargo == null)
        {
            respostaServico.AdicionarErro("Não existe cargo com esse id.");
            return respostaServico;
        }

        if (cargo.Nome == Cargos.Cliente.PegarNome())
        {
            respostaServico.AdicionarErro("Não se pode cadastrar um funcionácio com cargo de cliente.");
            return respostaServico;
        }
        
        return respostaServico;
    }

    private RespostaServico ValidarEnderecosDePedidoDeRegistro(PedidoDeRegistroCliente pedidoRegistro)
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