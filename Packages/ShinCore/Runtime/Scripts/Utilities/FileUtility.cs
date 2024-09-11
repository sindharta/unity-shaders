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

    
    /// <summary>
    /// Make file writable
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <returns>The file could be made writable or not</returns>
    public static bool MakeFileWritable(string path) {
        if (!File.Exists(path))  {
            Debug.LogError("[AnimeToolbox] TryMakeFileWritable() Path doesn't exist: " + path);
            return false;
        }

        FileAttributes attributes = File.GetAttributes(path);

        if (FileAttributes.ReadOnly == (attributes & FileAttributes.ReadOnly) ) {
            // Remove RO
            attributes.RemoveAttribute(FileAttributes.ReadOnly);
            File.SetAttributes(path, attributes);
        }

        return true;

    }
    
    
    /// <summary>
    /// Compute the MD5 hash code of a file
    /// </summary>
    /// <param name="path">The path to file</param>
    /// <returns>The MD5 hash code of the file</returns>
    public static string ComputeFileMD5(string path) {
        using (MD5 md5 = MD5.Create()) {
            using (FileStream stream = File.OpenRead(path)) {
                byte[] hash = md5.ComputeHash(stream);
                string str = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                return str;
            }
        }
    }
    
//----------------------------------------------------------------------------------------------------------------------

    #region json
    /// <summary>
    /// Deserialize a json file to an object
    /// </summary>
    /// <param name="path">The path to the json file</param>
    /// <typeparam name="T">The type of the object inside the json file</typeparam>
    /// <returns>The deserialized object with type T</returns>
    public static T DeserializeFromJson<T>(string path) where T: class {
        if (!File.Exists(path)) {
            return null;
        }
        
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
    
    /// <summary>
    /// Serializes an object into a json file
    /// </summary>
    /// <param name="obj">The object to be serialized</param>
    /// <param name="path">The path to the json file</param>
    /// <param name="prettyPrint">If true, format the output for readability.
    ///     If false, format the output for minimum size. Default is false.
    /// </param>
    /// <typeparam name="T">The type of the object</typeparam>
    public static void SerializeToJson<T>(T obj, string path, bool prettyPrint=false) {
        string dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir)) {
            Directory.CreateDirectory(dir);
        }
        
        string json = JsonUtility.ToJson(obj, prettyPrint);
        File.WriteAllText(path, json);
    }    
    
    #endregion
    
    
//---------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Delete files and folders under the passed directory
    /// </summary>
    /// <param name="path">The path to directory to be deleted</param>
    /// <returns>True if deletion is successful, false otherwise</returns>
    public static bool DeleteFilesAndFolders(string path) {
        return FileUtility.DeleteFilesAndFolders(new DirectoryInfo(path));        
    }

//---------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Delete files and folders under the passed directory
    /// </summary>
    /// <param name="di">The DirectoryInfo of the directory to be deleted</param>
    /// <returns>True if deletion is successful, false otherwise</returns>
    public static bool DeleteFilesAndFolders(DirectoryInfo di) {
        //Try to delete the internal contents of the directory 
        try {
            di.EnumerateFiles().Loop((FileInfo file) => {
                file.Delete();
            });
            di.EnumerateDirectories().Loop((DirectoryInfo dir) => {
                dir.Delete(true);
            });
        } catch (Exception e){
            Debug.LogError($"Exception when deleting {di.FullName}: {e.ToString()} ");
            return false;
        }        

        //Try delete the directory itself at the end.
        try {
            di.Delete(true);
        } catch (Exception e) {
            Debug.LogError($"Exception when deleting {di.FullName}: {e.ToString()} ");
            return false;
        }

        return true;
    }    
    
//----------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Copy a directory to another directory recursively
    /// </summary>
    /// <param name="sourceDir">The source directory</param>
    /// <param name="targetDir">The target directory</param>
    /// <param name="overwrite">true if the destination file can be overwritten; otherwise, false.</param>
    public static void CopyRecursive(string sourceDir, string targetDir, bool overwrite) {
        System.IO.Directory.CreateDirectory(targetDir);

        DirectoryInfo sourceDI = new DirectoryInfo(sourceDir);
        sourceDI.EnumerateFiles().Loop((FileInfo file) => {
            File.Copy(file.FullName, Path.Combine(targetDir, file.Name), overwrite);
        });

        sourceDI.EnumerateDirectories().Loop((DirectoryInfo dir) => {
            CopyRecursive(dir.FullName, Path.Combine(targetDir, dir.Name), overwrite);
        });
    }    
    
    
    
    

//---------------------------------------------------------------------------------------------------------------------
    
}

}//end namespace