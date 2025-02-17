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

List<Category> categories = new() 
{
    new Category {Id = 1, Name = "Movie"},
    new Category {Id = 2, Name = "Games"},
    new Category {Id = 3, Name = "Cars"}    
};

app.MapGet("/categories", () => categories);

app.MapGet("/categories/{id}", (int id) => {
    var category = categories.FirstOrDefault(c => c.Id == id);

    return category is not null ? Results.Ok(category) : Results.NotFound($"Ingen kategori med ID {id}.");
});

app.MapPost("/categories", (Category newCategory) => {
    if (categories.Any(c => c.Id == newCategory.Id)) return Results.BadRequest("Kategori-ID redan upptaget");

    categories.Add(newCategory);
    return Results.Created($"/categories/{newCategory.Id}", newCategory);
});

app.MapPut("/categories/{id}", (int id, Category updatedCategory) => {
    var category = categories.FirstOrDefault(c => c.Id == id);
    if (category is null) return Results.NotFound("Kategorin hittades inte.");

    category.Name = updatedCategory.Name;
    return Results.Ok(category);
});

app.MapDelete("/categories/{id}", (int id) => {
    var category = categories.FirstOrDefault(c => c.Id == id);
    if (category is null) return Results.NotFound("Kategorin hittades inte.");

    categories.Remove(category);
    return Results.Ok($"Kategorin med ID {id} togs bort.");

});

app.Run();