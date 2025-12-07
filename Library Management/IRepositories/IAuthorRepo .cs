namespace Library_Management.IRepositories
{
    public interface IAuthorRepo
    {
        Task<bool> CheckExistence(int id);

    }
}
