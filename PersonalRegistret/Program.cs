using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PersonalRegistret {
    class Program {
        static Company company = new Company();
        static bool isRunning = true;

        static MenuItem[] menu = {
            new MenuItem("exit", "Exit the program", Exit),
            new MenuItem("list", "List all employees", ListEmployees),
            new MenuItem("add", "Add an employee", AddEmployee),
            new MenuItem("rem", "Remove an employee", RemoveEmployee),
            new MenuItem("mod", "Modify an employee", ModifyEmployee),
            new MenuItem("save", "Save the registry to file", SaveCompany),
            new MenuItem("load", "Load the registry from file", LoadCompany),
        };

        static void Main(string[] args) {
            isRunning = true;
            while (isRunning) {
                int sel = Menu();
                if (sel < menu.Length) {
                    menu[sel].Callback();
                }
                else {
                    Console.WriteLine("Error????");
                }
                Console.WriteLine();
            }
        }

        private static void Exit() {
            isRunning = false;
        }

        private static void LoadCompany() {
            try {
                company = Company.Deserialize(File.ReadAllText("employees.txt"));
            }
            catch (FileNotFoundException) {
                Console.WriteLine("Could not find the file 'employees.txt'");
                return;
            }
            Console.WriteLine("Loaded 'employees.txt'.");
        }

        private static void SaveCompany() {
            File.WriteAllText("employees.txt", company.Serialize());
            Console.WriteLine("Saved to 'employees.txt'.");
        }

        private static void ModifyEmployee() {
            if (company.Employees.Count == 0) {
                Console.WriteLine("There are no employees to modify.");
                return;
            }

            Console.WriteLine("Enter the name of the employee to modify: \t(Whole name not needed, leave it empty to cancel)");
            Employee emp = PromptEmployee();
            if (emp == null) {
                return;
            }

            Console.WriteLine($"Enter a new name: ({emp.Name}) \t(leave it empty to not modify the name)");
            string newname = Prompt();
            if (!string.IsNullOrWhiteSpace(newname)) {
                emp.Name = newname;
            }

            Console.WriteLine($"Enter  a new salary: ({emp.Salary}) \t(only numbers, enter -1 or leave it empty to cancel)");
            int newsalary = PromptInt();
            if (newsalary < 0) {
                return;
            }
            emp.Salary = newsalary;
        }

        private static void RemoveEmployee() {
            Console.WriteLine("Enter the name of the employee you want to remove: \t (whole name not needed, leave it empty to cancel)");
            Employee employee = PromptEmployee();
            if (employee == null) {
                return;
            }
            
            Console.WriteLine($"Are you sure you want to remove {employee.Name}? \t(yes/no)");
            while (true) {
                string input = Prompt().ToLower().Trim();
                if (input == "ja" || input == "j" || input == "yes" || input == "y") {
                    company.RemoveEmployee(employee);
                    return;
                }
                else if (input == "nej" || input == "n" || input == "no") {
                    return;
                }
                else {
                    Console.WriteLine("Yes or No?");
                }
            }
        }

        private static void AddEmployee() {
            Console.WriteLine("Enter name: \t(leave it empty to cancel)");
            string name = Prompt().Trim();
            if (string.IsNullOrWhiteSpace(name)) {
                return;
            }

            Console.WriteLine("Enter salary: \t(only numbers, enter -1 or leave it empty to cancel)");
            int salary = PromptInt();
            if (salary < 0) {
                return;
            }
            

            company.AddEmployee(new Employee(name, salary));
        }

        private static void ListEmployees() {
            int total = 0;
            var employees = company.Employees;
            if (employees.Count == 0) {
                Console.WriteLine("No employees to list.");
                return;
            }

            string output = "";
            foreach (var emp in employees) {
                total += emp.Salary;
                output += $"{emp.Name}:\t{emp.Salary}kr ({emp.SalaryLevel})\n";
            }
            Console.WriteLine($"-- Total salary: {total} -- Average salary: {total / employees.Count} --");
            Console.WriteLine(output);
        }

        public static int Menu() {
            Console.WriteLine("------ Main Menu ------");
            for (int i = 0; i < menu.Length; i++) {
                Console.WriteLine($"{i}. {menu[i].Command}: {menu[i].Description}");
            }
            while (true) {
                string input = Prompt();
                if (Int32.TryParse(input, out int result)) {
                    if (result >= 0 && result <= menu.Length) {
                        return result;
                    }
                    else {
                        Console.WriteLine("Invalid choice.");
                    }
                }
                else {
                    for (int i = 0; i < menu.Length; i++) {
                        if (menu[i].Command == input) {
                            return i;
                        }
                    }
                }
                
            }
        }

        public static string Prompt() {
            Console.Write("> ");
            return Console.ReadLine().Trim();
        }

        public static Employee PromptEmployee() {
            while (true) {
                string input = Prompt().ToLower().Trim();
                if (string.IsNullOrWhiteSpace(input)) {
                    return null;
                }

                try {
                    return company.Employees.First(e => e.Name.ToLower().Contains(input));
                }
                catch (InvalidOperationException) {
                    Console.WriteLine($"Could not find an employee with the name {input}.");
                    return null;
                }
            }
        }

        public static int PromptInt() {
            while (true) {
                string input = Prompt();
                if (string.IsNullOrWhiteSpace(input)) {
                    return -1;
                }
                if (Int32.TryParse(input, out int result)) {
                    return result;
                }
                else {
                    Console.WriteLine("Only numbers please.");
                }
            }
        }
    }
}
