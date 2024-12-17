var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("groomer-manager-store")
    .WithDataVolume()
    .WithPgAdmin();

var backend = builder.AddProject<Projects.GroomerManager_API>("backend")
    .WaitFor(postgres)
    .WithReference(postgres);

var frontend = builder.AddProject<Projects.GroomerManager_Web>("frontend")
    .WithReference(backend);

builder.Build().Run();
