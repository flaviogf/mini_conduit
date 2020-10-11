using System;
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

        public async Task<Result> Add(User user)
        {
            try
            {
                await _uow.Connection.ExecuteAsync(
                    sql: @"
                        INSERT INTO [Users] ([Id], [UserName], [Email], [Bio], [Avatar], [PasswordHash])
                        VALUES (@Id, @UserName, @Email, @Bio, @Avatar, @PasswordHash)
                    ",
                    param: user,
                    transaction: _uow.Transaction
                );

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return Result.Failure("Something goes wrong");
            }
        }

        public async Task<Maybe<User>> FindByEmail(string email)
        {
            try
            {
                var user = await _uow.Connection.QueryFirstOrDefaultAsync<User>(
                    sql: "SELECT TOP 1 * FROM [Users] WHERE [Email] = @Email",
                    param: new
                    {
                        Email = email
                    },
                    transaction: _uow.Transaction
                );

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return null;
            }
        }

        public async Task<bool> CheckEmail(string email)
        {
            try
            {
                var result = await _uow.Connection.QueryFirstOrDefaultAsync<bool>(
                    sql: @"
                        SELECT
                            CASE WHEN EXISTS(SELECT TOP 1 * FROM [Users] WHERE [Email] = @Email)
                                THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT)
                            END
                    ",
                    param: new
                    {
                        Email = email
                    },
                    transaction: _uow.Transaction
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return default;
            }
        }
    }
}
