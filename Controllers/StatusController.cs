using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CidadeAlta_CodigoPenal.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace CidadeAlta_CodigoPenal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly CriminalCodeContext _context;

        public StatusController(CriminalCodeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os Status do Banco
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatus([FromServices] CriminalCodeContext context)
        {
            var status = await context.Status.ToListAsync();

            return status;
        }

        /// <summary>
        /// Retorno um Status, com base em seu Id
        /// </summary>
        /// <param name="id">Id do Status a ser Pesquisado</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Status>> GetStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        /// <summary>
        /// Edita um Status ja criado no Banco de Dados
        /// </summary>
        /// <param name="id">Id do Status a ser editado</param>
        /// <param name="status">Valores do "novo" Status</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(int id, Status status)
        {
            if (id != status.Id)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
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
        /// Cria um novo Status
        /// </summary>
        /// <param name="status">Status a ser criado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Status>> PostStatus(Status status)
        {
            _context.Status.Add(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatus", new { id = status.Id }, status);
        }

        /// <summary>
        /// Deleta um Status com base em seu Id
        /// </summary>
        /// <param name="id">Id do Status a ser Deletado</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.Id == id);
        }
    }
}
