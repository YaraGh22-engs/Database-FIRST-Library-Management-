namespace Library_Management.IRepositories
{
    public interface ILIbraryRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
