using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.AssetStoreOriginals._SNAPS_Tools.AssetSwapTool.Scripts
{
    public class UnpackNestedPrefab : EditorWindow
    {

        public const string GenSnapsPrototypePath = "GenSnapsPrototype";
        public const string GenSnapsHdPath = "GenSnapsHD";

        
        public const string PrefabRoot = "Assets/AssetStoreOriginals/_SNAPS_PrototypingAssets";
        
        

        static bool IsSnapsPrototypePrefab(GameObject targetGo)
        {

            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(targetGo);

            if (prefabType != PrefabAssetType.Regular && prefabType != PrefabAssetType.Variant)
                return false;


            Regex reg = new Regex(@"_snaps[0-9][0-9][0-9].prefab$");

            string prefabPath = SwapTool.GetOriginalPrefabPath(targetGo).ToLower();



            if (prefabPath.Contains(PrefabRoot.ToLower()) || prefabPath.Contains(GenSnapsPrototypePath.ToLower()) )
            {
                if (reg.IsMatch(prefabPath))
                    return true;
            }

            return false;
        }

        static bool IsSnapsHdPrefab(GameObject targetGo)
        {

            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(targetGo);

            if (prefabType != PrefabAssetType.Regular && prefabType != PrefabAssetType.Variant)
                return false;

            if (IsSnapsPrototypePrefab(targetGo) == false)
            {

                Regex reg = new Regex(@"_snaps[0-9][0-9][0-9].prefab$");

                string prefabPath = SwapTool.GetOriginalPrefabPath(targetGo).ToLower();

                string snapsPrototypePath = SwapTool.prefabPath.Replace(Application.dataPath, string.Empty).ToLower();

                if (reg.IsMatch(prefabPath) && !prefabPath.Contains(snapsPrototypePath))
                    return true;
            }

            return false;
        }

        public static string CreateGenSnapsHdFolder()
        {
            string createdPath = string.Format("Assets/{0}", GenSnapsHdPath);

            if (!AssetDatabase.IsValidFolder(createdPath))
            {
                createdPath = AssetDatabase.GUIDToAssetPath( AssetDatabase.CreateFolder("Assets", GenSnapsHdPath) );
            }

            return createdPath;
        }

        public static string CreateGenSnapsPrototypeFolder()
        {
            string createdPath = string.Format("{0}/{1}", PrefabRoot, GenSnapsPrototypePath);

            if (!AssetDatabase.IsValidFolder(createdPath))
            {
                createdPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(PrefabRoot, GenSnapsPrototypePath));
            }

            return createdPath;
        }


        static bool SetUnpackPrefab(GameObject target)
        {
            if (target == null)
                return false;

            if (IsSnapsHdPrefab(target))
                return false;

            string prefabPath = SwapTool.prefabPath;

            Dictionary<string,string> objInfo = SwapTool.GetObjectMatchingTable(prefabPath);

            string targetPrefabPath = Path.GetFileNameWithoutExtension( SwapTool.GetOriginalPrefabPath(target).ToLower() );

            if (objInfo.ContainsKey(targetPrefabPath))
                return false;


            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(target);

            if (prefabType == PrefabAssetType.Regular || prefabType == PrefabAssetType.Variant)
            {
                if (target.transform.childCount == 0 && target.GetComponent<MeshRenderer>() == null)
                    return false;

                PrefabUtility.UnpackPrefabInstance(target, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            }

            return true;

        }


        public static void UnpackSelectedPrefab(GameObject currentObject)
        {
            Stack<GameObject> nestedGameObject = new Stack<GameObject>();

            nestedGameObject.Clear();


            if (SetUnpackPrefab(currentObject) == false)
                return;

            for (int i = 0; i < currentObject.transform.childCount; i++)
            {
                GameObject childGameObject = currentObject.transform.GetChild(i).gameObject;

                nestedGameObject.Push(currentObject.transform.GetChild(i).gameObject);
            }

            while (nestedGameObject.Count != 0)
            {
                GameObject gObj = nestedGameObject.Pop();

                if (SetUnpackPrefab(gObj) == false)
                    continue;

                for (int i = 0; i < gObj.transform.childCount; i++)
                {
                    nestedGameObject.Push(gObj.transform.GetChild(i).gameObject);
                }
            }

        }


    }
}