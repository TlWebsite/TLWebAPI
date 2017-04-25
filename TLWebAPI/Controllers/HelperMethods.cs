using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model=TLWebAPI.Models;
using DAL=TLWebsiteDAL;
using AutoMapper;

namespace TLWebAPI.Controllers
{
    public class HelperMethods
    {
        public List<Model.Image> SerializeImageList(List<DAL.Image> ImageList)
        {
            List<Model.Image> SerializedImageList = new List<Model.Image>();
            foreach (var image in ImageList)
            {
                SerializedImageList.Add(Mapper.Map<Model.Image>(image));
            }
            return SerializedImageList;
        }

        public List<Model.ImageType> SerializeImageTypeList(List<DAL.ImageType> ImageTypeList)
        {
            List<Model.ImageType> SerializedImageTypeList = new List<Model.ImageType>();
            foreach (var imageType in ImageTypeList)
            {
                SerializedImageTypeList.Add(Mapper.Map<Model.ImageType>(imageType));
            }
            return SerializedImageTypeList;
        }

        public List<Model.ImageTag> SerializeImageTagList(List<DAL.ImageTag> ImageTagList)
        {
            List<Model.ImageTag> SerializedImageTagList = new List<Model.ImageTag>();
            foreach (var imageTag in ImageTagList)
            {
                SerializedImageTagList.Add(Mapper.Map<Model.ImageTag>(imageTag));
            }
            return SerializedImageTagList;
        }
    }
}