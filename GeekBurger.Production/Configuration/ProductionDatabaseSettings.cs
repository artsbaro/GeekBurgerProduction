namespace GeekBurger.Production.Configuration
{
    public class ProductionDatabaseSettings : IProductionDatabaseSettings
    {
        public string ProductionsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IProductionDatabaseSettings
    {
        string ProductionsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
