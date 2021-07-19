using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MLSpirte : MonoBehaviour
{

  [SerializeField]
  private bool initSpirteKey = true;
  [SerializeField]
  private string atlasname;
  [SerializeField]
  private string spritename;

  // Start is called before the first frame update
  void Start(){

    if (initSpirteKey == false)
      return;
    string Lang = PlayerPrefsManager._PlayerPrefsManager.Language;
    gameObject.GetComponent<SpriteRenderer>().sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite(atlasname, spritename + "_" + Lang);

    }


    // Update is called once per frame

}
