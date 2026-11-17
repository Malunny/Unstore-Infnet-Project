namespace UnstoreProjectADS.Dados.Contratos;

public interface IRepositorio<T>
{
    Task Deletar(T entidade);
    Task Inserir(T entidade);
    Task InserirVarias(IEnumerable<T> entidades);
    Task<IEnumerable<T>> ObterTodos();
    Task Salvar(T entidade);
}