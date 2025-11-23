using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkovTextGenerator.UI;
using Spectre.Console;

namespace MarkovTextGenerator
{


    internal class Program
    {
        static void Main(string[] args)
        {
            var ui = new UserInterface();
            ui.Start();
        }
    }
}
