using System;
using System.Data;
using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Users
{
    public class DapperUserRepository : IUserRepository
    {
        private readonly IUnitOfWork _uow;

        private readonly ILogger<DapperUserRepository> _logger;

        public DapperUserRepository(IUnitOfWork uow, ILogger<DapperUserRepository> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Maybe<User>> FindOne(string id)
        {
            try
            {
                var user = await _uow.Connection.QueryFirstOrDefaultAsync<User>(
                    sql: "SELECT * FROM Users WHERE Id = @Id",
                    param: new
                    {
                        Id = id
                    },
                    transaction: _uow.Transaction
                );

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return null;
            }
        }
    }
}
