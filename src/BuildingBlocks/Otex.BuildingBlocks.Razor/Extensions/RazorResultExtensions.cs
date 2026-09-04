using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Otex.BuildingBlocks.Domain.Errors;
using Otex.BuildingBlocks.Domain.Results;

namespace Otex.BuildingBlocks.Razor.Extensions;

public static class RazorResultExtensions
{
    // ۱. استخراج هوشمند خطاها (چه تکی، چه آرایه‌ای از Validation)
    extension(Result result)
    {
        public List<Error> GetErrors()
        {
            if (result.IsSuccess) return [];
        
            // هندل کردن مشکلی که در دیباگ داشتی: استخراج آرایه از درون ValidationError
            return result.Error is ValidationError validationError 
                ? validationError.Errors.ToList() 
                : [result.Error];
        }

        public void AddToModelState(ModelStateDictionary modelState)
        {
            if (result.IsSuccess) return;

            var errors = result.GetErrors();
            foreach (var error in errors)
            {
                // در اینجا Code می‌تواند نام فیلد باشد تا در زیر همان اینپوت در HTML نمایش داده شود
                // اگر Code نام فیلد نیست، می‌توانید کلید را string.Empty بگذارید تا خطای کلی فرم محسوب شود
                modelState.AddModelError(error.Code, error.Description);
            }
        }

        public IActionResult Match(Func<IActionResult> onSuccess,
            Func<List<Error>, IActionResult> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.GetErrors());
        }
        public async Task<IActionResult> MatchAsync(Func<Task<IActionResult>> onSuccessAsync,
            Func<List<Error>, IActionResult> onFailure)
        {
            return result.IsSuccess ? await onSuccessAsync() : onFailure(result.GetErrors());
        }
        public async Task<IActionResult> MatchAsync(Func<Task<IActionResult>> onSuccessAsync,
            Func<List<Error>, Task<IActionResult>> onFailureAsync)
        {
            return result.IsSuccess ? await onSuccessAsync() : await onFailureAsync(result.GetErrors());
        }
    }
    
    extension<TIn>(Result<TIn> result)
    {
        public IActionResult Match(Func<TIn, IActionResult> onSuccess,
            Func<List<Error>, IActionResult> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.GetErrors());
        }
        
        public async Task<IActionResult> MatchAsync(
            Func<TIn, Task<IActionResult>> onSuccessAsync,
            Func<List<Error>, IActionResult> onFailure)
        {
            return result.IsSuccess 
                ? await onSuccessAsync(result.Value) 
                : onFailure(result.GetErrors());
        }

        public async Task<IActionResult> MatchAsync(
            Func<TIn, Task<IActionResult>> onSuccessAsync,
            Func<List<Error>, Task<IActionResult>> onFailureAsync)
        {
            return result.IsSuccess 
                ? await onSuccessAsync(result.Value) 
                : await onFailureAsync(result.GetErrors());
        }
    }
}