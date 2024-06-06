﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Models;

namespace OmMediaWorkManagement.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OmMediaController : ControllerBase
    {
        private readonly OmContext _context;

        public OmMediaController(OmContext context)
        {
            _context = context;
        }

        // GET: api/OmMedia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OmClient>>> GetOmClient()
        {
            return await _context.OmClient.ToListAsync();
        }

        // GET: api/OmMedia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OmClient>> GetOmClient(int id)
        {
            var omClient = await _context.OmClient.FindAsync(id);

            if (omClient == null)
            {
                return NotFound();
            }

            return omClient;
        }

        // PUT: api/OmMedia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOmClient(int id, OmClient omClient)
        {
            if (id != omClient.Id)
            {
                return BadRequest();
            }

            _context.Entry(omClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OmClientExists(id))
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

        // POST: api/OmMedia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OmClient>> PostOmClient(OmClient omClient)
        {
            _context.OmClient.Add(omClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOmClient", new { id = omClient.Id }, omClient);
        }

        // DELETE: api/OmMedia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOmClient(int id)
        {
            var omClient = await _context.OmClient.FindAsync(id);
            if (omClient == null)
            {
                return NotFound();
            }

            _context.OmClient.Remove(omClient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OmClientExists(int id)
        {
            return _context.OmClient.Any(e => e.Id == id);
        }
    }
}
