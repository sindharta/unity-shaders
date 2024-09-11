using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Shin.Core {

public static class FileUtility {

//---------------------------------------------------------------------------------------------------------------------

    public static bool DeleteFolder(string path) {
        return FileUtility.DeleteFolder(new DirectoryInfo(path));        
    }

//---------------------------------------------------------------------------------------------------------------------

    public static bool DeleteFolder(DirectoryInfo di) {
        try {
            foreach (FileInfo file in di.EnumerateFiles()) {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories()) {
                dir.Delete(true); 
            }
        } catch {
            Debug.LogError("[PackageDebug] Error deleting files/folders inside : " + di.FullName);
            return false;
        }        

        try {
            di.Delete(true);
        } catch {
            Debug.LogError("[PackageDebug] Error deleting delete: " + di.FullName);
        }        
        return true;
    }

//---------------------------------------------------------------------------------------------------------------------

    public static void CopyFolder(string srcDir, string targetDir, bool overwrite) {
        Directory.CreateDirectory(targetDir);

        DirectoryInfo sourceDI = new DirectoryInfo(srcDir);
        foreach (FileInfo file in sourceDI.EnumerateFiles()) {
            File.Copy(file.FullName, Path.Combine(targetDir, file.Name), overwrite);
        }
        foreach (DirectoryInfo dir in sourceDI.EnumerateDirectories()) {
            CopyFolder(dir.FullName, Path.Combine(targetDir, dir.Name), overwrite);
        }

    }

//---------------------------------------------------------------------------------------------------------------------

    //Files that already exists in targetDir will be overwritten
    public static void MoveFolder(string srcDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);
        string[] srcFiles = Directory.GetFiles(srcDir, "*.*", SearchOption.AllDirectories);

        foreach(string srcFile in srcFiles) {
            FileInfo fileInfo = new FileInfo(srcFile);
            string dest = Path.Combine(targetDir, fileInfo.Name);
            if (File.Exists(dest)) {
                File.Delete(dest);
            }

            fileInfo.MoveTo(dest);
        }
    }

//---------------------------------------------------------------------------------------------------------------------

    public static bool ReplaceTextInFileUsingMemory(string path, string regex, string newStr, 
        RegexOptions options = RegexOptions.None) 
    {
        if (!File.Exists(path)) {
            Debug.LogError("[PackageDebug] File doesn't exist: " + path);
            return false;
        }
            

        string text = File.ReadAllText(path);
        string replacedText = System.Text.RegularExpressions.Regex.Replace(text, regex, newStr, options);

        File.WriteAllText(path, replacedText);
        return true;
    }

}

}//end namespace