using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

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
        Debug.Log("hello, huatuo");

        var go = new GameObject("HotFix2");
        go.AddComponent<CreateByHotFix2>();

        return 0;
    }

    /// <summary>
    /// ���� aot���ͣ�����������Լ���������
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
        // ���Լ�������aot assembly�Ķ�Ӧ��dll����������õ�mscorlib.dll����
        //
        // ���ش��ʱ unity��buildĿ¼�����ɵ� �ü����� mscorlib��ע�⣬����Ϊԭʼmscorlib
        //
        string mscorelib = @$"{Application.dataPath}/../build-win64-2020.3.33/huatuo/Managed/mscorlib.dll";
        byte[] dllBytes = File.ReadAllBytes(mscorelib);

        fixed (byte* ptr = dllBytes)
        {
            // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
            int err = Huatuo.HuatuoApi.LoadMetadataForAOTAssembly((IntPtr)ptr, dllBytes.Length);
            Debug.Log("LoadMetadataForAOTAssembly. ret:" + err);
        }
    }
}
