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
                    sql: "INSERT INTO Users VALUES (@Id, @UserName, @Email, @Bio, @Avatar, @PasswordHash)",
                    param: new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.Bio,
                        user.Avatar,
                        user.PasswordHash
                    },
                    transaction: _uow.Transaction
                );

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Result.Failure("Something goes wrong");
            }
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

        public async Task<bool> CheckId(string id)
        {
            try
            {
                var result = await _uow.Connection.QueryFirstOrDefaultAsync<bool>(
                    sql: "SELECT CASE WHEN EXISTS(SELECT TOP 1 1 FROM Users WHERE Id = @Id) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
                    param: new
                    {
                        Id = id
                    },
                    transaction: _uow.Transaction
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return false;
            }
        }

        public async Task<bool> CheckUserName(string userName)
        {
            try
            {
                var result = await _uow.Connection.QueryFirstOrDefaultAsync<bool>(
                    sql: "SELECT CASE WHEN EXISTS(SELECT TOP 1 1 FROM Users WHERE UserName = @UserName) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
                    param: new
                    {
                        UserName = userName
                    },
                    transaction: _uow.Transaction
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return false;
            }
        }

        public async Task<bool> CheckEmail(string email)
        {
            try
            {
                var result = await _uow.Connection.QueryFirstOrDefaultAsync<bool>(
                    sql: "SELECT CASE WHEN EXISTS(SELECT TOP 1 1 FROM Users WHERE Email = @Email) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
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
                _logger.LogError(ex.Message, ex);

                return false;
            }
        }
    }
}
