using System;
using System.Collections.Generic;

namespace newApp
{
    class Program
    {
        static void Main(string[] args)
        {

            const int size = 100;
            int[,] matrix = new int[size, size];
            Initialization(matrix, size);
            //Print(matrix, size);

            Dictionary<string, Node> nodes = CreateGraph(matrix, size);  // для каждой точки в матрице, в которую можем попасть, создаем узел с координатами и соседями

            Node start = nodes["90.99"];

            Queue<Node> frontier = new Queue<Node>();

            Dictionary<Node?, Node> cameFrom = new Dictionary<Node?, Node>();

            Node NULLnode = new Node(0, 0);

            cameFrom.Add(start, NULLnode);
            frontier.Enqueue(start);

            while (frontier.Count != 0)
            {
                Node current = frontier.Dequeue();
                foreach (Node next in current.Neigbours)
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom.Add(next, current);
                    }
                }
            }

            Node goal = nodes["99.99"];
            Stack<Node> path = new Stack<Node>();
            path.Push(goal);
            int count = 0;
            Node cur = goal;
            while (cur != start)
            {
                cur = cameFrom[cur];
                path.Push(cur);
                count++;
            }

            while (path.Count != 0)
            {
                Node node = path.Pop();
                matrix[node.X, node.Y] = 2;

            }

            Print(matrix, size);

            Console.WriteLine("Path length is :" + count);
        }


        public static Dictionary<string, Node> CreateGraph(int[,] m, int size)
        {
            Dictionary<string, Node> nodesWithNeighbours = new Dictionary<string, Node>();

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (m[i, j] == 7)                       //если элемент имеет значение 7, то это запрещенный блок, и такую точку в узлы не добавляем, т.к. в неё запрещенно попасть
                        continue;

                    Node currentNode = new Node(i, j);

                    string currentKey = currentNode.X.ToString() + "." + currentNode.Y.ToString();            //Вычисляем значение ключа-идентификатора по координатам узла в матрице

                    if (!nodesWithNeighbours.ContainsKey(currentKey))
                        nodesWithNeighbours.Add(currentKey, currentNode);       //добавление узла в список всей узлов (точек, в которые можно шагнуть)
                    else
                    {
                        currentNode = nodesWithNeighbours[currentKey];
                    }

                    for (int k = i - 1; k < i + 2; k++)
                    {       //цикл, в котором перебираются все точки вокруг добавленного узла, чтобы найти те, в которые можно перейти
                        if (k < 0 || k > size - 1)              //проверка, не вышли ли мы за границы массива (в случае граничных точек матрицы)
                            continue;
                        for (int l = j - 1; l < j + 2; l++)
                        {
                            if (l < 0 || l > size - 1 || i == k && j == l)      //проверка, не вышли ли мы за границы массива (в случае граничных точек матрицы) и не является ли рассматриваемая точка точкой добавленного узла
                                continue;

                            if (m[k, l] == 7)
                                continue;

                            Node neighbour = new Node(k, l);

                            string neighbourKey = neighbour.X.ToString() + "." + neighbour.Y.ToString();

                            //чтобы не создавать новые инстансы для одних и тех же точек в матрице и чтобы соединить соседей во всех уже существующих узлах с одними и теми же инстансами узлов,
                            //высчитываю ключ нового соседа: он уже содержится в списке узлов? если да, то добавляем узел из этого списка, если нового соседа в список еще не заносили, но заносим в него существующий инстанс
                            //и добавляем этот инстанс в список узлов

                            bool isNewNeighborAlreadyInNodesList = nodesWithNeighbours.ContainsKey(neighbourKey);

                            if (isNewNeighborAlreadyInNodesList)
                                currentNode.Neigbours.Add(nodesWithNeighbours[neighbourKey]);
                            else
                            {
                                nodesWithNeighbours.Add(neighbourKey, neighbour);
                                currentNode.Neigbours.Add(neighbour);
                            }


                        }
                    }
                }

            return nodesWithNeighbours;
        }
        public static void Print(int[,] matrix, int size)
        {
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
        public static void Initialization(int[,] matrix, int size)
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = 0;
                }

            for (int i = 3; i < 7; i++)
                for (int j = 3; j < 7; j++)
                    matrix[i, j] = 7;

            for (int i = 80; i < 90; i++)
                for (int j = 50; j < 70; j++)
                    matrix[i, j] = 7;

            for (int i = 40; i < 95; i++)
                for (int j = 50; j < 70; j++)
                    matrix[i, j] = 7;

            for (int i = 10; i < 30; i++)
                for (int j = 70; j < 90; j++)
                    matrix[i, j] = 7;

            for (int i = 85; i < size; i++)
                for (int j = 2; j < 17; j++)
                    matrix[i, j] = 7;


            for (int i = 0; i < 10; i++)
                for (int j = 21; j < 17; j++)
                    matrix[i, j] = 7;

            for (int i = 10; i < 35; i++)
                for (int j = 15; j < 40; j++)
                    matrix[i, j] = 7;

            matrix[2, size - 2] = 7;
        }
    }


#nullable enable
    internal class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<Node> Neigbours { get; set; }

        public Node(int x, int y)
        {

            X = x;
            Y = y;
            Neigbours = new List<Node>();
        }
    }

}