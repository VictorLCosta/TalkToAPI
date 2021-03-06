using System.Collections.Generic;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface IMessageRepository
    {
        IEnumerable<Message> FindAll(string userOneId, string userTwoId);
        Task<Message> FindMessageAsync(int id);
        Task CreateAsync(Message message);
        Task UpdateAsync(Message message);
    }
}