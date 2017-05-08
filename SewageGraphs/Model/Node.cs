using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SewageGraphs.Model
{
    public class Node
    {
        public int ID { get; set; }
        //Początkowo null
        public Point PointOnScreen { get; set; }
        public int Level { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public Node()
        {
            this.IsStart = false;
            this.IsEnd = false;
            this.HasPrevious = false;
            this.HasNext = false;
        }
    }
}
