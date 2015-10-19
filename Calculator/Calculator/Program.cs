using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
           Console.WriteLine("Please enter 1st argument of expression");
            double x = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("Please enter sign of expression");
            string sign = Console.ReadLine();
            Console.WriteLine("Please enter 2nd argument of expression");
            double y = Convert.ToSingle(Console.ReadLine());
            
            double rest = 0.0005; 
/*            int caseSwitch = 2;
            switch (caseSwitch)
            {
                case 1:
                    Console.WriteLine("Case 1");
                    break;
                case 2:
                    Console.WriteLine("Case 2");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }*/

//single line comment
           switch (sign)
            {
                case "+": 
                   rest = x + y;
                   break;
                case "-": 
                   rest = x - y;
                   break;
                case "*": 
                   rest = x * y;
                   break;
                case "/": 
                   rest = x / y;
                   break;
               default:
                   Console.WriteLine("Invalid sign, please choose another one on next run");
                   break;
            }
            Console.WriteLine("Result: " + rest.ToString());
            Console.ReadLine();
        }
    }
}
