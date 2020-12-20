using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using TalkToAPI.V1.Repositories.Contracts;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace TalkToAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private IConfiguration _conf;

        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly SignInManager<ApplicationUser> _manager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepo;

        public UserController(IConfiguration conf, IMapper mapper, IUserRepository repository, SignInManager<ApplicationUser> manager, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepo)
        {
            _conf = conf;
            _mapper = mapper;
            _repository = repository;
            _tokenRepo = tokenRepo;
            _manager = manager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]DTOUser _user)
        {
            if(_user == null)
            {
                return BadRequest();
            }

            ModelState.Remove("Name");
            ModelState.Remove("PasswordConfirm");

            if(ModelState.IsValid)
            {
                ApplicationUser user = await _repository.FindAsync(_user.Email, _user.Password);
                if(user != null)
                {
                    var token = GenerateToken(user);

                    var tokenModel = new Token
                    {
                        RefreshToken = token.RefreshToken,
                        ExpirationDate = token.ExpirationDate,
                        ExpirationRefreshToken = token.ExpirationRefreshToken,
                        User = user,
                        Created = DateTime.Now,
                        Used = false
                    };

                    await _tokenRepo.Create(tokenModel);
                    return Ok(token);
                }
                else
                {
                    return NotFound("Usuário não localizado!");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [HttpPost("", Name = "Register")]
        public async Task<IActionResult> Register([FromBody]DTOUser DTOuser)
        {
            if(DTOuser == null)
            {
                return BadRequest();
            }

            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();

                user.FullName = DTOuser.Name;
                user.UserName = DTOuser.Email;
                user.Email = DTOuser.Email;

                await _repository.CreateAsync(user, DTOuser.Password);

                var dtouser = _mapper.Map<ApplicationUser, DTOUser>(user);
                 dtouser.Links.Add(new DTOLink("create", Url.Link("Register", new { id = dtouser.Id } ), "POST"));
                dtouser.Links.Add(new DTOLink("self", Url.Link("FindUser", new { id = dtouser.Id } ), "GET"));
                dtouser.Links.Add(new DTOLink("update", Url.Link("Update", new { id = dtouser.Id } ), "PUT"));

                return Ok(dtouser);
            }
            else
            {
                return UnprocessableEntity();
            }
        }
        
        [HttpGet("", Name = "FindAll")]
        public IActionResult FindAll()
        {
            var users = _repository.FindAllAsync().ToList();
            
            var dtousers = _mapper.Map<List<ApplicationUser>, List<DTOUser>>(users);

            foreach(var user in dtousers)
            {
                user.Links.Add(new DTOLink("self", Url.Link("FindUser", new { id = user.Id }), "GET"));
            }

            var list = new DTOList<DTOUser> { List = dtousers};
            list.Links.Add(new DTOLink("self", Url.Link("FindAll", null), "GET"));

            return Ok(list);
        }

        [HttpGet("{id}", Name = "FindUser")]
        public async Task<IActionResult> FindUser(string id)
        {
            var dtouser = _mapper.Map<ApplicationUser, DTOUser>(await _repository.FindAsync(id));

            dtouser.Links.Add(new DTOLink("self", Url.Link("FindUser", new { id = dtouser.Id } ), "GET"));
            dtouser.Links.Add(new DTOLink("update", Url.Link("Update", new { id = dtouser.Id } ), "PUT"));

            return Ok(dtouser);
        }

        [Authorize]
        [HttpPut("{id}", Name = "Update")]
        public async Task<IActionResult> Update(string id, [FromBody]DTOUser dtouser)
        {
            /*if(_userManager.GetUserAsync(HttpContext.User).Result.Id != id)
            {
                return Forbid();
            }*/

            if(dtouser == null)
            {
                return BadRequest();
            }

            if(ModelState.IsValid)
            {
                var user = await _repository.FindAsync(dtouser.Email, dtouser.Password);
                
                if(user != null)
                {
                    var result = await _repository.UpdateAsync(user);
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, dtouser.Password);

                    if(result != "True")
                    {
                        return UnprocessableEntity(result);
                    }
                    
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return UnprocessableEntity();
            }
        }


        [HttpPost("Renovate")]
        public async Task<IActionResult> Renovate([FromBody]DTOToken _token)
        {
            var refreshToken = await _tokenRepo.GetToken(_token.RefreshToken);

            if(refreshToken == null)
            {
                return NotFound();
            }

            refreshToken.Updated = DateTime.Now;
            refreshToken.Used = true;
            await _tokenRepo.Update(refreshToken);

            //Gerando token
            var user = await _repository.FindAsync(refreshToken.UserId);

            return BuildNewToken(user);
        }


        private DTOToken GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf.GetValue<string>("Key")));

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Issuer = null,
                Audience = null,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);
            var expirationToken = DateTime.UtcNow.AddDays(15);

            return new DTOToken { Token = tokenString, ExpirationDate = tokenDescriptor.Expires.Value, ExpirationRefreshToken = expirationToken };
        }

        private IActionResult BuildNewToken(ApplicationUser user)
        {
            var token = GenerateToken(user);

            //Salvar o Token no Banco
            var tokenModel = new Token()
            {
                RefreshToken = token.RefreshToken,
                ExpirationDate = token.ExpirationDate,
                ExpirationRefreshToken = token.ExpirationRefreshToken,
                User = user,
                Created = DateTime.Now,
                Used = false
            };

            _tokenRepo.Create(tokenModel);
            return Ok(token);
        }
    }
}