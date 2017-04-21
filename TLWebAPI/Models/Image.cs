using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLWebAPI.Models
{
    public class Image
    {
        public string ImageName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ImageID { get; set; }
        public Nullable<int> ImageTypeID { get; set; }
        public string ImageURL { get; set; }
        public string ImageDescription { get; set; }

        public virtual ImageType ImageType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<ImageTag> ImageTags { get; set; }
    }
}