using System;

namespace Shin.Core {

public static class CollectionUtility
{
    public static T GetFirst<T>(T[] elements, Func<T, bool> func) {
        int numElements = elements.Length;
        for (int i = 0; i < numElements; ++i) {
            if (func(elements[i])) {
                return elements[i];
            }
        }
        return default(T);
    }
}

} //end namespace
