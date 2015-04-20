using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;

namespace TFV
{
    public class SavedConnection
    {
        public Uri ProjectURL { get; set; }
        public SimpleWebTokenCredential Credentials { get; set; }
        public SavedConnection()
        {
            ProjectURL = null;
            Credentials = null;
        }
        public override string ToString()
        {
            if (ProjectURL != null)
                return ProjectURL.ToString();
            return "<null>";
        }
    }

    public class SavedConnectionList
    {
        public List<SavedConnection> Connections { get; set; }
        public SavedConnectionList()
        {
            Connections = new List<SavedConnection>();
        }
    }
}