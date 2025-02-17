using MinimalAPI_par;

List<User> users =
[
    new User { Id = 1, Name = "Dave" },
    new User { Id = 2, Name = "Tony" },
    new User { Id = 3, Name = "Janne" }
];

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users", () => users);

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound($"User not found with ID: {id}");
});

app.Run();
