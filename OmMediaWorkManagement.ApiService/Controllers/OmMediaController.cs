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
        public async Task<ActionResult> AddPostOmClient(OmClientViewModel omClientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OmClient omClient = new OmClient()
            {
                Name = omClientViewModel.Name,
                CompanyName = omClientViewModel.CompanyName,
                MobileNumber = omClientViewModel.MobileNumber,
                Email = omClientViewModel.Email,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            _context.OmClient.Add(omClient);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetAllClients")]
        public async Task<ActionResult<IEnumerable<OmClient>>> GetAllClients()
        {
            return await _context.OmClient.ToListAsync();
        }

        [HttpGet("GetClientById/{id}")]
        public async Task<ActionResult<OmClient>> GetClientById(int id)
        {
            var client = await _context.OmClient.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        [HttpPut("UpdateClient/{id}")]
        public async Task<ActionResult> UpdateClient(int id, OmClientViewModel omClientViewModel)
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

            return Ok();
        }

        [HttpDelete("DeleteClientById/{id}")]
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

        #endregion

        #region Work Details

        [HttpGet("GetWorksByClientId/{clientId}")]
        public async Task<ActionResult<IEnumerable<OmClientWork>>> GetWorksByClientId(int clientId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId)
                .ToListAsync();

            return clientWorks;
        }

        [HttpPost("AddWork")]
        public async Task<ActionResult> AddWork(OmClientWorkViewModel omClientWorkViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OmClientWork omClientWork = new OmClientWork()
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

            return Ok();
        }

        [HttpPut("UpdateWork/{id}")]
        public async Task<ActionResult> UpdateWork(int id, OmClientWorkViewModel omClientWorkViewModel)
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

            return Ok();
        }

        #endregion

        #region JobTodo

        [HttpPost("AddJobTodo")]
        public async Task<ActionResult> AddJobToDo(JobToDoViewModel jobToDoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<byte[]> imageBytesList = new List<byte[]>();
                if (jobToDoViewModel.Images != null)
                {
                    foreach (var formFile in jobToDoViewModel.Images)
                    {
                        if (formFile != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                imageBytesList.Add(memoryStream.ToArray());
                            }
                        }
                    }
                }

                byte[] imageBytes = imageBytesList.FirstOrDefault();

                JobToDo jobToDos = new JobToDo()
                {
                    CompanyName = jobToDoViewModel.ComapnyName,
                    Quantity = jobToDoViewModel.Quantity,
                    Image = imageBytes,
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
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetJobToDoList")]
        public async Task<ActionResult<IEnumerable<JobToDo>>> GetJobToDoAll()
        {
            return await _context.JobToDo.ToListAsync();
        }

        [HttpGet("GetJobsToDosById/{jboToDoId}")]
        public async Task<ActionResult<IEnumerable<JobToDo>>> GetJobsToDosById(int jboToDoId)
        {
            var jobToDoRecord = await _context.JobToDo
                .Where(work => work.Id == jboToDoId)
                .ToListAsync();

            return jobToDoRecord;
        }

        #endregion

        #region OmMachines

        [HttpPost("AddMachine")]
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

        [HttpGet("GetMachines")]
        public async Task<ActionResult<IEnumerable<OmMachines>>> GetMachine()
        {
            return await _context.OmMachines.ToListAsync();
        }

        [HttpPut]
        [Route("UpdateMachineById/{id}")]
        public async Task<ActionResult> UpdateMachine(int id, OmMachinesViewModel omMachinesViewModel)
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

            return Ok();
        }

        [HttpGet("GetMachineById/{machineId}")]
        public async Task<ActionResult<IEnumerable<OmMachines>>> GetMachineById(int machineId)
        {
            var machineRecord= await _context.OmMachines
                .Where(work => work.Id == machineId)
                .ToListAsync();

            return machineRecord;
        }
       

        #endregion
    }
}
