using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<ApplicationUser> FindAllAsync()
        {
            return _context.Users.AsNoTracking();
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

        public async Task<string> UpdateAsync(ApplicationUser user)
        {
            var result = await _context.UpdateAsync(user);
            
            if(!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();

                foreach(var errors in result.Errors)
                {
                    sb.AppendLine(errors.Description);
                }

                return sb.ToString();
            }

            return result.Succeeded.ToString();
        }
    }
}