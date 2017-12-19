// ComputeIncludeParser.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.
using System.IO;
using System.Text;

#if UNITY_EDITOR
namespace Thinksquirrel.Fluvio.Internal
{
    class ComputeIncludeParser : FluvioComputeShader.IComputeIncludeParser
    {        
        public void ParseIncludePathsRecursive(string source, string path, StringBuilder sb)
        {
            using (var stringReader = new StringReader(source))
            {
                var line = stringReader.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("#include "))
                    {
                        var include = line.Substring(10, line.Length - 11);
                        var includePath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path)), include);
                        if (File.Exists(includePath))
                        {
                            var includeSrc = string.Empty;
                            using (var file = File.OpenText(includePath))
                            {
                                includeSrc = file.ReadToEnd();
                            }                                
                            includeSrc = FluvioComputeShader.RemoveComments(includeSrc);
                            includeSrc = FluvioComputeShader.Minify(includeSrc);
                            ParseIncludePathsRecursive(includeSrc, includePath, sb);
                        }
                        else
                        {
                            sb.AppendLine(string.Format("#error \"Could not find include - {0}\"", includePath));
                        }
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                    line = stringReader.ReadLine();
                }
            }
        }
    }
}
#endif