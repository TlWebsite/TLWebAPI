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
    }
}