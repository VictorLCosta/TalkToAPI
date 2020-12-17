using TalkToAPI.V1.Models; 
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task CreateAsync(ApplicationUser user, string password);
        IEnumerable<ApplicationUser> FindAllAsync();
        Task<ApplicationUser> FindAsync(string email, string password);
        Task<ApplicationUser> FindAsync(string id);
        Task<string> UpdateAsync(ApplicationUser user);
    }
}