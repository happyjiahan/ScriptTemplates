//
//  CustomScriptTemplate.cs
//  ScriptTemplates
//
//  Created by lijiahan on 03/20/2016
//  Copyright © 2016 CodingCoder.com. All rights reserved.
//


using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace CodingCoder.ScriptTemplate
{
    public class CustomScriptTemplate : UnityEditor.AssetModificationProcessor
    {
        public const string RootFolderName = "ScriptTemplates";
        public const string ScriptTemplatesFolderName = "_ScriptTemplates";

        public static void OnWillCreateAsset(string pathString)
        {
            /*
             * For scripts files, this *pathString* always be the .meta file path.  
             * we need to get the script file name.
             */ 
            pathString = pathString.Replace(".meta", "");
            string scriptName = Path.GetFileNameWithoutExtension(pathString);
            string scriptExtension = Path.GetExtension(pathString);
            string developerName = System.Environment.UserName;

            if (scriptExtension != ".cs" && scriptExtension != ".js")
            {
                return;
            }

            string templateFileName = null;
            switch (scriptExtension)
            {
                case ".js":
                    {
                        templateFileName = "Javascript-NewBehaviourScript.js.txt";
                        break;
                    }


                case ".cs":
                default:
                    {
                        templateFileName = "C#-NewBehaviour-ScriptTemplate.cs.txt";
                        break;
                    }
            }

            string[] rootFolders = Directory.GetDirectories("Assets", RootFolderName);
            if (rootFolders == null || rootFolders.Length == 0)
            {
                Debug.LogError("Not found Folder:" + RootFolderName);
                return;
            }

            string rootFolderPath = rootFolders[0];
            string[] subFolders = Directory.GetDirectories(rootFolderPath, ScriptTemplatesFolderName);
            if (subFolders == null || subFolders.Length == 0)
            {
                Debug.LogError("Not found Folder:" + ScriptTemplatesFolderName);
                return;
            }

            string scriptTemplateFolderPath = subFolders[0];

            string templateFilePath = scriptTemplateFolderPath + "/" + templateFileName;
            string fileContent = System.IO.File.ReadAllText(templateFilePath);

            fileContent = fileContent.Replace("#SCRIPT_NAME#", scriptName);
            fileContent = fileContent.Replace("#SCRIPT_EXT#", scriptExtension);
            fileContent = fileContent.Replace("#DEVELOPER_NAME#", developerName);
            fileContent = fileContent.Replace("#CREATION_DATE#", System.DateTime.Now.ToString("d"));
            fileContent = fileContent.Replace("#PROJECT_NAME#", PlayerSettings.productName);
            fileContent = fileContent.Replace("#YEAR_DATE#", System.DateTime.Now.ToString("yyyy"));
            fileContent = fileContent.Replace("#COPYRIGHT_HOST#", PlayerSettings.companyName);

            System.IO.File.WriteAllText(pathString, fileContent);
            AssetDatabase.Refresh();
        }
    }

}