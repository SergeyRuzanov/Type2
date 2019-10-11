using System;
using System.Collections.Generic;

namespace Type2
{
    class Program
    {
        static void Rasp(int[,] tree, int k)
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

        static void Dist(int[,] tree, List<int> priority, int k)
        {
            List<List<int>> jobs = new List<List<int>>(k);
            List<int> first = new List<int>(); // готовые к распределению
            List<int> second = new List<int>(); // могут распределяться после первых

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

            for (int i = 0; i < first.Count; i++)
            {
                bool flag = true;
                for(int j = 0; j< tree.GetLength(0); j++)
                {
                    if(tree[j, first[i]] == 1)
                    {
                        if(first.Contains(j)) second.
                    }
                }
            }



        }


        static void Main(string[] args)
        {
            int[,] tree = new int[,]
            {
                { 0,0,0,1,0,0,0},
                { 0,0,0,0,0,0,0},
                { 1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0},
                { 0,1,0,0,0,0,0},
                { 0,0,1,0,0,0,0},
                { 0,0,1,0,0,0,0}
            };
            Rasp(tree, 3);
        }
    }
}
