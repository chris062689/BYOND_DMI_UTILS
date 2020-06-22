using DMISharp;
using FS.Utilities.Application;
using System;
using System.IO;

namespace FS.Utilities.TurfGenerator
{
    class Program
    {
        /// <summary>
        /// Folder path where the DMI files are located.
        /// </summary>
        private const string FolderPath = @"";

        static void Main(string[] args)
        {
            string turfDm = @"/turf/";
            foreach (var filePath in Directory.EnumerateFiles(FolderPath, "*.dmi", SearchOption.TopDirectoryOnly))
            {
                var basePath = Path.GetFileNameWithoutExtension(filePath);

                using (var newDMI = new DMIFile(filePath))
                {
                    turfDm += string.Empty + Environment.NewLine;
                    turfDm += $@"
/turf/{basePath.ckey()}/
    icon = '{Path.GetFileName(filePath)}'";

                    foreach (var state in newDMI.States)
                    {
                        var stateName = state.Name.ckey();
                        if (string.IsNullOrEmpty(state.Name) == false)
                        {
                            turfDm +=
$@"
    {stateName}/
        icon_state = ""{state.Name}""
        density = 0";
                        }
                    }
                }
            }

            File.WriteAllText(FolderPath + "turf.dm", turfDm);
        }
    }
}
