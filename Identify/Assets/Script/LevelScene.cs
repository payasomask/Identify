using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelScene : MonoBehaviour,IScene
{

  string secneName;
  SceneDisposeHandler pDisposeHandler = null;
  GameObject root = null;
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

  public void initScene(string name, object[] extra_param = null){
    //...

    GameObject dynamicObj = gameObject;

    //
    // Intro prefab
    //
    root = GameObject.Find("Level");
    if (root == null){
      root = instantiateObject(dynamicObj, "Level");
    }

    AudioController._AudioController.crossFadeBGM("BGM_25",true);
    buildLevel_sit();
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
      if(name == "back_bt"){
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
    if(Input.GetKeyUp(KeyCode.R)){
      //root.transform.Find("ScreenSlider").GetComponent<ScreenSlider>().setVertialNormalizedPosition(500.0f);

    }
  } 

  GameObject instantiateObject(GameObject parent, string name){
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  void buildLevel_sit(){
    //計算總高
    float totalhight = 0;
    int levelcount = JsonLoader._JsonLoader.gettotallevelcount();
    float row_interval = 33.0f;
    int columns = 4;
    int row = levelcount / columns;
    if (levelcount % columns > 0)
      row++;
    float total_interval = (row-1) * row_interval;

    float column_interval = 33;

    //int targetlevel = 13;
    int targetlevel = PlayerPrefsManager._PlayerPrefsManager.currentmaxlevel;
    int targetrow = 0;

    GameObject sliderroot = root.transform.Find("ScreenSlider").gameObject;

    //希望這個是能拿到scale..
    float scale= sliderroot.GetComponent<CanvasScaler>().scaleFactor;

    Debug.Log("5481 - canvasScaler scale : " + scale);

    GameObject level_root = sliderroot.transform.Find("Panel_mask/Panel").gameObject;
    int level = 1;
    int currentmaxlevel = PlayerPrefsManager._PlayerPrefsManager.currentmaxlevel;
    int row_index = 0;
    float row_hight = 0;//單單只有prefab的高度
    //座標系統是由左上開始排
    for (int i = 0; i < row; i++){
      for(int j = 0; j < columns; j++){

        if (level > levelcount)
          break;

        if (targetlevel == level)
          targetrow = row_index;

        GameObject level_bt = instantiateObject(level_root, "level_bt");
        Vector2 level_size = level_bt.GetComponent<RectTransform>().sizeDelta;
        row_hight = level_bt.GetComponent<RectTransform>().sizeDelta.y;

        //CanvsaScaler 設定expand的時候 ， 動態掛上去的UI物件scale會被調整，要調回來1.0
        level_bt.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Button bt = level_bt.transform.Find("Button").GetComponent<Button>();
        level_bt.transform.Find("Button/text").GetComponent<TextMeshProUGUI>().text = 
          "Level" + level+"\n" + PlayerPrefsManager._PlayerPrefsManager.GetRecordStar(level.ToString());

        float x = level_size.x * 0.5f + level_size.x  * j + column_interval * j;
        float y = -level_size.y * 0.5f - level_size.y * i - row_interval * i;

        level_bt.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
        bt.name = level.ToString();

        //不能玩的關卡灰掉
        if (level > currentmaxlevel){
          bt.interactable = false;
          //更換圖
          bt.image.sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("common", "ButtonLevelG");
        }

        bt.onClick.AddListener(()=> {
          OnLevel_bt_Click(bt.name);
        });

        level++;
      }
      totalhight += row_hight;
      row_index++;
    }
    totalhight = totalhight + total_interval;

    sliderroot.GetComponent<ScreenSlider>().setSilderHigtht(totalhight, targetrow * row_hight + targetrow * row_interval);
  }

  void OnLevel_bt_Click(string level){
    Debug.Log("level : " + level + "，was been clicked");

    PlayerPrefsManager._PlayerPrefsManager.currentlevel = int.Parse(level);
    //進入關卡
    pDisposeHandler(SceneDisposeReason.USER_ACTION,null);
  }
}
