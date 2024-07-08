namespace MusteriMobilUygulamaAPI
{
    public class cDayElement
    {
        public int yemekid { get;set;}
        public string? adi { get;set;}
        public string? aciklama { get;set;}
        public string? servistarihi { get;set;}
        public int yemektipid { get;set;}
        public string? tipadi { get;set;}
    }


    //public class cDay
    //{
    //    public cDayElement[] oDayElement = new cDayElement[4];
    //}

    public class cWeek
    {
        public cDayElement[] pazartesi { get;set;} = new cDayElement[4];
        public cDayElement[] sali { get;set;} = new cDayElement[4];
        public cDayElement[] carsamba { get;set;} = new cDayElement[4];
        public cDayElement[] persembe { get;set;} = new cDayElement[4];
        public cDayElement[] cuma { get;set;} = new cDayElement[4];
        public cDayElement[] cumartesi { get;set;} = new cDayElement[4];
        public int hafta { get;set;}
        public string? tarih_bilgisi { get;set;}
    }
}
