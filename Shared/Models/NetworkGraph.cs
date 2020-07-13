using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TciPM.Blazor.Shared.Models
{
    [DataContract]
    public class NetworkGraph
    {
        [DataMember]
        public List<NetworkNode> nodes { get; set; } = new List<NetworkNode>();

        [DataMember]
        public List<NodeLink> links { get; set; } = new List<NodeLink>();
    }

    [DataContract]
    public class NetworkNode
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public bool loaded { get; set; } = true;

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string popup { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string auras { get; set; }

        public NetworkNode()
        { }

        public NetworkNode(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public NetworkNode(string id, string name, string type)
        {
            this.id = id;
            this.name = name;
            this.type = type;
        }
    }

    [DataContract]
    public class NodeLink
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string from { get; set; }

        [DataMember]
        public string to { get; set; }

        [DataMember]
        public string label { get; set; }

        [DataMember]
        public string popup { get; set; }

        [DataMember]
        public string direction { get; set; }

        public NodeLink()
        { }
        public NodeLink(string id, string from, string to)
        {
            this.id = id;
            this.from = from;
            this.to = to;
        }

        public NodeLink(string id, string from, string to, string label)
        {
            this.id = id;
            this.from = from;
            this.to = to;
            this.label = label;
        }
    }
}