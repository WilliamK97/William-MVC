namespace William.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<William.WilliamDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(William.WilliamDB context)
        {
            #region Categories
            context.Categories.AddOrUpdate(x => x.CategoryId,
                new Models.Category
                {
                    CategoryId = 1,
                    Name = "Jeans"
                },
                new Models.Category
                {
                    CategoryId = 2,
                    Name = "T-Shirt"
                },
                new Models.Category
                {
                    CategoryId = 3,
                    Name = "Sweatshirt"
                }
                );
            #endregion

            #region Product with CategoryId 1
            context.Products.AddOrUpdate(x => x.ProductId,
                new Models.Product
                {
                    ProductId = 1,
                    Name = "LEVIS Jeans Herr Svart",
                    Description = "Regular Fit Jeans I Storlek 30/30",
                    Price = 699,
                    CategoryId = 1
                },
                new Models.Product
                {
                    ProductId = 2,
                    Name = "H&M Jeans Herr LjusBlå",
                    Description = "Slim Fit Jeans I Storlek 32/30",
                    Price = 599,
                    CategoryId = 1
                });
            #endregion

            #region Product with CategoryId 2
            context.Products.AddOrUpdate(x => x.ProductId,
                new Models.Product
                {
                    ProductId = 3,
                    Name = "NIKE T-Shirt M",
                    Description = "Vit NIKE T-Shirt I Storlek M",
                    Price = 399,
                    CategoryId = 2
                });
            #endregion

            #region Product with CategoryId 3
            context.Products.AddOrUpdate(x => x.ProductId,
                new Models.Product
                {
                    ProductId = 4,
                    Name = "Adidas Sweatshirt L",
                    Description = "Grön Adidas Sweatshirt I Storlek M",
                    Price = 499,
                    CategoryId = 3
                },
                new Models.Product
                {
                    ProductId = 5,
                    Name = "H&M Sweatshirt S",
                    Description = "Grå Tjocktröja från H&M I Storlek S",
                    Price = 499,
                    CategoryId = 3
                }
                );
            #endregion
        }
    }
}
