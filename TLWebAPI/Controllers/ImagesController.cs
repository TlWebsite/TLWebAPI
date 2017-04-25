using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL = TLWebsiteDAL;
using Model = TLWebAPI.Models;
using AutoMapper;
using System.Web.Http.Cors;
using NLog;


namespace TLWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImagesController : ApiController
    {
        //Get Images Given Tag
        //Get Images Given Tags
        //Get All Types
        //Get All Tags

        HelperMethods helper = new HelperMethods();

        private DAL.TLWebSiteDBEntities db = new DAL.TLWebSiteDBEntities();

        /// <summary>
        /// Will return all images from the database
        /// </summary>
        /// <endpoint>HTTPGet: api/Images/GetAllImages</endpoint>
        /// <returns>List of Images</returns>
        [ActionName("GetAllImages")]
        [ResponseType(typeof(List<Model.Image>))]
        public async Task<HttpResponseMessage> GetAllImages()
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImages Method Invoked"));
            try
            {
                var ImageList = await db.Images.ToListAsync();

                if (ImageList.Count == 0 || ImageList == null) //No Images Found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImages Method Returned No Values"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Images Found");
                }

                //Take all db images and convert them into a list of serializable images
                List<Model.Image> SerializedImageList = helper.SerializeImageList(ImageList);

                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetAllImages Method Successfully Returned All {SerializedImageList.Count} Image elements from the database"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageList);
            }
            catch (Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetAllImages Method Encountered the following exception: {ex.Message}/n {ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Will return the image information given a specific Image ID
        /// </summary>
        /// <param name="id">string ImageID</param>
        /// <endpoint>HTTPGet: api/GetImage/?id={id}</endpoint>
        /// <returns>Image Model</returns>
        [ActionName("GetImage")]
        [ResponseType(typeof(Model.Image))]
        public async Task<HttpResponseMessage> GetImage(string id)
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImage Method Invoked with paramter {id}"));
            try
            {
                var image = await db.Images.Where(x => x.ImageID == id).ToListAsync();
                if (image == null || image.Count <= 0) //image not found for given id
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImage Method Was not able to return an image for paramter {id}"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Image Not Found");
                }
                //return image given id
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImage Method Successfully Returned The Image With The ID {id}"));
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Model.Image>(image.FirstOrDefault()));
            }catch(Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImage Method Encountered the following exception: {ex.Message}/n{ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

  
        }

        /// <summary>
        ///  Will return all image information that falls within the specified imagetype
        /// </summary>
        /// <param name="typeName">string ImageTypeName</param>
        /// <endpoint>HTTPGet: api/Images/GetImagesByType?typeName={id} </endpoint>
        /// <returns>Returns List of Image Models</returns>
        [ActionName("GetImagesByType")]
        [ResponseType(typeof(List<Model.Image>))]
        public HttpResponseMessage GetImagseByType(string typeName)
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByType Method invoked given typename: {typeName}"));
            try
            {
                var ImageList = db.ImageTypes.Where(x => x.ImageTypeName == typeName).FirstOrDefault().Images.ToList();
                if (ImageList.Count == 0 || ImageList == null) //No Images Found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByType Method Returned No Values given typename: {typeName}"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Images Found");
                }
                List<Model.Image> SerializedImageList = helper.SerializeImageList(ImageList);
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByType Method Successfully returned {SerializedImageList.Count} images given typename: {typeName}"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageList);
            }
            catch(Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByType Method Encountered the following exception: {ex.Message}/n{ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Will Return all image information that falls within the specified ImageTag
        /// </summary>
        /// <param name="tagName">string ImageTagName</param>
        /// <endpoint>HTTPGet: api/Images/GetImagesByTag?tagName={id}</endpoint>
        /// <returns>List of Image Models</returns>
        [ActionName("GetImagesByTag")]
        [ResponseType(typeof(List<Model.Image>))]
        public HttpResponseMessage GetImagesByTag(string tagName)
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByTag Method invoked given tagname: {tagName}"));
            try
            {
                var ImageList = db.ImageTags.Where(x => x.ImageTagName == tagName).FirstOrDefault().Images.ToList();
                if (ImageList.Count == 0 || ImageList == null) //No Images Found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByTag Method Returned No Values given tagname: {tagName}"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Images Found");
                }
                List<Model.Image> SerializedImageList = helper.SerializeImageList(ImageList);
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByTag Method Successfully returned {SerializedImageList.Count} images given tagname: {tagName}"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageList);
            }
            catch (Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesByTag Method Encountered the following exception: {ex.Message}/n{ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
