using System;
using System.Collections;
using System.Collections.Generic;

namespace Conduit.Api.Infrastructure
{
    public class Pagination
    {
        protected Pagination(IEnumerable items, int page, int pageSize, int total)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            Total = total;
            Pages = (int)Math.Ceiling(total / (double)pageSize);
        }

        public IEnumerable Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int Total { get; }

        public int Pages { get; }

        public static Pagination<T> Of<T>(IEnumerable<T> items, int page, int pageSize, int total)
        {
            return new Pagination<T>(items, page, pageSize, total);
        }
    }

    public class Pagination<T> : Pagination
    {
        public Pagination(IEnumerable<T> items, int page, int pageSize, int total) : base(items, page, pageSize, total)
        {
        }
    }
}
