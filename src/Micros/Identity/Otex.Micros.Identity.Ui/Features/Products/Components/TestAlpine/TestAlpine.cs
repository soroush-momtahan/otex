using Microsoft.AspNetCore.Mvc;
using Otex.Micros.Identity.Ui.Core.Tools;

namespace Otex.Micros.Identity.Ui.Features.Products.Components.TestAlpine;

public record TestAlpineViewModel();

[Route("htmx/[controller]")]
public class TestAlpineController : Controller
{
    // TODO: Add HTMX endpoints here
}

[ViewComponent(Name = "TestAlpine")]
public class TestAlpineInitializer : FeatureViewComponent
{
    public IViewComponentResult Invoke()
    {
        var viewModel = new TestAlpineViewModel();
        return FeatureView(viewModel);
    }
}