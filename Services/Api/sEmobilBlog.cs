using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public static class sEmobilBlog
    {

        public static cResponseModel<cEmobilBlog> getEmobilBlog(int page_number, int count)
        {
            //int y=0, x = 1/y;
            List<cEmobilBlog> items = new List<cEmobilBlog>();
            cResponseModel<cEmobilBlog> oResponseModel = new cResponseModel<cEmobilBlog>();
            oResponseModel.Data = items;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" select top {count} * from (select ROW_NUMBER() OVER ( order by created_date desc,updated_date desc) AS rankNumber
                                    , OwnId
                                    , e.Id
                                    , Parent
	                                , title
	                                , content
	                                , created_date
	                                , updated_date
	                                , link
	                                , externalId
	                                , internalId
	                                , [external]
	                                , internal
									, case when u.Users_ID is null and a.ID is null then 'UGM - Bizden Haberler'
									       when u.Users_ID is not null then Adi_Soyadi
										   when a.ID is not null then a.AdSoyad
									  end AdSoyad
									, case when u.Users_ID is null and a.ID is null then 'https://ubp.ugm.com.tr:9250/media/images/logo.jpg'
									       when u.Users_ID is not null then ISNULL(TCKimlik,'')
										   when a.ID is not null then ISNULL(TCKimlik,'')
									  end tckimlik
									, case when u.Users_ID is null and a.ID is null then 'Ünsped Gümrük Müşavirliği ve Lojistik Hizmetler A.Ş.'
									       when u.Users_ID is not null then GorevTanimi
										   when a.ID is not null then b.Gonderen_TamUnvan
									  end FirmaUnvan
                                from Evrim_DB.dbo.emobilBlog e with(nolock)
								left join Evrim_DB.dbo.users u with(nolock) on u.Users_ID = e.internalId 
								left join Evrim_DB.dbo.MobilKullanici a with(nolock) on a.ID = e.externalId
								left join Evrim_DB.dbo.MobilKullaniciFirma mbf with(nolock) on a.ID=mbf.MobilKullaniciID
                                left join Evrim_DB.dbo.Gonderen b with(nolock) on mbf.FirmaNo = b.Gonderen_No 
                                where 1=1
                                and isnull(deleted,0)=0) a
								where 1=1
								and rankNumber >= ({page_number}-1) * {count} + 1 ";
                
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        /*DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                                              
                        items = dataTable.AsEnumerable().Select(m => new cEmobilBlog()
                        {
                            //, Id = m.Field<int?>("Id")
                            //, Parent = m.Field<int?>("Parent")
                              title = m.Field<string?>("title")
                            , content = m.Field<string?>("content")
                            , created_date = m.Field<DateTime>("created_date")
                            , updated_date = m.Field<DateTime?>("updated_date")
                            , link = m.Field<string?>("link")
                            //, externalId = m.Field<int?>("externalId")
                            //, internalId = m.Field<int?>("internalId")
                            , external = m.Field<Boolean>("external")
                            , _internal = m.Field<Boolean>("internal")
                            , user_info = cCommon.getUserInfoForPost(m.Field<int?>("internalId"), m.Field<int?>("externalId"), true)
                            , blog_id = m.Field<int>("OwnId")
                            , images = cCommon.getBlogImages(m.Field<int>("OwnId"))
                            //, comments = new List<int>()
                            //, likes = new List<cEmobilLike>() //   cCommon.getEmobilLike(m.Field<int>("OwnId"))
                            , likes = sEmobilBlogLikes.getEmobilBlogLikes(m.Field<int>("OwnId"), false)?.Data //, ref dataTableForLikes)

                        }).ToList();
                        */
                        
                        while(reader.Read())
                        {
                            cEmobilBlog item = new cEmobilBlog();
                            //, Id = m.Field<int?>("Id")
                            //, Parent = m.Field<int?>("Parent")
                            item.title = reader["title"] == DBNull.Value ? null : reader.GetString("title");
                            item.content = reader["content"] == DBNull.Value ? null : reader.GetString("content");
                            item.created_date = reader.GetDateTime("created_date");
                            item.updated_date = reader["updated_date"] == DBNull.Value ? null : reader.GetDateTime("updated_date");
                            item.link = reader["link"] == DBNull.Value ? null : reader.GetString("link");
                            //, externalId = m.Field<int?>("externalId")
                            //, internalId = m.Field<int?>("internalId")
                            item.external = reader["external"] == DBNull.Value ? false : reader.GetBoolean("external");
                            item._internal = reader["internal"] == DBNull.Value ? false : reader.GetBoolean("internal");
                            item.user_info = cCommon.getUserInfoForPost(reader["internalId"] == DBNull.Value ? null : reader.GetInt32("internalId")
                                                                        , reader["externalId"] == DBNull.Value ? null : reader.GetInt32("externalId"), true);
                            item.blog_id = reader.GetInt32("OwnId");
                            item.images = cCommon.getBlogImages(reader.GetInt32("OwnId"));
                            //, comments = new List<int>()
                            //, likes = new List<cEmobilLike>() //   cCommon.getEmobilLike(m.Field<int>("OwnId"))
                            item.likes = sEmobilBlogLikes.getEmobilBlogLikes(reader.GetInt32("OwnId"), false)?.Data; //, ref dataTableForLikes)
                            items.Add(item);
                        }
                        //items = items.Skip((page_number - 1) * count).Take(count).ToList();
                    }
                }
                connection.Close();
            }

            oResponseModel.Data = items;

            return oResponseModel;
        }
    }
}
