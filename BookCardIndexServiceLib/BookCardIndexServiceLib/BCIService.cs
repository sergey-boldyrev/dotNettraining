using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BookCardIndexServiceLib
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface BookCardIndexService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Добавьте здесь операции служб
    }

    // Используйте контракт данных, как показано на следующем примере, чтобы добавить сложные типы к сервисным операциям.
    // В проект можно добавлять XSD-файлы. После построения проекта вы можете напрямую использовать в нем определенные типы данных с пространством имен "BookCardIndexServiceLib.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
    public class MySQLType
    {
        bool boolValue = true;
        string stringValue = "Hello ";
        //SQLDataReader dr = new SQLDataReader();

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
    public class MyBook
    {
        public string name = default(string);
        public string[] authors = default(string[]);
        public int published = default(int);

        public MyBook()
        {
            this.name = default(string);
            this.authors = default(string[]);
            this.published = default(int);
        }

        public MyBook(string name, string[] authors, int published)
        {
            this.name = name;
            this.authors = authors;
            this.published = published;
        }

        [DataMember]
        public string Name
        {
            get { return Name; }
            set { Name = value; }
        }

        public static bool VerifyName(string name)
        {
            if (name != "" && name != " " && name.Trim() != "")
                return true;

            return false;
        }

        public static bool VerifyAuthors(string authors)
        {
            if (authors != "" && authors != " " && authors.Trim() != "")
                return true;

            return false;
        }
        public static bool VerifyYear(string year)
        {
            if (year != "" && year != " " && year.Trim() != "")
                return true;

            return false;
        }
    }


    public class MagicEightBallService : IEightBall
    {
        // Для отображения на хосте.
        public MagicEightBallService()
        {
            Console.WriteLine("The 8-Ball awaits your question...");
        }
        public Answer ObtainAnswerToQuestion(string userQuestion)
        {
            Answer[] answers = { new Answer(false, "Future Uncertain"), new Answer(true, "Yes"), new Answer(false, "No"), new Answer(false, "Hazy"), new Answer(false, "Ask again later"), new Answer(true, "Definitely") };
            // Вернуть случайный ответ.
            Random r = new Random();
            return answers[r.Next(answers.Length)];
        }
    }
}
