using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class SpriteManager : IAssetReferenceManager
{
  public static SpriteManager _SpriteManager = null;

  [SerializeField]
  private List<SpriteAtlas> Texture_list = null;

  Dictionary<string, Sprite> mSpriteCache = new Dictionary<string, Sprite>();
  private void Awake()
  {
    _SpriteManager = this;
    mManagerName = "SpriteManager";
  }
  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {

  }

  public Sprite GetSprite(string texname,string spritename)
  {
    string cachekey = texname + "_" + spritename;
    if (mSpriteCache != null && mSpriteCache.ContainsKey(cachekey)){
      return mSpriteCache[cachekey];
    }

    SpriteAtlas targetTex = null;
    foreach (var v in Texture_list){
      if (v.name == texname)
        targetTex = v;
    }

    if (targetTex == null){
      //Debug.Log("864 - cant find Texture2D.name = " + texname);
      return null;
    }

    Sprite target = targetTex.GetSprite(spritename);
    mSpriteCache.Add(cachekey, target);

    return target;
  }

  //public void setAllAssetReference(string[] assetsGUID){
  //  List<AssetReference> tmp = new List<AssetReference>();
  //  foreach (var v in assetsGUID)
  //  {
  //    AssetReference ar = new AssetReference(v);
  //    tmp.Add(ar);
  //  }
  //  assetreferences_list = tmp.ToArray();
  //}
  //public bool IsLoadAssetReferenceDone(){
  //  if(AssetReferenceStatus_list == null){
  //    Debug.Log("need LoadAssetReference() BEFORE check IsLoadAssetReferenceDone()");
  //    return false;
  //  }
  //  return AssetReferenceStatus_list.Count == 0;
  //}
  public override IAssetReferenceManager GetManager()
  {
    return this;
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
  public override void LoadAssetReference(AssetReference ar)
  {
    if (ar == null)
      return;

    if (AssetReferenceStatus_list == null)
      AssetReferenceStatus_list = new List<AssetReferenceStatus>();

    if (Texture_list == null)
      Texture_list = new List<SpriteAtlas>();
    //string id = System.Guid.NewGuid().ToString();
    AssetReferenceStatus ars = new AssetReferenceStatus();
    ars.ar = ar;
    ars.Loaded = false;

    ars.ar.LoadAssetAsync<SpriteAtlas>().Completed += handle =>{
      Texture_list.Add(handle.Result);
      AssetReferenceStatus_list.Remove(ars);
    };

    AssetReferenceStatus_list.Add(ars);
  }
  //public void LoadAsset()
  //{
  //  Debug.Log("549 - SpriteManager assetreferences count : " + assetreferences_list.Length);
  //  foreach(var v in assetreferences_list){
  //    LoadAssetReference(v);
  //  }
  //}
}
