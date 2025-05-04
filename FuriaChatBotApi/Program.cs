using DotNetEnv;
using FuriaChatBotApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Carrega .env somente no dev
if (builder.Environment.IsDevelopment()) {
    Env.Load();
}

// Documentação Swagger para a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serviços de cache, HTTP e seus próprios
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WitAiService, WitAiService>();
builder.Services.AddTransient<PandaScoreService, PandaScoreService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IIntentProcessingService, IntentProcessingService>();
builder.Services.AddScoped<IChatService, ChatService>();

// Serviços da API
builder.Services.AddControllers();

// Serviços Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient();

var app = builder.Build();

// Pipeline

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
} else {
    app.UseCors(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
    );
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Endpoints da API
app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
