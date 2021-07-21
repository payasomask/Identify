using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//目前只支援水平方向
[RequireComponent(typeof(SpriteRenderer))]
public class SliceBarController : MonoBehaviour
{
  [SerializeField]
  private string BackroundAtlasName;
  [SerializeField]
  private string BackroundSpriteName;
  [SerializeField]
  private string BarAtlasName;
  [SerializeField]
  private string BarSpriteName;

  float bar_width;//bar原圖寬度
  SpriteRenderer bar_sr;

  bool inited = false;
  // Start is called before the first frame update
  void Start(){
    GetComponent<SpriteRenderer>().sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite(BackroundAtlasName, BackroundSpriteName);
    Transform bar_t = transform.Find("bar");
    if (bar_t == null){
      Debug.Log("5134 - SliceBarController need a child GameObject be named <bar>");
      return;
    }
    bar_sr = bar_t.GetComponent<SpriteRenderer>();
    if (bar_sr == null)
    {
      Debug.Log("5134 - SliceBarController need a child GameObject has <SpriteRenderer>");
      return;
    }
    bar_sr.sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite(BarAtlasName, BarSpriteName);
    bar_width = bar_sr.sprite.bounds.size.x;
    inited = true;
  }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void setNormalized(float percent){
    if (inited == false)
      return;
    bar_sr.size = new Vector2(Mathf.Lerp(0.0f, bar_width, percent), bar_sr.size.y);
  }
}
