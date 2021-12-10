using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Models;
using Kentico.Content.Web.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Infrastructure;

/// <summary> Default implementation of a service that can retrieve a StatusCodePage from the Xperience Content Tree. </summary>
public class StatusCodePageRetriever : IStatusCodePageRetriever
{
    #region Fields
    private readonly IPageRetriever pageRetriever;
    #endregion

    public StatusCodePageRetriever( IPageRetriever pageRetriever )
    {
        this.pageRetriever = pageRetriever;
    }

    /// <inheritdoc />
    public virtual async Task<StatusCodeNode?> RetrieveAsync( int statusCode, CancellationToken cancellation = default )
    {
        IEnumerable<StatusCodeNode>? nodes = await pageRetriever.RetrieveAsync<StatusCodeNode>(
            nodes => nodes.TopN( 1 ).WhereEquals( nameof( StatusCodeNode.HttpStatusCode ), statusCode ),
            cache => cache.Key( StatusCodeCacheKeys.StatusCodePage( statusCode ) )
                .Dependencies( ( _, __ ) => { }, true ),
            cancellation
        );

        return nodes?.FirstOrDefault();
    }
}
