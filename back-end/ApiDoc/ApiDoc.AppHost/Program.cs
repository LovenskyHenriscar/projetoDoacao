var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiDoc>("apidoc");

builder.Build().Run();
