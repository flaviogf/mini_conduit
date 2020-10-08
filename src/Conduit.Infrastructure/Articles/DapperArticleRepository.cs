using System;
using System.Threading.Tasks;
using Conduit.Domain.Articles;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Articles
{
    public class DapperArticleRepository : IArticleRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DapperArticleRepository> _logger;

        public DapperArticleRepository(IUnitOfWork uow, ILogger<DapperArticleRepository> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result> Add(Article article)
        {
            try
            {
                await _uow.Connection.ExecuteAsync(
                    sql: "INSERT INTO Articles VALUES (@Id, @Title, @Description, @Body, @AuthorId)",
                    param: new
                    {
                        article.Id,
                        article.Title,
                        article.Description,
                        article.Body,
                        AuthorId = article.Author.Id
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
    }
}
