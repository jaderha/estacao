﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using estacaoAPI.Models;
using estacaoAPI.DTO;

namespace estacaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeiturasController : ControllerBase
    {
        private readonly estacaoMetContext _context;

        public LeiturasController(estacaoMetContext context)
        {
            _context = context;
        }

        // GET: api/Leituras
        [HttpGet]
        public IEnumerable<Leitura> GetLeitura()
        {
            return _context.Leitura;
        }

        // GET: api/Leituras/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeitura([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var leitura = await _context.Leitura.FindAsync(id);

            if (leitura == null)
            {
                return NotFound();
            }

            return Ok(leitura);
        }

        // PUT: api/Leituras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeitura([FromRoute] int id, [FromBody] Leitura leitura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != leitura.IdLeitura)
            {
                return BadRequest();
            }

            _context.Entry(leitura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeituraExists(id))
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

        // POST: api/Leituras
        [HttpPost]
        public async Task<IActionResult> PostLeitura([FromBody] LeituraDTO leituraDTO)
        {

            if ( (string.Compare("6e1bbb5671b2dd6de8292c8374a1c01a", leituraDTO.Hash, false) != 0) )
            {
                return BadRequest(ModelState);
            }

            Leitura leitura = new Leitura();
            leitura.Data = System.DateTime.Now;
            leitura.Hash = leituraDTO.Hash;
            leitura.Temp = leituraDTO.Temp;
            leitura.Umid = leituraDTO.Umid;
            leitura.Veloc = leituraDTO.Veloc;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Leitura.Add(leitura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeitura", new { id = leitura.IdLeitura }, leitura);
        }

        // DELETE: api/Leituras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeitura([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var leitura = await _context.Leitura.FindAsync(id);
            if (leitura == null)
            {
                return NotFound();
            }

            _context.Leitura.Remove(leitura);
            await _context.SaveChangesAsync();

            return Ok(leitura);
        }

        private bool LeituraExists(int id)
        {
            return _context.Leitura.Any(e => e.IdLeitura == id);
        }
    }
}