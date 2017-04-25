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
        HelperMethods helper = new HelperMethods();
        private DAL.TLWebSiteDBEntities db = new DAL.TLWebSiteDBEntities();

        /// <summary>
        ///  Method will return all image types that are in the database
        /// </summary>
        /// <endpoint>HTTPGet: api/Images/GetAllImageTypes</endpoint>
        /// <returns>List of tag models</returns>
        [ActionName("GetAllImageTypes")]
        [ResponseType(typeof(List<Model.ImageType>))]
        public async Task<HttpResponseMessage> GetAllImageTypes()
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImageTypes Method Invoked"));
            try
            {
                var ImageTypeList = await db.ImageTypes.ToListAsync();
                if (ImageTypeList == null || ImageTypeList.Count <= 0) //No ImageTypes Found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImageTypes Method was not able to find any ImageTypes"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No ImageTypes Found");
                }
                //Serialized all found ImageTypes into ImageType Models
                var SerializedImageTypeList = helper.SerializeImageTypeList(ImageTypeList);
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetAllImageTypes Method Successfully found and returned {SerializedImageTypeList.Count} Types"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageTypeList);
            }
            catch (Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImageTypes Method the following exception: {ex.Message}/n {ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        ///  Method will return all tags that are in the database
        /// </summary>
        /// <endpoint>HTTPGet: api/Images/GetAllImageTags</endpoint>
        /// <returns>List of tag models</returns>
        [ActionName("GetAllImageTags")]
        [ResponseType(typeof(List<Model.ImageTag>))]
        public async Task<HttpResponseMessage> GetAllImageTags()
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImageTags Method Invoked"));
            try
            {
                var ImageTagList = await db.ImageTags.ToListAsync();
                if (ImageTagList==null|| ImageTagList.Count<=0) //No ImageTags Found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImageTags Method was not able to find any ImageTags"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No ImageTags Found");
                }
                //Serialized all found tags into Tag Models
                var SerializedImageTagList = helper.SerializeImageTagList(ImageTagList);
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetAllImageTags Method Successfully found and returned {SerializedImageTagList.Count} Tags"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageTagList);
            }catch(Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", "GetAllImageTags Method the following exception: {ex.Message}/n {ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

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

        /// <summary>
        ///  Will Return a list of image information that falls within the specified tags (Requires at least one of the specified tags)
        /// </summary>
        /// <param name="tagNames">(from request body)String List of ImageTagNames</param>
        /// <endpoint>HTTPPost: api/Images/GetImagesWithOneOfMultipleTags (POST TO ALLOW FROM BODY PARAMETER) </endpoint>
        /// <returns>List of Image Models</returns>
        [HttpPost]
        [ActionName("GetImagesWithOneOfMultipleTags")]
        [ResponseType(typeof(List<Model.Image>))]
        public async Task<HttpResponseMessage> GetImagesWithOneOfMultipleTags([FromBody]List<string> tagNames)
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method invoked given tagnames: {tagNames}"));
            try
            {
                //check to make sure tagNames isn't empty
                if(tagNames.Count<=0||tagNames==null)
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method was provided an invalid tag list"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty or invalid Tags provided");
                }

                // grab full list of tags at once to avoid multiple calls to database.  
                var TagList = await db.ImageTags.ToListAsync();
                if (TagList==null ||TagList.Count<=0) //No tags found
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method did not recieve any tags from the database"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Tag List Not Found");
                }
                List<DAL.Image> ImageList = new List<DAL.Image>();
                //go through each tag in the given list and add the images associated with those tags to the imagelist
                foreach (var tag in tagNames)
                {
                    var TempTag = TagList.Where(x => x.ImageTagName == tag).ToList();
                    if (TempTag.Count<=0 ||TempTag==null)
                    {
                        NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method could not find tag: {tag}"));
                        return Request.CreateResponse(HttpStatusCode.BadRequest, $"Tag: {tag} not a valid tag ");
                    }

                    ImageList.AddRange(TempTag.FirstOrDefault().Images);
                }
                //return serialized imagelist of only unique values.  List will contain image information of any image that contains at least one of the specified tags.
                var SerializedImageList = helper.SerializeImageList(ImageList.Distinct().ToList());

                if(SerializedImageList==null||SerializedImageList.Count<=0) //No Images Found for the given tags
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method did not find any images for the tags provided"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Images found that contain any of the tags provided");
                }

                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method successfully returned image information given tagnames: {tagNames}"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageList);
            }
            catch (Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithOneOfMultipleTags Method encountered thethe following exception: {ex.Message}/n{ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        ///   Will Return a list of image information that falls within the specified tags (Requires ALL of the specified tags
        /// </summary>
        /// <param name="tagNames">(from request body)String List of ImageTagNames</param>
        /// <endpoint>HTTPPost: api/Images/GetImagesWithAllOfMultipleTags (POST TO ALLOW FROM BODY PARAMETER)</endpoint>
        /// <returns>List of Image Models</returns>
        [HttpPost]
        [ActionName("GetImagesWithAllOfMultipleTags")]
        [ResponseType(typeof(List<Model.Image>))]
        public async Task<HttpResponseMessage> GetImagesWithAllOfMultipleTags([FromBody]List<string> tagNames)
        {
            NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithAllOfMultipleTags Method invoked given tagnames: {tagNames}"));
            try
            {
                // check to make sure tagNames isn't empty
                if (tagNames.Count<=0||tagNames==null)
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithAllOfMultipleTags Method was provided an invalid tag list"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty or invalid Tags provided");
                }
                //grab full list of Images, grab at once to reduce calls to database
                var Images = await db.Images.ToListAsync();

                if (Images.Count<=0||Images==null)//DB did not provide any images
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithAllOfMultipleTags Method did not recieve any tags from the database"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Database did not contain any images");
                }
                List<Models.Image> SerializedImageList = new List<Models.Image>();

                //Loop through each image and check the images tags.
                foreach (var image in Images)
                {
                    int tagcount = 0;
                    //will loop through each tag in the current image and see if that tag is in the image list.  if it is it will add to the image count.
                    foreach (var imagetag in image.ImageTags)
                    {
                        foreach (var tag in tagNames)
                        {
                            if (tag == imagetag.ImageTagName)
                            {
                                tagcount++;
                                break;
                            }
                        }
                    }
                    if (tagcount == tagNames.Count)// contains all tags in provided taglist
                    {

                        SerializedImageList.Add(Mapper.Map<Models.Image>(image));
                    }
                }
                if (SerializedImageList.Count<=0||SerializedImageList==null)
                {
                    NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithAllOfMultipleTags Method did not find any images that contain all of the tags provided"));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Images found that contain all tags provided");
                }
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithAllOfMultipleTags Method successfully returned image information given tagnames: {tagNames}"));
                return Request.CreateResponse(HttpStatusCode.OK, SerializedImageList);
            }catch(Exception ex)
            {
                NLogConfig.logger.Log(new LogEventInfo(LogLevel.Info, "TLWebsite Logger", $"GetImagesWithAllOfMultipleTags Method encountered thethe following exception: {ex.Message}/n{ex.StackTrace}"));
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }

            
        }
      
    }
}
