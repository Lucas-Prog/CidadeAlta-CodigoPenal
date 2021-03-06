using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CidadeAlta_CodigoPenal.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using System.Net;

namespace CidadeAlta_CodigoPenal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CriminalCodeContext _context;
        private IConfiguration _config;
        public UsersController(CriminalCodeContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        /// <summary>
        /// Retorno Todos os Users registrados no Banco de Dados
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retorna um User com base em seu Id
        /// </summary>
        /// <param name="id">Id do user a ser pesquisado</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Edita um User ja criado no Banco de Dados
        /// </summary>
        /// <param name="id">Id do User a ser editado</param>
        /// <param name="user">Dados do User</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Cria um Novo User no Banco de Dados
        /// </summary>
        /// <param name="user">User a ser Criado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Deleta um User com base em seu Id
        /// </summary>
        /// <param name="id">Id do user a ser Deletado</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>
        /// Retorna um token JWT para acesso aos demais endpoints, caso o usuário esteja registrado no Banco de Dados
        /// </summary>
        /// <param name="loginDetails">Dados para o Login</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]User loginDetails)
        {
            bool validate = ValidateUser(loginDetails);

            if (validate)
            {
                var tokenString = GenerateJWT();
                return Ok(new { token = tokenString});
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateJWT()
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool ValidateUser(User userLogin)
        {
            var user = _context.Users.FirstOrDefault(r => r.UserName == userLogin.UserName);
            bool retorno = false;

            if (user != null)
            {
                if (user.UserName == userLogin.UserName)
                {
                    if (user.Password == userLogin.Password)
                    {
                        retorno = true;
                    }
                }
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }
    }
}
