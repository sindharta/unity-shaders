﻿
using System.Collections.ObjectModel;
using System.Text;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;


namespace Shin.Core.Editor {

/// <summary>
/// Extension methods for ReadOnlyCollection<PackageInfo> class.
/// </summary>
internal static class PackageInfoCollectionExtension {

    /// <summary>
    /// Find a package inside a collection of PackageInfo 
    /// </summary>
    /// <param name="packageInfoCollection">The package collection</param>
    /// <param name="packageName">The name of the package to be searched</param>
    /// <returns>The PackageInfo of the package if found, null otherwise.</returns>
    public static PackageInfo FindPackage(this ReadOnlyCollection<PackageInfo> packageInfoCollection, string packageName) {
        if (string.IsNullOrEmpty(packageName))
            return null;

        using var enumerator = packageInfoCollection.GetEnumerator();
        while (enumerator.MoveNext()) {
            PackageInfo packageInfo = enumerator.Current;
            if (null != packageInfo && packageInfo.name == packageName) {
                return packageInfo;
            }
        }

        return null;

    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a multi-line description of the packages in the collection.
    /// </summary>
    /// <param name="packageInfoCollection">The package collection</param>
    /// <returns>Example:
    /// com.unity.foo@0.1.0-preview
    /// com.unity.bar@0.3.0-preview
    /// </returns>
    public static string JoinToString(this ReadOnlyCollection<PackageInfo> packageInfoCollection) {
        StringBuilder sb = new StringBuilder();
        
        using var enumerator = packageInfoCollection.GetEnumerator();
        while (enumerator.MoveNext()) {
            PackageInfo packageInfo = enumerator.Current;
            if (null == packageInfo) {
                continue;
            }
            sb.AppendLine($"{packageInfo.name}@{packageInfo.version}");
        }
        return sb.ToString();
    }    
}

} //end namespace

