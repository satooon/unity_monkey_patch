using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class test : MonoBehaviour {

    private hoge _hoge;

    void Awake()
    {
        var targetMethod = typeof(hoge).GetMethod ("getTxt", BindingFlags.Instance | BindingFlags.Public);
        var replacedMethod = typeof(fuga).GetMethod ("getTxt", BindingFlags.Instance | BindingFlags.Public);

        try {
            MethodUtil.ExchangeFunctionPointer (targetMethod, replacedMethod);
        } catch (Exception e) {
            Debug.LogError (e.ToString());
        }

        this._hoge = new hoge ();
    }
	
    void OnGUI()
    {
        GUILayout.Label (this._hoge.getTxt());
    }
}

public class hoge {
    public string getTxt() {
        return "hoge text";
    }
}

public class fuga {
    public string getTxt() {
        return "fuga text";
    }
}

public static class MethodUtil
{
    public static void ExchangeFunctionPointer(MethodInfo method0, MethodInfo method1)
    {
        unsafe
        {
            var functionPointer0 = method0.MethodHandle.Value.ToPointer();
            var functionPointer1 = method1.MethodHandle.Value.ToPointer();
            var tmpPointer = *((int*)new IntPtr(((int*)functionPointer0 + 1)).ToPointer());
            *((int*)new IntPtr(((int*)functionPointer0 + 1)).ToPointer()) = *((int*)new IntPtr(((int*)functionPointer1 + 1)).ToPointer());
            *((int*)new IntPtr(((int*)functionPointer1 + 1)).ToPointer()) = tmpPointer;
        }
    }
}

