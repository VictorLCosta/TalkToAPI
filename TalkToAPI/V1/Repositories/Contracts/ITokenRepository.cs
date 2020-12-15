using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface ITokenRepository
    {
        Task Create(Token token);
        Task<Token> GetToken(string refreshToken);
        Task Update(Token token);
    }
}