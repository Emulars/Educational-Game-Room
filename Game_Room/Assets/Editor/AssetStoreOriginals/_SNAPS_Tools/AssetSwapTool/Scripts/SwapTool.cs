using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.AssetStoreOriginals._SNAPS_Tools.AssetSwapTool.Scripts
{

    public class SwapTool : EditorWindow
    {

        static SnapRp _currentRp;


        public static string prefabPath;
        public static string prefabPostfix;

        public static string prefabHdPath;

        static bool _groupEnabled;

        static string _selectedRootPath;
        static string _selectedHdRootPath;


        private bool ValidateToolSettings()
        {

            if (prefabPath.Trim() == string.Empty || prefabPostfix.Trim() == string.Empty || prefabHdPath.Trim() == string.Empty)
                return false;

            if (!Directory.Exists(prefabPath))
                return false;

            if (!Directory.Exists(prefabHdPath))
                return false;

            string[] prefabFiles = Directory.GetFiles(prefabPath, "*_" + prefabPostfix.Trim() + ".prefab", SearchOption.AllDirectories);

            if (prefabFiles.Length == 0)
                return false;

            string[] prefabHdFiles = Directory.GetFiles(prefabHdPath, "*_" + prefabPostfix.Trim() + ".prefab", SearchOption.AllDirectories);

            if (prefabHdFiles.Length == 0)
                return false;

            return true;
        }

        public static string GetPrefabPath(Object asset)
        {
            string assetPath = string.Empty;

            Object targetObj = PrefabUtility.GetCorrespondingObjectFromSource<Object>(asset);

            assetPath = AssetDatabase.GetAssetPath(targetObj);

            if (assetPath == string.Empty)
                Debug.LogWarning("Cound not find AssetPath : " + asset.ToString());

            return assetPath;
        }

        public static string GetOriginalPrefabPath(Object asset)
        {
            string assetPath = string.Empty;

            Object targetObj = PrefabUtility.GetCorrespondingObjectFromOriginalSource<Object>(asset);

            assetPath = AssetDatabase.GetAssetPath(targetObj);

            if (assetPath == string.Empty)
                Debug.LogWarning("Cound not find AssetPath : " + asset.ToString());

            return assetPath;
        }


        static GameObject LoadPrefabByAssetPath(string targetPath)
        {
            Object loadedAsset = AssetDatabase.LoadAssetAtPath(targetPath, typeof(GameObject));

            Object instantiateObj = PrefabUtility.InstantiatePrefab(loadedAsset);

            return ((GameObject)instantiateObj);
        }

        static bool SwapGameObject(GameObject sourceGo, GameObject targetGo)
        {

            GameObject genGameObj;

            PrefabAssetType pref = PrefabUtility.GetPrefabAssetType(targetGo);

            if (pref == PrefabAssetType.Regular || pref == PrefabAssetType.Model)
            {
                genGameObj = (GameObject)PrefabUtility.InstantiatePrefab(targetGo);
            }
            else
            {
                genGameObj = (GameObject)UnityEditor.Editor.Instantiate(targetGo);
            }

            Transform gTransform = genGameObj.transform;

            gTransform.parent = sourceGo.transform.parent;
            genGameObj.name = targetGo.name;

            gTransform.localPosition = sourceGo.transform.localPosition;
            gTransform.localScale = sourceGo.transform.localScale;
            gTransform.localRotation = sourceGo.transform.localRotation;

            DestroyImmediate(sourceGo);

            return true;
        }

        static bool SwapGameObjectByTargetPath(GameObject sourceGo, string targetPath)
        {
            try
            {

                GameObject genGameObj = LoadPrefabByAssetPath(targetPath);

                if (genGameObj == null)
                    return false;

                Transform gTransform = genGameObj.transform;

                gTransform.parent = sourceGo.transform.parent;
                gTransform.name = sourceGo.name;

                gTransform.localPosition = sourceGo.transform.localPosition;
                gTransform.localScale = sourceGo.transform.localScale;
                gTransform.localRotation = sourceGo.transform.localRotation;
                gTransform.localEulerAngles = sourceGo.transform.localEulerAngles;

                gTransform.position = sourceGo.transform.position;

                DestroyImmediate(sourceGo);

                return true;
            }
            catch
            {
                return false;
            }
        }

        static void ExchangeAssetToSnap()
        {
            Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);

            if (transforms.Length == 0)
            {
                EditorUtility.DisplayDialog("No selections in the scene", "Need to select at least one object.", "Ok");
                return;
            }

            Dictionary<string, string> snapInfo = new Dictionary<string, string>();
            snapInfo.Clear();

            snapInfo = GetSnapMatchingTable(prefabPath);


            foreach (Transform t in transforms)
            {

                string foundPrefabPath = GetPrefabPath(t.gameObject);

                if (foundPrefabPath.ToLower().Contains(prefabPath.Replace(Application.dataPath, string.Empty).ToLower()))
                    return;

                foundPrefabPath = Path.GetFileNameWithoutExtension(foundPrefabPath).ToLower();


                


                if (snapInfo.ContainsKey(foundPrefabPath))
                {
                    string targetPrefab = string.Empty;
                    if (snapInfo.TryGetValue(foundPrefabPath, out targetPrefab))
                    {
                        bool swapResult = SwapGameObjectByTargetPath(t.gameObject, targetPrefab);

                        if (swapResult == false)
                            Debug.LogWarning(string.Format("Could not swap the object : {0}", t.name));
                    }

                }
                else
                {
                    Debug.LogWarning(string.Format("Could not find the matchable object : {0}", t.name));
                    
                }


            }
        }


        static bool ReExchangeSnapToObject()
        {
            bool swapResult = false;

            Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);

            if (transforms.Length == 0)
            {
                EditorUtility.DisplayDialog("No selections in the scene", "Need to select at least one object.", "Ok");
                return false;
            }

            Dictionary<string, string> objInfo = new Dictionary<string, string>();
            objInfo.Clear();

            objInfo = GetObjectMatchingTable(prefabPath);


            foreach (Transform t in transforms)
            {

                string foundPrefabPath = GetPrefabPath(t.gameObject);

                if (!foundPrefabPath.ToLower().Contains(prefabPath.Replace(Application.dataPath, string.Empty).ToLower()))
                    return false;

                foundPrefabPath = Path.GetFileNameWithoutExtension(foundPrefabPath).ToLower();


                if (objInfo.ContainsKey(foundPrefabPath))
                {
                    string targetPrefab = string.Empty;
                    if (objInfo.TryGetValue(foundPrefabPath, out targetPrefab))
                    {
                        swapResult = SwapGameObjectByTargetPath(t.gameObject, targetPrefab);

                        if (swapResult == false)
                            Debug.LogWarning(string.Format("Could not swap the object : {0}", t.name));
                    }

                }
            }

            return swapResult;
        }


        static void ExchangeSnapToObject()
        {
            
            Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);

            if (transforms.Length == 0)
            {
                EditorUtility.DisplayDialog("No selections in the scene", "Need to select at least one object.", "Ok");
                return;
            }

            Dictionary<string, string> objInfo = new Dictionary<string, string>();
            objInfo.Clear();

            objInfo = GetObjectMatchingTable(prefabPath);


            foreach (Transform t in transforms)
            {

                string foundPrefabPath = GetPrefabPath(t.gameObject);

                if (!foundPrefabPath.ToLower().Contains( prefabPath.Replace(Application.dataPath, string.Empty).ToLower() ))
                    return;

                foundPrefabPath = Path.GetFileNameWithoutExtension(foundPrefabPath).ToLower();


                if (objInfo.ContainsKey(foundPrefabPath))
                {
                    string targetPrefab = string.Empty;
                    if (objInfo.TryGetValue(foundPrefabPath, out targetPrefab))
                    {
                        bool swapResult = SwapGameObjectByTargetPath(t.gameObject, targetPrefab);

                        if (swapResult == false)
                            Debug.LogWarning(string.Format("Could not swap the object : {0}", t.name));
                    }

                }
                else
                {

                    Selection.activeObject = t;

                    Debug.LogWarning(string.Format("Could not find the matchable object : {0}", t.name));

                    

                    Regex reg = new Regex(@"_snaps[0-9][0-9][0-9]$");

                    
                    string pPath = GetPrefabPath(t.gameObject).ToLower();


                    Undo.RegisterCompleteObjectUndo(t.gameObject, "Before unpacking");
                    

                    UnpackNestedPrefab.UnpackSelectedPrefab(t.gameObject);
                    

                    Transform[] childTransforms = t.gameObject.GetComponentsInChildren<Transform>();

                    int successCount = 0;

                    foreach (Transform childTransform in childTransforms)
                    {
                        if (childTransform == null)
                            continue;

                        if (childTransform.gameObject == null)
                            continue;

                        if (PrefabUtility.GetPrefabAssetType(childTransform.gameObject) != PrefabAssetType.Regular)
                            continue;

                        Selection.activeObject = childTransform.gameObject;

                        if (ReExchangeSnapToObject())
                            successCount++;
                        
                    }
                    
                    if (successCount < 1)
                    {
                        Undo.PerformUndo();
                        //Undo.ClearUndo(t.gameObject);
                        continue;
                    }
                    
                    string genSnapsHdPath = UnpackNestedPrefab.CreateGenSnapsHdFolder();
                    string genSnapsPrototypePath = UnpackNestedPrefab.CreateGenSnapsPrototypeFolder();


                    string genSnapsName = string.Empty;

                    if (reg.IsMatch(foundPrefabPath))
                    {
                        genSnapsName = string.Format("{0}/{1}.prefab", genSnapsHdPath, foundPrefabPath);
                    }
                    else
                    {
                        genSnapsName = string.Format("{0}/{1}_{2}.prefab", genSnapsHdPath, foundPrefabPath, prefabPostfix);
                    }

                    
                    Object genPrefab = PrefabUtility.SaveAsPrefabAsset(t.gameObject, genSnapsName);
                    
                    
                    SwapGameObjectByTargetPath(t.gameObject, genSnapsName);



                    reg = new Regex(@"_snaps[0-9][0-9][0-9].prefab$");

                    if (pPath.Contains(UnpackNestedPrefab.PrefabRoot.ToLower()))
                    {
                        if (!reg.IsMatch(pPath))
                        {
                            AssetDatabase.RenameAsset(pPath, Path.GetFileNameWithoutExtension(pPath) + "_" + prefabPostfix);
                        }
                        
                    }
                    else
                    {
                        if (!reg.IsMatch(pPath))
                        {
                            AssetDatabase.MoveAsset(pPath, genSnapsPrototypePath + "/" + Path.GetFileNameWithoutExtension(pPath) + "_" + prefabPostfix + ".prefab");
                        }
                        else
                        {
                            AssetDatabase.MoveAsset(pPath, genSnapsPrototypePath + "/" + Path.GetFileName(pPath));
                        }
                    }


                }
            }

            Undo.ClearAll();

        }



        static void BulkExchangeSnapToHd()
        {
            
            Transform[] sceneTransforms = GameObject.FindObjectsOfType<Transform>();

            ArrayList targetTransform = new ArrayList();

            foreach(Transform sceneTransform in sceneTransforms)
            {
                GameObject parentPrefab = PrefabUtility.GetOutermostPrefabInstanceRoot(sceneTransform.gameObject);

                if (PrefabUtility.GetPrefabAssetType(sceneTransform) == PrefabAssetType.Regular)
                {
                    if (!targetTransform.Contains(parentPrefab))
                        targetTransform.Add(parentPrefab);
                    
                }
           
            }
            
            for(int i=0;i<targetTransform.Count;i++)
            {
                EditorUtility.DisplayProgressBar("Switching prefabs...", string.Format("{0}/{1} prefabs switched.", (i).ToString(), targetTransform.Count.ToString()), ((float)i / (float)targetTransform.Count ));
                Selection.activeObject = (GameObject)targetTransform[i];
                ExchangeSnapToObject();
            }

            EditorUtility.ClearProgressBar();

        }

        static void BulkExchangeHdToSnap()
        {
            Transform[] sceneTransforms = GameObject.FindObjectsOfType<Transform>();

            ArrayList targetTransform = new ArrayList();

            foreach (Transform sceneTransform in sceneTransforms)
            {
                GameObject parentPrefab = PrefabUtility.GetOutermostPrefabInstanceRoot(sceneTransform.gameObject);

                if (PrefabUtility.GetPrefabAssetType(sceneTransform) == PrefabAssetType.Variant || PrefabUtility.GetPrefabAssetType(sceneTransform) == PrefabAssetType.Regular)
                {
                    if (!targetTransform.Contains(parentPrefab))
                        targetTransform.Add(parentPrefab);
                }
            }

            for (int i = 0; i < targetTransform.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Switching prefabs...", string.Format("{0}/{1} prefabs switched.", (i).ToString(), targetTransform.Count.ToString()), ((float)i / (float)targetTransform.Count));
                Selection.activeObject = (GameObject)targetTransform[i];
                ExchangeAssetToSnap();
            }

            EditorUtility.ClearProgressBar();
        }



        public static Dictionary<string, string> GetObjectMatchingTable(string searchingRootPath)
        {

            

            Dictionary<string, string> objDic = new Dictionary<string, string>();
            objDic.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/../");

            string dataPath = dirInfo.FullName.Replace("\\", "/");

            string targetPrefabPath = searchingRootPath.ToLower().Replace(dataPath.ToLower(), string.Empty);


            //Debug.Log("@" + PrefabHDPath.ToLower().Replace(dataPath.ToLower(), string.Empty));

            
           
            string[] hDroots = new string[1];
            hDroots[0] = prefabHdPath.ToLower().Replace(dataPath.ToLower(), string.Empty);

            if (AssetDatabase.IsValidFolder("Assets/GenSnapsHD"))
            {
                hDroots = new string[2];
                hDroots[0] = prefabHdPath.ToLower().Replace(dataPath.ToLower(), string.Empty);
                hDroots[1] = "Assets/GenSnapsHD";
            }

            //string[] Assets = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            string[] assets = AssetDatabase.FindAssets("t:Prefab", hDroots);




            foreach (string asset in assets)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(asset);
                string assetNameWithoutEx = Path.GetFileNameWithoutExtension(assetPath).Trim();

                if (assetNameWithoutEx.ToLower().Contains(prefabPostfix.ToLower()))
                {
                    if (!assetPath.ToLower().Contains(targetPrefabPath))
                    {
                        if (!objDic.ContainsKey(assetNameWithoutEx.ToLower()))
                            objDic.Add(assetNameWithoutEx.ToLower(), assetPath);
                    }
                }
            }

            return objDic;
        }

        public static Dictionary<string, string> GetSnapMatchingTable(string searchingRootPath)
        {

            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/../");

            string ourput = dirInfo.FullName.Replace("\\", "/");

            string[] snapAssets = AssetDatabase.FindAssets("t:Prefab", new[] { searchingRootPath.Replace(ourput, string.Empty) });


            Dictionary<string, string> snap = new Dictionary<string, string>();
            snap.Clear();


            foreach (string asset in snapAssets)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(asset);
                string assetNameWithoutEx = Path.GetFileNameWithoutExtension(assetPath);


                if (assetNameWithoutEx.ToLower().Contains(prefabPostfix.ToLower()))
                {
                    if (!snap.ContainsKey(assetNameWithoutEx.ToLower()))
                    {
                        snap.Add(assetNameWithoutEx.ToLower(), assetPath);
                    }

                }

            }

            return snap;
        }


        [MenuItem("Snaps/Asset Swap Tool")]
        static void ShowSwapTool()
        {
            SwapTool swapWindow = EditorWindow.GetWindow<SwapTool>();

            swapWindow.titleContent.text = "Asset Swap Tool";

            Vector2 windowSize = new Vector2(400, 380);

            swapWindow.minSize = windowSize;
            swapWindow.maxSize = windowSize;

            swapWindow.Show();

            if (Directory.Exists(prefabPath))
            {
                _groupEnabled = true;
            }
            else
                _groupEnabled = false;

        }


        


        private string SearchFrequentPostfix(string targetPath)
        {
            string outString = string.Empty;

            Dictionary<string, int> postfixDic = new Dictionary<string, int>();

            if (!Directory.Exists(targetPath))
                return outString;

            string[] sPrefabs = Directory.GetFiles(targetPath, "*_snaps*.prefab", SearchOption.AllDirectories);

            foreach(string prefab in sPrefabs)
            {
                string[] tokens = Path.GetFileNameWithoutExtension(prefab).ToString().Split('_');
                string lasttoken = tokens[tokens.Length - 1].ToLower().Trim();


                if (!postfixDic.ContainsKey(lasttoken))
                {
                    postfixDic.Add(lasttoken, 1);
                }
                else
                {
                    postfixDic[lasttoken]++;
                }

            }

            var potfixList = postfixDic.ToList();
            potfixList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            if (potfixList.Count != 0)
                outString = potfixList[0].Key.Trim();

            return outString;
        }


        private bool FillOutSettingValues(string targetRootPath)
        {
            if (!Directory.Exists(targetRootPath))
            {
                EditorUtility.DisplayDialog("ERROR", string.Format("Could not find the Root Path : {0}", targetRootPath), "Ok");

                if (!Directory.Exists(prefabPath))
                    _groupEnabled = false;

                return false;
            }

            string frequentpostFix = SearchFrequentPostfix(targetRootPath);

            string[] foundSnapPrefabs = Directory.GetFiles(targetRootPath, "*_" + frequentpostFix + ".prefab", SearchOption.AllDirectories);

            if (foundSnapPrefabs.Length == 0)
            {
                EditorUtility.DisplayDialog("ERROR", string.Format("Could not find Snaps Prototype prefabs in \"{0}\"", targetRootPath), "Ok");
                _groupEnabled = false;
                return false;
            }

            
            prefabPath = targetRootPath;
            prefabPostfix = frequentpostFix;

            if (Directory.Exists(prefabHdPath))
                _groupEnabled = true;

            Repaint();

            return true;

        }

        private bool FillOutHdSettingValues(string targetHdRootPath)
        {
            if (!Directory.Exists(targetHdRootPath))
            {
                EditorUtility.DisplayDialog("ERROR", string.Format("Could not find the Root Path : {0}", targetHdRootPath), "Ok");

                if (!Directory.Exists(prefabHdPath))
                    _groupEnabled = false;

                return false;
            }


            prefabHdPath = targetHdRootPath;

            if (Directory.Exists(prefabPath))
                _groupEnabled = true;


            Repaint();

            return true;
        }
        private void OnGUI()
        {
            

            GUILayout.BeginVertical("Box");

            GUILayout.Label("Tool settings :", EditorStyles.boldLabel);
            prefabPath = EditorGUILayout.TextField("Snaps Prototype PATH :", prefabPath);
            prefabHdPath = EditorGUILayout.TextField("Snaps Art / HD PATH :", prefabHdPath);
            prefabPostfix = EditorGUILayout.TextField("Snaps Prototype Postfix :", prefabPostfix);

            
            
            if (GUILayout.Button(new GUIContent("Select a different project directory for Snaps Prototype prefabs.", "Set a root path to get a list of Snaps Prototype prefabs.")))
            {
                string targetPath = Path.Combine(Application.dataPath, @"AssetStoreOriginals\_SNAPS_PrototypingAssets");

                if (Directory.Exists(targetPath))
                {
                    _selectedRootPath = EditorUtility.OpenFolderPanel("Select a root path containing Snaps Prototype prefabs", targetPath, string.Empty);
                }
                else
                    _selectedRootPath = EditorUtility.OpenFolderPanel("Select a root path containing Snaps Prototype prefabs", Application.dataPath, string.Empty);

                if (FillOutSettingValues(_selectedRootPath))
                {
                    if (SwapShader.CheckShaderSwap(out _currentRp))
                    {
                        SwapShader.SwitchSnapPrototypeShader(_currentRp, prefabPath);
                    }

                }
            }


            if (GUILayout.Button(new GUIContent("Select a different project directory for Snaps Art / Art HD prefabs.", "Set a root path to get a list of Snaps Art / Art HD prefabs.")))
            {
                string targetPath = Application.dataPath;

                _selectedHdRootPath = EditorUtility.OpenFolderPanel("Select a root path containing Snaps Prototype prefabs", Application.dataPath, string.Empty);

                if (FillOutHdSettingValues(_selectedHdRootPath))
                {
                    if (SwapShader.CheckShaderSwap(out _currentRp))
                    {
                        SwapShader.SwitchSnapPrototypeShader(_currentRp, prefabPath);
                    }

                }
            }


            GUILayout.EndVertical();  ////

            GUILayout.Label(string.Empty, EditorStyles.boldLabel);
            GUILayout.Label(string.Empty, EditorStyles.boldLabel);


            GUI.enabled = _groupEnabled;


            GUILayout.BeginVertical("Box");

            GUILayout.Label("Swap prefabs by selections :", EditorStyles.boldLabel);

            if (GUILayout.Button(new GUIContent("Swap selections to Snaps Prototype prefabs", "Swap Snaps Art HD prefabs selected in the scene to corresponding Snaps Prototype prefabs.")))
            {
                if (ValidateToolSettings())
                {
                    ExchangeAssetToSnap();
                }
                else
                    _groupEnabled = false;
            }

            if (GUILayout.Button(new GUIContent("Swap selections to Snaps Art HD prefabs", "Swap Snaps Prototype prefabs selected in the scene to corresponding Snaps Art HD prefabs.")))
            {
                if (ValidateToolSettings())
                {
                    SwapSnapToObject();
                }
                else
                    _groupEnabled = false;
            }

            GUILayout.EndVertical();

            GUILayout.Label(string.Empty, EditorStyles.boldLabel);
            GUILayout.Label(string.Empty, EditorStyles.boldLabel);


            GUILayout.BeginVertical("Box");

            GUILayout.Label("Bulk Swaps in the Scene :", EditorStyles.boldLabel);

            if (GUILayout.Button(new GUIContent("Swap All to Snaps Prototype prefabs", "Swap all of Snaps Art HD prefabs in the scene to Snaps Prototype prefabs, if they have same names.")))
            {
                if (ValidateToolSettings())
                {
                    BulkExchangeHdToSnap();
                }
                else
                    _groupEnabled = false;
            }

            if (GUILayout.Button(new GUIContent("Swap All to Snaps Art HD prefabs", "Swap all of Snaps Prototype prefabs in the scene to Snaps Art HD prefabs, if they have same names.")))
            {
                if (ValidateToolSettings())
                {
                    BulkExchangeSnapToHd();
                }
                else
                    _groupEnabled = false;
            }

            GUILayout.EndVertical();

        }

        
        static void SwapObject()
        {
            ExchangeAssetToSnap();
        }


        static void SwapSnapToObject()
        {
            ExchangeSnapToObject();
        }


        

    }


}