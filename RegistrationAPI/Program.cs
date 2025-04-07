using RegistrationAPI.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Registration API", Version = "v1" });
});


builder.Services.AddCors();

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registration API v1");
        c.RoutePrefix = "swagger";
    });
}


app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


app.UseAuthorization();
app.MapControllers();

app.Run();
