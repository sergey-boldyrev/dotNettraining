using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter FullName for an assembly to be used for Exception type hierarchy building, e.g.");
            Console.WriteLine("'system, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' or ");
            Console.WriteLine("'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'");
            string FullName = Console.ReadLine();
            
            //string FullName = "system, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            Assembly info = Assembly.Load(FullName);


            //System.Reflection.Assembly info = typeof(Exception).Assembly;
            Console.WriteLine(info.FullName);

            Type[] types = info.GetTypes();
            List<string> typesHier;// = new Type[0];
            foreach (Type type in types)
            {
                typesHier = new List<string>(0);
                for (var current = type; current != null; current = current.BaseType)
                {
                    
                    typesHier.Add(current.Name);
                    if (current == typeof(Exception))
                    {
                        string line = "";
                        for (int i = typesHier.Count - 1; i != 0; i--)
                        {
                            Console.WriteLine(line + typesHier[i]);
                            line += "======";
                        }
                        Console.WriteLine(line + type.Name);
                        break;
                    }
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}