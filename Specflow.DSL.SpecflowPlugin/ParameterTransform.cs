using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Specflow.DSL
{
    public interface IParameterTransform
    {
        string Transform(string param, ScenarioContext context);
        IParameterTransform addTransformer(Func<string, string> transform);
    }

    public class ParameterTransform : IParameterTransform
    {
        List<Func<string, string>> _additonalTransformers = new List<Func<string, string>>();
   
        public IParameterTransform addTransformer(Func<string, string> transform)
        {
            _additonalTransformers.Add(transform);
            return this;
        }
        public virtual string Transform(string str, ScenarioContext context)
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
                        if (result.Length == 0) result.Append(TransformText(line, context));
                        else result.Append(Environment.NewLine + TransformText(line, context));
                    }

                } while (line != null);
            }

            return result.ToString();
        }


        protected virtual string TransformPattern(string pattern, ScenarioContext context)
        {
            // supports [[key=value]] assignment
            var isAssignment = Regex.Match(pattern, @"(.*)=(.*)");
            if (isAssignment.Success)
            {
                // bottom up travese
                var cxtValue = TransformText(isAssignment.Groups[2].Value.Trim(), context);

                // apply user filter
                foreach (var t in _additonalTransformers)
                    cxtValue = t.Invoke(cxtValue);

                // apply RegEx
                var regExM = Regex.Match(cxtValue, @"RegEx\((.*)\)", RegexOptions.IgnoreCase);
                if (regExM.Success)
                {
                    cxtValue = new Fare.Xeger(regExM.Groups[1].Value).Generate();
                }

                var cxtKey = TransformText(isAssignment.Groups[1].Value.Trim(), context);
                context[cxtKey] = cxtValue;

                Console.WriteLine(string.Format("[[{0}={1}]]", cxtKey, cxtValue));
                return cxtValue;
            }
            else
            {
                // read value from context
                try
                {
                    var cxtValue = context[pattern] as string;
                    Console.WriteLine(string.Format("[[{0}={1}]]", pattern, cxtValue));
                    return cxtValue;
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException("can't find key:" + pattern + " in scenario context");
                }
            }
        }
        protected virtual string TransformText(string str, ScenarioContext context)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var match = PatternMatch.Parse(str);

            return match == null ? str
                : TransformText(match.ReplaceMatched(TransformPattern(match.MatchedPattern, context)), context);
        }

        class PatternMatch
        {
            string Prefix { get; set; }
            public string MatchedPattern { get; set; }
            string Postfix { get; set; }

            public static PatternMatch Parse(string strToMatch)
            {
                if (strToMatch.IndexOf("[[") < 0 || strToMatch.IndexOf("]]") < 0) return null;


                // regroup
                var startPattern = strToMatch.IndexOf("[[") + 2;
                int endPattern = startPattern;
                var i = startPattern;
                int nested = 0;
                while (i++ < strToMatch.Length)
                {
                    if (strToMatch[i] == '[' && strToMatch[i - 1] == '[')
                    {
                        i++;
                        nested++;
                    }
                    else if (strToMatch[i] == ']' && strToMatch[i - 1] == ']')
                    {
                        if (nested == 0)
                        {
                            endPattern = i - 2;
                            break;
                        }
                        else
                        {
                            i++;
                            nested--;
                        }
                    }
                }

                return new PatternMatch()
                {
                    Prefix = strToMatch.Substring(0, startPattern - 2),
                    MatchedPattern = strToMatch.Substring(startPattern, endPattern - startPattern + 1),
                    Postfix = strToMatch.Substring(endPattern + 3),
                };
            }

            public string ReplaceMatched(string replace)
            {
                return Prefix + replace + Postfix;
            }
        }

    }
}
