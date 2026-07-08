using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using EWallet.Infrastructure.Mongo;
using EWallet.Domain.Entities;

namespace EWallet.Infrastructure.Repositories
{
    internal class MongoAccountReadModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static MongoAccountReadModel From(AccountReadModel m)
            => new()
            {
                Id = m.Id,
                UserName = m.UserName,
                Balance = m.Balance,
                Status = m.Status,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            };

        public AccountReadModel ToDomain()
            => new()
            {
                Id = Id,
                UserName = UserName,
                Balance = Balance,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
    }

    public class AccountReadRepository : IAccountReadRepository
    {
        private readonly IMongoCollection<MongoAccountReadModel> _collection;

        public AccountReadRepository(IMongoDatabase database, MongoSettings settings)
        {
            _collection = database.GetCollection<MongoAccountReadModel>(settings.AccountsCollectionName);
        }

        public async Task UpsertAsync(AccountReadModel account, CancellationToken cancellationToken = default)
        {
            var mongoModel = MongoAccountReadModel.From(account);
            var filter = Builders<MongoAccountReadModel>.Filter.Eq(a => a.Id, mongoModel.Id);
            await _collection.ReplaceOneAsync(filter, mongoModel, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public async Task<AccountReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MongoAccountReadModel>.Filter.Eq(a => a.Id, id);
            var res = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return res?.ToDomain();
        }

        public async Task<IEnumerable<AccountReadModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var results = await _collection.Find(_ => true).ToListAsync(cancellationToken);
            return results.Select(r => r.ToDomain());
        }
    }
}
