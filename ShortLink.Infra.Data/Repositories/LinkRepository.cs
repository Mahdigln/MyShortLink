using ShortLink.Domain.Interface;
using ShortLink.Domain.Models.Link;
using ShortLink.Infra.Data.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShortLink.Domain.ViewModels.Link;

namespace ShortLink.Infra.Data.Repositories
{
    public class LinkRepository : ILinkRepository
    {
        #region constractor
        private readonly ShortLinkContext _context;
        public LinkRepository(ShortLinkContext context)
        {
            _context = context;
        }


        #endregion

        #region link
        public async Task AddLink(ShortUrl url)
        {
            await _context.AddAsync(url);

        }

        public async Task AddOs(Os os)
        {
            await _context.Os.AddAsync(os);
        }

        public async Task AddBrowser(Browser browser)
        {
            await _context.Browsers.AddAsync(browser);
        }

        public async Task AddDevice(Device device)
        {
            await _context.Devices.AddAsync(device);
        }
        public async Task<ShortUrl> FindUrlByToken(string token)
        {
            return await _context.ShortUrls.AsQueryable().SingleOrDefaultAsync(u => u.Token == token);
        }

        public async Task AddRequestUrl(RequestUrl requestUrl)
        {
            await _context.RequestUrls.AddAsync(requestUrl);
        }

        
        public async Task<Tuple<List<AllLinkViewModel>, int>>  GetAllLink(int pageId, string filter, string orderBy,int take)
        {
            if (take == 0)
                take = 10;

            IQueryable<ShortUrl> result =  _context.ShortUrls;

            if (! string.IsNullOrEmpty(filter))
            {
                result =  result.Where(u => u.Token.Contains(filter));
            }


            switch (orderBy)
            {
                case "Ascending":
                    {
                        result = result.OrderBy(c => c.CreateDate);
                        break;
                    }
                case "Descending":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }

            }
            int skip = (pageId - 1) * take;

            int pageCount =  result.Select(c => new AllLinkViewModel
            {
                OrginalUrl = c.OrginalUrl.ToString(),
                Token = c.Token.ToString(),
                CreateDate = c.CreateDate,
                Value = c.Value.ToString()
            }).ToList().Count()/take;

            var query = result.Select(c => new AllLinkViewModel
            {
                OrginalUrl = c.OrginalUrl.ToString(),
                Token = c.Token.ToString(),
                CreateDate = c.CreateDate,
                Value = c.Value.ToString()
            }).Skip(skip).Take(take).AsEnumerable().ToList();

           
            return   Tuple.Create(query, pageCount);
           
        }

       
        #endregion

        #region dispose & save change
        public async ValueTask DisposeAsync()
        {
            if (_context != null) await _context.DisposeAsync();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        
        #endregion
    }
}
