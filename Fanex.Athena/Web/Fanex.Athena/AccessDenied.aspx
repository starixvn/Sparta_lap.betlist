<%@ Page Language="C#" AutoEventWireup="false" %>

<%
    if (!Request.FilePath.Contains("/Report/Balance/"))
    {
        var isAjaxRequest = (Request["X-Requested-With"] == "XMLHttpRequest")
            || ((Request.Headers != null) && (Request.Headers["X-Requested-With"] == "XMLHttpRequest"));
%>
<%if (isAjaxRequest)
  {%>
Sorry, You don't have permission to access this page.
<%}
  else
  {%>
<div style="color: red">
    Sorry, You don't have permission to access this page.</div>
<%}%>
<%}%>