using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DAL = TLWebsiteDAL;
using Models = TLWebAPI.Models;

namespace TLWebAPI
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DAL.Image, Models.Image>();
                cfg.CreateMap<DAL.ImageType, Models.ImageType>();
                cfg.CreateMap<DAL.ImageTag, Models.ImageTag>();

                cfg.CreateMap<Models.Image, DAL.Image>();
                cfg.CreateMap<Models.ImageType, DAL.ImageType>();
                cfg.CreateMap<Models.ImageTag, DAL.ImageTag>();
            });
        }
    }
}
