///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 10/02/2020 10:31
///-----------------------------------------------------------------

using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace Com.Pageriethibault.Assets.Utils.Scripts
{
    public static class GenerateEnum
    {
#if UNITY_EDITOR
        /// <summary>
        /// Hold enum values for génération.
        /// </summary>
        public struct GeneratedEnum
        {
            public string enumName;
            public List<string> enumEntries;
            public bool needDefaultEnum;
            public GeneratedEnum(string name, List<string> entries, bool needDefault)
            {
                enumName = name;
                enumEntries = entries;
                needDefaultEnum = needDefault;
            }
        }

        /// <summary>
        /// Write an enum in setted path.
        /// </summary>
        /// <param name="enumName">name of the enum.</param>
        /// <param name="enumEntries">List of enums.</param>
        /// <param name="path">directory path.</param>
        /// <param name="enumNamespace">namespace for the enum.</param>
        /// <param name="refreshAssetDatabase">compile after created the enum if true.</param>
        /// <param name="needDefaultEnum">add a enumName.NULL.</param>
        public static void Generate(string enumName, List<string> enumEntries, string path, string enumNamespace, bool refreshAssetDatabase = true, bool needDefaultEnum = false)
        {
            CheckForPresentFile(path);

            string filePathAndName = path + enumName + ".cs";

            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                if (needDefaultEnum) enumEntries.Add("DEFAULT");

                streamWriter.WriteLine("namespace " + enumNamespace);
                streamWriter.WriteLine("{");
                WriteEnumLines(streamWriter, enumName, enumEntries);
                streamWriter.WriteLine("}");
            }

            string databaseInfo = "database";
            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh();
                databaseInfo += " is";
            }
            else databaseInfo += " isn't";
            databaseInfo += " set as refresh.";


            Debug.Log(enumName + " enum as been written in " + path);
            Debug.LogWarning(databaseInfo);
        }

        /// <summary>
        /// Write an enum in setted path.
        /// </summary>
        /// <param name="generatedEnum">enum parameters.</param>
        /// <param name="path">directory path.</param>
        /// <param name="enumNamespace">namespace for the enum.</param>
        /// <param name="refreshAssetDatabase">compile after created the enum if true.</param>
        public static void Generate(GeneratedEnum generatedEnum, string path, string enumNamespace, bool refreshAssetDatabase = true)
        {
            Generate(generatedEnum.enumName, generatedEnum.enumEntries, path, enumNamespace, refreshAssetDatabase, generatedEnum.needDefaultEnum);
        }

        static private void WriteEnumLines(StreamWriter streamWriter, string enumName, List<string> enumEntries)
        {
            streamWriter.WriteLine("\tpublic enum " + enumName);
            streamWriter.WriteLine("\t{");

            string myEnum;
            for (int i = enumEntries.Count - 1; i >= 0; i--)
            {
                myEnum = CleanString(enumEntries[i]);
                if (myEnum == "") continue;
                if (i != 0) streamWriter.WriteLine("\t\t" + myEnum + ",");
                else streamWriter.WriteLine("\t\t" + myEnum);

            }
            streamWriter.WriteLine("\t}");
        }

        /// <summary>
        /// Generate a file with mutiple enums in it.
        /// </summary>
        /// <param name="generatedEnums">List of enums parameters.</param>
        /// <param name="fileName">name of the file.cs</param>
        /// <param name="path">directory path.</param>
        /// <param name="enumNamespace">namespace for the file.</param>
        /// <param name="refreshAssetDatabase">compile after created the file if true.</param>
        static public void GenerateMutipleEnum(List<GeneratedEnum> generatedEnums, string fileName, string path, string enumNamespace, bool refreshAssetDatabase = true)
        {
            CheckForPresentFile(path);

            string filePathAndName = path + fileName + ".cs";

            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("namespace " + enumNamespace);
                streamWriter.WriteLine("{");

                for (int i = generatedEnums.Count - 1; i >= 0; i--)
                {
                    GeneratedEnum generatedEnum = generatedEnums[i];
                    if (generatedEnum.needDefaultEnum) generatedEnum.enumEntries.Add("DEFAULT");
                    WriteEnumLines(streamWriter, generatedEnum.enumName, generatedEnum.enumEntries);
                    Debug.Log(generatedEnum.enumName + " enum as been written in " + filePathAndName);
                }
                streamWriter.WriteLine("}");
            }

            string databaseInfo = "database";
            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh();
                databaseInfo += " is";
            }
            else databaseInfo += " isn't";
            databaseInfo += " set as refresh.";

            Debug.Log(fileName + " enum as been written in " + path);
            Debug.LogWarning(databaseInfo);
        }

        /// <summary>
        /// Check if the directory exist, if not, create it.
        /// </summary>
        /// <param name="path">he directory path</param>
        private static void CheckForPresentFile(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogWarning("Directory at " + path + " doesn't exist and will be created.");
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Clean a string to set it as a enum value.
        /// </summary>
        /// <param name="element">string to clean.</param>
        /// <returns></returns>
#endif
        public static string CleanString(string element)
        {
            if (element.Contains(" "))
            {
                return element.Replace(" ", "").ToUpper();
            }
            return element.ToUpper();
        }
    }
}