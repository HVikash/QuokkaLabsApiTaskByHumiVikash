namespace QuokkaLabsApi_By_HumiVikash.Repository.IRepository
{
    public interface IQuokkaLabsBaseRepository<T> where T : class
    {
        T Add(object obj);
        T Update(int id,object obj);
        T Delete(int obj);
        void Save();
    }
}
