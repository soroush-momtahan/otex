using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace Otex.Micros.Identity.Razor.Pages.SignOn.Components.StorySlider;

public record StorySliderViewModel(List<StorySliderViewModel.Slide> Slides)
{
    public record Slide(string BgColor, string Title, string Desc, string Image);
}

public class StorySlider : HydroComponent
{
    public StorySliderViewModel ViewModel { get; set; } = new([
        new StorySliderViewModel.Slide(
            "bg-violet-600", 
            "خرید لذت بخش لنت", 
            "با چند لمس لنت خود را سفارش دهید و درب منزل تحویل بگیرید",
            "/images/slide-00.png"),
        new StorySliderViewModel.Slide(
            "bg-indigo-600", 
            "خرید عمده، سود بیشتر", 
            "به خانواده اُتکس بپیوندید. لیست قیمت فروش عمده را دریافت کرده و از شرایط ویژه و تخفیف‌های اختصاصی بهره‌مند شوید.",
            "/images/slide-04.png"),
        new StorySliderViewModel.Slide(
            "bg-emerald-500", 
            "100 درصد بدون آزبست", 
            "محافظت از سلامتی شما و محیط زیست بخاطر فرمولاسیون ایمن و نداشتن مواد سمی",
            "/images/slide-01.png"),
        new StorySliderViewModel.Slide(
            "bg-blue-600", 
            "سکوت مطلق در هر ترمز", 
            "100 درصد بی صدا ، با شیم ضد صدا",
            "/images/slide-02.png"),
        new StorySliderViewModel.Slide(
            "bg-yellow-600", 
            "دارای گرید A+", 
            "بالاترین کیفیت ترمز گیری در کنار بالاترین درجه کیفیت",
            "/images/slide-03.png"),
    ]);
    
}