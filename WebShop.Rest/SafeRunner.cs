using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Rest
{
    public interface ISafeRunner
    {
        Task<IActionResult> Run(Func<Task> run, Func<IActionResult> createActionResult);
        Task<IActionResult> Run<TResult>(Func<Task<TResult>> run, Func<TResult, IActionResult> mapToActionResult);
    }

    public class SafeRunner : ISafeRunner
    {
        public async Task<IActionResult> Run(Func<Task> run, Func<IActionResult> createActionResult)
        {
            try
            {
                await run();
            }
            catch (Exception e)
            {
                var actionResult = MapExceptionToActionResult(e);
                return actionResult;
            }

            return createActionResult();
        }

        public async Task<IActionResult> Run<TResult>(Func<Task<TResult>> run, Func<TResult, IActionResult> mapToActionResult)
        {
            TResult result;

            try
            {
                result = await run();
            }
            catch (Exception e)
            {
                var actionResult = MapExceptionToActionResult(e);
                return actionResult;
            }

            return mapToActionResult(result);
        }

        private IActionResult MapExceptionToActionResult(Exception e)
        {
            switch (e)
            {
                case ModelNotFoundException _:
                    return new NotFoundObjectResult("Model not found.");

                default:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}