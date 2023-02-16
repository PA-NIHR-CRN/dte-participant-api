using System.Threading.Tasks;

namespace Application.Enrichers
{
    public interface IEnricher<T>
    {
        Task<T> Enrich(T source);
    }
}