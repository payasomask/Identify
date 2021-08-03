using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllClearScene : MonoBehaviour,IScene
{

  string secneName;
  SceneDisposeHandler pDisposeHandler = null;
  bool mInited = false;

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

  public void initLoadingScene(string name, object[] extra_param = null){
    secneName = name;
  }

  GameObject Root = null;
  public void initScene(string name, object[] extra_param = null){
    //...

    GameObject dynamicObj = gameObject;

    //
    // Intro prefab
    //
     Root = GameObject.Find("AllClear");
    if (Root == null){
      Root = instantiateObject(dynamicObj, "AllClear");
    }

    InitBg();
    Root.transform.Find("text").GetComponent<TextMeshPro>().text = getAllClearString();

    AudioController._AudioController.crossFadeBGM("BGM_05");

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

  public void setUIEvent(string name, UIEventType type, object[] extra_info){
    if(type == UIEventType.BUTTON){
      if(name == "black"){
        //�o��levelscene�~�|�^��̤W��
        PlayerPrefsManager._PlayerPrefsManager.currentlevel = 1;
        pDisposeHandler(SceneDisposeReason.USER_EXIT, null);
      }
    }
  }

  // Start is called before the first frame update
  void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
    
  }

  void InitBg()
  {

    int width = 9;
    int hight = 12;
    float cellsize = 200.0f;

    GameLogic.Point[,] bgpoints = new GameLogic.Point[hight, width];
    GameObject point_parent = Root.transform.Find("Bg").gameObject;
    Vector2 offset_pivot = UtilityHelper.MazePoint(width * cellsize, hight * cellsize);
    float pointsize = 0.8f;

    for (int i = 0; i < hight; i++)
    {
      for (int j = 0; j < width; j++)
      {
        int index = i * width + j;
        GameObject point = instantiateObject(point_parent, "Point");
        Vector2 pointposition = new Vector2(j * cellsize + cellsize * 0.5f, i * cellsize + cellsize * 0.5f) - offset_pivot;
        point.transform.localPosition = new Vector3(pointposition.x, pointposition.y);
        point.transform.localScale = new Vector3(cellsize * pointsize, cellsize * pointsize, 1.0f);
        point.transform.Find("Icon").GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1.0f, 0f, 1.0f, 0.6f, 1.0f);
      }
    }

    updateBg();
  }

  //�O���H���@���I�b�餺�A�åB�i�H�����]�w���ɶ�
  void updateBg()
  {
    float circle_radius = 100.0f;
    //float speed = 10.0f;
    float duration = UtilityHelper.Random(2.5f, 3.0f);
    float move_need_dis = /*speed **/ duration;

    //���p��ثe���S����Z��
    Vector3 startposition = Vector3.zero;
    Vector3 currentposition = Root.transform.Find("Bg").localPosition;
    Vector2 circlepostion = Random.insideUnitCircle * circle_radius;
    Vector3 Targetpostion = new Vector3(circlepostion.x + startposition.x, circlepostion.y + startposition.y, startposition.z);
    float movedis = (Targetpostion - currentposition).magnitude;

    if (movedis < move_need_dis)
    {
      //�Z���������թԪ��Z��
      Vector3 newtargetposition = (Targetpostion - startposition).normalized * move_need_dis;
      float tartget_form_circle_poiont = (newtargetposition - startposition).magnitude;
      if (tartget_form_circle_poiont >= circle_radius)
      {
        //���O���p�o���I�b��~�A���N�N�o���I�Ԧ^�ꤺ
        newtargetposition = (startposition - newtargetposition).normalized * circle_radius;
        movedis = (newtargetposition - currentposition).magnitude;
        //�b�P�_�@���Z��==move_need_dis
        if (movedis < move_need_dis)
        {
          //���M���������N��taget��V���ʨ쨬���Z��
          newtargetposition = (newtargetposition - currentposition).normalized * move_need_dis;
        }
        else
        {
          //�Z�������i�H�ϥ�
          Targetpostion = newtargetposition;
        }
      }
      else
      {
        //���I�O�b��d�򤺡A���N�i�H�ϥγo���I
        Targetpostion = newtargetposition;
      }
    }

    SinePosition sp = Root.transform.Find("Bg").GetComponent<SinePosition>();
    sp.move(Targetpostion, duration, () => {
      updateBg();
      return;
    });
    return;
  }

  string getAllClearString(){
    string text = JsonLoader._JsonLoader.GetString("701");
    text += JsonLoader._JsonLoader.GetString("702");
    text += JsonLoader._JsonLoader.GetString("703");
    return text;
  }

  GameObject instantiateObject(GameObject parent, string name){
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }
}
