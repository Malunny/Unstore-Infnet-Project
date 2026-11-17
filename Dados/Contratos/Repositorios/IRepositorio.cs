namespace UnstoreProjectADS.Dados.Contratos.Repositorios;

public interface IRepositorio<T> where T : class
{
    Task<ICollection<T>> ObterTodosAsync();
    Task<T?> ObterPorIdAsync(int id);
    Task<bool> SalvarAsync(T entidade);
    Task<bool> DeletarPorIdAsync(int id);
}