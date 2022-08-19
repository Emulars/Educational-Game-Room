using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Assets.AssetsStore.AssetStoreOriginals._SNAPS_PrototypingAssets.Scripts.Editor.PackageChecker
{
    public class PackageChecker
    {

        private static ListRequest _listRequest;
        private static List<PackageEntry> _packageToAdd;

        private static AddRequest[] _addRequests;
        private static bool[] _installRequired;

        private static string _packageKeyword = "Assets/AssetStoreOriginals/_SNAPS_PrototypingAssets/";


        [InitializeOnLoadMethod]
        static void CheckPackage()
        {
            string filePath = Application.dataPath + "/../Library/PackageChecked";

 
            _packageToAdd = new List<PackageEntry>();
            _listRequest = null;

             
            if (!File.Exists(filePath))
            {
                var packageListFile = Directory.GetFiles(Application.dataPath, "PackageImportList.txt", SearchOption.AllDirectories);
                if (packageListFile.Length == 0)
                {
                    Debug.LogError("[Auto Package] : Couldn't find the packages list. Be sure there is a file called PackageImportList in your project");
                }
                else
                {
                    string packageListPath = packageListFile[0];
                    _packageToAdd = new List<PackageEntry>();
                    string[] content = File.ReadAllLines(packageListPath);
                    foreach (var line in content)
                    {
                        var split = line.Split('@');
                        PackageEntry entry = new PackageEntry();

                        entry.name = split[0];
                        entry.version = split.Length > 1 ? split[1] : null;

                        _packageToAdd.Add(entry);
                    }

                    File.WriteAllText(filePath, "Delete this to trigger a new auto package check");

                    _listRequest = Client.List();

                    while (!_listRequest.IsCompleted)
                    {
                        if (_listRequest.Status == StatusCode.Failure || _listRequest.Error != null)
                        {
                            Debug.LogError(_listRequest.Error.message);
                            break;
                        }
                    }

                    _addRequests = new AddRequest[_packageToAdd.Count];

                    _installRequired = new bool[_packageToAdd.Count];

                    for (int i = 0; i < _installRequired.Length; i++)
                        _installRequired[i] = true;

                     
                    
                    foreach (var package in _listRequest.Result)
                    {
                        for (int i = 0; i < _packageToAdd.Count; i++)
                        {
                            if (package.packageId.Contains(_packageToAdd[i].name))
                            {
                                _installRequired[i] = false;

                                if (package.versions.latestCompatible != "" && package.version != "")
                                {

                                    if (GreaterThan(package.versions.latestCompatible, package.version))
                                    {
                                        _installRequired[i] = EditorUtility.DisplayDialog("Confirm Package Upgrade", string.Format("The version of \"{0}\" in this project is not the latest version. Would you like to upgrade it to the latest version? (Recommmended)", _packageToAdd[i].name), "Yes", "No");

                                        if (_installRequired[i])
                                            _packageToAdd[i].version = package.versions.latestCompatible;

                                    }
                                }
                            }
                        }

                    }
                

                    for (int i = 0; i < _packageToAdd.Count; i++)
                    {
                        if (_installRequired[i])
                            _addRequests[i] = InstallSelectedPackage(_packageToAdd[i].name, _packageToAdd[i].version);
                    }


                    
                    ReimportPackagesByKeyword();


                }
            }
        }


        static AddRequest InstallSelectedPackage(string packageName, string packageVersion)
        {

            if (packageVersion != null)
                packageName = packageName + "@" + packageVersion;


            AddRequest newPackage = Client.Add(packageName);

            while (!newPackage.IsCompleted)
            {
                if (newPackage.Status == StatusCode.Failure || newPackage.Error != null)
                {
                    Debug.LogError(newPackage.Error.message);
                    return null;
                }
            }

            return newPackage;
        }
     
     

        static void ReimportPackagesByKeyword()
        {

            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(_packageKeyword, ImportAssetOptions.ImportRecursive);

        }

         

        static private bool GreaterThan(string versionA, string versionB)
        {
            var versionASplit = versionA.Split('.');
            var versionBSplit = versionB.Split('.');

            int previewA = 0;
            int previewB = 0;
            int patchA = 0;
            int patchB = 0;

            var majorA = Convert.ToInt32(versionASplit[0]);
            var minorA = Convert.ToInt32(versionASplit[1]);
            if (versionASplit.Length > 3)
            {
                patchA = Convert.ToInt32(versionASplit[2].Substring(0, versionASplit[2].Length - 8));
                previewA = Convert.ToInt32(versionASplit[3]);
            }
            else
            {
                patchA = Convert.ToInt32(versionASplit[2]);
            }

            var majorB = Convert.ToInt32(versionBSplit[0]);
            var minorB = Convert.ToInt32(versionBSplit[1]);
            if (versionBSplit.Length > 3)
            {
                patchA = Convert.ToInt32(versionBSplit[2].Substring(0, versionBSplit[2].Length - 8));
                previewA = Convert.ToInt32(versionBSplit[3]);
            }
            else
            {
                patchA = Convert.ToInt32(versionBSplit[2]);
            }

            if (versionASplit.Length > 3 && versionBSplit.Length > 3)
                return (majorA >= majorB && minorA >= minorB && patchA >= patchB && previewA > previewB);

            return (majorA >= majorB && minorA >= minorB && patchA > patchB);
        }



        public class PackageEntry
        {
            public string name;
            public string version;
        }


    }
}