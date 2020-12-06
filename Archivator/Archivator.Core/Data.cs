using System;
using System.Linq;
using System.Reflection;

namespace Archivator.Core
{
    public static class Data
    {
        static Data()
        {
            var splitedAssemblyPath = Assembly
                .GetExecutingAssembly()
                .Location
                .Split('\\')
                .SkipLast(5)
                .ToArray();
            
            var projectPath = String.Join('\\', splitedAssemblyPath);

            Location = projectPath + "\\" + _dataFolderPath;
        }

        public static string Location;

        private const string _inputFile = "file.txt";

        private const string _archivedFile = "archived.txt";

        private const string _outputFile = "deCompressed.txt";
        
        private const string _dataFolderPath = @"Archivator.Core\Data\";

        public static string InputFilePath => Location + _inputFile;
        public static string ArchivedFilePath => Location + _archivedFile;
        public static string OutputFilePath => Location + _outputFile;
        //= Assembly
        //.GetExecutingAssembly()
        //.Location
        //.Replace("Archivator.dll", "");
    }
}
