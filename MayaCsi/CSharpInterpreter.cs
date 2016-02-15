using Autodesk.Maya.OpenMaya;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.IO;
using System.Reflection;

namespace MayaCsi
{
    class CSharpInterpreter
    {
        public static string MayaRootPath { get; set; }

        public CSharpInterpreter(string filepath)
        {
            using (var reader = new StreamReader(filepath))
            {
                Compile(filepath, reader.ReadToEnd());
            }
        }

        public void Execute(string[] args)
        {
            _script.RunAsync().Wait();
        }

        private void Compile(string filepath, string sourceCode)
        {
            var options = ScriptOptions.Default
                .WithReferences(Assembly.GetEntryAssembly())
                .WithReferences(typeof(object).Assembly)
                .WithReferences(Path.Combine(MayaRootPath, "bin", "openmayacs.dll"))
                .WithFilePath(filepath);

            _script = CSharpScript.Create(sourceCode, options);

            var diags = _script.Compile();
            if (diags.Length > 0)
            {
                foreach (var diag in diags)
                {
                    MGlobal.displayError(diag.ToString());
                }
                throw new InvalidProgramException();
            }
        }

        private Script _script;
    }
}
