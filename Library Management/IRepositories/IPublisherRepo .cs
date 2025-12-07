namespace Library_Management.IRepositories
{
    public interface IPublisherRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
