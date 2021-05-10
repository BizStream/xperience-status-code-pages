using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Controllers
{

    /// <summary> Default controller for Xperience StatusCodePages. </summary>
    public sealed class DefaultStatusCodeController : Controller
    {

        public IActionResult Index( [FromServices] IPageDataContextRetriever pageContextRetriever )
        {
            if( !pageContextRetriever.TryRetrieve( out IPageDataContext<TreeNode> data ) )
            {
                return NotFound();
            }

            return View( data );
        }

    }

}
