using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    protected static T _instance;

    public static T Instance {
        get {
            if (_instance) {
                return _instance;
            }
            else { 
                T find = FindObjectOfType<T>();

                if (find) {
                    return find;
                } else {
                    return CreateNewInstance();
                }
            }
        }
    }

    protected static T CreateNewInstance () {
        var gObject = new GameObject(typeof(T).Name);

        DontDestroyOnLoad(gObject);

        return gObject.AddComponent<T>();
    }
}
