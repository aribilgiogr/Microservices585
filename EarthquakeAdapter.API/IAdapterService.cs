public interface IAdapterService
{
    Task<IEnumerable<EartquakeModel>> GetEartquakeModelAsync();
}
