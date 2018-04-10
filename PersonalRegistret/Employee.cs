using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRegistret {
    class Employee {
        public string Name { get; set; }

        int salary;
        public int Salary {
            get {
                return salary;
            }
            set {
                if (salary >= 0) {
                    salary = value;
                }
            }
        }

        public string SalaryLevel {
            get {
                if (salary < 25000) {
                    return "Låg";
                }
                else if (salary < 35000) {
                    return "Medel";
                }
                else {
                    return "Hög";
                }
            }
        }

        public Employee(string name, int salary) {
            Name = name;
            Salary = salary;
        }
    }
}
