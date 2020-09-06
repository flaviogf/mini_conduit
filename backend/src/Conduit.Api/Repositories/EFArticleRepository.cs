using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Repositories
{
    public class EFArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public EFArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Save(Article article)
        {
            await _context.Articles.AddAsync(article);
        }

        public async Task<Maybe<Article>> Find(string slug)
        {
            return await _context.Articles.Include(it => it.Author).FirstOrDefaultAsync(it => it.Slug == slug);
        }

        public IEnumerable<Article> Filter(ArticlesFilter filter, int offset, int limit)
        {
            FormattableString sql = $@"
                SELECT a.*
                FROM Articles a, json_each(a.TagList)
                JOIN Users u ON a.AuthorId = u.Id
                WHERE {filter.Tag} IS NULL OR json_each.value = {filter.Tag}
                AND {filter.Author} IS NULL OR u.Username = {filter.Author}
                ORDER BY a.CreatedAt DESC
                LIMIT {limit} OFFSET {offset}
            ";

            return _context.Articles.FromSqlInterpolated(sql).Include(it => it.Author);
        }
    }
}
