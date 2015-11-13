using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rectangle
{
    class Program
    {
        static void Main(string[] args)
        {
            //var objectGraph = new List<String> {"Jeff", "Kristin", "Aidan", "Grant"};
            var objectGraph = new MyRect(25.0, 10.0);
            Double test_area = objectGraph.r_area;
            SerializeBinaryFormat(objectGraph, "serialized.dat");
            objectGraph = null;
            // Десериализация объектов и проверка их работоспособности
            objectGraph = (MyRect)DeserializeBinaryFormat("serialized.dat");
            //foreach (var s in objectGraph)
            Console.WriteLine(objectGraph.r_area.ToString());
            Assert.AreEqual(test_area, objectGraph.r_area);
            Console.ReadLine();

        }
        private static void SerializeBinaryFormat(Object objectGraph, string fileName)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // Заставляем модуль форматирования сериализовать 
                // объекты в поток ввода-вывода
                formatter.Serialize(fStream, objectGraph);
            }
        }
        private static Object DeserializeBinaryFormat(string fileName)
        {
            // Задание форматирования при сериализации
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                // Заставляем модуль форматирования десериализовать 
                // объекты из потока ввода-вывода
                return formatter.Deserialize(fStream);
            }
        }

    }

    [Serializable]
    class MyRect
    {
        private Double r_x;
        private Double r_y;
        [NonSerialized]
        public Double r_area;

        public MyRect(Double x, Double y)
        {
            r_x = x;
            r_y = y;
            r_area = r_x * r_y;
        }
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            r_area = r_x * r_y;
        }
    }

}
