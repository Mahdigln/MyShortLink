using ShortLink.Application.DTOs.Link;
using ShortLink.Domain.Models.Link;
using System;
using System.Threading.Tasks;
using ShortLink.Domain.ViewModels.Link;

namespace ShortLink.Application.Interfaces
{
    public interface ILinkService
    {
        #region link
        ShortUrl QuickShortUrl(Uri uri);
        Task<UrlRequestResult> AddLink(ShortUrl url);
        Task AddUserAgent(string userAgnet);
        Task<ShortUrl> FindUrlByToken(string token);
        Task AddRequestUrl(string token);

         Task<Tuple<List<AllLinkViewModel>, int>> GetAllLink(int pageId = 1, string filter = "",
            string orderBy = "Descending", int take = 0);



        #endregion
    }
}
