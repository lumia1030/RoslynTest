using CodeAnalysis.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using CS = Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CodeAnalysis
{
    public class Roslyn
    {
        public static string ExecuteBeforeScript(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public class DataDeal");
            sb.Append("{");
            sb.Append(GetOrSetScript("../../../before.script"));
            sb.Append("}");
            sb.Append("return DataDeal.Before(arg1);");

            var result = CS.CSharpScript.RunAsync<string>(sb.ToString(), globals: new Arg { arg1 = name }, globalsType: typeof(Arg)).Result;
            return result.ReturnValue;
        }

        public static List<Person> ExecuteAfterScript(List<Person> personModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public class DataDeal");
            sb.Append("{");
            sb.Append(GetOrSetScript("../../../after.script"));
            sb.Append("}");
            sb.Append("return DataDeal.After(models);");

            var result = CS.CSharpScript.RunAsync<List<Person>>(sb.ToString(), ScriptOptions.Default.AddReferences(typeof(Roslyn).Assembly).AddImports("System.Collections.Generic", "System.Linq", "CodeAnalysis.Model"), globals: new Arg { models = personModel }, globalsType: typeof(Arg)).Result;
            return result.ReturnValue;
        }

        private static string GetOrSetScript(string file, string script = null)
        {
            //修改
            if (!string.IsNullOrEmpty(script))
            {
                if (!System.IO.File.Exists(file))
                    System.IO.File.Create(file);

                //false means overwrite the file
                using (StreamWriter writer = new StreamWriter(file, false))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine(script);
                }

                return script;
            }

            //查询
            if (System.IO.File.Exists(file))
                return System.IO.File.ReadAllText(file, Encoding.Default);

            return null;
        }
    }
}
