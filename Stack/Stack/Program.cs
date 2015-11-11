using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Setup
            int i = 28;
            int j = 10;
            
            //MyStack<int> arr_int = new Stack.MyStack<int>(j);
            
            //arr_int.Push(i);
            //Console.WriteLine("stack length: " + arr_int.Count().ToString());
            //arr_int.Push(j);
            
            /*for (i = 1; i <= j; i++)
            {
                arr_int.Push(i);
                Console.WriteLine("top element: " + arr_int.Top().ToString());
            }
            Console.WriteLine("stack length: " + arr_int.Count().ToString());*/
            
            //[3].ToString());
            //Console.WriteLine("top element: " + arr_int.Pop().ToString());
            
            //arr_int.Push(i);
            //Console.WriteLine("stack length: " + arr_int.Count().ToString());
            //arr_int.Top();
            //Console.WriteLine("stack length: " + arr_int.Count().ToString());
            
            /*for (i = 10; i > 0; i--)
            {
                Console.WriteLine("top element: " + arr_int.Top().ToString());
                arr_int.Pop();

            }
            Console.WriteLine("top element: " + arr_int.Top().ToString());*/
            SqlConnection conn = new SqlConnection ();
            MyStack<SqlConnection> stack = Init<SqlConnection>(conn);
            #endregion

            Console.WriteLine("top element: " + stack.Top().ToString());
            
            Console.ReadLine();

        }
        public static MyStack<T> Init<T>(T obj) 
            where T : ICloneable
        {
            int j = 10;
            Stack.MyStack<T> test = new Stack.MyStack<T>(j);
            for (int k = 1; k <=j ; k++)
            {
                test.Push(default(T));
            }
            return test;
        }
        

    }

    class MyStack<T> //where T : ICloneable
    {
        int max;
        T[] elements;
        int cur;
        
        /*public void Stack()
        {
        }*/

        public MyStack(int capacity)
        {
            if (capacity > 0 && capacity <= 10)
            {
                max = capacity;
            }
            else
            {
                max = 10;
            }
            elements = new T[capacity];
            cur = 0;
        }

        public void Push(T element)
        {
            if (cur + 1 <= max)
            {
                elements.SetValue(element, cur); 
                cur++;
            }

        }

        public T Top()
        {
            T temp = default(T);
            if (cur > 0 && cur <= max)
            {
                //return elements[cur-1];
                temp = elements[cur - 1];
            }
            return temp;
        }

        public T Pop()
        {
            T temp = default(T);
            if (cur > 0 && cur <= max)
            {
                temp = elements[cur - 1];
                elements[cur-1] = default(T);
                cur--;
                //return element;
            }
            return temp;
        }

        public int Count()
        {
            return cur;
        }

        /*public object Clone()
        {
            MyStack<T> t = new MyStack<T>(this.max);
            for (int k = 1; k <=this.max ; k++)
            {
                t.Push(this.elements[k-1]);
            }
            return t;
        }*/

    }

}
