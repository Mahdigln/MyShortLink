﻿using ShortLink.Application.DTOs.Link;
using ShortLink.Application.Generator;
using ShortLink.Application.Interfaces;
using ShortLink.Domain.Interface;
using ShortLink.Domain.Models.Link;
using System;
using System.Threading.Tasks;
using ShortLink.Domain.ViewModels.Link;
using UAParser;

namespace ShortLink.Application.Services
{
    public class LinkService : ILinkService
    {
        #region constractor
        private readonly ILinkRepository _linkRepository;
        public LinkService(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        
        #endregion

        #region link
        public ShortUrl QuickShortUrl(Uri uri)
        {
            var shortUrl = new ShortUrl();
            shortUrl.OrginalUrl = uri;
            shortUrl.CreateDate = DateTime.Now;
            shortUrl.Token = Generate.Token();
            shortUrl.Value = new Uri($"https://localhost:7060/{shortUrl.Token}");
            return shortUrl;
        }
        public async Task<UrlRequestResult> AddLink(ShortUrl url)
        {
            if (url == null) return UrlRequestResult.Error;

            await _linkRepository.AddLink(url);
            await _linkRepository.SaveChange();

            return UrlRequestResult.Success;
        }

        public async Task AddUserAgent(string userAgnet)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo client = uaParser.Parse(userAgnet);

            var Os = new Os()
            {
                Family = client.OS.Family,
                Major = client.OS.Major,
                Minor = "No Data",
               // Minor = client.OS.Minor,
                CreateDate = DateTime.Now
            };
            await _linkRepository.AddOs(Os);


            var device = new ShortLink.Domain.Models.Link.Device
            {
                IsBot = client.Device.IsSpider,
                Brand = client.Device.Brand,
                Family = client.Device.Family,
                Model = client.Device.Model,
                CreateDate = DateTime.Now
            };
            await _linkRepository.AddDevice(device);

            var browser = new ShortLink.Domain.Models.Link.Browser
            {
                Family = client.UA.Family,
                Major = client.UA.Major,
                Minor = client.UA.Minor,
                CreateDate = DateTime.Now
            };
            await _linkRepository.AddBrowser(browser);
            await _linkRepository.SaveChange();
        }

        public async Task<ShortUrl> FindUrlByToken(string token)
        {
            return await _linkRepository.FindUrlByToken(token);
        }

        public async Task AddRequestUrl(string token)
        {
            var shortUrl = await _linkRepository.FindUrlByToken(token);

            var requestUrl = new RequestUrl
            {
                ShortUrlId = shortUrl.Id,
                RequestDataTime = DateTime.Now
            };

            requestUrl.CreateDate = DateTime.Now;

            await _linkRepository.AddRequestUrl(requestUrl);
            await _linkRepository.SaveChange();
        }

        public async Task<Tuple<List<AllLinkViewModel>, int>> GetAllLink(int pageId = 1, string filter = "", string orderBy = "Ascending", int take = 0)
        {
            return await _linkRepository.GetAllLink(pageId, filter, orderBy, take);
        }


        

        #endregion
    }
}
