using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PrefabManager : IAssetReferenceManager
{
  public static PrefabManager _PrefabManager = null;

  [SerializeField]
  private List<GameObject> prefab_list = null;

  [SerializeField]
  private List<GameObject> staticprefab_list = null;
  // Start is called before the first frame update
  void Start()
    {
    _PrefabManager = this;
    mManagerName = "PrefabManager";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public GameObject GetPrefab(string name)
  {

    foreach (var v in prefab_list)
    {
      if (v == null)
        continue;
      if (v.name == name)
        return v;
    }

    foreach (var v in staticprefab_list)
    {
      if (v == null)
        continue;
      if (v.name == name)
        return v;
    }

    return null;
  }

  //public void setAllAssetReference(string[] assetsGUID){
  //  List<AssetReference> tmp = new List<AssetReference>();
  //  foreach(var v in assetsGUID){
  //    AssetReference ar = new AssetReference(v);
  //    tmp.Add(ar);
  //  }
  //  assetreferences_list = tmp.ToArray();
  //}
  //public bool IsLoadAssetReferenceDone()
  //{
  //  if (AssetReferenceStatus_list == null)
  //  {
  //    Debug.Log("need LoadAssetReference() BEFORE check IsLoadAssetReferenceDone()");
  //    return false;
  //  }
  //  return AssetReferenceStatus_list.Count == 0;
  //}
  public override void LoadAssetReference(AssetReference ar)
  {
    if (AssetReferenceStatus_list == null)
      AssetReferenceStatus_list = new List<AssetReferenceStatus>();

    if (prefab_list == null)
      prefab_list = new List<GameObject>();

    if (ar == null)
      return;
    //string id = System.Guid.NewGuid().ToString();
    AssetReferenceStatus ars = new AssetReferenceStatus();
    ars.ar = ar;
    ars.Loaded = false;

    ars.ar.LoadAssetAsync<GameObject>().Completed += handle => {
      prefab_list.Add(handle.Result);
      AssetReferenceStatus_list.Remove(ars);
    };
    AssetReferenceStatus_list.Add(ars);
  }
  public override bool IsLoadAssetReferenceDone()
  {
    if (AssetReferenceStatus_list == null)
    {
      Debug.Log("513 - " + mManagerName + " need LoadAssetReference() BEFORE check IsLoadAssetReferenceDone()");
      return false;
    }
    return AssetReferenceStatus_list.Count == 0;
  }
  public override IAssetReferenceManager GetManager()
  {
    return this;
  }

  //public void LoadAsset()
  //{
  //  Debug.Log("549 - PrefabManager assetreferences count : " + assetreferences_list.Length);
  //  foreach (var v in assetreferences_list)
  //  {
  //    LoadAssetReference(v);
  //  }
  //}

  //public AssetReference GetAssetPrefab(string name)
  //{

  //  foreach (var v in prefab_list)
  //  {
  //    if (v == null)
  //      continue;
  //    if (v.Asset.name == name)
  //      return v;
  //  }

  //  return null;
  //}
}
