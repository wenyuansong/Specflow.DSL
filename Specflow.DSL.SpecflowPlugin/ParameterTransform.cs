using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Specflow.DSL
{
    public interface IParameterTransform
    {
        string Transform(string obj, Func<string, string> additonalFilter = null);
        Table Transform(Table obj, Func<string, string> additonalFilter = null);
    }
    public class ParameterTransform : IParameterTransform
    {
        public virtual string Transform(string str, Func<string, string> additonalFilter = null)
        {
            if (string.IsNullOrEmpty(str)) return str;

            // deal with multiple line text
            StringBuilder result = new StringBuilder();
            using (StringReader reader = new StringReader(str))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (result.Length == 0) result.Append(TransformSingleLine(line, additonalFilter));
                        else result.Append(Environment.NewLine + TransformSingleLine(line, additonalFilter));
                    }

                } while (line != null);
            }
            
            return result.ToString();
        }

        public virtual Table Transform(Table table, Func<string, string> additonalFilter = null)
        {
            if (table == null) return table;

            foreach (var row in table.Rows)
            {
                foreach (var k in row.Keys)
                    row[k] = Transform(row[k], additonalFilter);
            }
            return table;
        }

        protected virtual string TransformSingleLine(string str, Func<string, string> additonalFilter = null)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var m = Regex.Match(str.Trim(), @"(.*)?\[\[(.*)?\]\](.*)", RegexOptions.Multiline);

            // match "[[.*]]"
            if (m.Success)
            {
                var value = m.Groups[2].Value.Trim();

                // supports [[key=value]] assignment
                var isAssignment = Regex.Match(value, @"(.*)=(.*)");
                if (isAssignment.Success)
                {
                    // convert value first
                    var cxtValue = Transform(isAssignment.Groups[2].Value.Trim(), additonalFilter);

                    // apply user filter
                    cxtValue = additonalFilter != null ? additonalFilter(cxtValue) : cxtValue;

                    var regExM = Regex.Match(cxtValue, @"RegEx\((.*)\)", RegexOptions.IgnoreCase);
                    if (regExM.Success)
                    {
                        cxtValue = new Fare.Xeger(regExM.Groups[1].Value).Generate();
                    }

                    var cxtKey = Transform(isAssignment.Groups[1].Value.Trim());
                    ScenarioContext.Current[cxtKey] = cxtValue;

                    Console.WriteLine(string.Format("[[{0}={1}]]", cxtKey, cxtValue));
                    return Transform(m.Groups[1].Value + cxtValue + m.Groups[3].Value);
                }
                else
                {
                    // read value from context
                    try
                    {
                        var cxtValue = ScenarioContext.Current[value] as string;
                        Console.WriteLine(string.Format("[[{0}={1}]]", value, cxtValue));
                        return Transform(m.Groups[1].Value + cxtValue + m.Groups[3].Value);
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new KeyNotFoundException("can't find key:" + value + " in scenario context");
                    }
                }
            }
            else
            {
                return str;
            }

        }
    }
}
