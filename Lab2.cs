using System;

namespace LAB2
// 5 вариант
/*Составить описание класса для представления комплексных чисел. 
 * Обеспечить выполнение операций сложения, вычитания и умножения комплексных чисел. 
 * Предусмотреть поддержку числа в экспоненциальной форме.
 * Все операции реализовать в виде перегрузки операторов. 
 * Программа должна содержать меню, позволяющее осуществлять проверку всех методов.*/
{
    class ComplexNum
    {

        private double reZ;     //действительная часть комплексного числа
        private double imZ;     //мнимая часть комлесного числа

        private double phi;     //аргумент комплексного числа
        private double r;       //модуль комплексного числа

        public void ToAlgForm()
        {
            //перевод числа в алгебраическую форму
            this.reZ = Math.Cos(phi) * r;
            this.imZ = Math.Sin(phi) * r;
            
        }
        public void ToExpForm()
        {
            //перевод числа в показательную форму
            this.r = Math.Sqrt((this.reZ * this.reZ + this.imZ * this.imZ));
            this.phi = Math.Acos(this.reZ / this.r);
        }
        public double Phi
        {
            set
            {
                phi = value;
            }
        }
        public double R
        {
            set
            {
                r = value;
            }
        }
        public double ReZ
        {
            set
            {
                reZ = value;
            }
            get
            {
                return reZ;
            }
        }
        public double ImZ
        {
            set
            {
                imZ = value;
            }
            get
            {
                return imZ;
            }
        }
        public ComplexNum()
        {
            this.reZ = 0;
            this.imZ = 0;
        }
        public ComplexNum (int ReZ, int ImZ)
        {
            this.reZ = ReZ;
            this.imZ = ImZ;
        }
        public static ComplexNum operator + (ComplexNum z1, ComplexNum z2)
        {
            ComplexNum z = new ComplexNum();
            z.reZ = z1.reZ + z2.reZ;
            z.imZ = z1.imZ + z2.imZ;
            return z;
        }
        public static ComplexNum operator - (ComplexNum z1, ComplexNum z2)
        {
            ComplexNum z = new ComplexNum();
            z.reZ = z1.reZ - z2.reZ;
            z.imZ = z1.imZ - z2.imZ;
            return z;
        }
        public static ComplexNum operator * (ComplexNum z1, ComplexNum z2)
        {
            ComplexNum z = new ComplexNum();
            z.reZ = z1.reZ*z2.reZ - z1.imZ*z2.imZ;
            z.imZ = z1.reZ*z2.imZ + z2.reZ*z1.imZ;
            z.r = z1.r * z2.r;
            z.phi = z1.phi + z2.phi;
            return z;
        }
        public void Print()
        {
            //вывод на консоль комплесного числа в алгебраической и показательной формах
            Console.WriteLine("z = {0} + {1} * i", reZ, imZ);
            Console.WriteLine("z = {0} * e ^({1} * i)", r, phi);
            
        }
    }
    class Menu
    {
        public ComplexNum EnterCN(int i)
        {
            //ввод комлексного числа
            string input = "";
            bool flag = true;
            ComplexNum z = new ComplexNum();

            Console.WriteLine("0 - создать комлексное число");
            Console.WriteLine("1 - создать комлексное число в показательной форме");
           
            while (flag)
            {
                input = Console.ReadLine();
                if ((input == "0") || (input == "1"))
                {
                    flag = false;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод!");
                }
            }
            switch (input)
            {
                case "0":
                    Console.WriteLine(" z = ReZ + ImZ*i");
                    Console.WriteLine("Введите действительную часть комлексного числа:");
                    Console.Write("ReZ = "); double a = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Введите мнимую часть комлексного числа:");
                    Console.Write("ImZ = "); double b = Convert.ToDouble(Console.ReadLine());
                    z.ReZ = a; z.ImZ = b; z.ToExpForm();
                    Console.Write("{0} комплексное число: ", i);
                    z.Print();
                    Console.WriteLine();
                    break;
                case "1":
                    Console.WriteLine(" z = r * e^(i*phi)");
                    Console.WriteLine("Введите модуль комплексного числа:");
                    Console.Write("r = "); double r = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Введите аргумент комлексного числа:");
                    Console.Write("phi = "); double p = Convert.ToDouble(Console.ReadLine());
                    z.R = r; z.Phi = p; z.ToAlgForm();
                    Console.Write("{0} комплексное число: ", i);
                    z.Print();
                    Console.WriteLine();
                    break;
            }
            return z;
        }
        public bool func(ComplexNum z1, ComplexNum z2)
        {
            //проверка всех методов
            bool flag = true;
            string input = "";
            ComplexNum z = new ComplexNum();
            
            while (flag)
            {
                Console.WriteLine("0 - Сложение комлексных чисел");
                Console.WriteLine("1 - Вычитание комлексных чисел");
                Console.WriteLine("2 - Умножение комлексных чисел");
                Console.WriteLine("3 - Создать новые комлексные числа");
                Console.WriteLine("4 - Выход");
                input = Console.ReadLine();
                if ((input == "0") || (input == "1") || (input == "2") || (input == "4"))
                {
                    Console.WriteLine();
                    switch (input)
                    {
                        case "0":
                            Console.WriteLine("z1 + z2");
                            z = z1 + z2;
                            z.ToExpForm();
                            z.Print();
                            break;
                        case "1":
                            Console.WriteLine("z1 - z2");
                            z = z1 - z2;
                            z.ToExpForm();
                            z.Print();
                            break;
                        case "2":
                            Console.WriteLine("z1 * z2");
                            z = z1 * z2;
                            z.Print();
                            break;
                        case "4":
                            flag = false;
                            break;
                    }
                    
                }
                else if (input == "3")
                {
                    if (func(EnterCN(1), EnterCN(2)) == false)
                    {
                        flag = false;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод!");
                }
            }
            return flag;
        }
        public void menu()
        {
            func(EnterCN(1), EnterCN(2));
        }
    }
    class Lab2
    {
        static void Main(string[] args)
        {
            Menu m = new Menu();
            m.menu();
        }
    }
}
