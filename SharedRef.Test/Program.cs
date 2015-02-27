using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SharedRef.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("GetReferenceList()");
            //new AopContext().Manager.GetReferenceList().ForEach(r => Console.WriteLine(r));

            var manager = new DependencyConteiner().Manager;

            var str = new DependencyConteiner().Manager.GetMatrixXml("tariffs", "tId=2", "cName,tName");
            Console.Write(str);
            Console.WriteLine();

            ParseXml(str);

            str = new DependencyConteiner().Manager.GetMatrixJson("tariffs", "cId=IN(1, 2);tId=1", "cName,tName");
            Console.Write(str);
            Console.WriteLine();

            str = new DependencyConteiner().Manager.GetMatrixJson("tariffs", "cId=IN(1, 2, 3);tId=1", "cName,tName");
            Console.Write(str);
            Console.WriteLine();

            str = new DependencyConteiner().Manager.GetMatrixJson("tariffs", "cId=1;tId=1", "tName");

            Console.Write(str);
            Console.WriteLine();

            Console.WriteLine("GetReferenceX(fias.Street)");
            str = new DependencyConteiner().Manager.GetValuesXml("fias.Street", "1B99DEE9-C980-414A-AA58-F153FED6A974", "", 400, "sName asc, fName desc");
            Console.Write(str);
            Console.WriteLine();

            //new DependencyConteiner().Clear();

            Console.WriteLine("GetReferenceX(fias.Street)");
            str = new DependencyConteiner().Manager.GetValuesXml("fias.Street", Guid.Empty.ToString(), "В", 2, "");
            Console.Write(str);
            Console.WriteLine();

            Console.WriteLine("GetReference(fias.Street)");
            var list = new DependencyConteiner().Manager.GetValues("fias.Street", "1B99DEE9-C980-414A-AA58-F153FED6A974", null, 2, "").ToList();
            list.ForEach(r => Console.WriteLine("{0} => [{1}]", r.Id, r));

            Console.WriteLine("GetReference(fias.Street)");
            list = new DependencyConteiner().Manager.GetValues("fias.Street", Guid.Empty.ToString(), null, 2, "").ToList();
            list.ForEach(r => Console.WriteLine("{0} => [{1}]", r.Id, r));

            Console.WriteLine("GetReference(fias.Street)");
            list = new DependencyConteiner().Manager.GetValues("fias.Street", Guid.Empty.ToString(), "", 10, "").ToList();
            list.ForEach(r => Console.WriteLine("{0} => [{1}]", r.Id, r));

            

            Console.WriteLine("GetReference(fias.Street)");
            list = new DependencyConteiner().Manager.GetValues("fias.Street", Guid.Empty.ToString(), "В", 10, "").ToList();
            list.ForEach(r => Console.WriteLine("{0} => [{1}]", r.Id, r));

            Console.WriteLine("GetReference(fias.Street)");
            list = new DependencyConteiner().Manager.GetValues("fias.Street", Guid.Empty.ToString(), "В", 10, "").ToList();
            list.ForEach(r => Console.WriteLine("{0} => [{1}]", r.Id, r));

            //new AopContext().Manager.Clear();

            Console.WriteLine("GetReferenceJ(fias.Street)");
            str = new DependencyConteiner().Manager.GetValuesJson("fias.Street", Guid.Empty.ToString(), "В", 10, "");
            Console.Write(str);
            Console.WriteLine();

            Console.WriteLine("GetReferenceX(fias.Street)");
            str = new DependencyConteiner().Manager.GetValuesXml("fias.Street", Guid.Empty.ToString(), "В", 10, "");
            Console.Write(str);
            Console.WriteLine();

            Console.WriteLine("GeByIdX(fias.Street)");
            str = new DependencyConteiner().Manager.GetByIdXml("fias.Street", "12f0ff9a-5e78-48ee-b02f-f5167e1aeb4d");
            Console.Write(str);
            Console.WriteLine();

            Console.WriteLine("GeByIdJ(fias.Street)");
            str = new DependencyConteiner().Manager.GetByIdJson("fias.Street", "12f0ff9a-5e78-48ee-b02f-f5167e1aeb4d");
            Console.Write(str);
            Console.WriteLine();

            

            Console.ReadLine();
        }

        private static void ParseXml(string str)
        {
            var list = new List<Dictionary<string, string>>();

            var doc = XDocument.Parse(str);

            foreach (var item in doc.Root.Elements("items").Elements())
	        {
                var dict = new Dictionary<string, string>();

                foreach (var attr in item.Attributes())
                {
                    dict.Add(attr.Name.LocalName, attr.Value);
                }

                list.Add(dict);                
	        }

            
        }
    }
}
