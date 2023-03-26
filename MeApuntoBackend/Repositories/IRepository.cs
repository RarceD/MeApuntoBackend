public interface IRepository<T> where T : class
{
    public IEnumerable<T> GetAll();
    public T GetById(int id);
    public void Add(T item);
    public void Remove(T item);
    public void Update(T item);
}
