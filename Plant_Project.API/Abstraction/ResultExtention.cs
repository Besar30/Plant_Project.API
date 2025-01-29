using Microsoft.AspNetCore.Mvc;

namespace Plant_Project.API.Abstraction
{
    public static class ResultExtention
    {
        public static ObjectResult ToProblem(this Result result, int StatusCode)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("cannot convert success result to a problem");
            }
            var problem = Results.Problem(statusCode: StatusCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
            {
                        {
                            "errors",new[]{result.error}
                        }
            };
            return new ObjectResult(problemDetails);
        }
    }
}
