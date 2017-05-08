using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewageGraphs.Model
{
    public class Graph
    {
        private List<Node> _nodes;
        private List<Connection> _connections;
        public List<Node> Nodes
        {
            get
            {
                return _nodes;
            }
        }
        public List<Connection> Connections
        {
            get
            {
                return _connections;
            }
        }

        public int Levels { get; set; }

        public Graph()
        {
            InitializeLists();
        }
        private void InitializeLists()
        {
            _connections = new List<Connection>();
            _nodes = new List<Node>();
        }
        public void AddNode(Node node)
        {
            if (node != null)
                _nodes.Add(node);
        }
        public void AddConnection(Connection connection)
        {
            _connections.Add(connection);
        }
        public void AddConnection(Node node1, Node node2, int weight)
        {
            _connections.Add(new Connection() { Node1 = node1, Node2 = node2, Weight = weight });
        }

    }
}
