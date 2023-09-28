using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class BuildPostProcessors
{
    public Object indexTemplate;
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.WebGL)
        {
            string template = AssetDatabase.GetAssetPath(Resources.Load<TextAsset>("index-template"));
            File.Copy(template, path + Path.DirectorySeparatorChar + "index.html", true);
            Debug.Log($"Overridden index.html using template from {template}");
        }
    }
}
