using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;

//"https://drive.google.com/drive/folders/1ZV0AHeTAil3fPORYYKVBEL1BVei6g71P?usp=sharing"
public class AssetAddressableManager : MonoBehaviour
{
  public static AssetAddressableManager _AssetAddressableManager = null;

  List<object> _updateKeys = null;
  CommonAction OnNeedUpdate = null;
  CommonAction OnDontNeedUpdate = null;
  CommonAction OnDownLoadSuccess = null;
  CommonAction OnDownLoadFailed = null;
  AsyncOperationHandle<long> downloadsize;

  //enum status{
  //  idle,
  //  CheckCatalog,
  //  CheckCatalog_done,
  //  CheckCatalog_Failed,

  //  DownLoad,
  //  DownLoad_done,
  //  DownLoad_Failed,

  //}

  //status currentstatus = status.idle;
  private void Awake()
  {
    _AssetAddressableManager = this;
  }

  IEnumerator CheckCatalog()
  {

    var handle = Addressables.CheckForCatalogUpdates(false);
    yield return handle;
    Debug.Log("check catalog status " + handle.Status);
    if (handle.Status == AsyncOperationStatus.Succeeded)
    {
      List<string> catalogs = handle.Result;
      if (catalogs != null && catalogs.Count > 0)
      {
        foreach (var catalog in catalogs)
        {
          Debug.Log("catalog  " + catalog);
        }
        Debug.Log("download catalog start ");
        var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
        yield return updateHandle;
        foreach (var item in updateHandle.Result)
        {
          Debug.Log("catalog result " + item.LocatorId);
          foreach (var key in item.Keys)
          {
            Debug.Log("catalog key " + key);
          }
          _updateKeys.AddRange(item.Keys);
        }
        Debug.Log("download catalog finish " + updateHandle.Status);
        downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
        yield return downloadsize;
        //currentstatus = status.CheckCatalog_done;
        if (OnNeedUpdate != null){
          OnNeedUpdate();
          OnNeedUpdate = null;
        }
      }
      else
      {
        //currentstatus = status.CheckCatalog_done;
        Debug.Log("dont need update catalogs");
        if (OnDontNeedUpdate != null){
          OnDontNeedUpdate();
          OnDontNeedUpdate = null;
        }
      }
    }
    else if(handle.Status == AsyncOperationStatus.Failed){
      //currentstatus = status.CheckCatalog_Failed;
    }
    Addressables.Release(handle);
  }

  IEnumerator DownAssetImpl()
  {
    Debug.Log("start download size :" + downloadsize.Result);

    if (downloadsize.Result > 0)
    {
      var download = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
      yield return download;
      if(download.Status == AsyncOperationStatus.Succeeded){
        Debug.Log("download result type " + download.Result.GetType());
        foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>){
          var ab = item.GetAssetBundle();
          Debug.Log("ab name " + ab.name);
          foreach (var name in ab.GetAllAssetNames()){
            Debug.Log("asset name " + name);
          }
        }

        //currentstatus = status.DownLoad_done;
        if (OnDownLoadSuccess != null){
          OnDownLoadSuccess();
          OnDownLoadSuccess = null;
          //只有在成功的時候卸載downloadsize
          Addressables.Release(downloadsize);
        }

      }
      else if(download.Status == AsyncOperationStatus.Failed){
        //currentstatus = status.DownLoad_Failed;
        if(OnDownLoadFailed != null){
          OnDownLoadFailed();
          OnDownLoadFailed = null;
        }
      }
      Addressables.Release(download);
    }
    //else{
    //  //currentstatus = status.DownLoad_done;
    //  if (OnDownLoadSuccess != null){
    //    OnDownLoadSuccess();
    //    OnDownLoadSuccess = null;
    //    //只有在成功的時候卸載downloadsize
    //    Addressables.Release(downloadsize);
    //  }
    //}
  }

  public void DownLoad(CommonAction onsuccess, CommonAction onfailed){
    //currentstatus = status.DownLoad;
    StartCoroutine(DownAssetImpl());
    OnDownLoadSuccess = onsuccess;
    OnDownLoadFailed = onfailed;
  }

  public void CheckDownLoad(CommonAction onneed, CommonAction ondontneed){
    //currentstatus = status.CheckCatalog;
    StartCoroutine(CheckCatalog());
    OnNeedUpdate = onneed;
    OnDontNeedUpdate = ondontneed;
  }

  //不接受的時候也要卸載
  public void ReleaseDownLoad(){
    Addressables.Release(downloadsize);
  }

  public long GetDonwLoadSize()
  {
    return downloadsize.Result;
  }
}
