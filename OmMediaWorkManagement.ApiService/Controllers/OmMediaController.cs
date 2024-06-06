﻿using System;
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


        #region Client Details

        [HttpPost]
        [Route("AddClient")]
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
       
        [HttpGet]
        [Route("GetAllClients")]
        public async Task<ActionResult<IEnumerable<OmClient>>> GetAllClients()
        {
            return await _context.OmClient.ToListAsync();
        }
        [HttpPut]
        [Route("UpdateClient/{id}")]
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
        #endregion


        #region Work Details

        [HttpGet]
        [Route("GetWorksByClientId/{clientId}")]
        public async Task<ActionResult<IEnumerable<OmClientWork>>> GetWorksByClientId(int clientId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId)
                .ToListAsync();

            return clientWorks;
        }

        [HttpPost]
        [Route("AddWork")]
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

        [HttpPut]
        [Route("UpdateWork/{id}")]
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
        [Route("AddJobTodo")]
        [HttpPost]
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

                // Here, we're assuming that you'll handle multiple images somehow.
                // For simplicity, let's just take the first image.
                byte[] imageBytes = imageBytesList.FirstOrDefault();

                JobToDo jobToDos = new JobToDo()
                {
                    CompanyName = jobToDoViewModel.ComapnyName,
                    Quantity = jobToDoViewModel.Quantity,
                    Image = imageBytes,
                    //PostedBy = jobToDoViewModel.PostedBy,
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
        [Route("updateMachineById")]
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
