using TalkToAPI.V1.Models; 
using System.Threading.Tasks;
using System.Text;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task CreateAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindAsync(string email, string password);
        Task<ApplicationUser> FindAsync(string id);
        Task<string> UpdateAsync(ApplicationUser user);
    }
}