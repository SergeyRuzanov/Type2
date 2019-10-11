using System;
using System.Collections.Generic;

namespace Type2
{
    class Program
    {
        static List<List<int>> Rasp(int[,] tree, int k)
        {
            List<int> priority = new List<int>();
            int indexPriority = 0; //
            int indexInPriorityList = 0;

            for (int i = 0; i < tree.GetLength(0); i++)
            {
                bool IsRoot = true;
                for (int j = 0; j < tree.GetLength(0); j++)
                {
                    if (tree[i, j] == 1)
                    {
                        IsRoot = false;
                        break;
                    }
                }

                if (IsRoot)
                {
                    priority.Add(i);
                    indexPriority++;
                }
            }

            while (indexInPriorityList < tree.GetLength(0))
            {
                List<int> list = new List<int>();

                for (int i = indexInPriorityList; i < priority.Count; i++)
                {
                    list.InsertRange(list.Count, A(priority, tree, priority[i]));
                }

                indexInPriorityList = priority.Count;
                priority.InsertRange(priority.Count, list);
            }

            return Dist(tree, priority, k);

        }

        static List<int> A(List<int> priority, int[,] tree, int indexCol)
        {
            List<int> list = new List<int>();
            for (int j = 0; j < tree.GetLength(0); j++)
            {
                if (tree[j, indexCol] == 1)
                {
                    list.Add(j);
                }
            }
            return list;
        }

        static List<List<int>> Dist(int[,] tree, List<int> priority, int k)
        {
            List<List<int>> jobs = new List<List<int>>(k);
            for (int i = 0; i < k; i++)
            {
                jobs.Add(new List<int>());
            }
            List<int> first = new List<int>(); // готовые к распределению
            List<int> second = new List<int>(); // могут распределяться после первых
            List<int> done = new List<int>();

            MakeLists(first, second, done, priority, tree);

            while (done.Count != tree.GetLength(0))
            {
                for (int j = 0; j < jobs.Count; j++)
                {
                    if (first.Count == 0)
                    {
                        jobs[j].Add(-1);
                        continue;
                    }
                    jobs[j].Add(first[0]);
                    done.Add(first[0]);
                    first.RemoveAt(0);
                }

                MakeListsTwo(first, second, done, priority, tree);
            }
            return jobs;
        }

        static void MakeListsTwo(List<int> first, List<int> second, List<int> done, List<int> priority, int[,] tree)
        {
            first.InsertRange(first.Count, second);
            second.Clear();

            for (int i = priority.Count - 1; i >= 0; i--)
            {
                bool flag = true;
                for (int j = 0; j < tree.GetLength(0); j++)
                {
                    if (tree[j, priority[i]] == 1)
                    {
                        if (!first.Contains(j) && !done.Contains(j))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    second.Add(priority[i]);
                    priority.RemoveAt(i);
                }
            }
        }

        static void MakeLists(List<int> first, List<int> second, List<int> done, List<int> priority, int[,] tree)
        {
            for (int i = priority.Count - 1; i >= 0; i--)
            {
                bool IsLeaf = true;
                for (int j = 0; j < tree.GetLength(0); j++)
                {
                    if (tree[j, priority[i]] == 1)
                    {
                        IsLeaf = false;
                        break;
                    }
                }

                if (IsLeaf)
                {
                    first.Add(priority[i]);
                    priority.RemoveAt(i);
                }
            }

            for (int i = priority.Count - 1; i >= 0; i--)
            {
                bool flag = true;
                for (int j = 0; j < tree.GetLength(0); j++)
                {
                    if (tree[j, priority[i]] == 1)
                    {
                        if (!first.Contains(j) && !done.Contains(j))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    second.Add(priority[i]);
                    priority.RemoveAt(i);
                }
            }
        }


        static void Main(string[] args)
        {
            int[,] tree = new int[,]
            {
                {0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0},
                {0,1,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0},
                {0,0,0,0,1,0,0,0,0},
                {0,0,0,0,1,0,0,0,0},
                {0,0,0,0,0,0,0,1,0}
            };


            List<List<int>> list = Rasp(tree, 4); ;

            for (int i = 0; i < list.Count; i++)
            {
                Console.Write($"Работник {i}:");
                for (int j = 0; j < list[i].Count; j++)
                {
                    if (list[i][j] != -1)
                        Console.Write(String.Format("{0, 4}", char.ConvertFromUtf32(list[i][j] + 65)));
                    else
                        Console.Write(String.Format("{0, 4}", '-'));
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
