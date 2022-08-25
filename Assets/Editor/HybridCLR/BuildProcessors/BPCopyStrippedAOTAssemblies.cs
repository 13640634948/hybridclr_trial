﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Il2Cpp;
using UnityEditor.UnityLinker;
using UnityEngine;

namespace HybridCLR.Editor.BuildProcessors
{
    internal class BPCopyStrippedAOTAssemblies
#if !UNITY_2021_1_OR_NEWER
     : IIl2CppProcessor
#endif
    {
        [InitializeOnLoadMethod]
        private static void Setup()
        {
#if UNITY_2021_1_OR_NEWER && UNITY_EDITOR_WIN
            HookEditorStripAOTAction.InstallHook();
            HookEditorStripAOTAction.OnAssembliyScripped2 += CopyStripDlls;
#endif
        }

        public int callbackOrder => 0;

#if !UNITY_2021_1_OR_NEWER
        private string GetBuildStripAssembliesDir2020(BuildTarget target)
        {
            string subPath = target == BuildTarget.Android ?
                "assets/bin/Data/Managed" :
                "Data/Managed/";
            return $"{BuildConfig.ProjectDir}/Temp/StagingArea/{subPath}";
        }

        public void OnBeforeConvertRun(BuildReport report, Il2CppBuildPipelineData data)
        {            
            // 此回调只在 2020中调用
            CopyStripDlls(GetBuildStripAssembliesDir2020(data.target), data.target);
        }
#endif

        public static void CopyStripDlls(string srcStripDllPath, BuildTarget target)
        {
            Debug.Log($"[BPCopyStrippedAOTAssemblies] CopyScripDlls. src:{srcStripDllPath} target:{target}");

            var dstPath = BuildConfig.GetAssembliesPostIl2CppStripDir(target);

            Directory.CreateDirectory(dstPath);

            //string srcStripDllPath = BuildConfig.GetOriginBuildStripAssembliesDir(target);

            foreach (var fileFullPath in Directory.GetFiles(srcStripDllPath, "*.dll"))
            {
                var file = Path.GetFileName(fileFullPath);
                Debug.Log($"[BPCopyStrippedAOTAssemblies] copy strip dll {fileFullPath} ==> {dstPath}/{file}");
                File.Copy($"{fileFullPath}", $"{dstPath}/{file}", true);
            }
        }
    }
}
