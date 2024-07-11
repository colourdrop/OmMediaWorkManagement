using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Models;
using OmMediaWorkManagement.ApiService.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace OmMediaWorkManagement.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OmMediaController : ControllerBase
    {
        private readonly OmContext _context;
        private readonly ILogger<OmMediaController> _logger;

        public OmMediaController(OmContext context, ILogger<OmMediaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Client Details
        [HttpPost("AddClient")]
        [Authorize]
        public async Task<IActionResult> AddClient(OmClientViewModel omClientViewModel)
        {
            // Get UserId from claims
            var userId = User.FindFirst("UserId").Value;
            if (userId == null)
            {
                return Unauthorized("UserId claim not found.");
            }


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
                IsDeleted = false,
                UserId = userId  // Assigning UserId from claims
            };

            _context.OmClient.Add(omClient);
            await _context.SaveChangesAsync();

            return Ok($"{omClientViewModel.Name} successfully added");
        }


        [HttpGet("GetAllClients")]
        [Authorize]
        public async Task<IActionResult> GetAllClients()
        {

            var clients = await _context.OmClient.Include(d => d.UserRegistration).OrderByDescending(d => d.CreatedAt).Select(d => new
            {
                d.Id,
                d.Name,
                d.CompanyName,
                d.MobileNumber,
                d.Email,
                d.CreatedAt,
                d.IsDeleted,
                d.UserId,
                d.UserRegistration.UserName


            }).ToListAsync();

            return Ok(clients);
        }

        [HttpGet("GetClientById/{id}")]
        [Authorize]
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
        [Authorize]
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

            return Ok($"{omClientViewModel.Name} is Successfully Added");
        }

        [HttpDelete("DeleteClientById/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteClientById(int id)
        {
            var client = await _context.OmClient.FindAsync(id);
            if (client == null)
            {
                return NotFound("Client not Found");
            }

            _context.OmClient.Remove(client);
            await _context.SaveChangesAsync();

            return Ok($"{client.Name} is Deleted Successfully");
        }

        #endregion

        #region Work Details

        [HttpGet("GetWorksByClientId/{clientId}")]
        [Authorize]
        public async Task<IActionResult> GetWorksByClientId(int clientId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId && work.IsDeleted == false).Select(d => new
                {
                    d.Id,
                    d.WorkDate,
                    d.WorkDetails,
                    d.PrintCount,
                    d.Price,
                    d.Total,
                    d.Remarks,
                    d.IsPaid,
                    d.IsDeleted,
                    d.IsEmailSent,
                    d.IsSMSSent,
                    d.IsPushSent,
                    d.UserId,
                    d.UserRegistration.UserName

                }).OrderByDescending(d => d.WorkDate)
                .ToListAsync();

            return Ok(clientWorks);
        }

        [HttpDelete("DeleteWorksByClientId")]
        [Authorize]
        public async Task<IActionResult> DeleteWorksByClientId(int clientWorkId, int omClientId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == omClientId && work.Id == clientWorkId)
                .FirstOrDefaultAsync();
            if (clientWorks != null)
            {
                clientWorks.IsDeleted = true;
                _context.OmClientWork.Update(clientWorks);
                _context.SaveChanges();
                return Ok($" Work is  Deleted Successfully");
            }
            else
            {
                return NotFound("History not found");
            }
        }

        [HttpPut("UpdatePaymentWorksStatusByClientId")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentWorksStatusByClientId(int clientId, int clientWorkId, bool isPaid)
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
        [Authorize]
        public async Task<IActionResult> GetAllClientWork()
        {
            var clientWorks = await _context.OmClientWork.Where(d => d.IsDeleted == false).Select(d => new
            {
                d.Id,
                d.WorkDate,
                d.WorkDetails,
                d.PrintCount,
                d.OmClientId,
                d.Price,
                d.Total,
                d.Remarks,
                d.IsPaid,
                d.TotalPayable,
                d.DueBalance,
                d.PaidAmount,
                d.IsDeleted,
                d.IsEmailSent,
                d.IsSMSSent,
                d.IsPushSent,
                d.UserId,
                d.UserRegistration.UserName

            }).OrderByDescending(d => d.WorkDate)
                .ToListAsync();

            return Ok(clientWorks);
        }

        [HttpPost("AddWork")]
        [Authorize]
        public async Task<IActionResult> AddWork(OmClientWorkViewModel omClientWorkViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst("UserId").Value;
            var omClientWork = new OmClientWork
            {
                OmClientId = omClientWorkViewModel.ClientId,
                WorkDate = omClientWorkViewModel.WorkDate,
                WorkDetails = omClientWorkViewModel.WorkDetails,
                PrintCount = omClientWorkViewModel.PrintCount,
                Price = omClientWorkViewModel.Price,
                PaidAmount = omClientWorkViewModel.PaidAmount,
                DueBalance = omClientWorkViewModel.DueBalance,
                Total = omClientWorkViewModel.Total,
                TotalPayable = omClientWorkViewModel.TotalPayable,
                IsDeleted = omClientWorkViewModel.IsDeleted,
                IsPaid = omClientWorkViewModel.IsPaid,
                Remarks = omClientWorkViewModel.Remarks,
                UserId = userId
            };

            _context.OmClientWork.Add(omClientWork);
            await _context.SaveChangesAsync();

            return Ok($" Work History Added ");
        }

        [HttpPut("UpdateWork/{id}")]
        [Authorize]
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
            existingWork.PaidAmount = omClientWorkViewModel.PaidAmount;
            existingWork.TotalPayable = omClientWorkViewModel.TotalPayable;
            existingWork.DueBalance = omClientWorkViewModel.DueBalance;
            existingWork.Total = omClientWorkViewModel.Total;
            existingWork.IsPaid = omClientWorkViewModel.IsPaid;
            existingWork.IsDeleted = omClientWorkViewModel.IsDeleted;
            existingWork.Remarks = omClientWorkViewModel.Remarks;

            _context.OmClientWork.Update(existingWork);
            await _context.SaveChangesAsync();

            return Ok($"Work Updated Successfully");
        }

        #endregion

        #region JobTodo

        [HttpPost("AddJobTodo")]
        [Authorize]
        public async Task<IActionResult> AddJobTodo(JobToDoViewModel jobToDoViewModel)
        {
            _logger.LogInformation("got hit");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst("UserId").Value;
            try
            {
                var jobToDo = new JobToDo
                {

                    OmClientId = jobToDoViewModel.OmClientId,

                    Price = jobToDoViewModel.Price,
                    PaidAmount = jobToDoViewModel.PaidAmount,
                    TotalPayable = jobToDoViewModel.TotalPayable,
                    DueBalance = jobToDoViewModel.DueBalance,
                    total = jobToDoViewModel.total,
                    Quantity = jobToDoViewModel.Quantity,
                    Description = jobToDoViewModel.Description,
                    IsStatus = jobToDoViewModel.IsStatus,
                    JobStatusType = jobToDoViewModel.JobStatusType,
                    JobPostedDateTime = DateTime.UtcNow,
                    OmEmpId = Convert.ToInt32(userId)
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateJobTodo/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateJobTodo(int id, JobToDoViewModel jobToDoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobToDo = await _context.JobToDo
                .Include(j => j.JobImages)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobToDo.Id == 0 || jobToDo.Id == null)
            {
                return NotFound();
            }

            jobToDo.OmClientId = jobToDoViewModel.OmClientId;

            jobToDo.Quantity = jobToDoViewModel.Quantity;
            jobToDo.Price = jobToDoViewModel.Price;
            jobToDo.PaidAmount = jobToDoViewModel.PaidAmount;
            jobToDo.DueBalance = jobToDoViewModel.DueBalance;
            jobToDo.TotalPayable = jobToDoViewModel.TotalPayable;
            jobToDo.total = jobToDoViewModel.total;
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
                _context.JobToDo.Update(jobToDo);
                _context.SaveChanges();
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
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

        }

        private bool JobToDoExists(int id)
        {
            return _context.JobToDo.Any(e => e.Id == id);
        }


        [HttpGet("GetJobToDoList")]
        [Authorize]
        public async Task<IActionResult> GetJobToDoList()
        {
            var jobList = await _context.JobToDo
                .Include(d => d.JobImages)
                .Include(d => d.OmClient)
                .Include(d => d.OmEmployee)
                .OrderByDescending(job => job.JobPostedDateTime)
                .ToListAsync();

            var jobStatusDictionary = await _context.JobTypeStatus
                .ToDictionaryAsync(x => x.JobStatusType, x => x.JobStatusName);

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // Use Task.WhenAll to await all async projections
            var jobToDoResponsesTasks = jobList.Select(async job => new JobToDoResponseViewModel
            {
                Id = job.Id,
                OmClientId = job.OmClientId,
                ClientName = job.OmClient.Name,
                CompanyName = job.CompanyName,
                Description = job.Description,
                Price = job.Price,
                Total = job.total,
                TotalPayable = job.TotalPayable,
                DueBalance = job.DueBalance,
                PaidAmount = job.PaidAmount,
                Quantity = job.Quantity,
                JobStatusType = job.JobStatusType,
                IsStatus = job.IsStatus,
                JobStatusName = jobStatusDictionary.TryGetValue(job.JobStatusType, out var jobStatusName) ? jobStatusName : null,
                JobPostedDateTime = job.JobPostedDateTime,
                OmEmpId = job.OmEmpId,
                OmEmpName = job.OmEmployee.Name,
                Images = job.JobImages.Select(img => $"{baseUrl}/images/{Path.GetFileName(img.ImagePath)}").ToList()
            }).ToList();

            // Await all tasks to get the list of JobToDoResponseViewModel
            var jobToDoResponses = await Task.WhenAll(jobToDoResponsesTasks);

            return Ok(jobToDoResponses);
        }



        [HttpGet("GetJobsToDosById/{jobToDoId}")]
        [Authorize]
        public async Task<IActionResult> GetJobsToDosById(int jobToDoId)
        {
            
            var jobList = await _context.JobToDo
               .Include(d => d.JobImages)
               .Include(d => d.OmClient)
               .Include(d => d.OmEmployee).OrderByDescending(d => d.JobPostedDateTime)
                .Where(j => j.Id == jobToDoId)
               .ToListAsync();

            var jobStatusDictionary = await _context.JobTypeStatus
                .ToDictionaryAsync(x => x.JobStatusType, x => x.JobStatusName);

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // Use Task.WhenAll to await all async projections
            var jobToDoResponsesTasks = jobList.Select(async job => new JobToDoResponseViewModel
            {
                Id = job.Id,
                OmClientId = job.OmClientId,
                ClientName = job.OmClient.Name,
                CompanyName = job.CompanyName,
                Description = job.Description,
                Price = job.Price,
                Total = job.total,
                TotalPayable = job.TotalPayable,
                DueBalance = job.DueBalance,
                PaidAmount = job.PaidAmount,
                Quantity = job.Quantity,
                JobStatusType = job.JobStatusType,
                IsStatus = job.IsStatus,
                JobStatusName = jobStatusDictionary.TryGetValue(job.JobStatusType, out var jobStatusName) ? jobStatusName : null,
                JobPostedDateTime = job.JobPostedDateTime,
                OmEmpId = job.OmEmpId,
                OmEmpName = job.OmEmployee.Name,
                Images = job.JobImages.Select(img => $"{baseUrl}/images/{Path.GetFileName(img.ImagePath)}").ToList()
            }).ToList();

            // Await all tasks to get the list of JobToDoResponseViewModel
            var jobToDoResponses = await Task.WhenAll(jobToDoResponsesTasks);

          
            if (jobToDoResponses == null)
            {
                return NotFound();
            }

            return Ok(jobToDoResponses);
        }


        [HttpDelete("DeleteJobTodo/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteJobTodo(int id)
        {
            var jobToDo = await _context.JobToDo
                .Include(j => j.JobImages) // Include JobImages to ensure they are loaded
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobToDo == null)
            {
                return NotFound(); // Return NotFound if JobToDo with given id is not found
            }

            // Delete associated images physically from wwwroot and from database
            foreach (var jobImage in jobToDo.JobImages)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), jobImage.ImagePath);

                // Check if file exists before attempting to delete
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.JobImages.Remove(jobImage); // Remove each JobImage from DbSet
            }

            _context.JobToDo.Remove(jobToDo); // Remove the JobToDo itself

            try
            {
                await _context.SaveChangesAsync(); // Save changes to database
                return Ok("JobTodo Deleted "); // Return Ok if deletion is successful
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting JobToDo: {ex.Message}");
            }
        }

        [HttpGet("GetJobsToDosByEmpId")]
        //[Authorize]
        public async Task<IActionResult> GetJobsToDosByUserId(int empId)
        {
            var getJobList = await _context.JobToDo.Include(d=>d.OmEmployee).Include(d=>d.OmClient).Include(d=>d.JobImages).Where(d => d.OmEmpId == empId).OrderByDescending(d=>d.JobPostedDateTime).ToListAsync();
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var jobStatusDictionary = await _context.JobTypeStatus
              .ToDictionaryAsync(x => x.JobStatusType, x => x.JobStatusName);
            // Use Task.WhenAll to await all async projections
            var jobToDoResponsesTasks = getJobList.Select(async job => new JobToDoResponseViewModel
            {
                Id = job.Id,
                OmClientId = job.OmClientId,
                ClientName = job.OmClient.Name,
                CompanyName = job.OmClient.CompanyName,
                Description = job.Description,           
                Quantity = job.Quantity,
                JobStatusType = job.JobStatusType,
                IsStatus = job.IsStatus,
                JobStatusName = jobStatusDictionary.TryGetValue(job.JobStatusType, out var jobStatusName) ? jobStatusName : null,
                JobPostedDateTime = job.JobPostedDateTime,
                OmEmpId = job.OmEmpId,
                OmEmpName = job.OmEmployee.Name,
                Images = job.JobImages.Select(img => $"{baseUrl}/images/{Path.GetFileName(img.ImagePath)}").ToList()
            }).ToList();

            // Await all tasks to get the list of JobToDoResponseViewModel
            var jobToDoResponses = await Task.WhenAll(jobToDoResponsesTasks);


            return Ok(jobToDoResponses.ToList());
        }
        #endregion

        #region OmMachines

        [HttpPost("AddMachine")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetMachines()
        {
            var machinesList = await _context.OmMachines.ToListAsync();
            return Ok(machinesList);
        }

        [HttpPut("UpdateMachineById/{id}")]
        [Authorize]
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

            return Ok($"Machine Updated Successfully");
        }

        [HttpGet("GetMachineById/{machineId}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> PostJobTypeStatus(JobTypeStatus jobTypeStatus)
        {
            _context.JobTypeStatus.Add(jobTypeStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTypeStatus", new { id = jobTypeStatus.Id }, jobTypeStatus);
        }



        [Route("GetJobTypeStatusList")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetJobTypeStatusList()
        {
            var records = await _context.JobTypeStatus.ToListAsync();
            return Ok(records);
        }
        [Route("GetJobTypeStatusById")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetJobTypeStatusById(int id)
        {
            var record = await _context.JobTypeStatus.FirstOrDefaultAsync(m => m.Id == id);
            return Ok(record);
        }

        #endregion

        #region Send Notification
        [HttpPost]
        [Route("SendEmailByClientId")]
        [Authorize]
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

        #region AddEmployee

        [HttpPost("AddEmployee")]
        [Authorize]
        public async Task<IActionResult> AddEmployee([FromForm] OmEmployeeViewModels omEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("UserId").Value;
                var employee = new OmEmployee
                {
                    Name = omEmployeeViewModel.Name,
                    CompanyName = omEmployeeViewModel.CompanyName,
                    Address = omEmployeeViewModel.Address,
                    CreatedDate = DateTime.UtcNow,
                    Description = omEmployeeViewModel.Description,
                    Email = omEmployeeViewModel.Email,
                    PhoneNumber = omEmployeeViewModel.PhoneNumber,
                    SalaryAmount = omEmployeeViewModel.SalaryAmount,
                    IsSalaryPaid = false,
                    UserId = userId,
                };

                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                if (omEmployeeViewModel.EmployeeProfile != null)
                {
                    var profileFileName = Guid.NewGuid().ToString("N").Substring(0, 16) + "_" + Path.GetFileName(omEmployeeViewModel.EmployeeProfile.FileName);
                    var profileFilePath = Path.Combine(imagesFolder, profileFileName);

                    using (var fileStream = new FileStream(profileFilePath, FileMode.Create))
                    {
                        await omEmployeeViewModel.EmployeeProfile.CopyToAsync(fileStream);
                    }

                    employee.EmployeeProfilePath = Path.Combine("images", profileFileName);
                }

                var employeeDocuments = new List<OmEmployeeDocuments>();
                if (omEmployeeViewModel.EmployeeDocuments != null)
                {
                    foreach (var formFile in omEmployeeViewModel.EmployeeDocuments)
                    {
                        var documentFileName = Guid.NewGuid().ToString("N").Substring(0, 16) + "_" + Path.GetFileName(formFile.FileName);
                        var documentFilePath = Path.Combine(imagesFolder, documentFileName);

                        using (var fileStream = new FileStream(documentFilePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(fileStream);
                        }

                        var documentPath = Path.Combine("images", documentFileName);

                        employeeDocuments.Add(new OmEmployeeDocuments
                        {
                            EmployeeDocumentsPath = documentPath,
                            OmEmployee = employee // Ensures correct foreign key relation
                        });
                    }
                }

                _context.OmEmployee.Add(employee);
                await _context.SaveChangesAsync();
                if (employeeDocuments.Count != 0)
                {
                    _context.OmEmployeeDocuments.AddRange(employeeDocuments);
                }
                await _context.SaveChangesAsync();

                return Ok("Added Successfully");
            }

            return BadRequest(ModelState);
        }


        [HttpPut("UpdateEmployee/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(int id, [FromForm] OmEmployeeViewModels omEmployeeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.OmEmployee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Update basic fields
            employee.Name = omEmployeeViewModel.Name;
            employee.CompanyName = omEmployeeViewModel.CompanyName;
            employee.Address = omEmployeeViewModel.Address;
            employee.Description = omEmployeeViewModel.Description;
            employee.Email = omEmployeeViewModel.Email;
            employee.PhoneNumber = omEmployeeViewModel.PhoneNumber;
            employee.SalaryAmount = omEmployeeViewModel.SalaryAmount;
            employee.IsSalaryPaid = omEmployeeViewModel.IsSalaryPaid;
            employee.IsDeleted = omEmployeeViewModel.IsDeleted;

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Update profile image if a new one is provided
            if (omEmployeeViewModel.EmployeeProfile != null)
            {
                var profileFileName = Guid.NewGuid().ToString("N").Substring(0, 16) + "_" + Path.GetFileName(omEmployeeViewModel.EmployeeProfile.FileName);
                var profileFilePath = Path.Combine(imagesFolder, profileFileName);

                using (var fileStream = new FileStream(profileFilePath, FileMode.Create))
                {
                    await omEmployeeViewModel.EmployeeProfile.CopyToAsync(fileStream);
                }

                employee.EmployeeProfilePath = Path.Combine("images", profileFileName);
            }

            // Update employee documents if new ones are provided
            if (omEmployeeViewModel.EmployeeDocuments != null && omEmployeeViewModel.EmployeeDocuments.Any())
            {
                // Remove old documents
                var existingDocuments = _context.OmEmployeeDocuments.Where(d => d.OmEmployeeId == id).ToList();
                _context.OmEmployeeDocuments.RemoveRange(existingDocuments);

                // Add new documents
                foreach (var formFile in omEmployeeViewModel.EmployeeDocuments)
                {
                    var documentFileName = Guid.NewGuid().ToString("N").Substring(0, 16) + "_" + Path.GetFileName(formFile.FileName);
                    var documentFilePath = Path.Combine(imagesFolder, documentFileName);

                    using (var fileStream = new FileStream(documentFilePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }

                    var documentPath = Path.Combine("images", documentFileName);

                    _context.OmEmployeeDocuments.Add(new OmEmployeeDocuments
                    {
                        OmEmployeeId = id,
                        EmployeeDocumentsPath = documentPath
                    });
                }
            }

            _context.OmEmployee.Update(employee);
            await _context.SaveChangesAsync();

            return Ok("Updated Successfully");
        }


        [HttpGet("GetAllEmployee")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employeeList = await _context.OmEmployee.Include(d => d.EmployeeDocuments).ToListAsync();
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var employeeResponses = employeeList.Select(emp => new OmEmployeeResponseViewModel
            {
                Id = emp.Id,
                Name = emp.Name,
                Address = emp.Address,
                CompanyName = emp.CompanyName,
                Email = emp.Email,
                PhoneNumber = emp.PhoneNumber,
                SalaryAmount = emp.SalaryAmount,
                IsSalaryPaid = emp.IsSalaryPaid,
                Description = emp.Description,
                EmployeeProfilePath = $"{baseUrl}/images/{Path.GetFileName(emp.EmployeeProfilePath)}",
                CreatedDate = emp.CreatedDate,
                IsDeleted = emp.IsDeleted,
                EmployeeDocuments = emp.EmployeeDocuments.Select(doc => $"{baseUrl}/images/{Path.GetFileName(doc.EmployeeDocumentsPath)}").ToList()
            }).ToList();

            return Ok(employeeResponses);
        }

        [HttpPost("AddSalaryManagement")]
        [Authorize]
        public async Task<IActionResult> AddSalaryManagement(OmEmployeeSalaryManagementViewModel omEmployeeSalaryManagementViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the employee by ID
            var employee = await _context.OmEmployee.FindAsync(omEmployeeSalaryManagementViewModel.OmEmployeeId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            // Find the latest salary record
            var latestSalary = await _context.OmEmployeeSalaryManagement
                .Where(d => d.OmEmployeeId == omEmployeeSalaryManagementViewModel.OmEmployeeId)
                .OrderByDescending(d => d.CreatedDate)
                .FirstOrDefaultAsync();

            // Calculate DueBalance based on whether there is a latest salary record
            decimal? dueBalance = (latestSalary?.DueBalance ?? employee.SalaryAmount) - omEmployeeSalaryManagementViewModel.AdvancePayment;

            // Create new OmEmployeeSalaryManagement object
            var omEmployeeSalary = new OmEmployeeSalaryManagement()
            {
                OmEmployeeId = omEmployeeSalaryManagementViewModel.OmEmployeeId,
                AdvancePayment = omEmployeeSalaryManagementViewModel.AdvancePayment,
                AdvancePaymentDate = DateTime.UtcNow,
                DueBalance = dueBalance,
                OverBalance = omEmployeeSalaryManagementViewModel.OverBalance,
                OverTimeSalary = omEmployeeSalaryManagementViewModel.OverTimeSalary,
                CreatedDate = DateTime.UtcNow
            };

            // Add to DbContext and save changes
            _context.OmEmployeeSalaryManagement.Add(omEmployeeSalary);
            await _context.SaveChangesAsync();

            return Ok("Added Successfully");
        }

        [HttpPut("UpdateSalaryManagement")]
        [Authorize]
        public async Task<IActionResult> UpdateSalaryManagement(int salaryManagementid, [FromForm] OmEmployeeSalaryManagementViewModel omEmployeeSalaryManagementViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeSalaryManagement = await _context.OmEmployeeSalaryManagement.FindAsync(salaryManagementid);
            var getEmploye = await _context.OmEmployee.FindAsync(omEmployeeSalaryManagementViewModel.OmEmployeeId);
            if (employeeSalaryManagement == null)
            {
                return NotFound();
            }

            // Update the salary management fields
            employeeSalaryManagement.AdvancePayment = omEmployeeSalaryManagementViewModel.AdvancePayment;
            employeeSalaryManagement.AdvancePaymentDate = DateTime.UtcNow;
            employeeSalaryManagement.DueBalance = employeeSalaryManagement.DueBalance - omEmployeeSalaryManagementViewModel.DueBalance;
            employeeSalaryManagement.OverBalance = omEmployeeSalaryManagementViewModel.OverBalance;
            employeeSalaryManagement.OverTimeSalary = omEmployeeSalaryManagementViewModel.OverTimeSalary;
            employeeSalaryManagement.CreatedDate = DateTime.UtcNow;

            _context.OmEmployeeSalaryManagement.Update(employeeSalaryManagement);
            await _context.SaveChangesAsync();

            return Ok("Updated Successfully");
        }



        [HttpGet("GetSalaryManagementByEmployeeId")]
        [Authorize]
        public async Task<IActionResult> GetSalaryManagementByEmployeeId(int OmEmployeeId)
        {
            var employeeRecords = await _context.OmEmployeeSalaryManagement.Where(d => d.OmEmployeeId == OmEmployeeId).OrderByDescending(d => d.CreatedDate).ToListAsync();
            return Ok(employeeRecords);

        }

        #endregion
    }



}

