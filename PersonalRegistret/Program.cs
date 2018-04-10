using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PersonalRegistret {
    class Program {
        static Company company = new Company();

        static void Main(string[] args) {
            bool running = true;
            while (running) {
                switch (Menu()) {
                    case 0: // Quit
                        running = false;
                        break;
                    case 1: // List
                        ListEmployees();
                        break;
                    case 2: // Add
                        AddEmployee();
                        break;
                    case 3: // Remove
                        RemoveEmployee();
                        break;
                    case 4: // Modify
                        ModifyEmployee();
                        break;
                    case 5: // Save
                        SaveCompany();
                        break;
                    case 6: // Load
                        LoadCompany();
                        break;
                    default: // Unreachable
                        Console.WriteLine("Error???");
                        break;
                }
                Console.WriteLine();
            }
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
            Console.WriteLine("1. list: List all employees");
            Console.WriteLine("2. add:  Add a new employee");
            Console.WriteLine("3. rem:  Remove an employee");
            Console.WriteLine("4. mod:  Modify an employee");
            Console.WriteLine("5. save: Save employees to file");
            Console.WriteLine("6. load: Load employees from file");
            Console.WriteLine("0. quit: Exit the program");
            while (true) {
                string input = Prompt();
                if (Int32.TryParse(input, out int result)) {
                    if (result >= 0 && result <= 6) {
                        return result;
                    }
                    else {
                        Console.WriteLine("Invalid choice.");
                    }
                }
                else if (input == "list") {
                    return 1;
                }
                else if (input == "add") {
                    return 2;
                }
                else if (input == "rem") {
                    return 3;
                }
                else if (input == "mod") {
                    return 4;
                }
                else if (input == "save") {
                    return 5;
                }
                else if (input == "load") {
                    return 6;
                }
                else if (input == "quit") {
                    return 0;
                }
                else {
                    Console.WriteLine("Invalid command.");
                }
            }
        }

        public static string Prompt() {
            Console.Write("> ");
            return Console.ReadLine();
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
