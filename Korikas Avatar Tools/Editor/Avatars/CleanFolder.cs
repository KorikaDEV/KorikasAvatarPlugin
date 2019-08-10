using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class CleanFolder
{
    static string path;

    ///<summery>
    ///It searches for the Folder of the avatarname and tries to put every .mat, .shader, .anim, .png and .jpg File into the right Folder
    ///<summery>
    public static void cleanFolder(string avatarname)
    {
        path = "Assets/KATAvatars/" + avatarname + "/";
        List<string> files = new List<string>();
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        foreach (FileInfo f in fileInfo)
        {
            if (!f.ToString().Contains(".meta"))
            {
                files.Add("Assets" + f.ToString().Split(new string[] { "Assets" }, StringSplitOptions.None)[1].Replace('\\', '/'));
            }
        }
        foreach (string s in files)
        {
            moveFileIfContains(s, ".anim", "Animations");
            moveFileIfContains(s, ".png", "Textures");
            moveFileIfContains(s, ".jpg", "Textures");
            moveFileIfContains(s, ".mat", "Materials");
            moveFileIfContains(s, ".shader", "Shaders");
        }
    }

    ///<summery>
    ///It moves a File if it contains the indicator to the destination
    ///<summery>
    public static void moveFileIfContains(string file, string indicator, string destination)
    {
        if (file.Contains(indicator))
        {
            string name = file.Split('/')[3];
            Debug.Log(name);
            AssetDatabase.MoveAsset(file, path + destination + "/" + name);
        }
    }
}