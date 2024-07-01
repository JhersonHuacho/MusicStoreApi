using Microsoft.EntityFrameworkCore;
using MusicStore.Persistence;
using MusicStore.Repositories;
using MusicStore.Services.Implementation;
using MusicStore.Services.Interfaces;
using MusicStore.Services.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuring Context
builder.Services.AddDbContext<ApplicationDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Registering services
builder.Services.AddTransient<IGenreRespository,GenreRespository>();
builder.Services.AddTransient<IConcertRepository,ConcertRepository>();
builder.Services.AddTransient<ISaleRepository, SaleRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

builder.Services.AddTransient<IConcertService, ConcertService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<ISaleService, SaleService>();

builder.Services.AddAutoMapper(config => 
{
    config.AddProfile<ConcertProfile>();
    config.AddProfile<GenreProfile>();
	config.AddProfile<SaleProfile>();
});

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
