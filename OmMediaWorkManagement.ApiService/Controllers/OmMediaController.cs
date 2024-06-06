using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Models;
using OmMediaWorkManagement.ApiService.ViewModels;

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
        public async Task<ActionResult> PostOmClient(OmClient omClient)
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

        #region JOBTODO

        [Route("AddJobTodo")]
        [HttpPost]
        public async Task<ActionResult> AddJobToDo(JobToDoViewModel jobToDoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] imageBytes = null;
            if (jobToDoViewModel.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await jobToDoViewModel.Image.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            JobToDo jobToDos = new JobToDo()
            {
                CompanyName = jobToDoViewModel.ComapnyName,
                Quantity = jobToDoViewModel.Quantity,
                Image = imageBytes,
                PostedBy = jobToDoViewModel.PostedBy,
                JobIsRunning = jobToDoViewModel.JobIsRunning,
                JobIsDeclained = jobToDoViewModel.JobIsDeclained,
                JobIsFinished = jobToDoViewModel.JobIsFinished,
                JobIsHold = jobToDoViewModel.JobIsHold,
                JobPostedDateTime = DateTime.Now,
            };

            _context.JobToDo.Add(jobToDos);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("GetJobToDoList")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobToDo>>> GetJobToDoAll()
        {
            return await _context.JobToDo.ToListAsync();
        }

        #endregion

        #region OmMachines

        [Route("AddMachine")]
        [HttpPost]
        public async Task<ActionResult> AddMachines(OmMachinesViewModel omMachinesViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            OmMachines omMachines = new OmMachines()
            {
                MachineName = omMachinesViewModel.MachineName,
                CreatedAt = DateTime.Now,
                IsRunning = omMachinesViewModel.IsRunning,
                MachineDescription = omMachinesViewModel.MachineDescription

            };

            _context.OmMachines.Add(omMachines);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("GetMachines")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OmMachines>>> GetMachine()
        {
            return await _context.OmMachines.ToListAsync();
        }

        //[HttpPut]
        //[Route("UpdateMachine")]
        //[Authorize]
        //public async Task<IActionResult> UpdateMachines(OmMachinesViewModel omMachinesViewModel)
        //{
        //    OmMachines stateMaster = new OmMachines()
        //    {
        //        //Id = omMachinesViewModel.,
        //        MachineName = omMachinesViewModel.MachineName,
        //        MachineDescription = omMachinesViewModel.MachineDescription,
        //        IsRunning = omMachinesViewModel.IsRunning

        //    };
        //    var state = await _EAMSService.UpdateStateById(stateMaster);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, OmMachines OmMachines)
        {
            if (id != OmMachines.Id)
            {
                return BadRequest();
            }

            _context.Entry(OmMachines).State = EntityState.Modified;

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
        #endregion

    }
    }
