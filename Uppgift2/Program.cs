using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uppgift2 {
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
                Console.WriteLine("Kunde inte hitta filen 'employees.txt'");
                return;
            }
            Console.WriteLine("Laddat.");
        }

        private static void SaveCompany() {
            File.WriteAllText("employees.txt", company.Serialize());
            Console.WriteLine("Sparat.");
        }

        private static void ModifyEmployee() {
            if (company.Employees.Count == 0) {
                Console.WriteLine("Det finns inga anställda att redigera.");
                return;
            }

            Console.WriteLine("Skriv in namnet på den anställda du vill redigera: \t(hela namnet behövs inte, lämna blankt för att avbryta)");
            Employee emp = PromptEmployee();
            if (emp == null) {
                return;
            }

            Console.WriteLine($"Skriv in ett nytt namn: ({emp.Name}) \t(Lämna blankt för att inte ändra namnet)");
            string newname = Prompt();
            if (!string.IsNullOrWhiteSpace(newname)) {
                emp.Name = newname;
            }

            Console.WriteLine($"Skriv in en ny lön: ({emp.Salary}kr) \t(Endast siffror, -1 eller blankt för att avbryta)");
            int newsalary = PromptInt();
            if (newsalary < 0) {
                return;
            }
            emp.Salary = newsalary;
        }

        private static void RemoveEmployee() {
            Console.WriteLine("Skriv in namnet på den anställda du vill ta bort: \t (hela namnet behövs inte, lämna blankt för att avbryta)");
            Employee employee = PromptEmployee();
            if (employee == null) {
                return;
            }
            
            Console.WriteLine($"Är du säker på att du vill ta bort {employee.Name}? \t(ja/nej)");
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
                    Console.WriteLine("Ja eller nej?");
                }
            }
        }

        private static void AddEmployee() {
            Console.WriteLine("Skriv in namnet på den anställda: \t(lämna blankt för att avbryta)");
            string name = Prompt().Trim();
            if (string.IsNullOrWhiteSpace(name)) {
                return;
            }

            Console.WriteLine("Skriv in lön: \t(endast siffror, -1 eller blankt för att avbryta)");
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
                Console.WriteLine("Inga anställda.");
                return;
            }

            string output = "";
            foreach (var emp in employees) {
                total += emp.Salary;
                output += $"{emp.Name}:\t{emp.Salary}kr ({emp.SalaryLevel})\n";
            }
            Console.WriteLine($"-- Totallön: {total}kr -- Medellön: {total / employees.Count}kr --");
            Console.WriteLine(output);
        }

        public static int Menu() {
            Console.WriteLine("Huvudmeny");
            Console.WriteLine("-------------------");
            Console.WriteLine("1. list: Visa alla anställda");
            Console.WriteLine("2. add:  Lägg till en ny anställd");
            Console.WriteLine("3. rem:  Ta bort en anställd");
            Console.WriteLine("4. mod:  Ändra på en anställd");
            Console.WriteLine("5. save: Spara ändringar till fil");
            Console.WriteLine("6. load: Ladda anställda från fil");
            Console.WriteLine("0. quit: Avsluta programmet");
            while (true) {
                string input = Prompt();
                if (Int32.TryParse(input, out int result)) {
                    if (result >= 0 && result <= 6) {
                        return result;
                    }
                    else {
                        Console.WriteLine("Ogiltigt val.");
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
                    Console.WriteLine("Ogiltigt kommando.");
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
                    Console.WriteLine($"Hittade ingen anställd med namnet {input}.");
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
                    Console.WriteLine("Endast siffror tack.");
                }
            }
        }
    }
}
