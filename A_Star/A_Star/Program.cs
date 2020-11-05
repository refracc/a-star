using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace A_Star
{
    internal class Program
    {
        /// <summary>
        /// The entry point for the program.
        /// </summary>
        /// <param name="args">The array of arguments to be parsed in from the command line.</param>
        private static void Main(string[] args)
        {
            List<Node> nodes = new List<Node>();
            CreateNodeList($"{args[0]}.cav", nodes);
            List<Node> unvisitedNodes = new List<Node> { nodes[0] };

            unvisitedNodes[0].DistanceFirstNode = 0;
            nodes.First().CalculateDistanceToLocation(nodes.Last().X, nodes.Last().Y);
            int visiting = 0;

            while (unvisitedNodes.Any() && visiting != nodes.Last().ID)
            {
                visiting = FindNextNode(unvisitedNodes);

                foreach (int connected in nodes[unvisitedNodes[visiting].ID].Connections.Where(connected => !nodes[connected].Visited))
                {
                    nodes[connected].CalculateDistanceToNode(nodes[unvisitedNodes[visiting].ID]);
                    nodes[connected].CalculateDistanceToLocation(nodes.Last().X, nodes.Last().Y);

                    if (nodes[connected].InUnvisited) continue;
                    unvisitedNodes.Add(nodes[connected]);
                    unvisitedNodes.Last().InUnvisited = true;
                }

                unvisitedNodes[visiting].Visited = true;
                unvisitedNodes[visiting].InUnvisited = false;
                unvisitedNodes.RemoveAt(visiting);
            }

            string route = "";

            if (nodes.Last().PreviousNode != -1)
            {
                int index = nodes.Last().ID;
                while (index != -1)
                {
                    route = route.Insert(0, $"{index + 1} ");
                    index = nodes[index].PreviousNode;
                }
                route = route[..^1];
            }
            else route = "0";

            File.WriteAllText($"{args[0]}.csn", route);
        }

        /// <summary>
        /// The method will create a node list from a file name and a list of nodes.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="nodes">The list of nodes </param>
        private static void CreateNodeList(string fileName, IList<Node> nodes)
        {
            string file = File.ReadAllText(fileName);
            string[] data = file.Split(',');
            int node = int.Parse(data[0]);
            int id = 0;

            for (int i = 1; i <= node * 2; i += 2)
            {
                nodes.Add(new Node(id, int.Parse(data[i]), int.Parse(data[i + 1])));
                id++;
            }

            int matrix = node * 2 + 1;

            for (int i = 0; i < node; i++) 
            {
                for (int j = 0; j < node; j++)
                {
                    if (int.Parse(data[matrix]) == 1)
                        nodes[j].Connections.Add(Convert.ToInt32(i));
                    matrix++;
                }
            }
        }

        /// <summary>
        /// This method will find the next node from a given list of unvisited nodes.
        /// </summary>
        /// <param name="unvisited">The list of unvisited nodes</param>
        /// <returns>An index value of the unvisited nodes.</returns>
        private static int FindNextNode(IList<Node> unvisited)
        {
            double minimumDistance = double.MaxValue;         
            int index = -1;

            for (int i = 0; i < unvisited.Count; i++)
            {
                Node n = unvisited[i];
                double distance = n.DistanceFirstNode + n.DistanceTargetNode;

                if (!(distance < minimumDistance)) continue;
                minimumDistance = distance;
                index = i;
            }

            return index;
        }
    }
}