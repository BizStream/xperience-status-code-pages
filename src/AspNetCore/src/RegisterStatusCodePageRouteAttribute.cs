using System;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Controllers;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Models;
using CMS;
using Kentico.Content.Web.Mvc.Routing;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages
{

    /// <summary> Registers the default page route definition for Xperience Status Code Pages. </summary>
    [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false )]
    public sealed class RegisterStatusCodePageRouteAttribute : Attribute, IPreInitAttribute
    {
        #region Fields
        private readonly RegisterPageRouteAttribute pageRouteAttribute;
        #endregion

        #region Properties

        /// <inheritdoc/>
        public Type MarkedType => pageRouteAttribute.MarkedType;
        #endregion

        public RegisterStatusCodePageRouteAttribute( )
            => pageRouteAttribute = new RegisterPageRouteAttribute( StatusCodeNode.CLASS_NAME, typeof( DefaultStatusCodeController ) );

        /// <summary> Registers the page route during application pre-initialization. </summary>
        public void PreInit( )
            => pageRouteAttribute.PreInit();
    }

}
