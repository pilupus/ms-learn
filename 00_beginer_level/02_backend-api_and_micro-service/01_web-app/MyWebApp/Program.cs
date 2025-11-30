using Microsoft.AspNetCore.Rewrite;
using MyWebApp.Interfaces;
using MyWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPersonService, PersonService>();
// builder.Services.AddSingleton<IWelcomeService, WelcomeService>();  // 싱글톤
// builder.Services.AddScoped<IWelcomeService, WelcomeService>();  // 범위 지정
builder.Services.AddTransient<IWelcomeService, WelcomeService>();  // 임시

var app = builder.Build();

app.Use(async (context, next) =>
{
    await next();
    Console.WriteLine($"{context.Request.Method} {context.Request.Path} {context.Response.StatusCode}");
});

app.UseRewriter(new RewriteOptions().AddRedirect("history", "about"));

// app.MapGet("/", (IWelcomeService welcomeService) => welcomeService.GetWelcomeMessage());
app.MapGet("/", async (IWelcomeService welcomeService1, IWelcomeService welcomeService2) =>
    {
        string message1 = $"Message1: {welcomeService1.GetWelcomeMessage()}";
        string message2 = $"Message2: {welcomeService2.GetWelcomeMessage()}";
        return $"{message1}\n{message2}";
    }
);
app.MapGet("/about", (IPersonService personService) =>
    {
        return $"Hello, {personService.GetPersonName()}!";
    }
);

app.Run();