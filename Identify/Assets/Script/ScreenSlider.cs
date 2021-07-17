using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//�򥻤W�S�F���A�N�O��U�]�w�@�ǪF��
//�ثe�o�ӬO�Φb�H���W�����y�ж��I������A�n�󴫭n���@�w�{�ת��ק�
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

  //�]�w�ưʤ��e�����סA�|�v�T�W�U���A�ЫO�Ҧ����e����e�{������
  //�̩��U�O1171.036
  //�̤W���O-2271.052
  //2271+1171 = *3,442* 
  //��ڤW����ܰ��׷|�Qmask_size.y����
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
