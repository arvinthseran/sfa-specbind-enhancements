namespace ConfigTransformTool
{
    using System;
    using System.IO;
    using System.Reflection;

    using Microsoft.Web.XmlTransform;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Apply XML Config Transform");
            Console.WriteLine($"    Usage: {GetExeName()} <source file> <transform file> <output file>");
            if (args.Length >= 3)
            {
                Console.WriteLine();
                TransformConfig(args[0], args[1], args[2]);
            }
            Console.WriteLine();
        }

        public static void TransformConfig(string configFileName, string transformFileName, string targetFileName)
        {
            var document = new XmlTransformableDocument();
            document.PreserveWhitespace = true;
            document.Load(configFileName);

            var transformation = new XmlTransformation(transformFileName);
            if (!transformation.Apply(document))
            {
                throw new Exception($"An error occurred applying the transform {transformFileName}");
            }
            
            document.Save(targetFileName);
            Console.WriteLine($" Saved {targetFileName}");
        }

        public static string GetExeName()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            return Path.GetFileName(codeBase);
        }
    }
}