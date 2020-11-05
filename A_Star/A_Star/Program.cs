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
            List<Node> unvisited = new List<Node> { nodes[0] };

            // Set the first unvisited node's distance to 0
            unvisited[0].DistanceFirstNode = 0;
            nodes.First().CalculateDistanceToLocation(nodes.Last().X, nodes.Last().Y); // Calculate the first node's distance to the last node
            int visiting = 0;

            // Iterate over the unvisited nodes, ensure the visiting number isn't also the last node's ID
            while (unvisited.Any() && visiting != nodes.Last().ID)
            {
                visiting = FindNextNode(unvisited); // Visiting node ID is the next node in this list.

                // Iterate over all node connections where the node we are in hasn't been visited.
                foreach (int connected in nodes[unvisited[visiting].ID].Connections.Where(connected => !nodes[connected].Visited))
                {
                    // Calculate the distances to the currently visiting unvisited node and location of the end objective.
                    nodes[connected].CalculateDistanceToNode(nodes[unvisited[visiting].ID]);
                    nodes[connected].CalculateDistanceToLocation(nodes.Last().X, nodes.Last().Y);

                    // Add the node to the unvisited list & set the last node's InUnvisited state to true.
                    if (nodes[connected].InUnvisited) continue;
                    unvisited.Add(nodes[connected]);
                    unvisited.Last().InUnvisited = true;
                }

                // Set the visited flag to true and unset the unvisited flag.
                unvisited[visiting].Visited = true;
                unvisited[visiting].InUnvisited = false;
                unvisited.RemoveAt(visiting);
            }

            string route = "";

            // Check we have a valid path created.
            if (nodes.Last().PreviousNode != -1)
            {
                int index = nodes.Last().ID;
                while (index != -1)
                {
                    route = route.Insert(0, $"{index + 1} ");
                    index = nodes[index].PreviousNode;
                }
                route = route[..^1]; // Remove the last trailing character
            }
            else route = "0";

            // Write the text to a file.
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

            // Add the node to the list of nodes.
            for (int i = 1; i <= node * 2; i += 2)
            {
                nodes.Add(new Node(id, int.Parse(data[i]), int.Parse(data[i + 1])));
                id++;
            }

            int matrix = node * 2 + 1; // Keep up with the data being iterated over in the for loop

            // Retrieve node j and add connection ID i
            for (int i = 0; i < node; i++) 
            {
                for (int j = 0; j < node; j++)
                {
                    if (int.Parse(data[matrix]) == 1) nodes[j].Connections.Add(Convert.ToInt32(i));
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

            // Iterate over the unvisited nodes
            for (int i = 0; i < unvisited.Count; i++)
            {
                Node n = unvisited[i];
                double distance = n.DistanceFirstNode + n.DistanceTargetNode;

                // Inverted finding minimum standard algorithm :D
                if (!(distance < minimumDistance)) continue;
                minimumDistance = distance;
                index = i;
            }

            return index;
        }
    }
}