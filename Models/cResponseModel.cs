namespace MusteriMobilUygulamaAPI.Models;

public class cErrorModel
{
    public int ErrorCode { get; set; } = 0;
    public string ErrorMessage { get; set; } = string.Empty;
}

public class cResponseModel<T>
{
    public cErrorModel errorModel { get; set; } = new cErrorModel();
    public List<T>? Data { get; set; }
    public cResponseModel()
    {
        Data = new List<T>();
    }
}
