using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRegistret {
    public class MenuItem {
        public delegate void MenuDelegate();

        public string Command { get; private set; }
        public string Description { get; private set; }
        public MenuDelegate Callback { get; private set; }

        public MenuItem(string command, string desc, MenuDelegate callback) {
            Command = command;
            Description = desc;
            Callback = callback;
        }
    }
}
