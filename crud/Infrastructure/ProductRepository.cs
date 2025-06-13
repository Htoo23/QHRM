using crud.Domain;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace crud.Infrastructure
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT Id, Name, Description, CreatedAt FROM Products ORDER BY CreatedAt DESC"; // Order by CreatedAt
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT Id, Name, Description, CreatedAt FROM Products WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<Product> AddAsync(Product product)
        {
            using var connection = CreateConnection();
            var sql = @"
            INSERT INTO Products (Name, Description, CreatedAt)
            VALUES (@Name, @Description, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);"; // Get the newly inserted Id
            var id = await connection.QuerySingleAsync<int>(sql, product);
            product.Id = id;
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using var connection = CreateConnection();
            var sql = @"
            UPDATE Products
            SET Name = @Name, Description = @Description
            WHERE Id = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, product);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Products WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}
