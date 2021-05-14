using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions;
using Kentico.Content.Web.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages
{

    /// <summary> Default implementation of a service that can retrieve the <see cref="PageUrl"/> of a StatusCodePage from the Xperience Content Tree. </summary>
    public class StatusCodePageUrlRetriever : IStatusCodePageUrlRetriever
    {
        #region Fields
        private readonly IStatusCodePageRetriever statusCodePageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        #endregion

        public StatusCodePageUrlRetriever(
            IPageUrlRetriever pageUrlRetriever,
            IStatusCodePageRetriever statusCodePageRetriever
        )
        {
            this.pageUrlRetriever = pageUrlRetriever;
            this.statusCodePageRetriever = statusCodePageRetriever;
        }

        /// <inheritdoc />
        public virtual async Task<PageUrl> RetrieveAsync( int statusCode )
        {
            var node = await statusCodePageRetriever.RetrieveAsync( statusCode );
            if( node == null )
            {
                return null;
            }

            return pageUrlRetriever.Retrieve( node );
        }

    }

}
