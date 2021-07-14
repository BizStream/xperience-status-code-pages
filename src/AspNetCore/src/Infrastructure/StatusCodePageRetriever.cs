using System.Linq;
using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Models;
using Kentico.Content.Web.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Infrastructure
{

    /// <summary> Default implementation of a service that can retrieve a StatusCodePage from the Xperience Content Tree. </summary>
    public class StatusCodePageRetriever : IStatusCodePageRetriever
    {
        #region Fields
        private readonly IPageRetriever pageRetriever;
        #endregion

        public StatusCodePageRetriever( IPageRetriever pageRetriever )
            => this.pageRetriever = pageRetriever;

        /// <inheritdoc />
        public virtual async Task<StatusCodeNode> RetrieveAsync( int statusCode )
        {
            var nodes = await pageRetriever.RetrieveAsync<StatusCodeNode>(
                nodes => nodes.TopN( 1 ).WhereEquals( nameof( StatusCodeNode.HttpStatusCode ), statusCode ),
                cache => cache.Key( CacheKeys.StatusCodeNode( statusCode ) )
                    .Dependencies( ( _, __ ) => { } )
            );

            return nodes?.FirstOrDefault();
        }

    }

}
