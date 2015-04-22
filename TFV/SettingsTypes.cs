using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using System.Runtime.Serialization;
using System.Configuration;
using System.Xml.Serialization;

namespace TFV
{
	[Serializable]
    public class SavedConnection
    {
        public string ProjectURL { get; set; }
        public string UserName { get; set; }
        public string Workspace { get; set; }
        public SavedConnection()
        {
            ProjectURL = null;
            UserName = null;
            Workspace = null;
        }
        public override string ToString()
        {
            if (ProjectURL != null)
                return ProjectURL + " (" + Workspace + ")";
            return "<null>";
        }
    }

	[Serializable]
    public class SavedConnectionList
    {
        public List<SavedConnection> Connections { get; set; }
        public SavedConnectionList()
        {
            Connections = new List<SavedConnection>();
        }
    }
}