var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.OmMediaWorkManagement_ApiService>("apiservice");

builder.AddProject<Projects.OmMediaWorkManagement_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
