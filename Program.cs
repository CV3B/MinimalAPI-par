using MinimalAPI_par;

List<User> users =
[
    new() { Id = 1, Name = "Dave" },
    new() { Id = 2, Name = "Tony" },
    new() { Id = 3, Name = "Janne" }
];

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

//GET
app.MapGet("/users", () => users);

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound($"User not found with ID: {id}");
});

//POST
app.MapPost("/users", (User user) =>
{
    if (users.Any(u => u.Id == user.Id))
        return Results.BadRequest($"User with ID: {user.Id} already exists");
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

//PUT
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound($"User not found with ID: {id}");
    
    user.Name = updatedUser.Name;
    return Results.Ok(user);
});

//DELETE
app.MapDelete("/users/{id}", (int id) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound($"User not found with ID: {id}");
    users.Remove(user);
    return Results.Ok($"User with id {id} deleted");
});

app.Run();