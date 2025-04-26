using DotNetEnv;
using FuriaChatBotApi.Configs;
using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Services;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Carrega variáveis do .env
Env.Load();

// Configurações manuais (simples e seguras)
builder.Services.Configure<GeminiSettings>(options => {
    options.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
    options.Endpoint = Environment.GetEnvironmentVariable("GEMINI_API_ENDPOINT");
});

// Serviços da API
builder.Services.AddHttpClient<ILLMService, LLMService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
