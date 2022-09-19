using WordleSolver.Algorithm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IWordleAlgorithm, WordleAlgorithm>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options =>
    options
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowOrigin");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
