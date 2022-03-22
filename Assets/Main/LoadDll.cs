using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        LoadGameDll();
        RunMain();
    }

    public static System.Reflection.Assembly gameAss;

    private void LoadGameDll()
    {
#if UNITY_EDITOR
        string gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
        // ʹ��File.ReadAllBytes��Ϊ�˱���Editor��gameDll�ļ���ռ�õ��º���������޷�����
        gameAss = System.Reflection.Assembly.Load(File.ReadAllBytes(gameDll));
#else
        string gameDll = Application.streamingAssetsPath + "/HotFix.dll";
        gameAss = System.Reflection.Assembly.LoadFile(gameDll);
#endif
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
    }
}
