using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using CS = Microsoft.CodeAnalysis.CSharp.Scripting;
using CodeAnalysis.Model;

namespace RoslynTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var result=GetPersonData();
            var condition = CodeAnalysis.Roslyn.ExecuteBeforeScript("B");
            result=result.Where(d => d.Name.Contains(condition)).ToList();

            result = CodeAnalysis.Roslyn.ExecuteAfterScript(result);
            foreach (Person p in result)
            {
                Console.WriteLine($"Name:{p.Name},Age:{p.Age},Gender:{p.Gender}");
            }
            Console.ReadKey();
        }

        private static List<Person> GetPersonData()
        {
            List<Person> list = new List<Person>();
            for (int i=1;i<=100;i++)
            {
                Person p = new Person();
                p.Age = i;
                if (i % 2 == 0)
                {
                    p.Name = "A " + i;
                    p.Gender = 'm';
                }
                else
                {
                    p.Name = "B " + i;
                    p.Gender = 'f';
                }
                list.Add(p);
            }
            return list;
        }

        
    }
}
