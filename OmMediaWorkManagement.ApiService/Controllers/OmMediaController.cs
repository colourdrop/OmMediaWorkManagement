using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        #region Client Details

        [HttpPost("AddClient")]
        public async Task<IActionResult> AddClient(OmClientViewModel omClientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var omClient = new OmClient
            {
                Name = omClientViewModel.Name,
                CompanyName = omClientViewModel.CompanyName,
                MobileNumber = omClientViewModel.MobileNumber,
                Email = omClientViewModel.Email,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.OmClient.Add(omClient);
            await _context.SaveChangesAsync();

            return Ok($"{omClientViewModel.Name} successfully added");
        }

        [HttpGet("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _context.OmClient.ToListAsync();
            return Ok(clients);
        }

        [HttpGet("GetClientById/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _context.OmClient.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPut("UpdateClient/{id}")]
        public async Task<IActionResult> UpdateClient(int id, OmClientViewModel omClientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingClient = await _context.OmClient.FindAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = omClientViewModel.Name;
            existingClient.CompanyName = omClientViewModel.CompanyName;
            existingClient.MobileNumber = omClientViewModel.MobileNumber;
            existingClient.Email = omClientViewModel.Email;

            _context.OmClient.Update(existingClient);
            await _context.SaveChangesAsync();

            return Ok(existingClient);
        }

        [HttpDelete("DeleteClientById/{id}")]
        public async Task<IActionResult> DeleteClientById(int id)
        {
            var client = await _context.OmClient.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.OmClient.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Work Details

        [HttpGet("GetWorksByClientId/{clientId}")]
        public async Task<IActionResult> GetWorksByClientId(int clientId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId)
                .ToListAsync();

            return Ok(clientWorks);
        }

        [HttpPost("AddWork")]
        public async Task<IActionResult> AddWork(OmClientWorkViewModel omClientWorkViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var omClientWork = new OmClientWork
            {
                OmClientId = omClientWorkViewModel.ClientId,
                WorkDate = omClientWorkViewModel.WorkDate,
                WorkDetails = omClientWorkViewModel.WorkDetails,
                PrintCount = omClientWorkViewModel.PrintCount,
                Price = omClientWorkViewModel.Price,
                Total = omClientWorkViewModel.PrintCount * omClientWorkViewModel.Price,
                Remarks = omClientWorkViewModel.Remarks
            };

            _context.OmClientWork.Add(omClientWork);
            await _context.SaveChangesAsync();

            return Ok(omClientWork);
        }

        [HttpPut("UpdateWork/{id}")]
        public async Task<IActionResult> UpdateWork(int id, OmClientWorkViewModel omClientWorkViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingWork = await _context.OmClientWork.FindAsync(id);
            if (existingWork == null)
            {
                return NotFound();
            }

            existingWork.WorkDate = omClientWorkViewModel.WorkDate;
            existingWork.WorkDetails = omClientWorkViewModel.WorkDetails;
            existingWork.PrintCount = omClientWorkViewModel.PrintCount;
            existingWork.Price = omClientWorkViewModel.Price;
            existingWork.Total = omClientWorkViewModel.PrintCount * omClientWorkViewModel.Price;
            existingWork.Remarks = omClientWorkViewModel.Remarks;

            _context.OmClientWork.Update(existingWork);
            await _context.SaveChangesAsync();

            return Ok(existingWork);
        }

        #endregion

        #region JobTodo

        [HttpPost("AddJobTodo")]
        public async Task<IActionResult> AddJobTodo(JobToDoViewModel jobToDoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var jobToDo = new JobToDo
                {
                    CompanyName = jobToDoViewModel.ComapnyName,
                    Quantity = jobToDoViewModel.Quantity,
                    JobIsRunning = jobToDoViewModel.JobIsRunning,
                    JobIsDeclained = jobToDoViewModel.JobIsDeclained,
                    JobIsFinished = jobToDoViewModel.JobIsFinished,
                    JobIsHold = jobToDoViewModel.JobIsHold,
                    JobPostedDateTime = DateTime.UtcNow,
                };

                if (jobToDoViewModel.Images != null)
                {
                    foreach (var formFile in jobToDoViewModel.Images)
                    {
                        if (formFile != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                byte[] imageBytes = memoryStream.ToArray();

                                jobToDo.JobImages.Add(new JobImages
                                {
                                    Image = imageBytes
                                });
                            }
                        }
                    }
                }

                _context.JobToDo.Add(jobToDo);
                  _context.SaveChanges();
                return Ok("Job Posted Successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetJobToDoList")]
        public async Task<IActionResult> GetJobToDoList()
        {
            var jobList = await _context.JobToDo
                .Include(d => d.JobImages)
                .ToListAsync();

            var jobToDoResponses = jobList.Select(job => new JobToDoResponseViewModel
            {
                Id = job.Id,
                CompanyName = job.CompanyName,
                Quantity = job.Quantity,
                JobIsRunning = job.JobIsRunning,
                JobIsDeclained = job.JobIsDeclained,
                JobIsFinished = job.JobIsFinished,
                JobIsHold = job.JobIsHold,
                JobPostedDateTime = job.JobPostedDateTime,
                Images = job.JobImages.Select(img => Convert.ToBase64String(img.Image)).ToList()
            }).ToList();

            return Ok(jobToDoResponses);
        }


        [HttpGet("GetJobsToDosById/{jobToDoId}")]
        public async Task<IActionResult> GetJobsToDosById(int jobToDoId)
        {
            var jobToDoRecord = await _context.JobToDo
                .Include(j => j.JobImages)
                .FirstOrDefaultAsync(j => j.Id == jobToDoId);

            if (jobToDoRecord == null)
            {
                return NotFound();
            }

            return Ok(jobToDoRecord);
        }

        #endregion

        #region OmMachines

        [HttpPost("AddMachine")]
        public async Task<IActionResult> AddMachine(OmMachinesViewModel omMachinesViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var omMachine = new OmMachines
            {
                MachineName = omMachinesViewModel.MachineName,
                CreatedAt = DateTime.UtcNow,
                IsRunning = omMachinesViewModel.IsRunning,
                MachineDescription = omMachinesViewModel.MachineDescription
            };

            _context.OmMachines.Add(omMachine);
            await _context.SaveChangesAsync();
            return Ok(omMachine);
        }

        [HttpGet("GetMachines")]
        public async Task<IActionResult> GetMachines()
        {
            var machinesList = await _context.OmMachines.ToListAsync();
            return Ok(machinesList);
        }

        [HttpPut("UpdateMachineById/{id}")]
        public async Task<IActionResult> UpdateMachineById(int id, OmMachinesViewModel omMachinesViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMachine = await _context.OmMachines.FindAsync(id);
            if (existingMachine == null)
            {
                return NotFound();
            }

            existingMachine.IsRunning = omMachinesViewModel.IsRunning;
            existingMachine.MachineDescription = omMachinesViewModel.MachineDescription;
            existingMachine.MachineName = omMachinesViewModel.MachineName;

            _context.OmMachines.Update(existingMachine);
            await _context.SaveChangesAsync();

            return Ok(existingMachine);
        }

        [HttpGet("GetMachineById/{machineId}")]
        public async Task<IActionResult> GetMachineById(int machineId)
        {
            var machineRecord = await _context.OmMachines
                .FirstOrDefaultAsync(m => m.Id == machineId);

            if (machineRecord == null)
            {
                return NotFound();
            }

            return Ok(machineRecord);
        }

        #endregion
    }
}
