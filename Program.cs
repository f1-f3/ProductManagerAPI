using Microsoft.EntityFrameworkCore;
using ProductManagerAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Remove all input formatters except JSON
    options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.XmlDataContractSerializerInputFormatter>();
    options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerInputFormatter>();
    // Add JSON formatter, not needed??
    // options.InputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.JsonInputFormatter());

    // Remove all output formats except JSON
    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.XmlDataContractSerializerOutputFormatter>();
    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter>();
  

});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultCon")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    DataSeeder(context);
}

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

static void DataSeeder(AppDbContext context)
{
    // seeding product groups if no data present
    if (!context.ProductGroups.Any())
    {
        // parent groups
        var joogidGroup = new ProductGroup { ProductGroupName = "Joogid" };
        context.ProductGroups.Add(joogidGroup);

        var toidudGroup = new ProductGroup { ProductGroupName = "Toidud" };
        context.ProductGroups.Add(toidudGroup);

        context.SaveChanges();

        // sub groups
        var alkoholGroup = new ProductGroup { ProductGroupName = "Alkohol", MainGroupID = joogidGroup.ProductGroupID };
        context.ProductGroups.Add(alkoholGroup);
        var pirukadGroup = new ProductGroup { ProductGroupName = "Pirukad", MainGroupID = toidudGroup.ProductGroupID };
        context.ProductGroups.Add(pirukadGroup);
        context.SaveChanges();
        var olledGroup = new ProductGroup { ProductGroupName = "Õlled", MainGroupID = alkoholGroup.ProductGroupID };
        context.ProductGroups.Add(olledGroup); 
        var veinidGroup = new ProductGroup { ProductGroupName = "Veinid", MainGroupID = alkoholGroup.ProductGroupID };
        context.ProductGroups.Add(veinidGroup);
        context.SaveChanges();
    }
    // seeding stores if no data present
    if (!context.Stores.Any())
    {
        var store1 = new Store { StoreName = "Köögiladu"};
        var store2 = new Store { StoreName = "Baariladu"};
        var store3 = new Store { StoreName = "Keskladu"};

        context.Stores.AddRange(store1, store2, store3);
        context.SaveChanges();
    }

    // seeding products if no data present
    if (!context.Products.Any())
    {
        
        var pirukadGroup = context.ProductGroups.FirstOrDefault(pg => pg.ProductGroupName == "Pirukad");
        var olledGroup = context.ProductGroups.FirstOrDefault(pg => pg.ProductGroupName == "Õlled");
        var veinidGroup = context.ProductGroups.FirstOrDefault(pg => pg.ProductGroupName == "Veinid");

        if(pirukadGroup!= null && olledGroup != null &&veinidGroup != null)
        {
            var product1 = new Product { ProductName = "Kapsapirukas", ProductPrice = 2.50d, ProductVAT = 20, ProductPriceWithVAT = 3d, ProductGroupID = pirukadGroup.ProductGroupID };
            var product2 = new Product { ProductName = "Porgandipirukas", ProductPrice = 2d, ProductVAT = 20, ProductPriceWithVAT = 2.40d, ProductGroupID = pirukadGroup.ProductGroupID };
            var product3 = new Product { ProductName = "Sass", ProductPrice = 3.99d, ProductVAT = 20, ProductPriceWithVAT = 4.79d, ProductGroupID = olledGroup.ProductGroupID };
            var product4 = new Product { ProductName = "Pilku", ProductPrice = 2d, ProductVAT = 20, ProductPriceWithVAT = 2.40d, ProductGroupID = olledGroup.ProductGroupID };
            var product5 = new Product { ProductName = "Kallis punane", ProductPrice = 200d, ProductVAT = 20, ProductPriceWithVAT = 240d, ProductGroupID = veinidGroup.ProductGroupID };
            var product6 = new Product { ProductName = "Keskmise-hinnaga-valge", ProductPrice = 19d, ProductVAT = 20, ProductPriceWithVAT = 22.80d, ProductGroupID = veinidGroup.ProductGroupID };

            context.Products.AddRange(product1, product2, product3, product4, product5, product6);
            context.SaveChanges();
        }
    }
    // seeding storeproducts if no data present
    if (!context.StoreProducts.Any())
    {
        var allStores = context.Stores.ToList();
        var allProducts = context.Products.ToList();

        foreach (var store in allStores)
        {
            foreach (var product in allProducts)
            {
                var storeProduct = new StoreProduct { StoreID = store.StoreID, ProductID = product.ProductID, Quantity = 20 }; // quantity in store
                context.StoreProducts.Add(storeProduct);
            }
        }

        context.SaveChanges();
    }


}
