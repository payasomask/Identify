using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//基本上沒幹嘛，就是協助設定一些東西
public class ScreenSliderTextMesh : MonoBehaviour
{
  [SerializeField]
  private Vector2 mask_size;
  [SerializeField]
  private Vector3 mask_position;
  [SerializeField]
  private bool horizontalscorll = false;
  [SerializeField]
  private bool verticalscorll = true;
  TextMeshProUGUI t;

  public void Start(){
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().horizontal = horizontalscorll;
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().vertical = verticalscorll;
    gameObject.transform.Find("Panel_mask").GetComponent<RectTransform>().sizeDelta = mask_size;
    gameObject.transform.Find("Panel_mask").GetComponent<RectTransform>().localPosition = mask_position;
    t = gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<TextMeshProUGUI>();
  }

  public void setTextSize(Vector2 size){
    gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<RectTransform>().sizeDelta = size;
  }

  public void setTextHight(float hight){
    gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<RectTransform>().sizeDelta = new Vector2(mask_size.x, hight);
  }
  public void setTextWidth(float width){
    gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<RectTransform>().sizeDelta = new Vector2(width, mask_size.y);
  }

  public void setTextMargins(Vector4 size){
    t.margin = size;
  }

  public void setText(string text){
    if(t == null)
      t = gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<TextMeshProUGUI>();
    t.text = text;
    //必須確保該物件是active狀態，才能更新
    t.ForceMeshUpdate();
    //update之後針對該"字串"取得實際的height(並不會針對目前被賦予的t.text給你正確的高度，要傳入string)
    //Debug.Log("preferredHeight after text mesh update : " + t.GetPreferredValues(text).y);

    if (verticalscorll)
      setTextHight(t.GetPreferredValues(text).y);

    if (horizontalscorll)
      setTextWidth(t.GetPreferredValues(text).x);
  }

  public void setVerticalNormalizedPosition(float value)
  {
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().verticalNormalizedPosition = value;
  }
}
