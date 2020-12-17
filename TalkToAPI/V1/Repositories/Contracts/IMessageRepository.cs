using System.Collections.Generic;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface IMessageRepository
    {
        IEnumerable<Message> FindAllAsync(string userOneId, string userTwoId);
        Task CreateAsync(Message message);
    }
}