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
    gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<RectTransform>().sizeDelta = mask_size;
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

    if(verticalscorll)
      setTextHight(t.textBounds.size.y);

    if (horizontalscorll)
      setTextWidth(t.textBounds.size.x);
  }
}
