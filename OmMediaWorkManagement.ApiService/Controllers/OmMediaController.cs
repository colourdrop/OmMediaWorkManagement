using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Models;
using OmMediaWorkManagement.ApiService.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Net.Http;

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
                .Where(work => work.OmClientId == clientId && work.IsDeleted == false)
                .ToListAsync();

            return Ok(clientWorks);
        }
        [HttpGet("DeleteWorksByClientId")]
        public async Task<IActionResult> DeleteWorksByClientId(int clientId, int clientWorkId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId && work.Id == clientWorkId)
                .FirstOrDefaultAsync();

            _context.SaveChanges();
            return Ok("Client record Deleted");
        }
        [HttpPut("UpdatePaymentWorksStatusByClientId")]
        public async Task<IActionResult> DeleteWorksByClientId(int clientId, int clientWorkId, bool isPaid)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId && work.Id == clientWorkId)
                .FirstOrDefaultAsync();
            clientWorks.IsPaid = isPaid;

            _context.OmClientWork.Update(clientWorks);
            _context.SaveChanges();
            return Ok("Client record Updated");
        }

        [HttpGet("GetAllClientWork")]
        public async Task<IActionResult> GetAllClientWork()
        {
            var clientWorks = await _context.OmClientWork.Where(d => d.IsDeleted == false)
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
                Total = omClientWorkViewModel.Total,
                IsDeleted = omClientWorkViewModel.IsDeleted,
                IsPaid = omClientWorkViewModel.IsPaid,
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
            existingWork.Total = omClientWorkViewModel.Total;
            existingWork.IsPaid = omClientWorkViewModel.IsPaid;
            existingWork.IsDeleted = omClientWorkViewModel.IsDeleted;
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
                    Description = jobToDoViewModel.Description,
                    IsStatus = jobToDoViewModel.IsStatus,
                    JobStatusType = jobToDoViewModel.JobStatusType,
                    JobPostedDateTime = DateTime.UtcNow,
                };

                if (jobToDoViewModel.Images != null)
                {
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    foreach (var formFile in jobToDoViewModel.Images)
                    {
                        if (formFile != null)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(formFile.FileName);
                            var filePath = Path.Combine(imagesFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(fileStream);
                            }

                            var imagePath = Path.Combine("images", uniqueFileName);

                            jobToDo.JobImages.Add(new JobImages
                            {
                                ImagePath = imagePath
                            });
                        }
                    }
                }

                _context.JobToDo.Add(jobToDo);
                await _context.SaveChangesAsync();
                return Ok("Job Posted Successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateJobTodo/{id}")]
        public async Task<IActionResult> UpdateJobTodo(int id, JobToDoViewModel jobToDoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobToDo = await _context.JobToDo
                .Include(j => j.JobImages)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobToDo == null)
            {
                return NotFound();
            }

            jobToDo.CompanyName = jobToDoViewModel.ComapnyName;
            jobToDo.Quantity = jobToDoViewModel.Quantity;
            jobToDo.Description = jobToDoViewModel.Description;
            jobToDo.IsStatus = jobToDoViewModel.IsStatus;
            jobToDo.JobStatusType = jobToDoViewModel.JobStatusType;
            jobToDo.JobPostedDateTime = DateTime.UtcNow;

            if (jobToDoViewModel.Images != null)
            {
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Delete existing images from the folder
                foreach (var jobImage in jobToDo.JobImages)
                {
                    var existingFilePath = Path.Combine(imagesFolder, Path.GetFileName(jobImage.ImagePath));
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }

                // Clear existing images if new images are provided
                jobToDo.JobImages.Clear();

                foreach (var formFile in jobToDoViewModel.Images)
                {
                    if (formFile != null)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(formFile.FileName);
                        var filePath = Path.Combine(imagesFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(fileStream);
                        }

                        var imagePath = Path.Combine("images", uniqueFileName);

                        jobToDo.JobImages.Add(new JobImages
                        {
                            ImagePath = imagePath
                        });
                    }
                }
            }

            try
            {
                _context.Entry(jobToDo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobToDoExists(id))
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

        private bool JobToDoExists(int id)
        {
            return _context.JobToDo.Any(e => e.Id == id);
        }


        [HttpGet("GetJobToDoList")]
        public async Task<IActionResult> GetJobToDoList()
        {
            var jobList = await _context.JobToDo
                .Include(d => d.JobImages)
                .OrderByDescending(job => job.JobPostedDateTime)
                .ToListAsync();

            var jobStatusDictionary = await _context.JobTypeStatus
                .ToDictionaryAsync(x => x.JobStatusType, x => x.JobStatusName);

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var jobToDoResponses = jobList.Select(job => new JobToDoResponseViewModel
            {
                Id = job.Id,
                CompanyName = job.CompanyName,
                Description = job.Description,
                Quantity = job.Quantity,
                JobStatusType = job.JobStatusType,
                IsStatus = job.IsStatus,
                JobStatusName = jobStatusDictionary.TryGetValue(job.JobStatusType, out var jobStatusName) ? jobStatusName : null,
                JobPostedDateTime = job.JobPostedDateTime,
                Images = job.JobImages.Select(img => $"{baseUrl}/images/{Path.GetFileName(img.ImagePath)}").ToList()
            }).ToList();

            return Ok(jobToDoResponses);
        }



        [HttpGet("GetJobsToDosById/{jobToDoId}")]
        public async Task<IActionResult> GetJobsToDosById(int jobToDoId)
        {
            var jobToDoRecord = await _context.JobToDo
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

        #region Job Type Status

        // POST: api/OmMedia
        [Route("AddPostJobTypeStatus")]
        [HttpPost]
        public async Task<IActionResult> PostJobTypeStatus(JobTypeStatus jobTypeStatus)
        {
            _context.JobTypeStatus.Add(jobTypeStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTypeStatus", new { id = jobTypeStatus.Id }, jobTypeStatus);
        }



        [Route("GetJobTypeStatusList")]
        [HttpGet]
        public async Task<IActionResult> GetJobTypeStatusList()
        {
            var records = await _context.JobTypeStatus.ToListAsync();
            return Ok(records);
        }
        [Route("GetJobTypeStatusById")]
        [HttpGet]
        public async Task<IActionResult> GetJobTypeStatusById(int id)
        {
            var record = await _context.JobTypeStatus.FirstOrDefaultAsync(m => m.Id == id);
            return Ok(record);
        }

        #endregion

        #region Send Notification
        [HttpPost]
        [Route("SendEmailByClientId")]
        public async Task<IActionResult> SendEmailByClientId(int clientId, int clientWorkId)
        {
            var client = await _context.OmClient.FirstOrDefaultAsync(d => d.Id == clientId);
            var clientWork = await _context.OmClientWork.FirstOrDefaultAsync(d => d.Id == clientWorkId && d.OmClientId == clientId);
            if (client == null)
            {
                return NotFound("Client not found");
            }

            try
            {
                // Gmail SMTP server address
                string smtpServer = "smtp.gmail.com";
                int port = 587; // Gmail SMTP port
                string fromAddress = "colourdrop99@gmail.com";
                string password = "uepe ssdz gylo xaaj";

                // Create a new SmtpClient
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, port))
                {
                    // Enable SSL/TLS encryption
                    smtpClient.EnableSsl = true;
                    // Set the credentials
                    smtpClient.Credentials = new NetworkCredential(fromAddress, password);

                    // Create the email message
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(fromAddress);
                        mailMessage.To.Add(client.Email);
                        mailMessage.Subject = "Payment Reminder: Pending Invoice for " + client.CompanyName; // Subject line
                                                                                                             // Constructing the email body using the template
                        string body = $"Dear {client.Name},\n\n";
                        body += "I hope this email finds you well.\n\n";
                        body += "This is a gentle reminder regarding the pending payment for the work done on " + clientWork.WorkDate.ToString("yyyy-MM-ddTHH:mm:ssZ") + ".\n";
                        body += "We have provided services for " + clientWork.WorkDetails + " totaling " + clientWork.PrintCount + " prints, at a rate of " + clientWork.Price + " per print, resulting in a total amount of " + clientWork.Total + ".\n\n";
                        body += "However, as of now, we have not received the payment for this job. We kindly request you to settle the outstanding amount at your earliest convenience.\n\n";
                        body += "If you have already initiated the payment, please disregard this reminder. Otherwise, we would appreciate your prompt attention to this matter.\n\n";
                        body += "Thank you for your cooperation. Should you have any questions or concerns, please feel free to contact us.\n\n";
                        body += "Best regards,\n\n";
                        body += "Sukhdev Singh\n";
                        body += "Om Media Solutions\n";

                        mailMessage.Body = body;

                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                }
                clientWork.IsEmailSent = true;
                _context.OmClientWork.Update(clientWork);
                _context.SaveChanges();

                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }
        #endregion

    }



}

