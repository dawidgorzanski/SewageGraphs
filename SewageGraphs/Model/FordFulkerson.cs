using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewageGraphs.Model
{
    class FordFulkerson
    {
        static int CountOfNodes = 0;
        public static int MaxFlow(int[,] matrix, int count)
        {
            CountOfNodes = count;
            int u, v, max_flow = 0,s=0,t=count-1;
            int[] parent = new int[CountOfNodes];
            while (bfs(matrix, s, t, parent))
            {
                // Find minimum residual capacity of the edhes
                // along the path filled by BFS. Or we can say
                // find the maximum flow through the path found.
                int path_flow = Int32.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, matrix[u, v]);
                }

                // update residual capacities of the edges and
                // reverse edges along the path
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    matrix[u, v] -= path_flow;
                    matrix[v, u] += path_flow;
                }
                // Add path flow to overall flow
                max_flow += path_flow;
            }

            // Return the overall flow
            return max_flow;
        }

        static bool bfs(int [,] matrix, int s, int t, int [] parent)
        {
            // Create a visited array and mark all vertices as not
            // visited
            bool []visited = new bool[CountOfNodes];
            for (int i = 0; i < CountOfNodes; ++i)
                visited[i] = false;

            // Create a queue, enqueue source vertex and mark
            // source vertex as visited
            LinkedList<int> queue = new LinkedList<int>();
            queue.AddLast(s);
            visited[s] = true;
            parent[s] = -1;

            // Standard BFS Loop
            while (queue.Count != 0)
            {
                int u = queue.First.Value;
                queue.RemoveFirst();
                for (int v = 0; v < CountOfNodes; v++)
                {
                    if (visited[v] == false && matrix[u,v] > 0)
                    {
                        queue.AddLast(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            // If we reached sink in BFS starting from source, then
            // return true, else false
            return (visited[t] == true);
        }
    }
}
