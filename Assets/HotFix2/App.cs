using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

struct MyValue
{
    public int x;
    public float y;
    public string s;
}

public class App
{
    public static int Main()
    {
#if !UNITY_EDITOR
        LoadMetadataForAOTAssembly();
#endif
        // ���Բ���Ԫ���ݺ�ʹ�� AOT����
        TestAOTGeneric();

        Debug.Log("hello, huatuo");
        var go = new GameObject("HotFix2");
        go.AddComponent<CreateByHotFix2>();

        // ����AOTԪ�������Լ��ֶ����á�
        // ���ڵ�ǰ mscorlib.dll δ���� common ab����
        // ֱ�ӵ��û�������Լ��޸Ĵ���ű������ü����mscorlib.dll
        // ���� common 

        return 0;
    }

    /// <summary>
    /// ���� aot����
    /// </summary>
    public static void TestAOTGeneric()
    {
        var arr = new List<MyValue>();
        arr.Add(new MyValue() { x = 1, y = 10, s = "abc" });
        var e = arr[0];
        Debug.LogFormat("x:{0} y:{1} s:{2}", e.x, e.y, e.s);
    }

    /// <summary>
    /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
    /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
    /// </summary>
    public static unsafe void LoadMetadataForAOTAssembly()
    {
        // ���Լ�������aot assembly�Ķ�Ӧ��dll����Ҫ��dll������unity build���������ɵĲü����dllһ�£�������ֱ��ʹ��
        // ԭʼdll��
        // ��Щdll������Ŀ¼ Temp\StagingArea\Il2Cpp\Managed ���ҵ���
        // ����Win Standalone��Ҳ������ buildĿ¼�� {Project}/ManagedĿ¼���ҵ���
        // ����Android������target, ���������в�û����Щdll����˻��ǵ�ȥ Temp\StagingArea\Il2Cpp\Managed ��ȡ��
        //
        // ��������õ�mscorlib.dll����
        //
        // ���ش��ʱ unity��buildĿ¼�����ɵ� �ü����� mscorlib��ע�⣬����Ϊԭʼmscorlib
        //
        //string mscorelib = @$"{Application.dataPath}/../Temp/StagingArea/Il2Cpp/Managed/mscorlib.dll";

        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        /// 
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll", // ���ʹ����Linq����Ҫ���
            // "Newtonsoft.Json.dll",
            // "protobuf-net.dll",
            // "Google.Protobuf.dll",
            // "MongoDB.Bson.dll",
            // "DOTween.Modules.dll",
            // "UniTask.dll",
        };

        AssetBundle dllAB = LoadDll.AssemblyAssetBundle;
        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = dllAB.LoadAsset<TextAsset>(aotDllName).bytes;
            fixed (byte* ptr = dllBytes)
            {
                // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
                int err = Huatuo.HuatuoApi.LoadMetadataForAOTAssembly((IntPtr)ptr, dllBytes.Length);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
            }
        }
    }



}
