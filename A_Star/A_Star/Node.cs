using System;
using System.Collections.Generic;
using System.Text;

namespace A_Star
{
    internal class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ID { get; set; }
        public int PreviousNode { get; set; }
        public List<int> Connections = new List<int>();
        public double DistanceFirstNode { get; set; }
        public double DistanceTargetNode { get; set; }
        public bool Visited { get; set; }
        public bool InUnvisited { get; set; }

        // Private constructor. Cannot create an empty node.
        private Node() { }

        /// <summary>
        /// This is a public constructor which is the only way to create a new node.
        /// </summary>
        /// <param name="id">This is the ID of the node.</param>
        /// <param name="x">The x value of the node.</param>
        /// <param name="y">The y value of the node.</param>
        public Node(int id, int x, int y)
        {
            ID = id;
            X = x;
            Y = y;
            DistanceFirstNode = double.MaxValue;
            DistanceTargetNode = double.MaxValue;
            Visited = false;
            InUnvisited = false;
            PreviousNode = -1;
        }

        /// <summary>
        /// Calculate a distance from the first node to another node.
        /// </summary>
        /// <param name="node">The target node.</param>
        public void CalculateDistanceFirstNode(Node node)
        {
            //Calculating the distance between this node and the previous node
            double routeLength = node.DistanceFirstNode + Math.Pow(Math.Pow(X - node.X, 2) + Math.Pow(Y - node.Y, 2), 0.5);

            // Update the path for the algorithm if we find a shorter one.
            // Also, another finding minimum standard algorithm :)
            if (routeLength < DistanceFirstNode)
            {
                DistanceFirstNode = routeLength;
                PreviousNode = node.ID;
            }
        }

        // Calculate a node distance to target node.
        // Sadly, this is calculated every time (for now) :>
        public void CalculateDistanceTargetNode(int tX, int tY)
        {
            DistanceTargetNode = Math.Pow(Math.Pow(tX - X, 2) + Math.Pow(tY - Y, 2), 0.5);
        }
    }
}
