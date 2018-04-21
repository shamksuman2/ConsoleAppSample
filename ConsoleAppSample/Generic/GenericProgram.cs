using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ConsoleAppSample.Generic
{
    public class GenericProgram
    {
        public void Main()
        {
            //CircularBufferGeneric();

            //GenericList();
            //Queue();

            //HashSet();

            /*With HashSet and IEquality*/
            //var departments = new SortedDictionary<string, HashSet<Employee>>();
            //departments.Add("Sales", new HashSet<Employee>(new EmployeeComparer()));
            //departments["Sales"].Add(new Employee() { Name = "x" });
            //departments["Sales"].Add(new Employee() { Name = "A" });
            //departments["Sales"].Add(new Employee() { Name = "y" });

            //departments.Add("Engineering", new HashSet<Employee>(new EmployeeComparer()));

            //departments["Engineering"].Add(new Employee() { Name = "B" });
            //departments["Engineering"].Add(new Employee() { Name = "A" });
            //departments["Engineering"].Add(new Employee() { Name = "B" });

            var departments = new DepartmentCollection();
            departments.Add("Sales", new Employee() { Name = "x" })
            .Add("Sales", new Employee() { Name = "A" })
            .Add("Sales", new Employee() { Name = "y" })
            .Add("Engineering", new Employee() { Name = "B" })
            .Add("Engineering", new Employee() { Name = "A" })
            .Add("Engineering", new Employee() { Name = "B" });


            foreach (var department in departments)
            {
                Console.WriteLine(department.Key);

                foreach (var emp in department.Value)
                {
                    Console.WriteLine("Employee name " + emp.Name);
                }
            }
        }

        private static void HashSet()
        {
            HashSet<int> set = new HashSet<int>();
            set.Add(1);
            set.Add(2);
            set.Add(3);

            HashSet<int> set1 = new HashSet<int>();
            set.Add(2);
            set.Add(3);
            set.Add(4);

            set.IntersectWith(set1);
            set.ExceptWith(set1);
            set.UnionWith(set1);
            set.SymmetricExceptWith(set1);

            foreach (var item in set)
            {
                Console.WriteLine(item);
            }
        }

        private static void Queue()
        {
            Queue<Employee> employees = new Queue<Employee>();
            employees.Enqueue(new Employee() { Name = "A" });
            employees.Enqueue(new Employee() { Name = "B" });
            employees.Enqueue(new Employee() { Name = "C" });

            while (employees.Count > 0)
            {
                var emp = employees.Dequeue();

                Console.WriteLine(emp.Name);
            }
        }


        private static void GenericList()
        {
            var number = new List<int>(10);
            var capacity = -1;
            while (true)
            {
                if (number.Capacity != capacity)
                {
                    capacity = number.Capacity;
                    Console.WriteLine(capacity);
                }

                number.Add(1);
            }
        }

        private static void CircularBufferGeneric()
        {
            var buffer = new Buffer<double>();


            WriteToBuffer(buffer);
            foreach (var item in buffer)
            {
                Console.WriteLine(item);
            }
            ReadFromBuffer(buffer);
        }

        private static void ReadFromBuffer(IBuffer<double> buffer)
        {
            Console.WriteLine("Buffer");
            var sum = 0.0;
            while (!buffer.IsEmpty)
            {
                sum += buffer.Read();
            }
            Console.WriteLine("\t" + sum);

        }

        private static void WriteToBuffer(IBuffer<double> buffer)
        {
            while (true)
            {
                var parsed = false;
                var value = 0.0;
                var input = Console.ReadLine();
                if (double.TryParse(input, out value))
                {
                    buffer.Write(value);
                    continue;
                }

                break;
            }
        }
    }

    public class DepartmentCollection : SortedDictionary<string, SortedSet<Employee>>
    {
        public DepartmentCollection Add(string departmentName, Employee employee)
        {
            if (!ContainsKey(departmentName))
            {
                Add(departmentName, new SortedSet<Employee>(new EmployeeComparer()));
            }

            this[departmentName].Add(employee);
            return this;
        }
    }

    public class Employee
    {
        public string Name { get; set; }
    }

    public class EmployeeComparer : IEqualityComparer<Employee>, IComparer<Employee>
    {
        public bool Equals(Employee x, Employee y)
        {
            return String.Equals(x.Name, y.Name);
        }

        public int GetHashCode(Employee obj)
        {
            return obj.Name.GetHashCode();
        }

        public int Compare(Employee x, Employee y)
        {
            return String.Compare(x.Name, y.Name);
        }
    }
}
