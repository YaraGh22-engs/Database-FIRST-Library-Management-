namespace Library_Management.IRepositories
{
    public interface ICategoryRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
