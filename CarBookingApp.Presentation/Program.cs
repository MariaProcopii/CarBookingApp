using CarBookingApp.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseDbTransaction();
app.MapControllers();
app.Run();