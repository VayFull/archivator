using System.Linq;
using System.Reflection;

namespace Archivator.Core
{
    /// <summary>
    /// Используется только для тестов. Нужен для удобной работы с файлами. Здесь раасположены названия файлов с изначальным файлом для тестов,
    /// название файла для архивированной версии и тп.
    /// </summary>
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

            var projectPath = string.Join('\\', splitedAssemblyPath);

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
    }
}
