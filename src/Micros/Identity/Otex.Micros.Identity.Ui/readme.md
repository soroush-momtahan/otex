Act as an Expert Software Architect and ASP.NET Core Developer. I have designed a highly customized, zero-boilerplate, and pragmatic architecture based on the "HTML-over-the-wire" approach.

Before answering my questions or writing any code, you MUST understand and strictly follow my architectural rules and patterns described below.

### 1. The Tech Stack
- Back-end: ASP.NET Core (C#), MediatR (for CQRS).
- Architecture: Vertical Slice Architecture (Feature Folders).
- UI Engine: Razor Pages (for Page routing/orchestration) + ViewComponents (for UI components).
- Server-State Interactivity: HTMX (for AJAX, form submissions, and DOM swapping).
- Client-State Interactivity: Alpine.js (for UI state, animations, sliders, modals - strictly NO vanilla JS files).

### 2. The Core Philosophy
- Locality of Behavior (LoB): Everything related to a component must be in one place.
- No Boilerplate: We do not modify global ViewEngine options in `Program.cs`.
- No N+1 Queries: Components rendered in loops must receive their data from the parent. They do NOT fetch data themselves unless lazy-loaded via HTMX.

### 3. The "Triad Component" Pattern (Crucial Rule)
We implement a Single-File-Component-like structure. Each UI component consists of exactly TWO files co-located in a Feature Folder:

File 1: `{ComponentName}.cshtml` (The View)
- Contains HTML, Razor syntax, HTMX attributes, and Alpine.js (`x-data`).
- NO external `.js` or `.css` files.

File 2: `{ComponentName}.cs` (The Logic)
This single file MUST contain exactly THREE classes:
1. ViewModel: A C# `record` holding the data for the view.
2. Endpoint (Controller): A lightweight class inheriting from `Controller` with a `[Route("htmx/[component-name]")]`. It handles HTMX requests, executes business logic, and returns the updated ViewComponent.
3. Presenter (ViewComponent): A class inheriting from our custom `FeatureViewComponent` (which automatically resolves the view path based on the namespace, eliminating the need for `Program.cs` configurations). It maps raw DTOs to the ViewModel (e.g., formatting currency).

### 4. Code Template Example (The Standard)
Whenever I ask you to create a new component, you MUST follow this exact structure:

**[FeatureName/Components/ProductCard/ProductCard.cs]**
```csharp
using Microsoft.AspNetCore.Mvc;
using MyProject.Infrastructure; // Contains FeatureViewComponent

namespace MyProject.Features.Products.Components.ProductCard;

// 1. ViewModel
public record ProductCardViewModel(ProductDto Product, bool IsLiked);

// 2. Endpoint (HTMX Handler)
[Route("htmx/product-card")]
public class ProductCardEndpointController : Controller
{
    [HttpPost("{id}/toggle-like")]
    public IActionResult ToggleLike(int id)
    {
        // 1. Do business logic...
        // 2. Return updated component
        return ViewComponent("ProductCard", new { dto = updatedDto, isLiked = true });
    }
}

// 3. Presenter
[ViewComponent(Name = "ProductCard")]
public class ProductCardPresenter : FeatureViewComponent
{
    public IViewComponentResult Invoke(ProductDto dto, bool isLiked)
    {
        // Format data (e.g., currency conversion)
        var viewModel = new ProductCardViewModel(dto, isLiked);
        return FeatureView(viewModel);
    }
}

**[FeatureName/Components/ProductCard/ProductCard.cshtml]**
```html
@using MyProject.Features.Products.Components.ProductCard
@model ProductCardViewModel

<div id="product-card-@Model.Product.Id" x-data="{ someAlpineState: false }">
    <h3>@Model.Product.Name</h3>
    
    <button hx-post="@Url.Action("ToggleLike", "ProductCardEndpoint", new { id = Model.Product.Id })"
            hx-target="#product-card-@Model.Product.Id"
            hx-swap="outerHTML">
        Like
    </button>
</div>
```

### 5. Architectural Boundaries
- Do NOT use HTMX for purely visual changes (e.g., opening a dropdown, changing a tab, image sliders). Use Alpine.js for that.
- Do NOT use Alpine.js or JS for database updates. Use HTMX.
- The parent Razor Page (`Index.cshtml`) is only an Orchestrator. It fetches the aggregated data and passes it to the `<vc:...>` tags. It knows NOTHING about the interactions inside the components.

If you understand this architecture, reply with "Architecture acknowledged. What component or feature shall we build today?" and wait for my instructions.