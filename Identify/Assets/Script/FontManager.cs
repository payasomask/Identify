using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontManager : MonoBehaviour
{
  public static FontManager _FontManager = null;

  TMP_FontAsset current_font = null;

  //[SerializeField]
  //private TMP_FontAsset[] Font_list = null;
  [System.Serializable]
  public class fontdata{
    public string key;
    public TMP_FontAsset font;
  }
  public List<fontdata> font_list = new List<fontdata>();
  private void Awake()
  {
    _FontManager = this;
  }
  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void init(){

    string Lang = PlayerPrefsManager._PlayerPrefsManager.Language;

    foreach (var v in font_list){
      if(v.key == Lang){
        current_font = v.font;
      }
    }
  }

  public TMP_FontAsset getFont(TextMeshPro TMP, bool InstanceMaterial = true)
  {
    //TMP_FontAsset tmp = mCxt.mABL.InstantiateFontAsset("font_" + mCxt.mPPM.Language.ToLower());
    TMP.font = current_font;
    if (InstanceMaterial)
      TMP.fontSharedMaterial = current_font.material;

    //default setup is Mask Soft (The Mask Off setup let the compiler strips its function after code compilation)
    //restore back to Mask Off
    //TMP.maskType = MaskingTypes.MaskOff;
    return current_font;
  }
  public TMP_FontAsset getFont(TextMeshProUGUI TMP, bool InstanceMaterial = true)
  {
    //TMP_FontAsset tmp = mCxt.mABL.InstantiateFontAsset("font_" + mCxt.mPPM.Language.ToLower());
    TMP.font = current_font;
    if (InstanceMaterial)
      TMP.fontSharedMaterial = current_font.material;

    //default setup is Mask Soft (The Mask Off setup let the compiler strips its function after code compilation)
    //restore back to Mask Off
    //TMP.maskType = MaskingTypes.MaskOff;
    return current_font;
  }

}
