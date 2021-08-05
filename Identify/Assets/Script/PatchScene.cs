using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatchScene : MonoBehaviour,IScene
{
  string secneName;
  SceneDisposeHandler pDisposeHandler = null;
  private bool mInited = false;
  GameObject Root;

  List<IAssetReferenceManager> needLoad_list = new List<IAssetReferenceManager>();

  enum status{
    Null,
    check,
    wait_check,
    need_downlaod,
    download,
    wait_download,
    download_done,
    download_fiald,
    goback_intro,
    assetpullcompleted,
    loadasset,
    wait_manager_loadasset,
    loadasset_done,
    wait
  }
  status currentstatus = status.Null;
  public void disposeScene(bool forceDispose)
  {
    pDisposeHandler = null;
  }

  public int getSceneInitializeProgress()
  {
    return 0;
  }

  public string getSceneName()
  {
    return secneName;
  }

  public void initLoadingScene(string name, object[] extra_param = null)
  {
    secneName = name;
  }

  public void initScene(string name, object[] extra_param = null)
  {
    //...

    GameObject dynamicObj = gameObject;

    //
    // Intro prefab
    //
    Root = GameObject.Find("Patch");
    if (Root == null){
      Root = instantiateObject(dynamicObj, "Patch");
    }
    Root.transform.Find("patchtext").GetComponent<TextMeshPro>().text = "checkpatch..";
    currentstatus = status.check;
    mInited = true;
    return;
  }

  public bool isSceneDisposed()
  {
    return (pDisposeHandler == null);
  }

  public bool isSceneInitialized()
  {
    return mInited;
  }

  public void registerSceneDisposeHandler(SceneDisposeHandler pHandler)
  {
    pDisposeHandler = pHandler;
  }

  public void setUIEvent(string name, UIEventType type, object[] extra_info)
  {

  }

  GameObject instantiateObject(GameObject parent, string name)
  {
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  void Update(){
    if(currentstatus == status.check){
      Root.transform.Find("patchtext").GetComponent<TextMeshPro>().text = "check..";
      AssetAddressableManager._AssetAddressableManager.CheckDownLoad(
        () => {
          //need update
          currentstatus = status.need_downlaod;
        },
        () => {
          //dont need update
          //轉跳畫面
          currentstatus = status.assetpullcompleted;
        }
        );
      currentstatus = status.wait_check;
    }
    else if(currentstatus == status.need_downlaod){
      UIDialog._UIDialog.show(new NormalDialog("need download...", new InteractiveDiaLogHandler[] {
          ()=>{
            //接受下載
            currentstatus = status.download;
          },
          ()=>{
          //不接受下載
          AssetAddressableManager._AssetAddressableManager.ReleaseDownLoad();
          //踢回去intro
          currentstatus = status.goback_intro;
          }
          }));
      currentstatus = status.wait;
    }
    else if(currentstatus == status.assetpullcompleted){
      Root.transform.Find("patchtext").GetComponent<TextMeshPro>().text = "load asset..";
      //pDisposeHandler(SceneDisposeReason.USER_ACTION, new object[] { });
      currentstatus = status.loadasset;
    }
    else if(currentstatus == status.loadasset){
      //可能會根據真的有更新的資源做load管理?
      //還是不管怎樣都要load不太清楚ㄟ..
      JsonLoader._JsonLoader.LoadAsset();
      AudioManager._AudioManager.LoadAsset();
      SpriteManager._SpriteManager.LoadAsset();
      PrefabManager._PrefabManager.LoadAsset();
      needLoad_list.Add(JsonLoader._JsonLoader.GetManager());
      needLoad_list.Add(AudioManager._AudioManager.GetManager());
      needLoad_list.Add(SpriteManager._SpriteManager.GetManager());
      needLoad_list.Add(PrefabManager._PrefabManager.GetManager());
      currentstatus = status.wait_manager_loadasset;
    }
    else if(currentstatus == status.wait_manager_loadasset){
      for(int i  = 0; i < needLoad_list.Count; i++){
        if (needLoad_list[i].IsLoadAssetReferenceDone() == false)
          return;
      }
      currentstatus = status.loadasset_done;
    }
    else if(currentstatus == status.loadasset_done){
      pDisposeHandler(SceneDisposeReason.USER_ACTION, new object[] { });
      currentstatus = status.wait;
    }
    else if(currentstatus == status.goback_intro){
      pDisposeHandler(SceneDisposeReason.USER_EXIT, new object[] { });
      currentstatus = status.wait;
    }
    else if(currentstatus == status.download){
      Root.transform.Find("patchtext").GetComponent<TextMeshPro>().text = "download..";
      AssetAddressableManager._AssetAddressableManager.DownLoad(
        ()=> {
          currentstatus = status.download_done;
        },
        () => {
          currentstatus = status.download_fiald;

        }
        );
      currentstatus = status.wait_download;
    }
    else if(currentstatus == status.download_done){
      currentstatus = status.assetpullcompleted;
      return;
    }else if(currentstatus == status.download_fiald){
      //失敗的話..重試?
      UIDialog._UIDialog.show(new NormalDialog("download failed...", new InteractiveDiaLogHandler[] {
          ()=>{
            //重新嘗試下載
            currentstatus = status.download;
          },
          ()=>{
          //不接受下載
          AssetAddressableManager._AssetAddressableManager.ReleaseDownLoad();
          //踢回去intro
          currentstatus = status.goback_intro;
          }
          }));
      currentstatus = status.wait;
    }
  }

  void updateUI(){
    Sprite sound_s = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("common", "Sound" + PlayerPrefsManager._PlayerPrefsManager.Music_T);
    Root.transform.Find("LobbyUI/sound_bt/icon").GetComponent<SpriteRenderer>().sprite = sound_s;
  }
}
