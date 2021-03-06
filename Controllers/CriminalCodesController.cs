using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CidadeAlta_CodigoPenal.Models;
using Microsoft.AspNetCore.Authorization;
using CidadeAlta_CodigoPenal.Wrappers;

namespace CidadeAlta_CodigoPenal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CriminalCodesController : ControllerBase
    {
        private readonly CriminalCodeContext _context;
        private CriminalCode criminalCode = new CriminalCode();

        public CriminalCodesController(CriminalCodeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os CriminalCodes Registrados no Banco ou alguns dependendo dos parametros passados
        /// </summary>
        /// <param name="orderBy">Campo que será ordenado(Ordem Crescente)</param>
        /// <param name="pagFilter">pagFilter Recebe os valores PageNumber e PageSize, para realizar a paginação</param>
        /// <param name="filter">Retorna os resultados do banco que tenham o valor passado. Deve ser informado também qual campo a ser pesquisado.
        /// Exemplo: 'Name_Roberto'. Onde [Name] é o campo a ser pesquisado e [Roberto] o valor a ser pesquisado</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCriminalCodes([FromQuery] string orderBy, [FromQuery] PaginationFilter pagFilter, [FromQuery] string filter)
        {
            var filterSplit = filter == null ? null : new Filter(filter);
            var validFilter = new PaginationFilter(pagFilter.PageNumber, pagFilter.PageSize);
            var pagedData = await (from CriminalCode in _context.CriminalCodes
                               join Status in _context.Status on CriminalCode.StatusId equals Status.Id
                               join User in _context.Users on CriminalCode.CreateUserId equals User.Id
                               join u2 in _context.Users on CriminalCode.UpdateUserId equals u2.Id into gj
                               from x in gj.DefaultIfEmpty()
                               select new ResponseCriminalCode
                               {
                                   Id = CriminalCode.Id,
                                   Name = CriminalCode.Name,
                                   Description = CriminalCode.Description,
                                   Penalty = CriminalCode.Penalty,
                                   PrisonTime = CriminalCode.PrisonTime,
                                   Status = Status.Name,
                                   CreateDate = CriminalCode.CreateDate,
                                   UpdateDate = CriminalCode.UpdateDate,
                                   CreateUser = User.UserName,
                                   UpdateUser = (x.UserName == null ? "0" : x.UserName)
                               }).ToListAsync();
            int totalRecord = pagedData.Count;

            if (filterSplit != null)
            {
                if (filterSplit.valueType == "Name" || filterSplit.valueType == "name") pagedData = pagedData.Where(x => x.Name == filterSplit.value).ToList();
                else if (filterSplit.valueType == "Description" || filterSplit.valueType == "description") pagedData = pagedData.Where(x => x.Description == filterSplit.value).ToList();
                else if (filterSplit.valueType == "Penalty" || filterSplit.valueType == "penalty") pagedData = pagedData.Where(x => x.Penalty == Decimal.Parse(filterSplit.value)).ToList();
                else if (filterSplit.valueType == "PrisonTime" || filterSplit.valueType == "PrisonTime") pagedData = pagedData.Where(x => x.PrisonTime == int.Parse(filterSplit.value)).ToList();
                else if (filterSplit.valueType == "Status" || filterSplit.valueType == "status") pagedData = pagedData.Where(x => x.Status == filterSplit.value).ToList();
                else if (filterSplit.valueType == "CreateDate" || filterSplit.valueType == "createDate") pagedData = pagedData.Where(x => x.CreateDate == DateTime.Parse(filterSplit.value)).ToList();
                else if (filterSplit.valueType == "UpdateDate" || filterSplit.valueType == "updateDate") pagedData = pagedData.Where(x => x.UpdateDate == DateTime.Parse(filterSplit.value)).ToList();
                else if (filterSplit.valueType == "CreateUser" || filterSplit.valueType == "createUser") pagedData = pagedData.Where(x => x.CreateUser == filterSplit.value).ToList();
                else if (filterSplit.valueType == "UpdateUser" || filterSplit.valueType == "updateUser") pagedData = pagedData.Where(x => x.UpdateUser == filterSplit.value).ToList();
            }                  

            if (orderBy == "Name" || orderBy == "name") { pagedData = pagedData.OrderBy(x => x.Name).ToList(); }
            else if (orderBy == "Description" || orderBy == "description") pagedData = pagedData.OrderBy(x => x.Description).ToList();
            else if (orderBy == "Penalty" || orderBy == "penalty") pagedData = pagedData.OrderBy(x => x.Penalty).ToList();
            else if (orderBy == "PrisonTime" || orderBy == "PrisonTime") pagedData = pagedData.OrderBy(x => x.PrisonTime).ToList();
            else if (orderBy == "Status" || orderBy == "status") pagedData = pagedData.OrderBy(x => x.Status).ToList();
            else if (orderBy == "CreateDate" || orderBy == "createDate") pagedData = pagedData.OrderBy(x => x.CreateDate).ToList();
            else if (orderBy == "UpdateDate" || orderBy == "updateDate") pagedData = pagedData.OrderBy(x => x.UpdateDate).ToList();
            else if (orderBy == "CreateUser" || orderBy == "createUser") pagedData = pagedData.OrderBy(x => x.CreateUser).ToList();
            else if (orderBy == "UpdateUser" || orderBy == "updateUser") pagedData = pagedData.OrderBy(x => x.UpdateUser).ToList();

            pagedData = pagedData
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();

            return Ok(new Pagination<List<ResponseCriminalCode>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalRecord));
        }

        /// <summary>
        /// Retorna um CriminalCode, com base em seu Id
        /// </summary>
        /// <param name="id">Id do CriminalCode</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CriminalCode>> GetCriminalCode(int id)
        {
            var criminalCode = await _context.CriminalCodes.FindAsync(id);

            if (criminalCode == null)
            {
                return NotFound();
            }

            return criminalCode;
        }

        /// <summary>
        /// Edita um CriminalCode ja criado no Banco de Dados
        /// </summary>
        /// <param name="id">Id do CriminalCode a ser editado</param>
        /// <param name="criminalCode">Valore do Criminal Code</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCriminalCode(int id, CriminalCode criminalCode)
        {
            if (id != criminalCode.Id)
            {
                return BadRequest();
            }

            _context.Entry(criminalCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriminalCodeExists(id))
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
        /// Cria um CriminalCode
        /// </summary>
        /// <param name="criminalCode">CriminalCode a ser Criado</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CriminalCode>> PostCriminalCode(CriminalCode criminalCode)
        {
            _context.CriminalCodes.Add(criminalCode);
            await _context.SaveChangesAsync();
            var response = CreatedAtAction("GetCriminalCode", new { id = criminalCode.Id }, criminalCode);

            return response;
        }

        /// <summary>
        /// Deleta um CriminalCode
        /// </summary>
        /// <param name="id">Id do CriminalCode a ser Deletado</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCriminalCode(int id)
        {
            var criminalCode = await _context.CriminalCodes.FindAsync(id);
            if (criminalCode == null)
            {
                return NotFound();
            }

            _context.CriminalCodes.Remove(criminalCode);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CriminalCodeExists(int id)
        {
            return _context.CriminalCodes.Any(e => e.Id == id);
        }
    }
}
