using EntityWorker.Core.Attributes;
using EntityWorker.Core.Interface;
using EntityWorker.Core.InterFace;
using EntityWorker.Core.Transaction;
using IProduct.Modules.Library;
using IProduct.Modules.Library.Base_Entity;
using IProduct.Modules.Rules;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace IProduct.Modules.Data
{
    public class DbContext : Transaction
    {
        public DbContext() : base(GetConnectionString(), EntityWorker.Core.Helper.DataBaseTypes.Mssql)
        {

        }
        protected override void OnModuleConfiguration(IModuleBuilder moduleBuilder)
        {
            #region User 
            moduleBuilder.Entity<User>()
                .HasForeignKey<Person, Guid>(x => x.Person_Id)
                .HasForeignKey<Role, Guid>(x => x.Role_Id)
                .HasDataEncode(x => x.Password)
                .HasDataEncode(x => x.Email);
            #endregion

            #region Person
            moduleBuilder.Entity<Person>()
                .HasForeignKey<Address, Guid>(x => x.Address_Id)
                .ExcludeFromAbstract(x => x.FullName);
            #endregion

            #region Address
            moduleBuilder.Entity<Address>()
                .HasForeignKey<Country, Guid>(x => x.Country_Id);
            #endregion

            #region City
            moduleBuilder.Entity<City>()
                .HasForeignKey<Country, Guid>(x => x.Country_Id);
            #endregion

            #region Role
            moduleBuilder.Entity<Role>()
                .HasStringify(c => c.RoleType);
            #endregion

            #region Category 
            moduleBuilder.Entity<Category>()
                .HasForeignKey<Category, Guid?>(x => x.Parent_Id);
            #endregion

            #region Files
            moduleBuilder.Entity<Files>()
                .HasForeignKey<Mapps, Guid>(x => x.Mapp_Id)
                .ExcludeFromAbstract(x=> x.FileBytes)
                .ExcludeFromAbstract(x=> x.FileThumpFullPath)
                .ExcludeFromAbstract(x=> x.FileFullPath)
                .HasJsonIgnore(x=> x.FileBytes)
                .HasRule<FileRules>();

            #endregion

            #region Mapps
            moduleBuilder.Entity<Mapps>()
                .HasForeignKey<Mapps, Guid?>(x => x.Parent_Id)
                .HasRule<MappRules>();

            #endregion

            #region ProductCategories
            moduleBuilder.Entity<ProductCategories>()
                .HasForeignKey<Product, Guid>(x => x.Product_Id)
                .HasForeignKey<Category, Guid>(x => x.Category_Id)
                .HasIndependentData(x => x.Category);

            #endregion

            #region ProductImages
            moduleBuilder.Entity<ProductImages>()
                .HasForeignKey<Product, Guid>(x => x.Product_Id)
                .HasForeignKey<Files, Guid>(x => x.Image_Id)
                .HasIndependentData(x => x.Images);

            #endregion

            #region Products
            moduleBuilder.Entity<Product>()
                .HasToBase64String(x => x.Content)
                .HasForeignKey<Country, Guid?>(x => x.PriceCode)
                .HasIndependentData(x => x.Country)
                .HasDefaultOnEmpty(x => x.Country, DateTime.Now)
                .HasStringify(x => x.Status);

            #endregion

            #region ProductDiscounts
            moduleBuilder.Entity<ProductDiscounts>()
                .HasForeignKey<Product, Guid>(x => x.Product_Id)
                .HasForeignKey<Discounts, Guid>(x => x.Discount_Id)
                .HasIndependentData(x => x.Discounts);

            #endregion

            #region ColumnValue
            moduleBuilder.Entity<ColumnValue>()
                .HasForeignKey<Country, Guid>(x => x.Country_Id)
                .HasForeignKey<Column, Guid>(x => x.Column_Id);

            #endregion

            #region ProductTabs
            moduleBuilder.Entity<ProductTabs>()
                .HasForeignKey<Product, Guid>(x => x.Product_Id)
                .HasToBase64String(x => x.Content);
            #endregion

            #region Pages 
            moduleBuilder.Entity<Pages>()
                .HasForeignKey<Pages, Guid?>(x => x.Parent_Id);
            #endregion

            #region PagesCategories
            moduleBuilder.Entity<PageCategories>()
                .HasForeignKey<PageSection, Guid>(x => x.Section_Id)
                .HasForeignKey<Category, Guid>(x => x.Category_Id)
                .HasIndependentData(x => x.Category);
            #endregion

            #region PageSection
            moduleBuilder.Entity<PageSection>()
                .HasStringify(x => x.PageType)
                .HasStringify(x => x.ProductShow)
                .HasForeignKey<Pages, Guid>(x => x.Page_Id);
            #endregion

            #region PagesSlider 
            moduleBuilder.Entity<PagesSlider>()
                .HasForeignKey<Pages, Guid>(x => x.Pages_Id)
                .HasForeignKey<Files, Guid>(x => x.Files_Id)
                .HasIndependentData(x => x.Files);
            #endregion

            #region Invoice 
            moduleBuilder.Entity<Invoice>()
                .HasJsonDocument(x => x.ProductTotalInformations)
                .HasJsonDocument(X => X.Products)
                .ExcludeFromAbstract(x => x.Total)
                .HasStringify(x => x.InvoiceState)
                .HasForeignKey<User, Guid>(x => x.User_Id);
            #endregion
        }

        public override IRepository Save<T>(T entity, params Expression<Func<T, object>>[] ignoredProperties)
        {
            void Prepare(object data)
            {
                if (data == null)
                    return;
                var props = EntityWorker.Core.FastDeepCloner.DeepCloner.GetFastDeepClonerProperties(data.GetType()).Where(x => !x.IsInternalType && x.CanRead);
                foreach (var prop in props)
                {
                    if (prop.ContainAttribute<JsonDocument>() ||
                        prop.ContainAttribute<XmlDocument>() ||
                        prop.ContainAttribute<ExcludeFromAbstract>()) // Ignore 
                        continue;
                    var value = prop.GetValue(data);
                    if (value == null)
                        continue;
                    if (value is IList)
                    {
                        IList newList = (IList)Activator.CreateInstance(value.GetType());
                        var ilist = value as IList;
                        var i = ilist.Count - 1;
                        while (i >= 0)
                        {
                            var e = ilist[i] as Entity;
                            i--;
                            if (e.Object_Status == EnumHelper.ObjectStatus.Removed)
                            {
                                Delete(e);
                            }
                            else newList.Add(e);
                        }
                        prop.SetValue(data, newList);

                    }
                    else
                    {
                        var e = value as Entity;
                        if (e.Object_Status == EnumHelper.ObjectStatus.Removed)
                        {
                            Delete(e);
                            prop.SetValue(data, null);
                        }
                        else Prepare(e);
                    }
                }
            }

            if (entity as Entity != null)
            {
                if ((entity as Entity).Object_Status != EnumHelper.ObjectStatus.Removed)
                    Prepare(entity);
                else
                {
                    Delete(entity);
                    return this;
                }
            }
            return base.Save(entity, ignoredProperties);
        }

        protected override void OnModuleStart()
        {
            if (!base.DataBaseExist())
                base.CreateDataBase();


            // You could choose to use this to apply you changes to the database or create your own migration
            // that will update the database, like alter drop or create.
            // Limited support for sqlite
            // Get the latest change between the code and the database. 
            // Property Rename is not supported. renaming property x will end up removing the x and adding y so there will be dataloss
            // Adding a primary key is not supported either
            var latestChanges = GetCodeLatestChanges();
            if (latestChanges.Any())
                latestChanges.Execute(true);

            // Start the migration
            InitializeMigration();
        }

        // get the full connection string
        public static string GetConnectionString()
        {
            return @"Server=.\SQLEXPRESS; Database=IProduct; Trusted_Connection=True; User Id=root; Password=root;";

        }
    }
}
