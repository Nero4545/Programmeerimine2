namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<Result<List<BeerType>>> List();
        Task<Result> Save(BeerType item);
        Task<Result> Delete(int id);
    }
}
