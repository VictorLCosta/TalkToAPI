using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TalkToAPI.Database;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _context;

        public UserRepository(UserManager<ApplicationUser> context)
        {
            _context = context;
        }

        public async Task CreateAsync(ApplicationUser user, string password)
        {
            var result = await _context.CreateAsync(user, password);
            if(!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder(); 
                foreach (var erro in result.Errors)
                {
                    sb.AppendLine(erro.Description);
                }
                
                throw new Exception($"Usuário não cadastrado! {sb.ToString()}");
            }
        }

        public async Task<ApplicationUser> FindAsync(string email, string password)
        {
            var user = await _context.FindByEmailAsync(email);
            if(await _context.CheckPasswordAsync(user, password))
            {
                return user;
            }
            else
            {
                throw new Exception("Usuário nã encotrado!");
            }
        }

        public async Task<ApplicationUser> FindAsync(string id)
        {
            return await _context.FindByIdAsync(id);
        }
    }
}