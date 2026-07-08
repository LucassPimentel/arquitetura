namespace EWallet.Infrastructure.Mongo
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "EWalletRead";
        public string AccountsCollectionName { get; set; } = "Accounts";
    }
}
