using System.IO;

namespace Shin.Core {
    internal static class FileAttributesExtension {

        public static void RemoveAttribute(this FileAttributes attributes, FileAttributes attributesToRemove) {
            attributes &=~attributesToRemove;
        }

    }

}

