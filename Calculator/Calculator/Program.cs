using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic

namespace Calculator
{
    
    class Calculator
    {

        private delegate double OperationDelegate(double x, double y);
        //private Dictionary<string, OperationDelegate> _operations;

        private Dictionary<string, Func<double, double, double>> _operations;


        static void Main(string[] args)
        {
            
           // ops = new Dictionary<string, string>;
            
            Console.WriteLine("Please enter 1st argument of expression");
            double x = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("Please enter sign of expression");
            string op = Console.ReadLine();
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

//single line comment, I'll leave one more there

           switch (op)
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

            
            Calculator calc = new Calculator();
            /*
            Console.WriteLine("Please enter 1st argument of expression");
            double x1 = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("Please enter sign of expression");
            string op1 = Console.ReadLine();
            Console.WriteLine("Please enter 2nd argument of expression");
            double y1 = Convert.ToSingle(Console.ReadLine());
            */
            calc.Calculator1();
            rest = calc.PerformOperation(op, x, y);

            var mod = calc.PerformOperation("/", 3.0, 2.0);
            Assert.AreEqual(1.5, mod);

            mod = calc.PerformOperation("*", 3.0, 2.5);
            Assert.AreEqual(7.5, mod);

            mod = calc.PerformOperation("-", 3.7, 2.1);
            Assert.AreEqual(1.6, mod);

            mod = calc.PerformOperation("+", 3.0, 2.0);
            Assert.AreEqual(5.0, mod);

            //rest = PerformOperation(op, x, y);

            Console.WriteLine("Result: " + rest.ToString());
            //Calculator
            Console.ReadLine();
            
            calc.DefineOperation("mod", (x1, y1) => x1 % y1);
            mod = calc.PerformOperation("mod", 3.0, 2.0);
            Assert.AreEqual(1.0, mod);

            Console.ReadLine();

        }

    public void Calculator1()
    {
	    _operations =
		    /*new Dictionary<string, OperationDelegate>
		    {
			    { "+", this.DoAddition },
			    { "-", this.DoSubtraction },
			    { "*", this.DoMultiplication },
			    { "/", this.DoDivision },
		    };*/
            
            new Dictionary<string, Func<double, double, double>>
	        {
		        { "+", (x, y) => x + y },
		        { "-", (x, y) => x - y },
		        { "*", this.DoMultiplication },
		        { "/", this.DoDivision },
	        };

    }

    public double PerformOperation(string op, double x, double y)
    {
	    if (!_operations.ContainsKey(op))
		    throw new ArgumentException(string.Format("Operation {0} is invalid", op), "op");
	    return _operations[op](x, y);
    }
    private double DoDivision(double x, double y) { return x / y; }
    private double DoMultiplication(double x, double y) { return x * y; }
    //private double DoSubtraction(double x, double y) { return x - y; }
    //private double DoAddition(double x, double y) { return x + y; }
    
    public void DefineOperation(string op, Func<double, double, double> body)
    {
        if (_operations.ContainsKey(op))
            throw new ArgumentException(string.Format("Operation {0} already exists", op), "op");
        _operations.Add(op, body);
    }


    }
}