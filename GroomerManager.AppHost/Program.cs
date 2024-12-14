var builder = DistributedApplication.CreateBuilder(args);

var backend = builder.AddProject<Projects.GroomerManager_API>("backend");

var frontend = builder.AddProject<Projects.GroomerManager_Web>("frontend").WithReference(backend);

builder.Build().Run();
