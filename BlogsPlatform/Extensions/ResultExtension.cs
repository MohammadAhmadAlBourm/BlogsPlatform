using BlogsPlatform.Abstractions;

namespace BlogsPlatform.Extensions;

public static class ResultExtension
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't convert success result to problem");
        }

        return CustomResults.Problem(result);
    }
}