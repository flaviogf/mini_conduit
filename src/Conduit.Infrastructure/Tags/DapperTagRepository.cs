using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Domain.Tags;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Tags
{
    public class DapperTagRepository : ITagRepository
    {
        private readonly IUnitOfWork _uow;

        private readonly ILogger<DapperTagRepository> _logger;

        public DapperTagRepository(IUnitOfWork uow, ILogger<DapperTagRepository> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<IEnumerable<Tag>> FindAll()
        {
            try
            {
                var tags = await _uow.Connection.QueryAsync<Tag>("SELECT * FROM Tags", transaction: _uow.Transaction);

                return tags;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Enumerable.Empty<Tag>();
            }
        }
    }
}
