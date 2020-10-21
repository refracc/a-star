using System;
using System.Collections.Generic;
using System.Text;

namespace A_Star
{
    class Node
    {
        public int X { get; set; } //X Value
        public int Y { get; set; }  //Y Value
        public int ID { get; set; } //Cavern ID
        public int PreviousNode { get; set; } //ID of Previous Cavern in Route
        public List<int> Connections = new List<int>(); //List if IDs for Connected Caverns
        public double G { get; set; } //Distance from first Cavern (Route Distance)
        public double H { get; set; } //Distance to Target Cavers
        public bool Visited { get; set; }
        public bool IsInUnvisited { get; set; }

        //Constructor
        public Node(int id, int x, int y)
        {
            ID = id;
            X = x;
            Y = y;
            G = double.MaxValue;
            H = double.MaxValue;
            Visited = false;
            IsInUnvisited = false;
            PreviousNode = -1;
        }

        //Calculate the StarDistance and Distance to Target
        public void CalcG(Node node)
        {
            //Calculating the distance between this node and the previous cavern
            double routeLength = node.G + Math.Sqrt(Math.Pow(X - node.X, 2) + Math.Pow(Y - node.Y, 2));

            //If routeLength is < than StartDistance we have found a shorter route. We update the information for this Cavern
            if (routeLength < G)
            {
                G = routeLength;
                PreviousNode = node.ID;
            }
        }

        //Calculating this cavern distance to target node. [Unfortunately this is calculated every time] :(
        public void CalcH(int tX, int tY)
        {
            H = Math.Sqrt(Math.Pow(tX - X, 2) + Math.Pow(tY - Y, 2));
        }
    }
}
