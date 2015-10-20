using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exception;
//using System.Collections.Generic


    private delegate double OperationDelegate(double x, double y);
    private Dictionary<string, OperationDelegate> _operations;
namespace Calculator
{


    
    class Program
    {
        static void Main(string[] args)
        {
            
           // ops = new Dictionary<string, string>;
            
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

//single line comment, I'll leave one more there

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
    /*class Calculate
    {
        double PerformOperation()
        { 
        
        }
        
        void AddOperation()
        {
            

        }
    }*/

    public Calculator()
    {
	    _operations =
		    new Dictionary<string, OperationDelegate>
		    {
			    { "+", this.DoAddition },
			    { "-", this.DoSubtraction },
			    { "*", this.DoMultiplication },
			    { "/", this.DoDivision },
		    }
    }

public double PerformOperation(string op, double x, double y)
    {
	    if (!_operations.ContainsKey(op))
		    throw new ArgumentException(string.Format("Operation {0} is invalid", op), "op");
	    return _operations[op](x, y);
    }
private double DoDivision(double x, double y) { return x / y; }
private double DoMultiplication(double x, double y) { return x * y; }
private double DoSubtraction(double x, double y) { return x - y; }
private double DoAddition(double x, double y) { return x + y; }


}
