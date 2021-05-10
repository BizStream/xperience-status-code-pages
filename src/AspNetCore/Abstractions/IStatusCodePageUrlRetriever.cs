using System.Threading.Tasks;
using Kentico.Content.Web.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions
{

    /// <summary> Describes a service that can retrieve a <see cref="PageUrl"/> of a StatusCodePage from the Xperience Content Tree. </summary>
    public interface IStatusCodePageUrlRetriever
    {

        /// <summary> Retrieve a <see cref="PageUrl"/> of a StatusCodePage from the Xperience Content Tree. </summary>
        /// <param name="statusCode"> The StatusCode of the StatusCodePage to retrieve a <see cref="PageUrl"/> of. </param>
        Task<PageUrl> RetrieveAsync( int statusCode );

    }

}
