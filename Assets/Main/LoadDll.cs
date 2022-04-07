using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        BetterStreamingAssets.Initialize();
        LoadGameDll();
        RunMain();
    }

    public static System.Reflection.Assembly gameAss;

    private void LoadGameDll()
    {
#if UNITY_EDITOR
        string gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
        byte[] dllBytes = File.ReadAllBytes(gameDll);
        // ʹ��File.ReadAllBytes��Ϊ�˱���Editor��gameDll�ļ���ռ�õ��º���������޷�����
#else
        string gameDll = "HotFix.dll";
        byte[] dllBytes = BetterStreamingAssets.ReadAllBytes(gameDll);
#endif
        gameAss = System.Reflection.Assembly.Load(dllBytes);
    }

    public void RunMain()
    {
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dllδ����");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);

        // �����Update֮��ĺ������Ƽ���ת��Delegate�ٵ��ã���
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateMethod(deltaTime);
    }
}
