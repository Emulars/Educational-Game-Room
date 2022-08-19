using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Assets.Editor.AssetStoreOriginals._SNAPS_Tools.AssetSwapTool.Scripts
{ 
    public enum SnapRp
    {
        Default,
        Hdrp,
        Lwrp
    }


    public class SwapShader : MonoBehaviour
    {

        const string PackageNameProbuilder = "com.unity.probuilder";
        const string PackageNameHdrp = "com.unity.render-pipelines.high-definition";
        const string PackageNameLwrp = "com.unity.render-pipelines.lightweight";




        public static bool CheckShaderSwap(out SnapRp checkedRp)
        {
            checkedRp = SnapRp.Default;

            ListRequest listRequest = Client.List(true);

            while (!listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Failure || listRequest.Error != null)
                {
                    Debug.LogError(listRequest.Error.message);
                    return false;
                }
            }

            if (listRequest == null)
                return false;


            ArrayList allInstalledPackages = new ArrayList();

            foreach (var package in listRequest.Result)
            {
                string[] tokens = package.packageId.Split('@');

                string packageName = tokens[0];
                tokens.Initialize();

                if (!allInstalledPackages.Contains(packageName))
                    allInstalledPackages.Add(packageName);

            }

            if (!allInstalledPackages.Contains(PackageNameProbuilder))
            {
                EditorUtility.DisplayDialog("Package install required", "Need to install Probuilder through the PackageManager.", "Confirm");
            }

            if (allInstalledPackages.Contains(PackageNameHdrp))
            {
                checkedRp = SnapRp.Hdrp;
            }
            else if (allInstalledPackages.Contains(PackageNameLwrp))
            {
                checkedRp = SnapRp.Lwrp;
            }
            else
            {
                checkedRp = SnapRp.Default;
            }

            allInstalledPackages.Clear();
            return true;

        }


        public static void SwitchSnapPrototypeShader(SnapRp rpType, string materialRootFullPath)
        {
            
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Application.dataPath, "../"));
            
            string rootPathRel = materialRootFullPath.Replace(dInfo.FullName.Replace("\\","/"), string.Empty);

            string[] materialPaths = AssetDatabase.FindAssets("t:material", new string[] { rootPathRel });


            foreach (string materialPath in materialPaths)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(materialPath);
                Material exMaterial = (Material)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Material));


                Color color = exMaterial.color;

                float smoothness;

                if (exMaterial.name == "ProBuilder/Standard Vertex Color")
                    smoothness = exMaterial.GetFloat("_Glossiness");
                else
                    smoothness = exMaterial.GetFloat("_Smoothness");

                float metallic = exMaterial.GetFloat("_Metallic");

                

                Shader newShader = Shader.Find("ProBuilder/Standard Vertex Color");

                if (newShader == null)
                    return;

                switch (rpType)
                {
                    case SnapRp.Default:
                        newShader = Shader.Find("ProBuilder/Standard Vertex Color");
                        break;

                    case SnapRp.Hdrp:
                        newShader = Shader.Find("HDRP/Lit");

                        if (newShader == null)
                            newShader = Shader.Find("HDRenderPipeline/Lit");

                        break;

                    case SnapRp.Lwrp:
                        newShader = Shader.Find("Lightweight Render Pipeline/Lit");
                        break;
                }

                if (exMaterial == null || exMaterial.shader == null)
                    continue;

                
                if (exMaterial.shader.name.ToLower() != newShader.name.ToLower())
                {
                    exMaterial.shader = newShader;
                    exMaterial.SetColor("_BaseColor", color);

                    exMaterial.SetFloat("_Metallic", metallic);

                    if (exMaterial.name != "ProBuilder/Standard Vertex Color")
                        exMaterial.SetFloat("_Smoothness", smoothness);
                    else
                        exMaterial.SetFloat("_Glossiness", smoothness);
                }
                else
                {
                    exMaterial.SetFloat("_Metallic", 0);

                    if (exMaterial.name != "ProBuilder/Standard Vertex Color")
                        exMaterial.SetFloat("_Smoothness", 0.5f);
                    else
                        exMaterial.SetFloat("_Glossiness", 0);
                }
                
            }

        }

    }

}
