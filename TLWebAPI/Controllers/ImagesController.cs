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
        //Get Images Given Type
        //Get Images Given Tag
        //Get Images Given Tags
        //Get All Types
        //Get All Tags



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
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetALlImages Method Invoked"));
            try
            {
                var ImageList = await db.Images.ToListAsync();

                if (ImageList.Count == 0 || ImageList == null) //No Images Found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImages Method Returned No Values"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Images Found");
                }

                //Take all db images and convert them into a list of serializable images
                List<Model.Image> SerializedImageList = new List<Model.Image>();
                foreach (var image in ImageList)
                {
                    SerializedImageList.Add(Mapper.Map<Model.Image>(image));
                }
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImages Method Successfully Returned All Image elements from the database"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageList);
            }
            catch (Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetAllImages Method Encountered the following exception: {ex.Message}/n {ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // GET: api/Images/5
        [ResponseType(typeof(DAL.Image))]
        public async Task<IHttpActionResult> GetImage(int id)
        {
            DAL.Image image = await db.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }


    }
}
