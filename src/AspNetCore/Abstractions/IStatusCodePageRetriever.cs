using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.Models;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions
{

    /// <summary> Describes a service that can retrieve a StatusCodePage from the Xperience Content Tree. </summary>
    public interface IStatusCodePageRetriever
    {

        /// <summary> Retrieve a StatusCodePage from the Xperience Content Tree. </summary>
        /// <param name="statusCode"> The StatusCode of the StatusCodePage to retrieve. </param>
        Task<StatusCodeNode> RetrieveAsync( int statusCode );

    }

}
