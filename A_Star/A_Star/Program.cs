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
            // Creating a list of nodes 
            List<Node> nodes = new List<Node>();

            // Create a node list from a given file and its associated list of nodes.
            CreateNodeList((args[0] + ".cav"), nodes);

            // Creating a list of unvisited nodes initialised by the first node in nodes
            List<Node> unvisitedNodes = new List<Node> { nodes[0] };

            // Setting the first cavern distance from starting node to 0.
            unvisitedNodes[0].DistanceFirstNode = 0;

            // Calculating the distance of the first node to the target node.
            nodes.First().CalculateDistanceTargetNode(nodes.Last().X, nodes.Last().Y);

            // Index of the node being visited.
            int visiting = 0;

            // Iterate through the entire node list until we exhaust all possible options or find the target (end) node.
            while (unvisitedNodes.Any() && visiting != nodes.Last().ID)
            {
                // Search for closest node to the target.
                visiting = FindNextNode(unvisitedNodes);

                // Iterate through the IDs in the node list, comparing the IDs to ones we haven't checked.
                foreach (int connectedId in nodes[unvisitedNodes[visiting].ID].Connections.Where(connectedId => !nodes[connectedId].Visited))
                {
                    // Calculate distance between first & target nodes
                    nodes[connectedId].CalculateDistanceFirstNode(nodes[unvisitedNodes[visiting].ID]);
                    nodes[connectedId].CalculateDistanceTargetNode(nodes.Last().X, nodes.Last().Y);

                    if (nodes[connectedId].InUnvisited) continue;

                    // Add the node to a list if unvisited nodes.
                    unvisitedNodes.Add(nodes[connectedId]);

                    // Mark unvisited, this avoids repetition of the same node (avoids infinite loop :D)
                    unvisitedNodes.Last().InUnvisited = true;
                }

                // Mark the current node as visited.
                unvisitedNodes[visiting].Visited = true;

                // Remove it from the unvisited nodes list.
                unvisitedNodes[visiting].InUnvisited = false;
                unvisitedNodes.RemoveAt(visiting);
            }

            string route = "";

            if (nodes.Last().PreviousNode != -1) // Did we find a path to the target node?
            {
                int index = nodes.Last().ID;
                while (index != -1) //ind will become -1 when we reach the Starting Node
                {
                    route = route.Insert(0, $"{index + 1} -> ");
                    index = nodes[index].PreviousNode;
                }

                // Removing the last 4 characters (arrows and spaces)
                route = route[..^4];
            }
            else route = "0"; // The route will be set to 0 if we cannot find a path from start to end node.

            // Write the resulting path to the output file.
            File.WriteAllText(args[0] + ".csn", route);
        }

        /// <summary>
        /// The method will create a node list from a file name and a list of nodes.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="nodes">The list of nodes </param>
        private static void CreateNodeList(string fileName, IList<Node> nodes)
        {
            string node = File.ReadAllText(fileName);

            string[] data = node.Split(',');

            // Grab the first extracted number.
            int nNode = int.Parse(data[0]);

            int nodeId = 0;
            for (int i = 1; i <= nNode * 2; i += 2)
            {
                nodes.Add(new Node(nodeId, int.Parse(data[i]), int.Parse(data[i + 1])));
                nodeId++;
            }

            // Add some connections between nodes.
            int matrixI = nNode * 2 + 1; // Keep track of the data in the data array
            {
                for (int i = 0; i < nNode; i++) 
                {
                    for (int j = 0; j < nNode; j++)
                    {
                        // 1 refers to a connection between a pair of nodes.
                        if (int.Parse(data[matrixI]) == 1)
                            nodes[j].Connections.Add(Convert.ToInt32(i));
                        matrixI++;
                    }
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
            double minDist = double.MaxValue; // Create a minimum distance value and assign it a maximum value.          
            int unvisitedIndex = -1;

            for (int i = 0; i < unvisited.Count; i++)
            {
                Node n = unvisited[i]; // The node being indexed from the list of unvisited nodes.
                double distance = n.DistanceFirstNode + n.DistanceTargetNode;

                // Standard "inverted" standard algorithm
                if (!(distance < minDist)) continue;

                minDist = distance;
                unvisitedIndex = i;
            }

            return unvisitedIndex;
        }
    }
}