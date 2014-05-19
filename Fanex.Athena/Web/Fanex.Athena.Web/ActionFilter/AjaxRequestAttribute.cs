// -----------------------------------------------------------------------
// <copyright file="AjaxRequestAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.ActionFilter
{
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    ///  Ajax request attribute class.
    /// </summary>
    public sealed class AjaxRequestAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}