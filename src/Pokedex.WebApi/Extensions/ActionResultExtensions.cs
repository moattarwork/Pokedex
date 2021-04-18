using System;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Core.Domain;

namespace Pokedex.WebApi.Extensions
{
    public static class ActionResultExtensions
    {
        public static ActionResult<T> ToActionResult<T>(this OperationResult<T> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            
            if (result.Succeed)
                return new OkObjectResult(result.Result);

            if (result.Failed && result.ErrorReason == OperationErrorReason.ResourceNotFound)
                return new NotFoundResult();

            return new StatusCodeResult(500);
        }
    }
}