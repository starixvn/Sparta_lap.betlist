// --------------------------------------------------------------------------
// <copyright file="Test.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena
{
    using System;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Test class.
    /// </summary>
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowSessionSize();
        }

        /// <summary>
        /// Show session size.
        /// </summary>
        private void ShowSessionSize()
        {
            Page.Trace.Write("Session Trace Info");

            long totalSessionBytes = 0;
            var b = new BinaryFormatter();
            foreach (string key in Session)
            {
                var obj = Session[key];
                var m = new System.IO.MemoryStream();
                b.Serialize(m, obj);
                totalSessionBytes += m.Length;

                Page.Trace.Write(string.Format("{0}: {1:n} kb", key, m.Length / 1024));
            }

            var message = string.Format("Total Size of Session Data: {0:n} kb", totalSessionBytes / 1024);
            Page.Trace.Write(message);
        }
    }
}