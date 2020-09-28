using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Conduit.Api.Infrastructure
{
    public class Pagination
    {
        protected Pagination(IEnumerable items, int page, int pageSize)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            Total = items.Cast<object>().Count();
            Pages = (int)Math.Ceiling(Total / (double)pageSize);
        }

        public IEnumerable Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int Total { get; }

        public int Pages { get; }

        public static Pagination<T> Of<T>(IEnumerable<T> items, int page, int pageSize)
        {
            return new Pagination<T>(items, page, pageSize);
        }
    }

    public class Pagination<T> : Pagination
    {
        public Pagination(IEnumerable<T> items, int page, int pageSize) : base(items, page, pageSize)
        {
        }
    }
}
