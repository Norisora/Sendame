using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static bool isQuitting = false;

    // ----------------------------------------------------------------
    // MonoBehaviour
    // ----------------------------------------------------------------
    protected virtual void Awake()
    {
        OnCreateSingleton();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this as T)
        {
            OnDestroySingleton();
            instance = null;
        }
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    // ----------------------------------------------------------------
    // Method
    // ----------------------------------------------------------------
    protected virtual void OnCreateSingleton()
    {
    }

    protected virtual void OnDestroySingleton()
    {
    }

    // ----------------------------------------------------------------
    // Property
    // ----------------------------------------------------------------
    public static T Instance => InstanceCheck();

    public static void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(Instance.gameObject);
            instance = null;
        }
    }

    public static T InstanceCheck()
    {
        if (isQuitting)
        {
            return null;
        }
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                var obj = new GameObject(typeof(T).ToString());
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
        }
        return instance;
    }

    public static bool IsInstantiated => instance;

    // ----------------------------------------------------------------
    // Field
    // ----------------------------------------------------------------

    static T instance;
}
