using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class IAssetReferenceManager : MonoBehaviour
{
  [SerializeField]
  protected AssetReference[] assetreferences_list = null;

  protected List<AssetReferenceStatus> AssetReferenceStatus_list = null;
  protected string mManagerName;

  public void setAllAssetReference(string[] assetsGUID)
  {
    List<AssetReference> tmp = new List<AssetReference>();
    foreach (var v in assetsGUID)
    {
      AssetReference ar = new AssetReference(v);
      tmp.Add(ar);
    }
    assetreferences_list = tmp.ToArray();
  }
  public abstract bool IsLoadAssetReferenceDone();
  public abstract void LoadAssetReference(AssetReference ar);

  public void LoadAsset()
  {
    Debug.Log("549 - " + mManagerName + " assetreferences count : " + assetreferences_list.Length);
    foreach (var v in assetreferences_list)
    {
      LoadAssetReference(v);
    }
  }

  public abstract IAssetReferenceManager GetManager();
}
