using System;
using System.Collections.Generic;

namespace LAB1
{
    public class MyStack<T>
    {
        //класс обобщенный, T - универсальный параметр

        private T[] array;              //массив с данными типа Т
        private int defCapacity = 10;   //дефолтная вместимость стека
        private int size;               //размер стека

        public MyStack()
        {
            //дефолтные параметры
            array = new T[defCapacity];
            size = 0;
        }

        public int StackSize
        {
            //размер стека
            get
            {
                return size;
            }
        }

        public void Add(T element)
        {
            //добавление нового элемента
            if (size == array.Length)
            {
                T[] buf = new T[size * 2];
                Array.Copy(array, buf, array.Length);
                array = buf;

            }
            array[size] = element;
            size++;
        }

        public T Pop()
        {
            //удаление "верхушки" стека
            if (size == 0)
            {
                throw new Exception();
            }
            int previous = size - 1;
            size--;
            return array[previous];
        }

        public T GiveElement(int i)
        {
            //возвращает элемент из стека
            if (i >= size)
            {
                throw new Exception();
            }
            return array[i];
        }

    }

    public struct Cell
    {
        //структура клетки лабиринта с её координатами
        public int x;
        public int y;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
    public class Maze
    {
        const int width = 11;
        const int height = 11;
        public int[,] example1 = new int[width, height]
        {
               //0  1  2  3  4  5  6  7  8  9  10 
                {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},  //0
                {1, 0, 0, 0, 0 , 0, 0, 0, 1, 0, 1}, //1
                {1, 1, 1, 1, 1, 1, 1, 0, 1, 0 ,1},  //2
                {1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1},  //3
                {1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1},  //4
                {1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1},  //5
                {1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1},  //6
                {1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1},  //7
                {1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1},  //8
                {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},  //9
                {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},  //10
         };
        public int[,] example2 = new int[width, height]
        {
             //0  1  2  3  4  5  6  7  8  9  10 
                {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},  //0
                {1, 0, 0, 0, 0 , 0, 0, 0, 1, 0, 1}, //1
                {1, 1, 1, 1, 1, 1, 1, 0, 1, 0 ,1},  //2
                {1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1},  //3
                {1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1},  //4
                {1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1},  //5
                {1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1},  //6
                {1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1},  //7
                {1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1},  //8
                {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},  //9
                {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},  //10
        };
        public bool IsEmpty(int i, int j)
        {
            //есть ли проход
            bool result;
            try
            {
                result = example1[i, j] == 0 ? true : false;
            }
            catch (System.IndexOutOfRangeException)
            { result = false; }
            return result;
        }

        public void MakeVisited(int x, int y)
        {
            //сделать клетку посещенной
            example1[x, y] = 1;
        }

        public void MakeUnVisited(int x, int y)
        {
            //сделать клетку НЕпосещенной
            example1[x, y] = 0;
        }
       
        public List<Cell> GetNbrs(int x, int y)
        {
            Cell top = new Cell(x- 1, y);
            Cell bottom = new Cell(x + 1, y);
            Cell right = new Cell(x, y + 1);
            Cell left = new Cell(x, y - 1);
            List<Cell> result = new List<Cell>();
            
            if ((0<=top.x) && (top.x < height) && IsEmpty(top.x, y))
            {
                result.Add(top);
            }
            if ((0 <= bottom.x) && (bottom.x < height) && IsEmpty(bottom.x, y))
            {
                result.Add(bottom);
            }
            if ((0 <= right.y) && (right.y < width) && IsEmpty(x, right.y))
            {
                result.Add(right);
            }
            if ((0 <= left.y) && (left.y < width) && IsEmpty(x, left.y))
            {
                result.Add(left);
            }
            
            return result; 
        }

        public Cell NextCell(List<Cell> list)
        {
            Cell next = new Cell();
            next = list[0];
            return next;
        }
    }

    public class Solution
    {
        MyStack<Cell> way = new MyStack<Cell>();
        private Maze maze = new Maze();
        
        
        private void PrintWay(MyStack<Cell> w)
        {
            for (int i = 0; i < w.StackSize; i++)
            {
                Console.WriteLine("({0}, {1})", w.GiveElement(i).x, w.GiveElement(i).y);
            }
        }

        public void Find(Cell start, Cell finish)
        {
            /*  1. Сделать начальную клетку текущей и отметить ее как посещенную.
                2. Пока не найден выход:
                    1. Если текущая клетка имеет непосещенных «соседей»
                        1. Добавить текущую клетку в стек
                        2. Выбрать случайную клетку из соседних
                        3. Сделать выбранную клетку текущей и отметить ее как посещенную.
                    2. Иначе, если стек не пуст:
                        1. Удалить клетку из стека
                        2. Сделайте ее текущей
                    3. Иначе выхода нет.
            */

            Cell current = start;
            maze.MakeVisited(current.x, current.y);
            Cell next = new Cell();
            List<Cell> nbrs = new List<Cell>();
            bool flag = true;
            bool success = false;
            while (flag)
            {
                nbrs = maze.GetNbrs(current.x, current.y);
                if (nbrs.Count != 0)
                {
                    way.Add(current);
                    next = maze.NextCell(nbrs);
                    current = next;
                    maze.MakeVisited(current.x, current.y);
                }
                else if (way.StackSize != 0)
                {
                    current = way.Pop();
                }
                else 
                {
                    flag = false;
                }

                if ((current.x == finish.x) && (current.y == finish.y))
                {
                    way.Add(current);
                    flag = false;
                    success = true;
                }
            }

            if (success)
            {
                Console.WriteLine("Путь найден");
                PrintWay(way);
            }
            else
            {
                Console.WriteLine("Не найдено");
            }

            
        }

        
    }
    class lab1
    {
        static void Main(string[] args)
        {
            Solution s = new Solution();
            Cell start = new Cell(0, 1);
            Cell finish = new Cell(10, 5);
            s.Find(start, finish);
        }
    }
}
