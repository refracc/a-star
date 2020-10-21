using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace A_Star
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creating the list of Nodes
            List<Node> caverns = new List<Node>();

            //string cavernSource = "generated5000-1.cav";
            //Extracting the caverns list and connection from the file.
            string inputFile = args[0] + ".cav";
            CreateNodeList(inputFile, caverns);

            //Creating the list of unvisited caverns
            List<Node> unvisitedNodes = new List<Node> {caverns[0]};

            //Adding the first cavern to the unvisited caverns list

            //Setting the first cavern distance from Start to 0
            unvisitedNodes[0].G = 0;
            //Calculating distance of first cavern to Target
            caverns.First().CalcH(caverns.Last().X, caverns.Last().Y);

            int visiting = 0; //Index of visiting cavern

            while (unvisitedNodes.Any() && visiting != caverns.Last().ID
            ) //Loop Until we have visited every cavern or we visited the target cavern
            {
                visiting = FindNextNode(unvisitedNodes); //Searching the closest cavern to Target

                foreach (int connectedId in caverns[unvisitedNodes[visiting].ID].Connections)
                {
                    if (!caverns[connectedId].Visited)
                    {
                        caverns[connectedId].CalcG(caverns[unvisitedNodes[visiting].ID]);
                        caverns[connectedId].CalcH(caverns.Last().X, caverns.Last().Y);

                        if (!caverns[connectedId].IsInUnvisited)
                        {
                            //Adding cavern t unvisited
                            unvisitedNodes.Add(caverns[connectedId]);
                            //Marking the cavern as ToBeExplored - this will avoid repetition of the same cavern
                            unvisitedNodes.Last().IsInUnvisited = true;
                        }
                    }
                }

                //Marking the visited cavern as Visited
                unvisitedNodes[visiting].Visited = true;
                //Removing the visited cavern from the UnvisitedNodes List
                unvisitedNodes[visiting].IsInUnvisited = false;
                unvisitedNodes.RemoveAt(visiting);
            }

            string route = string.Empty; //This string will store the route
            string solutionFileName = args[0] + ".csn";

            if (caverns.Last().PreviousNode != -1) //Checking if we managed to find a route to the Target Node
            {
                int ind = caverns.Last().ID;
                while (ind != -1) //ind will become -1 when we reach the Starting Node
                {
                    route = route.Insert(0, $"{(ind + 1).ToString()} ");
                    ind = caverns[ind].PreviousNode;
                }
            }
            else
                route = "0";

            //Creating SOLUTION file
            File.WriteAllText(solutionFileName, route);
        }

        public static void CreateNodeList(string fileName, List<Node> cavernsL)
        {
            string cavernsInformation = File.ReadAllText(fileName);

            string[] extracted = cavernsInformation.Split(',');

            //Getting Nodes Number (First Extracted Number)
            int nNode = int.Parse(extracted[0]);

            int cIndex = 0; //Node ID
            for (int i = 1; i <= nNode * 2; i += 2)
            {
                cavernsL.Add(new Node(cIndex, int.Parse(extracted[i]), int.Parse(extracted[i + 1])));
                cIndex++;
            }

            //Adding Connections to the Nodes
            int matrixI = nNode * 2 + 1; //Keeps track of the extracted values in extractedList
            {
                for (int i = 0; i < nNode; i++)
                {
                    for (int j = 0; j < nNode; j++)
                    {
                        if (int.Parse(extracted[matrixI]) == 1) //1 means we have a connection between two Nodes
                            cavernsL[j].Connections.Add(Convert.ToInt32(i));
                        matrixI++;
                    }
                }
            }
        }

        public static int FindNextNode(List<Node> unvisited)
        {
            double minDist = double.MaxValue; //Setting Min distance to MAX VALUE            
            int unvisitedIndex = -1; //Index in UnvisitedNodes
            int i = 0; //Keeps track of ID
            foreach (Node c in unvisited)
            {
                double f = c.G + c.H;
                if (f < minDist)
                {
                    minDist = f; //Updating minDist with new value                    
                    unvisitedIndex = i; //Index of the closest cavern in the UnvisitedNodes List
                }

                i++;
            }

            return unvisitedIndex;
        }
    }
}