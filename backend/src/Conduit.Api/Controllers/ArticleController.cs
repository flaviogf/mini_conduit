using System;
using System.Threading.Tasks;
using Conduit.Api.Infrastructure;
using Conduit.Api.Models;
using Conduit.Api.Repositories;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("articles")]
    public class ArticleController : ApplicationController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuth _auth;
        private readonly IUnitOfWork _uow;

        public ArticleController(IArticleRepository articleRepository, IAuth auth, IUnitOfWork uow)
        {
            _articleRepository = articleRepository;
            _auth = auth;
            _uow = uow;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] StoreArticleRequest request)
        {
            User user = await _auth.GetUser();

            var article = new Article
            {
                Slug = Guid.NewGuid().ToString(),
                Title = request.Article.Title,
                Description = request.Article.Description,
                Body = request.Article.Body,
                TagList = request.Article.TagList,
                Author = user
            };

            await _articleRepository.Save(article);

            await _uow.Commit();

            return Created(new ArticleResponse(article));
        }
    }
}
