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
using DinkToPdf.Contracts;
using DinkToPdf;

namespace OmMediaWorkManagement.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OmMediaController : ControllerBase
    {
        private readonly OmContext _context;
        private readonly ILogger<OmMediaController> _logger;
        private readonly IConverter _converter;

        public OmMediaController(OmContext context, ILogger<OmMediaController> logger, IConverter converter)
        {
            _context = context;
            _logger = logger;
            _converter = converter;
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


        [HttpGet("GetWorkDetailsPdfByClientId")]
        [Authorize]
        public async Task<byte[]> GetWorkDetailsPdfByClientId(int clientId)
        {
            var clientWorks = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId && work.IsDeleted == false&&work.IsPaid==false)
                 .Select(d => new ClientWorkDto
                 {
                     Id = d.Id,
                     WorkDate = d.WorkDate,
                     WorkDetails = d.WorkDetails,
                     PrintCount = d.PrintCount,
                     OmClientId = d.OmClientId,
                     Price = d.Price,
                     Total = d.Total,
                     Remarks = d.Remarks,
                     IsPaid = d.IsPaid,
                     TotalPayable = d.TotalPayable,
                     DueBalance = d.DueBalance,
                     PaidAmount = d.PaidAmount,
                     IsDeleted = d.IsDeleted,
                     IsEmailSent = d.IsEmailSent,
                     IsSMSSent = d.IsSMSSent,
                     IsPushSent = d.IsPushSent,
                     UserId = d.UserId,
                     UserName = d.UserRegistration.UserName
                 }).OrderByDescending(d => d.WorkDate)
                .ToListAsync();
            if (clientWorks.Count != 0)
            {
                var generateHTML = await GenerateWorkHistoryHtml(clientWorks);
                var clientPDF = await GeneratePdfAsync(generateHTML);
                return clientPDF;
            }
            else
            {
                return null;
            }
        }
        private async Task<string> GenerateWorkHistoryHtml(List<ClientWorkDto> data)
        {
            var GetClientName = _context.OmClient.FirstOrDefault(d => d.Id == data.FirstOrDefault().OmClientId).Name;
            // Initialize the HTML content with the opening tags
            var htmlContent = "<html><body><div class='container mt-3'><div class='d-flex justify-content-between mb-3'>";

            // Add headers for Client Name, Estimate, and Print Date
            htmlContent += $"<div class='p-2'>{GetClientName}</div>";     
            htmlContent += $"<div class='p-2'>Print Date: {DateTime.Now}</div>";
            htmlContent += "</div>";

            // Add the table structure
            htmlContent += "<div class='table-responsive'><table class='table table-bordered'><thead><tr>";

            // Add table headers
            htmlContent += "<th>Work Date</th>";
            htmlContent += "<th>Detail</th>";
            htmlContent += "<th>Quantity</th>";
            htmlContent += "<th>Rate</th>";
            htmlContent += "<th>Total Payable</th>";
            htmlContent += "<th>Paid Amount</th>";
            htmlContent += "<th>Due Balance</th>";
            htmlContent += "</tr></thead><tbody>";

            int? totalPayable = 0;
            int? totalPaidAmount = 0;
            int? totalDueBalance = 0;

            // Add rows dynamically from the provided data
            foreach (var item in data)
            {
                htmlContent += "<tr>";
                htmlContent += $"<td class='text-center'>{ConvertUtcToIst(item.WorkDate)}</td>";
                htmlContent += $"<td class='text-center'>{item.WorkDetails}</td>";
                htmlContent += $"<td class='text-center'>{item.PrintCount}</td>";
                htmlContent += $"<td class='text-center'>{item.Price}</td>";
                htmlContent += $"<td class='text-center'>{item.TotalPayable}</td>";
                htmlContent += $"<td class='text-center'>{item.PaidAmount}</td>";
                htmlContent += $"<td class='text-center'>{item.DueBalance}</td>";
                htmlContent += "</tr>";

                // Calculate totals
                totalPayable += item.TotalPayable;
                totalPaidAmount += item.PaidAmount;
                totalDueBalance += item.DueBalance;
            }

            // Add total row
            htmlContent += "<tr>";
            htmlContent += "<td colspan='4'><strong>Total:</strong></td>";
            htmlContent += $"<td class='text-center'><strong>{totalPayable}</strong></td>";
            htmlContent += $"<td class='text-center'><strong>{totalPaidAmount}</strong></td>";
            htmlContent += $"<td class='text-center'><strong>{totalDueBalance}</strong></td>";
            htmlContent += "</tr>";

            // Close the table and container tags
            htmlContent += "</tbody></table></div></div></body></html>";

            return htmlContent;
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
    
        [HttpGet("GetJobsToDosByClientId/{clientId}")]
        [Authorize]
        public async Task<IActionResult> GetJobsToDosByClientId(int clientId)
        {

            var jobList = await _context.JobToDo
               .Include(d => d.JobImages)
               .Include(d => d.OmClient)
               .Include(d => d.OmEmployee).OrderByDescending(d => d.JobPostedDateTime)
                .Where(j => j.OmClientId==clientId)
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
            var getJobList = await _context.JobToDo.Include(d => d.OmEmployee).Include(d => d.OmClient).Include(d => d.JobImages).Where(d => d.OmEmpId == empId).OrderByDescending(d => d.JobPostedDateTime).ToListAsync();
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

        [HttpGet("GetTodoDetailsPdfByClientId")]
        [Authorize]
        public async Task<byte[]> GetTodoDetailsPdfByClientId(int clientId)
        {
            var jobList = await _context.JobToDo
                .Include(d => d.JobImages)
                .Include(d => d.OmClient)
                .Include(d => d.OmEmployee)
                .OrderByDescending(job => job.JobPostedDateTime).Where(d=>d.OmClientId==clientId)
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
            if (jobToDoResponses.Length != 0)
            {
                // Assuming jobToDoResponses is an array (JobToDoResponseViewModel[])
                var jobToDoResponsesArray = await Task.WhenAll(jobToDoResponsesTasks);

                // Convert array to list if needed
                var jobToDoResponsesList = jobToDoResponsesArray.ToList();

                // Example of passing list to a method
                var generateHTML = await GenerateTodoHistoryHtml(jobToDoResponsesList);

                var clientPDF = await GeneratePdfAsync(generateHTML);
                return clientPDF;
            }
            else
            {
                return null;
            }
        }
        private async Task<string> GenerateTodoHistoryHtml(List<JobToDoResponseViewModel> data)
        {
            var GetClientName = _context.OmClient.FirstOrDefault(d => d.Id == data.FirstOrDefault().OmClientId).Name;
            // Initialize the HTML content with the opening tags
            var htmlContent = "<html><body><div class='container mt-3'><div class='d-flex justify-content-between mb-3'>";

            // Add headers for Client Name, Estimate, and Print Date
            htmlContent += $"<div class='p-2'>{GetClientName}</div>";
            htmlContent += $"<div class='p-2'>Print Date: {DateTime.Now}</div>";
            htmlContent += "</div>";

            // Add the table structure
            htmlContent += "<div class='table-responsive'><table class='table table-bordered'><thead><tr>";

            // Add table headers
            htmlContent += "<th>Work Date</th>";
            htmlContent += "<th>Detail</th>";
            htmlContent += "<th>Quantity</th>";
            htmlContent += "<th>Rate</th>";
            htmlContent += "<th>Total Payable</th>";
            htmlContent += "<th>Paid Amount</th>";
            htmlContent += "<th>Due Balance</th>";
            htmlContent += "</tr></thead><tbody>";

            int? totalPayable = 0;
            int? totalPaidAmount = 0;
            int? totalDueBalance = 0;

            // Add rows dynamically from the provided data
            foreach (var item in data)
            {
                htmlContent += "<tr>";
                htmlContent += $"<td class='text-center'>{ConvertUtcToIst(item.JobPostedDateTime)}</td>";
                htmlContent += $"<td class='text-center'>{item.Description}</td>";
                htmlContent += $"<td class='text-center'>{item.Quantity}</td>";
                htmlContent += $"<td class='text-center'>{item.Price}</td>";
                htmlContent += $"<td class='text-center'>{item.TotalPayable}</td>";
                htmlContent += $"<td class='text-center'>{item.PaidAmount}</td>";
                htmlContent += $"<td class='text-center'>{item.DueBalance}</td>";
                htmlContent += "</tr>";

                // Calculate totals
                totalPayable += item.TotalPayable;
                totalPaidAmount += item.PaidAmount;
                totalDueBalance += item.DueBalance;
            }

            // Add total row
            htmlContent += "<tr>";
            htmlContent += "<td colspan='4'><strong>Total:</strong></td>";
            htmlContent += $"<td class='text-center'><strong>{totalPayable}</strong></td>";
            htmlContent += $"<td class='text-center'><strong>{totalPaidAmount}</strong></td>";
            htmlContent += $"<td class='text-center'><strong>{totalDueBalance}</strong></td>";
            htmlContent += "</tr>";

            // Close the table and container tags
            htmlContent += "</tbody></table></div></div></body></html>";

            return htmlContent;
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


        [HttpPost]
        [Route("SendBulkWorkEmailByClientId")]
        [Authorize]
        public async Task<IActionResult> SendBulkWorkEmailByClientId(int clientId)
        {
            var client = await _context.OmClient.FirstOrDefaultAsync(d => d.Id == clientId);
            var clientWorkList = await _context.OmClientWork
                .Where(work => work.OmClientId == clientId && !work.IsDeleted && !work.IsPaid)
                .Select(d => new ClientWorkDto
                {
                    Id = d.Id,
                    WorkDate = d.WorkDate,
                    WorkDetails = d.WorkDetails,
                    PrintCount = d.PrintCount,
                    OmClientId = d.OmClientId,
                    Price = d.Price,
                    Total = d.Total,
                    Remarks = d.Remarks,
                    IsPaid = d.IsPaid,
                    TotalPayable = d.TotalPayable,
                    DueBalance = d.DueBalance,
                    PaidAmount = d.PaidAmount,
                    IsDeleted = d.IsDeleted,
                    IsEmailSent = d.IsEmailSent,
                    IsSMSSent = d.IsSMSSent,
                    IsPushSent = d.IsPushSent,
                    UserId = d.UserId,
                    UserName = d.UserRegistration.UserName
                })
                .OrderByDescending(d => d.WorkDate)
                .ToListAsync();

            if (client == null)
            {
                return BadRequest("Client not found");
            }
            if (clientWorkList.Count == 0)
            {
                return BadRequest("Please Select other client there is not work history ");
            }
            try
            {
                // Generate the HTML content and PDF
                var htmlContent = await GenerateWorkHistoryHtml(clientWorkList);
                var pdfBytes = await GeneratePdfAsync(htmlContent);

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
                        mailMessage.Subject = "Payment Reminder: Pending Invoice for " + client.CompanyName;
                        // Constructing the email body using the template
                        string body = $"Dear {client.Name},\n\n";
                        body += "We have completed the work for you, and the payment is now pending. Please find the work details in the attached PDF.\n\n";
                        body += "Please initiate the payment at your earliest convenience. If you have already made the payment, kindly ignore this email.\n\n";
                        body += "This email was generated by our system. For more information about our services, please visit www.codersf5.com.\n\n";
                        body += "Best regards,\n";
                        body += "Sukhdev Singh\n";
                        body += "Om Media Solutions\n";

                        mailMessage.Body = body;

                        // Attach the PDF
                        using (var ms = new MemoryStream(pdfBytes))
                        {
                            ms.Position = 0;
                            mailMessage.Attachments.Add(new Attachment(ms, "WorkEstimate.pdf", "application/pdf"));

                            // Send the email
                            smtpClient.Send(mailMessage);
                        }
                    }
                }

               

                return Ok("Email sent successfully with PDF attachment");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("SendBulkTodoEmailByClientId")]
        [Authorize]
        public async Task<IActionResult> SendBulkTodoEmailByClientId(int clientId)
        {
            var client = await _context.OmClient.FirstOrDefaultAsync(d => d.Id == clientId);
            var jobList = await _context.JobToDo
                .Include(d => d.JobImages)
                .Include(d => d.OmClient)
                .Include(d => d.OmEmployee).OrderByDescending(d => d.JobPostedDateTime)
                 .Where(j => j.OmClientId == clientId)
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
            var jobListTodo = jobToDoResponses.ToList();
            if (client == null)
            {
                return BadRequest("Client not found");
            }
            if (jobListTodo.Count == 0)
            {
                return BadRequest("Please Select other client there is not work history ");
            }
            try
            {
                // Generate the HTML content and PDF
                var htmlContent = await GenerateTodoHistoryHtml(jobListTodo);
                var pdfBytes = await GeneratePdfAsync(htmlContent);

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
                        mailMessage.Subject = "Payment Reminder: Pending Invoice for " + client.CompanyName;
                        // Constructing the email body using the template
                        string body = $"Dear {client.Name},\n\n";
                        body += "We have completed the work for you, and the payment is now pending. Please find the work details in the attached PDF.\n\n";
                        body += "Please initiate the payment at your earliest convenience. If you have already made the payment, kindly ignore this email.\n\n";
                        body += "This email was generated by our system. For more information about our services, please visit www.codersf5.com.\n\n";
                        body += "Best regards,\n";
                        body += "Sukhdev Singh\n";
                        body += "Om Media Solutions\n";

                        mailMessage.Body = body;

                        // Attach the PDF
                        using (var ms = new MemoryStream(pdfBytes))
                        {
                            ms.Position = 0;
                            mailMessage.Attachments.Add(new Attachment(ms, "WorkEstimate.pdf", "application/pdf"));

                            // Send the email
                            smtpClient.Send(mailMessage);
                        }
                    }
                }



                return Ok("Email sent successfully with PDF attachment");
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



        #region PDF Generator
        private async Task<byte[]> GeneratePdfAsync(string? htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10 },
                DocumentTitle = "Digital Print Work Estimate"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bootstrap", "bootstrap.min.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Center = "ESTIMATE", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "System Generated Report\nDeveloped & Designed by www.codersf5.com" }
            };

            var watermark = "<div style='position: fixed; width: 100%; height: 100%; z-index: -1; font-size: 200px; color: lightgray; opacity: 0.5; transform: rotate(-45deg); text-align: center; vertical-align: middle; line-height: 20rem;'>ESTIMATE</div>";

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = watermark + htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bootstrap", "bootstrap.min.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Center = "ESTIMATE", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "System Generated Report\nDeveloped & Designed by www.codersf5.com" }
            }
        }
            };

            return _converter.Convert(pdf);
        }

        #endregion

        string ConvertUtcToIst(DateTime utcDateTime)
        {
            // Get Indian Standard Time zone
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            // Convert UTC to IST
            DateTime istDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, istZone);

            // Format datetime as "M/d/yyyy h:mm tt"
            return istDateTime.ToString("M/d/yyyy h:mm tt");
        }
    }



}

