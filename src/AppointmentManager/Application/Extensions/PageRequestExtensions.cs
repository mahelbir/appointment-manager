using NArchitecture.Core.Application.Requests;

namespace Application.Extensions;

public static class PageRequestExtensions
{
    public static int PageIndexNormalized(this PageRequest pageRequest)
    {
        return Math.Max(0, pageRequest.PageIndex);
    }

    public static int PageSizeNormalized(this PageRequest pageRequest)
    {
        return pageRequest.PageSize > 0 ? Math.Min(100, pageRequest.PageSize) : 20;
    }
}