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
        /// Calculate a distance from this node to another node.
        /// </summary>
        /// <param name="node">The target node.</param>
        public void CalculateDistanceToNode(Node node)
        {
            // Calculate route cost using Euclidean Distance
            double route = node.DistanceFirstNode + Math.Pow(Math.Pow(X - node.X, 2) + Math.Pow(Y - node.Y, 2), 0.5);

            if (!(route < DistanceFirstNode)) return;
            DistanceFirstNode = route;
            PreviousNode = node.ID;
        }

        /// <summary>
        /// Calculate the distance from the current node to a location.
        /// This unfortunately has to be calculated every time we compare a new node :(
        /// and there is nothing we can do about it *plays sad violin*
        /// </summary>
        /// <param name="x">The X co-ordinate.</param>
        /// <param name="y">The Y co-ordinate.</param>
        public void CalculateDistanceToLocation(int x, int y)
        {
            // Calculate cost using Euclidean Distance
            DistanceTargetNode = Math.Pow(Math.Pow(x - X, 2) + Math.Pow(y - Y, 2), 0.5);
        }
    }
}
