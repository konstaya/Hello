using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Lab3
{
    

    public enum Positions
    {
        None,
        CEO,    //Chief Executive Officer
        CFO,    //Chief Financial Officer
        
        Lawyer,

        SystemAdministrator,
        SystemEngineer,
        AndroidDeveloper,
        WedDesigner,
        Programmer1C,

        Cleaner,
        SecurityGuard
    }

    //базовый абстрактный класс Сотрудник
    public abstract class Employee
    {
        protected double _salary;
        protected double _bonus;
        protected double _pay;
        protected Positions _post;
        protected bool _fixedPay;
        

        private static readonly Dictionary<Positions, double> _bonuses =
            new Dictionary<Positions, double>
            {
                [Positions.CEO] = 1.5,
                [Positions.CFO] = 1.0,
                [Positions.Lawyer] = 0.75,
                [Positions.SystemAdministrator] = 0.5,
                [Positions.SystemEngineer] = 0.5,
                [Positions.AndroidDeveloper] = 0.85,
                [Positions.WedDesigner] = 0.7,
                [Positions.Programmer1C] = 0.5,
                [Positions.Cleaner] = 1.0,
                [Positions.SecurityGuard] = 0.7,
                [Positions.None] = 0
            };
        protected static readonly Dictionary<Positions, double> _wages =
            new Dictionary<Positions, double>
            {
                [Positions.CEO] = 110000,
                [Positions.CFO] = 80000,
                [Positions.SystemEngineer] = 65000,
                [Positions.AndroidDeveloper] = 70000,
                [Positions.WedDesigner] = 50000,
                [Positions.Programmer1C] = 40000,
                
                [Positions.Lawyer] = 340,                 //for hour
                [Positions.SystemAdministrator] = 180,    //for hour
                [Positions.Cleaner] = 95,                 //for hour
                [Positions.SecurityGuard] = 123,          //for hour

                [Positions.None] = 0
            };

        public Employee() { }
        public Employee(Positions post, string name, string date, bool isFixedPay, int id)
        {
            Post = post;
            Name = name;
            Date = date;
            IsFixedPay = isFixedPay;
            ID = id;
        }

        public Positions Post 
        { get { return _post; }
          set
            {
                _post = value;
                _bonus = _bonuses[_post];
                _pay = _wages[_post];
            }
        }
        
        public string Name { get; set; }
        public string Date { get; set; }
        public bool IsFixedPay { get; set; }
        public int ID { get; set; }
        public abstract double Wage { get; }

    }

    public class EmplFixedWage: Employee
    {
        public EmplFixedWage() { }
        public EmplFixedWage(Positions post, string name, string date, bool isFixedPay = true, int id = 0) 
            : base(post, name, date, isFixedPay, id) { }

        public override double Wage
        {
            get { return _pay * (1 + _bonus); }
        }

    }
    public class EmplHourlyWage : Employee
    {
        public EmplHourlyWage() { }
        public EmplHourlyWage(Positions post, string name, string date, bool isFixedPay = false, int id = 0)
            : base(post, name, date, isFixedPay, id) { }

        public override double Wage
        {
            get { return (_pay*8*20.8) * (1 + _bonus); }
        }

    }

    public class Organisation
    {
        
        private List<Employee> _employees = new List<Employee>();

        //Среднемесячная заработная плата по организации
        public double AverageWage
        {
            get
            {
                double allWages = 0.0;
                int count = 0;

                foreach (var employee in _employees)
                {
                    allWages += employee.Wage;
                    count += 1;
                }

                if (count != 0) return allWages / count;
                else return 0;
            }
        }

        //Добавить работника
        public void Add (Employee employee)
        {
            employee.ID = _employees.Count;
            if ((employee == null) || (employee.Post == Positions.None))
            {
                throw new ArgumentException(nameof(employee));
            }
            _employees.Add(employee);
        }

        //Выдать список самолетов
        public IEnumerable<Employee> GetEmployees()
        {
            return _employees;
        }

        private class ByWagesComparer: IComparer<Employee>
        {
            public int Compare (Employee x, Employee y)
            {
           
                if (x.Wage < y.Wage)
                    return 1;
                else if (x.Wage > y.Wage)
                    return -1;
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
        }

        //Отсортировать по среднемесячной зароботной плате
        public void SortByWage()
        {
            _employees.Sort(new ByWagesComparer());
        }

        /*Вывод на консоль сотрудников. 
         * 
         * Дефолтный параметр - вывод всех сотрудников
         * Параметры: начало и конец списка сотрудников 
         * и/или конкретная характеристика сотрдукников (имя или id по условию)
         * 
         */
        public void PrintEmployees(int begin = -1, int end = -1, string field = "")
        {
            bool flag = ((begin < 0) || (end < 0))& (end-begin >= 0)? false : true;
            bool name = (field == "name") ? true : false;
            bool id = (field == "id") ? true : false;

            if (flag)
            {
                try
                {
                    List<Employee> list = _employees;
                    for (int i = begin; i < end; i++)
                    {
                        if (name) Console.WriteLine($"{list[i].Name}");
                        else if (id) Console.WriteLine($"{list[i].ID}");
                        else
                        {
                            Console.WriteLine($"Employee: ID_{list[i].ID}, {list[i].Post}, {list[i].Name}, {list[i].Date}, {list[i].Wage} rub");
                        }

                    }
                }

                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
            else
            {
                foreach (var empl in this.GetEmployees())
                {
                    Console.WriteLine($"Employee: ID_{empl.ID}, {empl.Post}, {empl.Name}, {empl.Date}, {empl.Wage} rub");

                }
            }
        }

        public int NumberOfEmployees()
        {
            return _employees.Count;
        }

        //Сериализация в JSON-файл
        public void ToJson (string fileName)
        {
            File.WriteAllText(
                fileName, JsonConvert.SerializeObject(
                    _employees, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }));
        }

        //Десериализация из JSON-файла
        public static Organisation FromJson (string fileName)
        {
            var organisation = new Organisation();
            var employees = JsonConvert.DeserializeObject<List<Employee>>(
                File.ReadAllText(fileName), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            if (employees != null) organisation._employees.AddRange(employees);
            return organisation;
        }
    }

    class Menu
    {
        Organisation _organisation;
        bool flag = true;
        string input = "";
        Menu() { }
        public Menu(Organisation org) 
        {
            _organisation = org;
        }
        
        private Positions Post(string value)
        {
            switch (value)
            {
                case "CEO": return Positions.CEO;
                case "CFO": return Positions.CFO;
                case "Laywer": return Positions.Lawyer;
                case "System Administrator": return Positions.SystemAdministrator;
                case "System Engineer": return Positions.SystemEngineer;
                case "Android Developer": return Positions.AndroidDeveloper;
                case "Wed Designer": return Positions.WedDesigner;
                case "Programmer 1C": return Positions.Programmer1C;
                case "Cleaner": return Positions.Cleaner;
                case "Security Guard": return Positions.SecurityGuard;
                default: return Positions.None;

            }
        }
        void Add(Organisation organisation)
        {
            Positions post;
            string name;
            string date;
            string flag;
            Console.Write("Должность: ");  post = Post(Console.ReadLine());
            Console.Write("ФИО: "); name = Console.ReadLine();
            Console.Write("Дата рождения: "); date = Console.ReadLine();
            Console.Write("Фиксированная оплата - 0\nЧасовая оплата - 1\n"); flag = Console.ReadLine();
            if (flag == "0") organisation.Add(new EmplFixedWage(post, name, date));
            else if (flag == "1") organisation.Add(new EmplHourlyWage(post, name, date));
            
        }
        public void mainMenu()
        {
            while (flag)
            {
                Console.WriteLine();
                Console.WriteLine("Добавить нового работника - 0");
                Console.WriteLine("Вывести среднюю зарплату - 1");
                Console.WriteLine("Выход - 2");
                input = Console.ReadLine();
                if ((input == "0") || (input == "1") || (input == "2"))
                {
                    switch (input)
                    {
                        case "0":
                            Add(_organisation);
                            
                            break;
                        case "1":
                            Console.WriteLine($"Средняя зарплата : {_organisation.AverageWage - _organisation.AverageWage%0.01}");
                            break;
                        case "2":
                            flag = false;
                            return;
                        default:
                            flag = false;
                            return;
                    }
                }
                else Console.WriteLine("Некорректный ввод!");

            }

        }
    }

    class LAB3
    {
        static void Main(string[] args)
        {
            var myOrganisation = new Organisation();
            myOrganisation.Add(new EmplFixedWage(Positions.CEO, "Кузнецова ТК", "20.10.2001"));
            myOrganisation.Add(new EmplFixedWage(Positions.AndroidDeveloper, "Дремин МА", "01.12.2000"));
            myOrganisation.Add(new EmplHourlyWage(Positions.Cleaner, "Иванова ИИ", "23.03.2020"));
            myOrganisation.Add(new EmplHourlyWage(Positions.Cleaner, "Петрова ПП", "01.03.2020"));
            myOrganisation.Add(new EmplFixedWage(Positions.AndroidDeveloper, "Смиронов СС", "05.12.1993"));
            myOrganisation.Add(new EmplHourlyWage(Positions.Lawyer, "Увереный ГГ", "28.01.1985"));
            myOrganisation.Add(new EmplHourlyWage(Positions.SecurityGuard, "Путин ВП", "09.10.1955"));

            //сортировка 
            myOrganisation.SortByWage();
            
            const string fileName = @"C:\Users\home\source\repos\Lab3\Json.json";
           
            //сериализация
            myOrganisation.ToJson(fileName);

            //десериализация
            //myOrganisation = Organisation.FromJson(fileName);

            Console.WriteLine("Работники организации:");
            myOrganisation.PrintEmployees();
            
            Console.WriteLine("\nПервые 6 имен работников:");
            myOrganisation.PrintEmployees(0, 6, "name");
           
            Console.WriteLine("\nID последних 4-х работников:");
            myOrganisation.PrintEmployees(myOrganisation.NumberOfEmployees() - 4, myOrganisation.NumberOfEmployees(), "id");
            
            Console.WriteLine("\n\t\tМЕНЮ");

            Menu m = new Menu(myOrganisation);
            m.mainMenu();
        }
    }
}
