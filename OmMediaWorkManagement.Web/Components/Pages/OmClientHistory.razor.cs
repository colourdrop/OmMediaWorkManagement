
using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using Radzen;
using Radzen.Blazor;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class OmClientHistory
    {
        [Inject]
        public IOmService OmService { get; set; }

        private string responseMessage = "";
        private Radzen.AlertStyle alertColor = Radzen.AlertStyle.Info;
        private bool showAlert = false;
        private RadzenDataGrid<OmClient> clientsGrid = new RadzenDataGrid<OmClient>();
        public List<OmClient> clients { get; set; } = new List<OmClient>();
        private List<OmClient> clientsToInsert = new List<OmClient>();
        private List<OmClient> clientsToUpdate = new List<OmClient>();
        private DataGridEditMode editMode = DataGridEditMode.Multiple;
        private string columnEditing;
        private List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();
        private bool IsFirstRender { get; set; } = true;


        private int GetRowIndex(OmClient client)
        {
            return clients.IndexOf(client);
        }

        private void Reset()
        {
            clientsToInsert.Clear();
            clientsToUpdate.Clear();
        }

        private void Reset(OmClient client)
        {
            clientsToInsert.Remove(client);
            clientsToUpdate.Remove(client);
        }

        protected override async Task OnInitializedAsync()
        {
            clients = await OmService.GetAllClients();
        }

        private async Task EditRow(OmClient client)
        {
            if (editMode == DataGridEditMode.Single && clientsToInsert.Count() > 0)
            {
                Reset();
            }

            clientsToUpdate.Add(client);
            await clientsGrid.EditRow(client);
        }

        private async void OnUpdateRow(OmClient client)
        {
            Reset(client);
            var response = await OmService.UpdateClient(client);
            response.EnsureSuccessStatusCode();
            responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == true)
            {
                alertColor = Radzen.AlertStyle.Success;
            }
            else
            {
                alertColor = Radzen.AlertStyle.Danger;

            }
            showAlert = true; // Show alert
            await clientsGrid.Reload();

        }

        private async Task SaveRow(OmClient client)
        {
            
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationResults, true);
            if (isValid)
            {
                if (client.Id == 0)
                {
                    var response = await OmService.AddClient(client);
                    response.EnsureSuccessStatusCode();
                    responseMessage = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode == true)
                    {
                        alertColor = Radzen.AlertStyle.Success;
                    }
                    else
                    {
                        alertColor = Radzen.AlertStyle.Danger;

                    }
                    showAlert = true; // Show alert
                }
                else
                {
                    await clientsGrid.UpdateRow(client);

                }
            }
            else
            {
                responseMessage = "Please fill all columns";
                alertColor = Radzen.AlertStyle.Warning;
                showAlert = true; // Show alert
            }
          await   RefreshTable();


        }

        private void CancelEdit(OmClient client)
        {
            Reset(client);
            clientsGrid.CancelEditRow(client);
        }

        private async Task DeleteRow(OmClient client)
        {
            Reset(client);

            if (client.Id == 0)
            {
                clientsToInsert.Remove(client);
                clients.Remove(client);
                await clientsGrid.Reload();
            }
            else if (client.Id != 0)
            {
                var response = await OmService.DeleteClient(client.Id);
                clients = clients.Where(c => c.Id != client.Id).ToList();
                await clientsGrid.Reload();
                response.EnsureSuccessStatusCode();
                responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == true)
                {
                    alertColor = Radzen.AlertStyle.Success;
                }
                else
                {
                    alertColor = Radzen.AlertStyle.Danger;

                }
                showAlert = true; // Show alert

            }
            else
            {
                clientsGrid.CancelEditRow(client);
                clientsToInsert.Remove(client);
                await clientsGrid.Reload();
            }
        }

        private async Task InsertRow(int numberOfRowsToAdd)
        {
            for (int i = 0; i < numberOfRowsToAdd; i++)
            {
                var client = new OmClient { CreatedAt = DateTime.UtcNow };
                clientsToInsert.Add(client);
                clients.Insert(0, client);
            }

            foreach (var client in clientsToInsert)
            {
                await clientsGrid.EditRow(client);
            }

            await clientsGrid.Reload();
            StateHasChanged();
        }

        private async Task ClearEmptyRows()
        {
            var emptyClients = clients.Where(c => string.IsNullOrWhiteSpace(c.Name) || string.IsNullOrWhiteSpace(c.CompanyName) || string.IsNullOrWhiteSpace(c.Email) || string.IsNullOrWhiteSpace(c.MobileNumber)).ToList();

            foreach (var client in emptyClients)
            {
                clients.Remove(client);
                clientsToInsert.Remove(client);
                clientsToUpdate.Remove(client);
            }

            await clientsGrid.Reload();
            StateHasChanged();
        }

        private async Task SaveAllRecords()
        {
            var validClientsToInsert = new List<OmClient>();

            foreach (var client in clientsToInsert)
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
                    await OnCreateRow(client);
                }

                clientsToUpdate.Clear();
                clientsToInsert.Clear();

                await clientsGrid.Reload();
                await RefreshTable();
            }
            else
            {
                  responseMessage = "Kindly check all rows as empty rows have been detected by CodersF5 AI.";
                alertColor = Radzen.AlertStyle.Warning;
                showAlert = true; // Show alert

            }
        }

        private async Task OnCreateRow(OmClient client)
        {
            var response = await OmService.AddClient(client);
            clientsToInsert.Remove(client);
            response.EnsureSuccessStatusCode();
            responseMessage = await response.Content.ReadAsStringAsync();
            alertColor = Radzen.AlertStyle.Success;
            showAlert = true; // Show alert
            await RefreshTable();
        }

        private async Task RefreshTable()
        {
            clients = await OmService.GetAllClients();
            await clientsGrid.Reload();
        }

        private async void OnCellClick(DataGridCellMouseEventArgs<OmClient> args)
        {
            if (clientsToUpdate.Any())
            {
                editedFields.Add(new(clientsToUpdate.First().Id, columnEditing));
            }

            columnEditing = args.Column.Property;

            await EditRow(args.Data);
        }

        private bool IsEditing(string columnName, OmClient client)
        {
            return columnEditing == columnName && clientsToUpdate.Contains(client);
        }

        private bool IsValidClient(OmClient client, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(client.Name))
            {
                errors.Add("Name is required.");
            }

            if (string.IsNullOrWhiteSpace(client.MobileNumber))
            {
                errors.Add("Phone number is required.");
            }
            else if (!Regex.IsMatch(client.MobileNumber, @"^\d+$"))
            {
                errors.Add("Invalid phone number.");
            }

            if (string.IsNullOrWhiteSpace(client.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!new EmailAddressAttribute().IsValid(client.Email))
            {
                errors.Add("Invalid email address.");
            }

            if (client.CreatedAt == default)
            {
                errors.Add("Creation date is required.");
            }

            return errors.Count == 0;
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


    }
}
