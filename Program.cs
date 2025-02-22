using MinimalAPI_par;

List<User> users =
[
    new() { Id = 1, Name = "Dave" },
    new() { Id = 2, Name = "Tony" },
    new() { Id = 3, Name = "Janne" }
];

List<Order> orders = new() 
{
    new Order {Id = 1, Name = "Bil", Date = "2025-01-05"},
    new Order {Id = 2, Name = "Dator", Date = "2053-03-03"},
    new Order {Id = 3, Name = "Ferrari", Date = "2025-02-17"}    
};

List<Category> categories = new() 
{
    new Category {Id = 1, Name = "Movie"},
    new Category {Id = 2, Name = "Games"},
    new Category {Id = 3, Name = "Cars"}    
};

List<Product> products = new()
{
    new Product { Id = 1, Name = "Engine" },
    new Product { Id = 2, Name = "Gearbox" },
    new Product { Id = 3, Name = "Petrol" }
};

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


//USERS
app.MapGet("/users", () => users);

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound($"User not found with ID: {id}");
});

app.MapPost("/users", (User user) =>
{
    if (users.Any(u => u.Id == user.Id))
        return Results.BadRequest($"User with ID: {user.Id} already exists");
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound($"User not found with ID: {id}");
    
    user.Name = updatedUser.Name;
    return Results.Ok(user);
});

app.MapDelete("/users/{id}", (int id) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound($"User not found with ID: {id}");
    users.Remove(user);
    return Results.Ok($"User with id {id} deleted");
});


//// ORDERS
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


// CATEGORIES
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


// PRODUCTS
app.MapGet("/products", () => products);

app.MapGet("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound($"Product not found with ID: {id}");
});

app.MapPost("/products", (Product newProduct) =>
{
    if (products.Any(p => p.Id == newProduct.Id))
        return Results.BadRequest($"Product with ID: {newProduct.Id} already exists");
    products.Add(newProduct);
    return Results.Created($"/products/{newProduct.Id}", newProduct);
});

app.MapPut("/products/{id}", (int id, Product updatedProduct) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound($"Product not found with ID: {id}");
    
    product.Name = updatedProduct.Name;
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", (int id) => 
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound($"Product not found with ID: {id}");
    products.Remove(product);
    return Results.Ok($"Product with id {id} deleted");
});

app.Run();