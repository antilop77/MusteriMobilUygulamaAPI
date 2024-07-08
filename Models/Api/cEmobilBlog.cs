using MusteriMobilUygulamaAPI.Common;

namespace MusteriMobilUygulamaAPI.Models.Api
{
    public class cEmobilBlog
    {
        public string? title { get; set; }
        public string? content { get; set; }
        public DateTime created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public string? link { get; set; }
        //public int? externalId { get; set; }
        //public int? internalId { get; set; }
        public bool external { get; set; }
        public bool _internal { get; set; }
        public User_Info? user_info { get; set; }
        public int blog_id { get; set; }
        public List<cBlogImage>? images { get; set; }
        //public List<int>? comments { get; set; }
        public List<cEmobilBlogLike>? likes { get; set; }
    }

    public class User_Info
    {
        public int? internal_user_id { get; set; }
        public int? external_user_id { get; set; }
        public string? ad_soyad { get; set; }
        public string? user_image { get; set; }
        public string? unvan { get; set; }
    }

    public class Image
    {
        public string? link { get; set; }
        public string? thumbnail_link { get; set; }
    }
}