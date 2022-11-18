﻿using DeliveryVHGP.DeliveryAlgorithm.Model;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.DeliveryAlgorithm
{
    public class DeliveryPickupAlgorithm
    {
        private readonly IServiceProvider _serviceProvider;
        public DeliveryPickupAlgorithm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void AlgorithsProcess()
        {
            // Instantiate the data problem.
            DataModel data = new DataModel();

            // Create Routing Index Manager
            RoutingIndexManager manager =
                new RoutingIndexManager(data.DistanceMatrix.GetLength(0), data.VehicleNumber, data.Depot);


            // Create Routing Model.
            RoutingModel routing = new RoutingModel(manager);

            // Create and register a transit callback.
            int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
            {
                // Convert from routing variable Index to
                // distance matrix NodeIndex.
                var fromNode = manager.IndexToNode(fromIndex);
                var toNode = manager.IndexToNode(toIndex);
                return data.DistanceMatrix[fromNode, toNode];
            });

            // Define cost of each arc.
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            // Add Distance constraint.
            routing.AddDimension(transitCallbackIndex, 0, 20000,
                                 true, // start cumul to zero
                                 "Distance");
            RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
            distanceDimension.SetGlobalSpanCostCoefficient(100);

            // Define Transportation Requests.
            Solver solver = routing.solver();
            for (int i = 0; i < data.PickupsDeliveries.GetLength(0); i++)
            {
                long pickupIndex = manager.NodeToIndex(data.PickupsDeliveries[i][0]);
                long deliveryIndex = manager.NodeToIndex(data.PickupsDeliveries[i][1]);
                routing.AddPickupAndDelivery(pickupIndex, deliveryIndex);
                solver.Add(solver.MakeEquality(routing.VehicleVar(pickupIndex), routing.VehicleVar(deliveryIndex)));
                solver.Add(solver.MakeLessOrEqual(distanceDimension.CumulVar(pickupIndex),
                                                  distanceDimension.CumulVar(deliveryIndex)));
            }

            // Setting first solution heuristic.
            RoutingSearchParameters searchParameters =
                operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
            searchParameters.TimeLimit = new Duration { Seconds = 5 };

            // Solve the problem.
            Assignment solution = routing.SolveWithParameters(searchParameters);

            // Print solution on console.
            PrintSolution(data, routing, manager, solution);
        }
        public void PrintSolution(in DataModel data, in RoutingModel routing, in RoutingIndexManager manager,
                              in Assignment solution)
        {
            List<SegmentModel> list = new List<SegmentModel>() { new SegmentModel(){fromBuilding = "b1", toBuilding = "b3"},
                                                                 new SegmentModel(){fromBuilding = "b5", toBuilding = "b6"},
                                                                new SegmentModel(){fromBuilding = "b5", toBuilding = "b6"}};
            int[][] PickupsDeliveries = null;

            // Khai bao trong DataModel, them {get;set;} HERE HERE
            PickupsDeliveries = ChangeBuildingIdIntoInt(list, list.Count);//{1,6},{2,6},{1,6},{2,1}
            var nodes = ChangeListIntoNode(PickupsDeliveries);            //{0,1,2,6}
            var newNode = ChangeNodesIntoNew(nodes);                      //{0,1,2,3}
            //foreach (var a in PickupsDeliveries)
            //{
            //    Console.WriteLine(a[0] + " " + a[1]);
            //}
            //Console.WriteLine(String.Join(" ", nodes));
            //Console.WriteLine(String.Join(" ", newNode));
            List<NodesMapping> map = new List<NodesMapping>();           //real:{0,1,2,6}, fake:{0,1,2,3}
            for (int i = 0; i < nodes.Length; i++)
            {
                NodesMapping node = new NodesMapping() { RealNode = nodes[i], FakeNode = newNode[i] };
                map.Add(node);
                //Console.WriteLine(node.RealNode + " " + node.FakeNode);
            }

            // Inspect solution.----------------------------------------------------
            long maxRouteDistance = 0;
            long totalRouteDistance = 0;
            for (int i = 0; i < 4; ++i) //data.VehicleNumber = 4
            {
                //Console.WriteLine("Route for Vehicle {0}:", i);
                // Creat table Delivery Route
                long routeDistance = 0;
                var start = routing.Start(i);
                var index = solution.Value(routing.NextVar(start));
                while (routing.IsEnd(index) == false)
                {
                    //Console.Write("{0} -> ", manager.IndexToNode((int)index));
                    foreach (var node in map)
                    {
                        String previousBuiding = "";
                        if (index == node.FakeNode)
                        {
                            if (node.RealNode < 33)
                            {
                                var building = node.RealNode;
                            }
                            else
                            {
                                var building = node.RealNode - 32;
                                BuildingEnum buildId = (BuildingEnum)building;
                            }
                            //Create Route edge
                            //List order Id -> list segment -> check building id -> create orderAction()
                            //if segment type 2(hub - cus), 3(store - cus) -> remove order Queue
                            //if segment type 1(store - hub) -> remove then add to queue || do nothing

                            //load list order from queue -> segment(not done, done) -> 1 segment then remove order from queue 
                            //-> segment done -> order done, fail or at hub
                            //if order at hub -> add order to queue
                        }
                        previousBuiding = "";
                    }
                    var previousIndex = index;
                    index = solution.Value(routing.NextVar(index));
                    routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
                }
                //Console.WriteLine("{0}", manager.IndexToNode((int)index));
                Console.WriteLine("Distance of the route: {0}m", routeDistance);
                maxRouteDistance = Math.Max(routeDistance, maxRouteDistance);
                totalRouteDistance += routeDistance;
            }
            Console.WriteLine("Maximum distance of the routes: {0}m", maxRouteDistance);
        }
        //Ulity___________________________________________________________________________________________-
        
        // list Segment -> int[][] vector for algorithms input db -> {1,6},{2,6},{1,6},{2,1}
        public int[][] ChangeBuildingIdIntoInt(List<SegmentModel> input, int count)
        {
            int[][] result = new int[count][];
            var buildings = System.Enum.GetValues(typeof(BuildingEnum))
                        .Cast<BuildingEnum>()
                        .Select(d => (d, (int)d))
                        .ToList();

            int i = 0;
            foreach (var bu in input)
            {
                int[] a = new int[2];
                foreach (var build in buildings)
                {
                    if (build.Item1.ToString() == bu.fromBuilding)
                    {
                        a[0] = build.Item2;
                        //Console.WriteLine(build.Item2);
                    }
                    else if (build.Item1.ToString() == bu.toBuilding)
                    {
                        a[1] = build.Item2 + 10;
                        //Console.WriteLine(build.Item2);
                    }

                }
                result[i] = a;
                i++;
            }
            return result;
        }
        //Remove duplicate and sort and take node need to pass {1,6},{2,6},{1,6},{2,1} -> {0,1,2,6} add 0 to list
        public int[] ChangeListIntoNode(int[][] input)
        {

            List<int> nodes = new List<int>();
            foreach (var node in input)
            {
                nodes.Add(node[0]);
                nodes.Add(node[1]);
            }
            List<int> newNode = nodes.Distinct().ToList();
            newNode.Sort();

            int[] result = new int[newNode.Count() + 1];
            //add fake node 0
            result[0] = 0;
            for (int i = 1; i < newNode.Count() + 1; i++)
            {
                result[i] = newNode[i - 1];
            }

            return result;
        }
        //remove node in distance matrix, just take node in segment   take{0,1,2,6}-> new matrix inpt for algorithms  
        public int[,] Convert(int[,] DistanceMatrixAllNode, int[] NeedNode)
        {
            int n = NeedNode.GetLength(0);
            int[,] DistanceMatrixNeedNode = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    DistanceMatrixNeedNode[i, j] = DistanceMatrixAllNode[NeedNode[i], NeedNode[j]];
                    Console.Write(DistanceMatrixNeedNode[i, j] + " ");
                }
                Console.WriteLine("");
            }
            return DistanceMatrixNeedNode;
        }
        //changes node index after changes matrix distance {0,1,3,6} -> {0,1,2,3}
        public int[] ChangeNodesIntoNew(int[] input)
        {
            //Console.WriteLine(input.Length);
            int[] result = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = i;
            }
            return result;
        }              
    }
}
