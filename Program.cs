var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Minimal API";
    config.Title = "MinimalAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
	app.UseOpenApi();
	app.UseSwaggerUi(config =>
	{
		config.DocumentTitle = "Minimal Exercise API";
		config.Path = "/swagger";
		config.DocumentPath = "/swagger/{documentName}/swagger.json";
		config.DocExpansion = "list";
	});
}

List<Order> orders = new() 
{
    new Order {Id = 1, Name = "Bil", Date = "2025-01-05"},
    new Order {Id = 2, Name = "Dator", Date = "2053-03-03"},
    new Order {Id = 3, Name = "Ferrari", Date = "2025-02-17"}    
};

app.MapGet("/orders", () => orders);

app.MapGet("/orders/{id}", (int id) => {
    var order = orders.FirstOrDefault(o => o.Id == id);

    return order is not null ? Results.Ok(order) : Results.NotFound($"Ingen order med ID {id}.");
});

app.MapPost("/orders", (Order newOrder) => {
    if (orders.Any(o => o.Id == newOrder.Id)) return Results.BadRequest("Order-ID redan upptaget");

    orders.Add(newOrder);
    return Results.Created($"/orders/{newOrder.Id}", newOrder);
});

app.MapPut("/orders/{id}", (int id, Order updatedOrder) => {
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order is null) return Results.NotFound("Ordern hittades inte.");

    order.Name = updatedOrder.Name;
    order.Date = updatedOrder.Date;
    return Results.Ok(order);
});

app.MapDelete("/orders/{id}", (int id) => {
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order is null) return Results.NotFound("Ordern hittades inte.");

    orders.Remove(order);
    return Results.Ok($"Ordern med ID {id} togs bort.");

});

app.Run();
