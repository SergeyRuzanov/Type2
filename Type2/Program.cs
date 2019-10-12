using System;
using System.Collections.Generic;

namespace Type2
{
    class Program
    {
        static List<List<int>> Rasp(int[,] tree, int k)
        {
            //Список работы отсортированные по возрастанию приоритетов.
            List<int> priority = new List<int>();

            //Ищем все корни и назначаем им приоритеты.
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
                }
            }

            //Индекс в списке priority, указывающий на индекс работы, с которой надо искать зависимые от нее работы 
            int indexInPriorityList = 0;

            //Раставляем приоритеты оставшимся вершинам
            while (indexInPriorityList < tree.GetLength(0))
            {
                //Список работ
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

        /// <summary>
        /// Распределяет зависимые от indexCol работы по приоритету.
        /// </summary>
        /// <param name="priority">Список работ с приоритетами.</param>
        /// <param name="tree">Матрица смежности работ.</param>
        /// <param name="indexCol">Работа, для которой просматриваются зависимости.</param>
        /// <returns>Список работ, зависящих от indexCol, отсортированные по возрастанию приоритета.</returns>
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

        /// <summary>
        /// Распределение работ по работникам.
        /// </summary>
        /// <param name="tree">Матрица смежности работ.</param>
        /// <param name="priority">Список работ с приоритетами.</param>
        /// <param name="k">Количество работников.</param>
        /// <returns>Спсок работ распределенных по работникам.</returns>
        static List<List<int>> Dist(int[,] tree, List<int> priority, int k)
        {
            List<List<int>> jobs = new List<List<int>>(k); //список работ для каждого работника
            for (int i = 0; i < k; i++)
            {
                jobs.Add(new List<int>());
            }
            List<int> first = new List<int>(); // готовые к распределению
            List<int> second = new List<int>(); // могут распределяться после первых
            List<int> done = new List<int>(); // выполненные работы

            MakeLists(first, second, done, priority, tree);

            while (done.Count != tree.GetLength(0))
            {
                //распределяем работы по работникам (если для работника не хватит работы на данное время, то вставляем -1)
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

        /// <summary>
        /// Распределение работ по спискам для выполнения.
        /// </summary>
        /// <param name="first">Список работ готовых для выполнения.</param>
        /// <param name="second">Список работ, которые гоовы к выполнению после выполнения работ из списка first.</param>
        /// <param name="done">Список завершенных работ.</param>
        /// <param name="priority">Список работ с приоритетами.</param>
        /// <param name="tree">Матрица смежности работ.</param>
        static void MakeListsTwo(List<int> first, List<int> second, List<int> done, List<int> priority, int[,] tree)
        {
            first.InsertRange(first.Count, second); //добавляем в список first все работы из списка second
            second.Clear();

            //добавление работ в спсисок second
            for (int i = priority.Count - 1; i >= 0; i--)
            {
                //флаг: возможно ли добавление в список second
                bool flag = true;

                for (int j = 0; j < tree.GetLength(0); j++)
                {
                    if (tree[j, priority[i]] == 1)//проверка: зависит ли работа из списка priority от других работ
                    {
                        if (!first.Contains(j) && !done.Contains(j)) //проверка: находится ли работа, от которой зависит данная работа, в списке завершенных работ или готовых к выполению
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

        /// <summary>
        /// Распределение работ по спискам для выполнения при первом запуске.
        /// </summary>
        /// <param name="first">Список работ готовых для выполнения.</param>
        /// <param name="second">Список работ, которые гоовы к выполнению после выполнения работ из списка first.</param>
        /// <param name="done">Список завершенных работ.</param>
        /// <param name="priority">Список работ с приоритетами.</param>
        /// <param name="tree">Матрица смежности работ.</param>
        static void MakeLists(List<int> first, List<int> second, List<int> done, List<int> priority, int[,] tree)
        {
            //помещаем все листья в список firs
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

            //Помещаем все работы зависимые от литьев в список second
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
                {0,0,0,0,0,0},
                {0,0,0,0,1,0},
                {1,0,0,0,0,0},
                {0,0,0,0,0,1},
                {1,0,0,0,0,0},
                {1,0,0,0,0,0},

            };


            List<List<int>> list = Rasp(tree, 2); ;

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
