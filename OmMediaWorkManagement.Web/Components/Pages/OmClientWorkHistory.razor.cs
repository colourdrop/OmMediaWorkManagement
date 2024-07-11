using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
using OmMediaWorkManagement.Web.Helper;
using Org.BouncyCastle.Utilities;
using Radzen;
using Radzen.Blazor;
using File = System.IO.File;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class OmClientWorkHistory
    {
        [Inject]
        public IOmService OmService { get; set; }
        private int selectedClientId;
        private string responseMessage = "";
        private Radzen.AlertStyle alertColor = Radzen.AlertStyle.Info;
        private bool showAlert = false;
        private bool IsFirstRender { get; set; } = true;
        private RadzenDataGrid<OmClientWork> clientsWorkGrid;
        IEnumerable<OmClientWork> clientWork;
        bool allowRowSelectOnRowClick = false;

        IList<OmClientWork> selectedOmClientWork;
        public List<OmClientWork> ClientWorkHistory { get; set; } = new List<OmClientWork>();
        public List<OmClient> clients { get; set; } = new List<OmClient>();
        private List<OmClientWork> filteredClientWorkHistory = new List<OmClientWork>();
        private List<OmClientWork> clientsWorkToInsert = new List<OmClientWork>();
        private List<OmClientWork> clientsWorkToUpdate = new List<OmClientWork>();
        private DataGridEditMode editMode = DataGridEditMode.Multiple;
        private string columnEditing;
        private List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();
        [Inject]
        public IPdfService _pdfService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            ClientWorkHistory = await OmService.GetAllClientWork();
            clients = await OmService.GetAllClients();
            clientWork = ClientWorkHistory;
            filteredClientWorkHistory = ClientWorkHistory.ToList(); // Ensure a copy is made
        }
        private async Task OnClientSelected(object value)
        {
            selectedClientId = (int)value;
          
            if (selectedClientId != 0)
            {
                filteredClientWorkHistory = await OmService.GetClientWorkById(selectedClientId);

            }

            else
            {
                filteredClientWorkHistory = await OmService.GetAllClientWork();
            }
            await clientsWorkGrid.Reload(); // Reload the grid to display the new data

        }
        private void Reset()
        {
            clientsWorkToInsert.Clear();
            clientsWorkToUpdate.Clear();
        }

        private void Reset(OmClientWork client)
        {
            clientsWorkToInsert.Remove(client);
            clientsWorkToUpdate.Remove(client);
        }

        private async Task EditRow(OmClientWork client)
        {
            if (editMode == DataGridEditMode.Single && clientsWorkToInsert.Count() > 0)
            {
                Reset();
            }

            clientsWorkToUpdate.Add(client);
            await clientsWorkGrid.EditRow(client);
        }


        private async void OnUpdateRow(OmClientWork client)
        {
            if (client.Id != 0)
            {
                Reset(client);
                var response = await OmService.UpdateClientWork(client);
                await LoadData();
                response.EnsureSuccessStatusCode();
                responseMessage = await response.Content.ReadAsStringAsync();
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true; // Show alert
            }
        }

        private async Task SaveRow(OmClientWork client)
        {
            CalculateTotal(client);
            if (client.Id == 0)
            {
                AddWorkViewModel addWorkViewModel = new AddWorkViewModel()
                {
                    ClientId = client.OmClientId,
                    WorkDate = client.WorkDate.ToUniversalTime(),
                    PrintCount = client.PrintCount,
                    Price = client.Price,
                    PaidAmount = client.PaidAmount,
                    TotalPayable = client.TotalPayable,
                    DueBalance = client.DueBalance,
                    Remarks = client.Remarks,
                    Total = 0,
                    WorkDetails = client.WorkDetails
                };


                var response = await OmService.AddClientWork(addWorkViewModel);

                await LoadData();
                response.EnsureSuccessStatusCode();
                responseMessage = await response.Content.ReadAsStringAsync();
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true; // Show alert


            }
            else
            {
                await clientsWorkGrid.UpdateRow(client);
            }
        }

        private void CancelEdit(OmClientWork client)
        {
            Reset(client);
            clientsWorkGrid.CancelEditRow(client);


        }

        private async Task DeleteRow(OmClientWork client)
        {
            Reset(client);

            if (ClientWorkHistory.Contains(client))
            {
                var response = await OmService.DeleteClientWork(client.Id, client.OmClientId);

                await clientsWorkGrid.Reload();
                response.EnsureSuccessStatusCode();
                responseMessage = await response.Content.ReadAsStringAsync();
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true; // Show alert
            }
            else
            {
                clientsWorkGrid.CancelEditRow(client);

            }
            await LoadData();
        }



        private async Task OnCreateRow(OmClientWork client)
        {
            AddWorkViewModel addWorkViewModel = new AddWorkViewModel()
            {
                ClientId = client.OmClientId,
                WorkDate = client.WorkDate.ToUniversalTime(),
                PrintCount = client.PrintCount,
                Price = client.Price,
                Remarks = client.Remarks,
                Total = client.Total,
                WorkDetails = client.WorkDetails
            };
            if (clientsWorkToInsert.Contains(client))
            {
                clientsWorkToInsert.Remove(client);
                var response = await OmService.AddClientWork(addWorkViewModel);
                response.EnsureSuccessStatusCode();
                responseMessage = await response.Content.ReadAsStringAsync();
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true; // Show alert
            }
        }
        public async Task UpdateIsPaidStatus(OmClientWork work, bool isPaid)
        {
            var result = await OmService.UpdateClientPaymentWorkStatus(work.OmClientId, work.Id, isPaid);
            await LoadData();

        }
        public async Task SendEmail(OmClientWork work)
        {
            var response = await OmService.SendEmailByClientId(work.OmClientId, work.Id);
            response.EnsureSuccessStatusCode();
            responseMessage = await response.Content.ReadAsStringAsync();
            alertColor = Radzen.AlertStyle.Success;
            showAlert = true; // Show alert
            await LoadData();

        }
        private async void CalculateTotal(OmClientWork work)
        {
            work.TotalPayable = work.PrintCount * work.Price;
            work.DueBalance = work.TotalPayable - work.PaidAmount;
            StateHasChanged();

        }

        private async Task InsertRow(int numberOfRowsToAdd)
        {
            for (int i = 0; i < numberOfRowsToAdd; i++)
            {
                var clientWork = new OmClientWork { WorkDate = DateTime.UtcNow };
                clientsWorkToInsert.Add(clientWork);
                filteredClientWorkHistory.Insert(0, clientWork);
            }

            foreach (var client in clientsWorkToInsert)
            {
                await clientsWorkGrid.EditRow(client);
            }

            await clientsWorkGrid.Reload();
            StateHasChanged();
        }

        private async Task ClearEmptyRows()
        {
            var emptyClients = filteredClientWorkHistory.Where(c => string.IsNullOrWhiteSpace(c.WorkDetails) || c.Price == 0 || c.PrintCount == 0).ToList();

            foreach (var client in emptyClients)
            {
                filteredClientWorkHistory.Remove(client);
                clientsWorkToInsert.Remove(client);
                clientsWorkToUpdate.Remove(client);
            }

            await clientsWorkGrid.Reload();
            StateHasChanged();
        }


        private async Task SaveAllRecords()
        {
            var validClientsToInsert = new List<OmClientWork>();

            foreach (var client in clientsWorkToInsert)
            {
                if (IsValidClient(client, out List<string> errors))
                {
                    validClientsToInsert.Add(client);
                }
            }
            if (validClientsToInsert.Count > 0)
            {
                foreach (var client in validClientsToInsert)
                {
                    CalculateTotal(client);
                    await OnCreateRow(client);
                }

                clientsWorkToUpdate.Clear();
                clientsWorkToInsert.Clear();

                await clientsWorkGrid.Reload();

            }
            else
            {
                responseMessage = "Kindly check all rows as empty rows have been detected by CodersF5 AI.";
                alertColor = Radzen.AlertStyle.Warning;
                showAlert = true; // Show alert

            }
            await LoadData();

        }
        private bool IsValidClient(OmClientWork client, out List<string> errors)
        {
            errors = new List<string>();

            // Validate specific properties of OmClientWork
            if (client == null)
            {
                errors.Add("Client object is null.");
                return false;
            }

            if (client.WorkDate == default)
            {
                errors.Add("Work date is required.");
            }

            if (string.IsNullOrWhiteSpace(client.WorkDetails))
            {
                errors.Add("Work details are required.");
            }

            if (client.PrintCount <= 0)
            {
                errors.Add("Print count must be greater than zero.");
            }

            if (client.Price <= 0)
            {
                errors.Add("Price must be greater than zero.");
            }

            // Add more validations for other properties as needed


            return errors.Count == 0;
        }
        private async void OnCellClick(DataGridCellMouseEventArgs<OmClientWork> args)
        {
            if (clientsWorkToUpdate.Any())
            {
                editedFields.Add(new(clientsWorkToUpdate.First().Id, columnEditing));
            }

            columnEditing = args.Column.Property;

            await EditRow(args.Data);
        }
        private string GetClientName(int clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId)?.Name ?? "Unknown Client";
        }
        string ConvertUtcToIst(DateTime utcDateTime)
        {
            // Get Indian Standard Time zone
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            // Convert UTC to IST
            DateTime istDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, istZone);

            // Format datetime as "M/d/yyyy h:mm tt"
            return istDateTime.ToString("M/d/yyyy h:mm tt");
        }

        decimal CalculateTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (var item in filteredClientWorkHistory)
            {
                totalPrice += item.Price;
            }

            return totalPrice;
        }

        // Method to calculate total total (assuming it's a decimal column)
        int? CalculateTotalTotal()
        {
            int? totalTotal = 0;
            totalTotal = filteredClientWorkHistory.Sum(x => x.TotalPayable);


            return totalTotal;
        }
        int? CalculateTotalPaidAmount()
        {
            int? totalTotal = 0;
            totalTotal = filteredClientWorkHistory.Sum(x => x.PaidAmount);



            return totalTotal;
        }
        int? CalculateTotalDueAmount()
        {
            int? totalTotal = 0;
            totalTotal = filteredClientWorkHistory.Sum(x => x.DueBalance);



            return totalTotal;
        }
        private void OnPriceKeyDown(KeyboardEventArgs args, OmClientWork work)
        {
            if (args.Key == "Tab" || args.Key == "Enter")
            {
                CalculateTotal(work);
            }
        }
        private void OnPaidAmountKeyDown(KeyboardEventArgs args, OmClientWork work)
        {
            if (args.Key == "Tab" || args.Key == "Enter")
            {
                work.DueBalance = work.TotalPayable - work.PaidAmount;

                StateHasChanged();
            }
        }
        public async Task GeneratePdfFromDataGridAsync(List<OmClientWork> data)
        {
           
            var htmlContent =await GenerateHtmlFromDataGrid(data);
            responseMessage = "HTML Generated";
            alertColor = Radzen.AlertStyle.Success;
            showAlert = true; // Show alert
            try
            {
                var pdfBytes = await _pdfService.GeneratePdfAsync(htmlContent);
                responseMessage = "Generating PDF";
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true; // Show alert
                // Optionally, you can save or return the PDF bytes
                // Example: Save to file
                await jsRuntime.InvokeVoidAsync("BlazorDownloadFile", "DigitalPrintEST.pdf", "application/pdf", pdfBytes);
            }
            catch (Exception ex) { }
        }

        public async Task<string> GenerateHtmlFromDataGrid(List<OmClientWork> data)
        {
            // Initialize the HTML content with the opening tags
            var htmlContent = "<html><body><div class='container mt-3'><div class='d-flex justify-content-between mb-3'>";

            // Add headers for Client Name, Estimate, and Print Date
            htmlContent += $"<div class='p-2'>{GetClientName(data.FirstOrDefault().OmClientId)}</div>";
            htmlContent += $"<div class='p-2'>Estimate</div>";
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



    }
}
