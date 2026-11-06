var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. CORS Policy එක මෙතැනින් එකතු කරන්න (builder.Build() ට ඉහළින්)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 2. CORS Middleware එක මෙතැනින් සක්‍රිය කරන්න (UseAuthorization ට ඉහළින්)
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();