using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 28;
            int j = 23;
            MyStack<int> arr_int = new Stack.MyStack<int>(j);
            arr_int.Push(i);
            //arr_int.Push(j);
            for (i = 2; i <= j; i++)
            {
                arr_int.Push(i);
            }
            Console.WriteLine("stack length: " + arr_int.Count().ToString());
                //[3].ToString());
            arr_int.Pop();
            Console.WriteLine("stack length: " + arr_int.Count().ToString());
            Console.ReadLine();


        }
    }

    class MyStack<T>
    {
        int max;
        T[] elements;
        int cur;
        
        /*public void Stack()
        {
        }*/

        public MyStack(int capacity)
        {
            max = capacity;
            elements = new T[capacity];
            cur = 0;
        }

        public void Push(T element)
        {
            if (cur + 1 <= max)
            {
                //elements[cur] = element;
                elements.SetValue(element, cur); 
                cur++;
            }

        }

        public T Pop()
        {
            return elements[cur];
        }

        public T Top()
        {
            T temp = default(T);
            if (cur >= 0)
            {
                T element = elements[cur];
                cur--;
                return element;
            }
            return temp;
        }

        public int Count()
        {
            return elements.Length;
        }


    }

}
