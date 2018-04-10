using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRegistret {
    class Company {
        List<Employee> employees;

        public Company() {
            employees = new List<Employee>();
        }

        public IReadOnlyList<Employee> Employees {
            get {
                return employees;
            }
        }

        public void AddEmployee(Employee person) {
            if (!employees.Contains(person)) {
                employees.Add(person);
            }
        }

        public void RemoveEmployee(Employee person) {
            employees.Remove(person);
        }

        public string Serialize() {
            string output = "";
            foreach (var emp in employees) {
                output += $"{emp.Salary} {emp.Name}\n";
            }
            return output;
        }

        public static Company Deserialize(string input) {
            Company comp = new Company();

            foreach (var entry in input.Split('\n')) {
                if (string.IsNullOrWhiteSpace(entry)) {
                    continue;
                }

                List<string> values = entry.Split().Where(val => !string.IsNullOrWhiteSpace(val)).ToList();
                if (values.Count < 2) {
                    continue;
                }
                if (Int32.TryParse(values[0], out int salary)) {
                    string name = string.Join(" ", values.Skip(1));
                    comp.AddEmployee(new Employee(name, salary));
                }
                else {
                    Console.WriteLine("Varning: Ett fält hade ogiltigt värde, data kan ha gått förlorat.");
                }
            }

            return comp;
        }
    }
}
