using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Unity.FilmInternalUtilities {
internal class SceneComponents<T> where T : Component {

//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    internal static SceneComponents<T> GetInstance() {
        if (null != m_instance)
            return m_instance;

        m_instance = new SceneComponents<T>();
#if UNITY_EDITOR
        EditorSceneManager.sceneClosed += SceneComponents_OnSceneClosed;
#endif
        return m_instance;
    }

#if UNITY_EDITOR
    private static void SceneComponents_OnSceneClosed(Scene scene) {
        m_instance.Destroy();
    }
#endif

    private void Destroy() {
        m_cachedComponents.Clear();
        m_prevUpdateFrame = -1;
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------    

    //Force to use GetInstance()
    private SceneComponents() {
    }

    internal IList<T> GetCachedComponents() {
        return m_cachedComponents;
    }

    internal void SetIncludeInactive(bool includeInactive) {
        m_includeInactive = includeInactive;
    }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Update() {
        int curFrame = Time.frameCount;
        if (m_prevUpdateFrame == curFrame) return;

        ForceUpdate();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ForceUpdate() {
        m_cachedComponents.Clear();
        m_cachedComponents.AddRange(FilmInternalUtilities.ObjectUtility.FindSceneComponentsAsArray<T>(m_includeInactive));
        m_prevUpdateFrame = Time.frameCount;
    }
    

//--------------------------------------------------------------------------------------------------------------------------------------------------------------    
    private readonly List<T> m_cachedComponents = new List<T>();
    private          int     m_prevUpdateFrame  = -1;
    private          bool    m_includeInactive  = false;

    private static SceneComponents<T> m_instance = null;
}
} //end namespace