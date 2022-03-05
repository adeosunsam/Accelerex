namespace AccelerexTask;

public static class InjectDependency
{
    public static void MediatRInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}
