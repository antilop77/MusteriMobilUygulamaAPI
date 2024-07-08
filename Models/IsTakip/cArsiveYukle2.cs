using Newtonsoft.Json;

namespace MusteriMobilUygulamaAPI.Models.IsTakip
{
    public class cArsiveYukle2
    {
        public string? Tip { get; set; }
        public int Fototip { get; set; }
        public string? Dosyano { get; set; }

        public List<Image>? Images { get; set; }

    }

    public class Image
    {
        public string Filename { get; set; } = string.Empty;
        public string Base64 { get; set; } = string.Empty;
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class IsTakip2
    {
        public string IsTakipKod { get; set; } = string.Empty;
        public string IsTakipAciklama { get; set; } = string.Empty;
        public string Tarih { get; set; } = string.Empty;
        public string Adet { get; set; } = string.Empty;
    }

    public class cArsiveYukle2Data
    {
        public string Path { get; set; } = string.Empty;
        public string ArsivTip { get; set; } = string.Empty;
        public string ArsivDocName { get; set; } = string.Empty;
        public List<IsTakip2>? IsTakip { get; set; }
    }
}