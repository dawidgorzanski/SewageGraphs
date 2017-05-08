﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewageGraphs.Model
{
    public class Connection
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public int Weight { get; set; }
        public bool IsBestPath { get; set; }

        public Connection()
        {
            this.IsBestPath = false;
        }
    }
}
