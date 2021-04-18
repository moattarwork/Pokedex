using System.Threading.Tasks;
using Refit;

namespace Pokedex.Core.Clients.FunTranslation
{
    public interface IFunTranslationClient
    {
        [Post("/yoda")]
        Task<FunTranslationResponse> GetYodaTranslationAsync([Body] FunTranslationRequest request);        
        
        [Post("/shakespeare")]
        Task<FunTranslationResponse> GetShakespeareTranslationAsync([Body] FunTranslationRequest request);
    }
}