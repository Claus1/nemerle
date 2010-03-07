﻿using System;
using System.IO;
using System.Linq;
using Test.CodeGeneration.Verification;
using Microsoft.Cci;

namespace Test.CodeGeneration
{
    class Program
    {
        static void Main()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "codegen");
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);

            Directory.CreateDirectory(tempFolder);

            Run(
                MutuallyRecursiveTypes(tempFolder), 
                new CCI.MutuallyRecursiveTypes(), 
                new Cecil.MutuallyRecursiveTypes(), 
                new SRE.MutuallyRecursiveTypes()
                );
        }

        private static void Run(TestCase testCase, ICodeGenerator cci, ICodeGenerator cecil, ICodeGenerator sre)
        {
            Console.WriteLine("===Starting {0}", testCase.Description);
            Console.WriteLine("     CCI");
            testCase.Run(cci);
            Console.WriteLine("     Cecil");
            testCase.Run(cecil);
            Console.WriteLine("     SRE");
            testCase.Run(sre);
            Console.WriteLine("===Completed");
        }

        #region TestCases

        static TestCase MutuallyRecursiveTypes(string tempFolder)
        {
            /* 
             * class X<T> {}
             * class A<X<B>> {}
             * class B<X<A>> {}
             */

            Func<string, Action<ITypeDefinition>> getChecker =
                n => t =>
                         {
                             var baseType = (IGenericTypeInstance)t.BaseClasses.Single().ResolvedType;
                             Assert.Equals(baseType.GenericType.ResolvedType.GenericParameters.Single().Name, "T");
                         };
            return new TestCase(
                "MutuallyRecursiveTypes",
                s => Assert.Assembly(s)
                         .Type("X", 1, t => Assert.True(t.IsGeneric && t.GenericParameterCount == 1)).Complete()
                         .Type("A", t => getChecker("B")(t)).Complete()
                         .Type("B", t => getChecker("A")(t)).Complete(), 
                tempFolder);
        }

        #endregion
    }
}
