using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Setup
            Double i = 28.5;
            Double j = 10.7;


            #region Hell is made of comments
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
            //#endregion

            //SqlConnection conn = new SqlConnection ();
            //MyStack<SqlConnection> stack = Init<SqlConnection>(conn);
            #endregion

            MyRect rect = new MyRect(i, j);
            Console.WriteLine("base circle area: " + rect.area.ToString());
            MyStack<MyRect> stack = Init<MyRect>(rect);

            MyStack<MyRect> stack_new = new MyStack<MyRect>(10);
            stack_new.Push(rect);
            MyRect test_rect = stack_new.Top();
            
            if (test_rect.CompareTo(rect) == 1)
            {
                Console.WriteLine("rectangles are identical");
            }
            else
            {
                Console.WriteLine("rectangles are NOT identical");
            }

            #endregion


            Console.WriteLine("top stack element area: " + stack_new.Top().area.ToString());
            Assert.AreEqual(test_rect.area, rect.area);
            Console.WriteLine("stack size after push and top: " + stack_new.Count().ToString());
            MyRect test2_rect = stack_new.Pop();
            //Console.WriteLine("top stack element area: " + stack_new.Top().area.ToString());
            Assert.AreEqual(test2_rect.area, rect.area);
            Console.WriteLine("stack size after pop: " + stack_new.Count().ToString());

            #region exit
            Console.WriteLine("Press 'Enter' to exit app");
            Console.ReadLine();
            #endregion
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

    class MyStack<T> where T : ICloneable
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
            T result = default(T);
            if (temp != null)
            {
                result = (T)temp.Clone();
            }
            return result;
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

            T result = default(T);
            if (temp != null)
            {
                result = (T)temp.Clone();
            }
            return result;
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

    class MyRect: ICloneable, IComparable
    {
        public Double x;
        public Double y;
        public Double area;

        public MyRect(Double x, Double y)
        {
            this.x = x;
            this.y = y;
            this.area = x * y;
        }

        public object Clone()
        {
            MyRect temp = new MyRect(this.x, this.y);
                //throw new NotImplementedException();
            return temp;

        }

        public int CompareTo(object obj)
        {
            int result = 0;
            MyRect temp = (MyRect)obj;
            //throw new NotImplementedException();
            if (this.x == temp.x && this.y == temp.y && this.area == temp.area)
            {
                result = 1;
            }
            return result;
        }
    }


}
