using System;

namespace MusteriMobilUygulamaAPI.Models.IsTakip
{
    public class cIsTakipNew2Data
{
    public string? path { get; set; }
    public string? arsivtip { get; set; }
    public List<Istakip>? istakip { get; set; }
}

public class Istakip
{
    public string? istakipkod { get; set; }
    public string? tarih { get; set; }
    public int adet { get; set; }
}
}