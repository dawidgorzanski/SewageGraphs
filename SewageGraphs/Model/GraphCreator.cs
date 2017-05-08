using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewageGraphs.Model
{
    public static class GraphCreator
    {
        public static Graph CreateFullGraph(int Nodes = 0)
        {
            Graph fullGraph = new Graph();
            Random random = new Random();
            //Dodanie wierzchołków
            for (int i = 0; i < Nodes; i++)
                fullGraph.Nodes.Add(new Node() { ID = i });

            //Dodanie połączeń między wierzchołkami
            for (int i = 0; i < Nodes; i++)
            {
                for (int j = 0; j < Nodes; j++)
                {
                    if (i != j)
                    {
                        Connection connection = new Connection();
                        //Nie tworzymy nowych obiektów typu Node, tylko wyszukujemy w Nodes już istniejące - znacznie ułatwia
                        //to rysowanie grafu - każdy obiekt Node dostaje później swoje współrzędne, ktore w obu listach są takie same.
                        //Jeżeli utworzylibyśmy nowy obiekt to po dodaniu współrzędnych obiektom w Nodes, w Connection nie
                        //zostałyby one zmienione.
                        //(x => x.ID == i) jest to tzw. wyrażenie Lambda - w tym wypadku szukamy pierwszego elementu o ID równym i.
                        connection.Node1 = fullGraph.Nodes.FirstOrDefault(x => x.ID == i);
                        connection.Node2 = fullGraph.Nodes.FirstOrDefault(x => x.ID == j);
                        connection.Weight = random.Next(-5, 11);
                        fullGraph.Connections.Add(connection);
                    }
                }
            }

            return fullGraph;
        }

        public static Graph CreateRandomDigraph(int levels)
        {
            Graph graph = new Graph() { Levels = levels + 2 };
            //dodanie startu
            graph.AddNode(new Node() { ID = 0, IsStart = true, Level = 0 });          

            Random random = new Random();
            int currentID = 1;

            //Tworzenie na kazdej warstwie wierzcholkow z przedziału [2, N]
            for (int i = 1; i < graph.Levels - 1; ++i)
            {
                int nodes = random.Next(2, levels);
                for(int j = 0; j < nodes; j++)
                {
                    graph.AddNode(new Node() { ID = currentID++, Level = i });
                }
            }

            //dodanie końca
            graph.AddNode(new Node() { ID = currentID, IsEnd = true, Level = graph.Levels - 1 });

            //Dodanie połączeń od niższych warstw do wyższych
            for(int i = 1; i < graph.Levels; ++i)
            {
                var lowerItems = graph.Nodes.Where(x => x.Level == i - 1).ToList();
                var higherItems = graph.Nodes.Where(x => x.Level == i).ToList();

                //dopóki każdy wierzchołek na danym poziomie nie ma odpowiednio min 1 wychodzacego/1 przychodzacego
                while(lowerItems.Count(x => x.HasNext == false) > 0 || higherItems.Count(x => x.HasPrevious == false) > 0)
                {
                    Node firstNode = lowerItems[random.Next(0, lowerItems.Count)];
                    Node secondNode = higherItems[random.Next(0, higherItems.Count)];
                    //sprawdzam czy jest takie polaczenie
                    if (graph.Connections.Count(x => x.Node1 == firstNode && x.Node2 == secondNode) == 0)
                    {
                        firstNode.HasNext = true;
                        secondNode.HasPrevious = true;
                        graph.AddConnection(firstNode, secondNode, random.Next(1, 11));
                    }
                }
            }

            //Dodanie losowych połączeń
            int added = 0;
            int toAdd = levels * 2;
            while (added < toAdd)
            {
                int level = random.Next(0, graph.Levels);
                bool higher = random.Next(0, 2) == 1 ? true : false;

                //jezeli wykracza poza poziomy
                if ((level == 0 && higher == false) || (level == graph.Levels - 1 && higher == true))
                    continue;

                //losuje pierwszego kandydata
                var firsts = graph.Nodes.Where(x => x.Level == level).ToList();
                Node firstNode = firsts[random.Next(0, firsts.Count)];
                if (higher)
                {
                    //losuje drugiego kandydata
                    var seconds = graph.Nodes.Where(x => x.Level == level + 1).ToList();
                    Node secondNode = seconds[random.Next(0, seconds.Count)];
                    //sprawdzam czy jest takie polaczenie
                    if (firstNode.IsEnd == false && secondNode.IsStart == false && graph.Connections.Count(x => x.Node1 == firstNode && x.Node2 == secondNode) == 0)
                    {
                        graph.AddConnection(firstNode, secondNode, random.Next(1, 11));
                        ++added;
                    }
                }
                else
                {
                    //losuje drugiego kandydata
                    var seconds = graph.Nodes.Where(x => x.Level == level - 1).ToList();
                    Node secondNode = seconds[random.Next(0, seconds.Count)];
                    //sprawdzam czy jest takie polaczenie
                    if (firstNode.IsEnd == false && secondNode.IsStart == false && graph.Connections.Count(x => x.Node1 == firstNode && x.Node2 == secondNode) == 0)
                    {
                        graph.AddConnection(firstNode, secondNode, random.Next(1, 11));
                        ++added;
                    }
                }
            }

            return graph;
        }
    }
}
