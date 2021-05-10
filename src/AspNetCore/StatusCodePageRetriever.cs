using System.Linq;
using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.Models;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions;
using Kentico.Content.Web.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages
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
        public virtual Task<StatusCodeNode> RetrieveAsync( int statusCode )
            => pageRetriever.RetrieveAsync<StatusCodeNode>( nodes => nodes.TopN( 1 ).WhereEquals( nameof( StatusCodeNode.HttpStatusCode ), statusCode ) )
                .ContinueWith( task => task.Result.FirstOrDefault() );

    }

}
