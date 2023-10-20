using ShortLink.Domain.Models.Link;
using System;
using System.Threading.Tasks;
using ShortLink.Domain.ViewModels.Link;

namespace ShortLink.Domain.Interface
{
    public interface ILinkRepository:IAsyncDisposable
    {
        #region link
        Task AddLink(ShortUrl url);
        Task AddOs(Os os);
        Task AddDevice(Device device);
        Task AddBrowser(Browser browser);
        Task<ShortUrl> FindUrlByToken(string token);
        Task AddRequestUrl(RequestUrl requestUrl);
        public Task<Tuple<List<AllLinkViewModel>, int>> GetAllLink(int pageId, string filter, string orderBy,int take);

        #endregion
        Task SaveChange();
    }
}
