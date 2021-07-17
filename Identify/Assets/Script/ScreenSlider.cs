using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//基本上沒幹嘛，就是協助設定一些東西
//目前這個是用在以左上角為座標圓點的物件，要更換要做一定程度的修改
public class ScreenSlider : MonoBehaviour
{
  [SerializeField]
  private Vector2 mask_size;
  [SerializeField]
  private Vector3 mask_position;
  [SerializeField]
  private bool horizontalscorll = false;
  [SerializeField]
  private bool verticalscorll = true;

  public void Start(){
    gameObject.GetComponent<Canvas>().worldCamera = MainLogic._MainLogic.getMainCamera();
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().horizontal = horizontalscorll;
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().vertical = verticalscorll;
    gameObject.transform.Find("Panel_mask").GetComponent<RectTransform>().sizeDelta = mask_size;
    gameObject.transform.Find("Panel_mask").GetComponent<RectTransform>().localPosition = mask_position;

    //gameObject.transform.Find("Panel_mask/Panel/text").GetComponent<RectTransform>().sizeDelta = mask_size;
  }

  //設定滑動內容的高度，會影響上下限，請保所有內容都能呈現的高度
  //最底下是1171.036
  //最上面是-2271.052
  //2271+1171 = *3,442* 
  //實際上的顯示高度會被mask_size.y扣掉
  public void setSilderHigtht(float hight,float disvalue){
    gameObject.transform.Find("Panel_mask/Panel").GetComponent<RectTransform>().sizeDelta = new Vector2(0, hight);
    gameObject.transform.Find("Panel_mask/Panel").GetComponent<RectTransform>().localPosition = new Vector2(0, -hight*0.5f);
    float targetvalue = -hight * 0.5f + disvalue;
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - Mathf.InverseLerp(-hight * 0.5f, hight * 0.5f - mask_size.y, targetvalue);
    //gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    //gameObject.transform.Find("Panel_mask/Panel").GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(0, -hight * 0.5f + disvalue,0), Quaternion.identity);
  }

  public void setVertialNormalizedPosition(float value){
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().verticalNormalizedPosition = 0.5f;
  }

  public void setVertialPosition(float value)
  {
    gameObject.transform.Find("Panel_mask").GetComponent<ScrollRect>().verticalNormalizedPosition = 0.5f;
  }
}
