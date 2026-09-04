using Microsoft.AspNetCore.Mvc;
using Otex.Micros.Identity.Ui.Core.Tools;

namespace Otex.Micros.Identity.Ui.Pages.Account.Login.Components.StorySlider;

public record StorySliderViewModel(List<StorySliderViewModel.Slide> Slides)
{
    public record Slide(string BgColor, string Title, string Desc, string Image);
}

[Route("htmx/[controller]")]
public class StorySliderController : Controller
{
    // TODO: Add HTMX endpoints here
}

[ViewComponent(Name = "StorySlider")]
public class StorySliderInitializer : FeatureViewComponent
{
    private List<StorySliderViewModel.Slide> Slides { get; }= [
        new(
            "bg-violet-600", 
            "خرید لذت بخش لنت", 
            "با چند لمس لنت خود را سفارش دهید و درب منزل تحویل بگیرید",
            "/images/slide-00.png"),
        new(
            "bg-indigo-600", 
            "خرید عمده، سود بیشتر", 
            "به خانواده اُتکس بپیوندید. لیست قیمت فروش عمده را دریافت کرده و از شرایط ویژه و تخفیف‌های اختصاصی بهره‌مند شوید.",
            "/images/slide-04.png"),
        new(
            "bg-emerald-500", 
            "100 درصد بدون آزبست", 
            "محافظت از سلامتی شما و محیط زیست بخاطر فرمولاسیون ایمن و نداشتن مواد سمی",
            "/images/slide-01.png"),
        new(
            "bg-blue-600", 
            "سکوت مطلق در هر ترمز", 
            "100 درصد بی صدا ، با شیم ضد صدا",
            "/images/slide-02.png"),
        new(
            "bg-yellow-600", 
            "دارای گرید A+", 
            "بالاترین کیفیت ترمز گیری در کنار بالاترین درجه کیفیت",
            "/images/slide-03.png"),
    ];
    public IViewComponentResult Invoke()
    {
        var viewModel = new StorySliderViewModel(Slides);
        return FeatureView(viewModel);
    }
}