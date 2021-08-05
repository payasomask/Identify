using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class EditorUtilityHelper 
{
  [MenuItem("UtilityHelper/assign all Asset to Manager")]
  static void assignAssetToManager()
  {
    GameObject obj = null;
    //if (obj == null){
    //  Debug.LogError("Cant Find AssetAddressbleManager GameObject in Scene");
    //  return;
    //}
    var names = AssetDatabase.GetAllAssetBundleNames();
    Dictionary<string, List<string>> type_dic = new Dictionary<string, List<string>>();

    foreach (string name in names){
      //Debug.Log("Asset Bundle: " + name);
      var assetpaths = AssetDatabase.GetAssetPathsFromAssetBundle(name);
      foreach (string path in assetpaths){
        //Debug.Log("Asset Path: " + path);
        string asset_GUID = AssetDatabase.AssetPathToGUID(path);
        if (path.Contains(".prefab")){
          if (type_dic.ContainsKey("prefab") == false){
            type_dic.Add("prefab", new List<string>());
          }
          type_dic["prefab"].Add(asset_GUID);
        }
        else if(path.Contains(".spriteatlas")){
          if (type_dic.ContainsKey("spriteatlas") == false){
            type_dic.Add("spriteatlas", new List<string>());
          }
          type_dic["spriteatlas"].Add(asset_GUID);
        }
        else if (path.Contains(".mp3")){
          if (type_dic.ContainsKey("audio") == false){
            type_dic.Add("audio", new List<string>());
          }
          type_dic["audio"].Add(asset_GUID);
        }
        else if (path.Contains(".Json"))
        {
          if (type_dic.ContainsKey("json") == false)
          {
            type_dic.Add("json", new List<string>());
          }
          type_dic["json"].Add(asset_GUID);
        }

        else
        {
          Debug.LogWarning("detect not handle path : " + path);
        }
        //string[] dependencies = AssetDatabase.GetDependencies(path);
        //if (dependencies.Length > 0)
        //{
        //  foreach (string dependen in dependencies)
        //  {
        //    Debug.Log("dependen Asset Path: " + dependen);
        //  }
        //}
      }
    }

    if(type_dic.Count == 0){
      Debug.LogWarning("please set asset bundlename Manualy from asset inspector");
      return;
    }

    obj = GameObject.Find("PrefabManager");
    if (obj == null){
      Debug.LogError("Cant Find PrefabManager GameObject in Scene,Skip...");
    }
    else{
      if(type_dic.ContainsKey("prefab") == false)
        Debug.Log("Dont find any Asset Be set prefab BundleName");
      else{
        PrefabManager pm = obj.GetComponent<PrefabManager>();
        pm.setAllAssetReference(type_dic["prefab"].ToArray());
      }
    }

    obj = GameObject.Find("SpriteManager");
    if (obj == null)
    {
      Debug.LogError("Cant Find SpriteManager GameObject in Scene");
    }
    else
    {
      if (type_dic.ContainsKey("spriteatlas") == false)
        Debug.Log("Dont find any Asset Be set spriteatlas BundleName");
      else
      {
        SpriteManager sm = obj.GetComponent<SpriteManager>();
        sm.setAllAssetReference(type_dic["spriteatlas"].ToArray());
      }
    }

    obj = GameObject.Find("AudioManager");
    if (obj == null)
    {
      Debug.LogError("Cant Find AudioManager GameObject in Scene");
    }
    else
    {
      if (type_dic.ContainsKey("audio") == false)
        Debug.Log("Dont find any Asset Be set audio BundleName");
      else
      {
        AudioManager am = obj.GetComponent<AudioManager>();
        am.setAllAssetReference(type_dic["audio"].ToArray());
      }
    }

    obj = GameObject.Find("JsonLoader");
    if (obj == null)
    {
      Debug.LogError("Cant Find JsonLoader GameObject in Scene");
    }
    else
    {
      if (type_dic.ContainsKey("json") == false)
        Debug.Log("Dont find any Asset Be set json BundleName");
      else{
        if (type_dic["json"].Count > 1)
          Debug.LogWarning("Detect json AssetBundleName > 1");
        //json 理論上只會有一個
        JsonLoader am = obj.GetComponent<JsonLoader>();
        am.setAllAssetReference(type_dic["json"].ToArray());
      }
    }

    //AssetAddressableManager aam = obj.GetComponent<AssetAddressableManager>();
    //aam.setAllAssetReference(assetsGUID.ToArray());
    Debug.Log("setAllAssetReference Done");
  }
}
