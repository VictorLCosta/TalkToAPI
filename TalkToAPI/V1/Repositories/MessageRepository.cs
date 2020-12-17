using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.Database;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly TalkToContext _context;

        public MessageRepository(TalkToContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Message> FindAll(string userOneId, string userTwoId)
        {
            return _context.Messages.Where(m => (m.SenderId == userOneId || m.SenderId == userTwoId) && (m.ToId == userTwoId || m.ToId == userTwoId));
        }
    }
}