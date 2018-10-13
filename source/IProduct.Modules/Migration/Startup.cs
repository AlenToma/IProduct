using System.Collections.Generic;
using System.Globalization;
using EntityWorker.Core.InterFace;
using IProduct.Modules.Library;
using System.Linq;

namespace IProduct.Modules.Migration
{
    public class Startup : EntityWorker.Core.Object.Library.Migration
    {
        public override void ExecuteMigration(IRepository repository)
        {

            // Insert test data
            var sql = Actions.GetSql("testData.sql").Trim();
            if (!string.IsNullOrEmpty(sql))
                repository.ExecuteNonQuery(repository.GetSqlCommand(sql));
            else
            {

                CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
                cinfo.ToList().Select(x => new Country() { Name = x.DisplayName, CountryCode = x.Name, Active = x.Name == "en-US" }).ToList().ForEach(x => repository.Save(x));

                var adminUser = new User()
                {
                    Email = "alen.toma@gmail.com",
                    Password = "theway",
                    System = true,
                    Role = new Role()
                    {
                        RoleType = EnumHelper.Roles.Administrator,
                        Name = "Administrator",
                        Description = "this is admin role",
                        System = true
                    },
                    Person = new Person()
                    {
                        FirstName = "Alen",
                        LastName = "Toma",
                        Address = new Address()
                        {
                            AddressLine = "Bogsätravägen 21",
                            Code = "127 38",
                            State = "Skärholmen",
                            Country = repository.Get<Country>().Where(x => x.Active).ExecuteFirstOrDefault()
                        }
                    }
                };

                repository.Save(adminUser);

                var testCat = new Category
                {
                    Name = "Elektronik",
                    Description = "elektronik tillbehör",
                    Categories = new List<Category>
                {
                    new Category()
                    {
                     Name= "Stationära datorer"
                    },
                    new Category()
                    {
                     Name= "Datorer & Spel"
                    }
                }
                };
                repository.Save(testCat);

                var product = new Product()
                {
                    Name = "test",
                    Description = "test",
                    Content = "<p>hahaha</p>",
                    Price = 150,
                    ProductCategories = new List<ProductCategories>()
                {
                  new ProductCategories()
                  {
                      Category = testCat
                  }
                },
                    Country = repository.Get<Country>().ExecuteFirstOrDefault(),
                };

                repository.Save(product);

                var root = new Mapps
                {
                    Name = "Root",
                    Description = "This is default mapp, and cant be deleted",
                    System = true,
                    Children = new List<Mapps>
                {
                    new Mapps()
                    {
                        Name = "Images",
                        Description = "Image files only",
                        System = true
                    },
                    new Mapps() {
                        Name = "Extra",
                        Description = "None Image file",
                        System = true
                    }
                }
                };
                repository.Save(root);
                repository.SaveChanges();
            }
        }
    }
}
