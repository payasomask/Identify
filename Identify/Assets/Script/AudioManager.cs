using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : IAssetReferenceManager
{
  public static AudioManager _AudioManager = null;

  [SerializeField]
  private List<AudioClip> audio_list = null;

  [SerializeField]
  private AudioMixer[] AudioMixer_list = null;

  private void Awake()
  {
    _AudioManager = this;
    mManagerName = "AudioManager";
  }

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {

  }

  public AudioClip GetAudio(string name)
  {

    foreach (var v in audio_list)
    {
      if (v == null)
        continue;
      if (v.name == name)
        return v;
    }

    return null;
  }

  public AudioMixer GetAudioMixer(string name)
  {
    if (AudioMixer_list == null)
      return null;

    foreach (var v in AudioMixer_list)
    {
      if (v.name == name)
        return v;
    }

    return null;
  }

  //public void setAllAssetReference(string[] assetsGUID)
  //{
  //  List<AssetReference> tmp = new List<AssetReference>();
  //  foreach (var v in assetsGUID)
  //  {
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

  public override void LoadAssetReference(AssetReference ar){
    if (ar == null)
      return;
    if (AssetReferenceStatus_list == null)
      AssetReferenceStatus_list = new List<AssetReferenceStatus>();

    if (audio_list == null)
      audio_list = new List<AudioClip>();
    //string id = System.Guid.NewGuid().ToString();
    AssetReferenceStatus ars = new AssetReferenceStatus();
    ars.ar = ar;
    ars.Loaded = false;

    ars.ar.LoadAssetAsync<AudioClip>().Completed += handle => {
      audio_list.Add(handle.Result);
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
  //public void LoadAsset(){
  //  Debug.Log("549 - AudioManager assetreferences count : " + assetreferences_list.Length);
  //  foreach (var v in assetreferences_list){
  //    LoadAssetReference(v);
  //  }
  //}
}
