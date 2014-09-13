using System;
using System.IO;

namespace M3.Helpers
{
    public class DirectoryHelper
    {
        public static string GetSolutionDirectoryPath(string currentDirectoryPath)
        {
            var currentDirectory = new DirectoryInfo(currentDirectoryPath);
            var parent = currentDirectory;
            while (parent != null && parent.Name != "M3.Applications" && parent.Name != "M3.Website")
            {
                parent = parent.Parent;
            }
            if (parent != null)
            {
                return parent.Parent.FullName;
            }
            else
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
        }
    }
}
