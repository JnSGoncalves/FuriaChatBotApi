using DotNetEnv;
using FuriaChatBotApi.Configs;
using FuriaChatBotApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Carrega variáveis do .env
Env.Load();

builder.Services.Configure<GeminiSettings>(options => {
    options.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
    options.Endpoint = Environment.GetEnvironmentVariable("GEMINI_API_ENDPOINT");
});

// Serviços da API
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WitAiService, WitAiService>();
builder.Services.AddTransient<PandaScoreService, PandaScoreService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IIntentProcessingService, IntentProcessingService>();
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
