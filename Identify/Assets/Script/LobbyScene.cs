using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviour,IScene
{
  string secneName;
  SceneDisposeHandler pDisposeHandler = null;
  private bool mInited = false;
  GameObject Root;
  enum State
  {
    NULL = 0,
    DONE,
  }

  State currentstate = State.NULL;

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

  public void initLoadingScene(string name, object[] extra_param = null)
  {
    secneName = name;
  }

  public void initScene(string name, object[] extra_param = null)
  {
    //...

    GameObject dynamicObj = gameObject;

    //
    // Intro prefab
    //
    Root = GameObject.Find("Lobby");
    if (Root == null){
      Root = instantiateObject(dynamicObj, "Lobby");
    }

    InitBg();

    mInited = true;

    updateUI();


    GameObject RecordUI = Root.transform.Find("RecordUI").gameObject;

    AdsHelper._AdsHelper.RequestBannerAds(GoogleMobileAds.Api.AdPosition.Top,null);
    //AdsHelper._AdsHelper.RequestInterstitialAds();

    //UIDialog._UIDialog.show(new summaryDialog(1f,3, null));

    ShowLobby();
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

  public void setUIEvent(string name, UIEventType type, object[] extra_info)
  {
    if(type == UIEventType.BUTTON){
      if (name == "play_bt"){
        //強制出現教學
        PlayerPrefsManager._PlayerPrefsManager.tutorial = "off";
        if (PlayerPrefsManager._PlayerPrefsManager.tutorial == "off"){
          //顯示一張教學圖
          //..
          Root.transform.Find("LobbyUI/tutorail").gameObject.SetActive(true);
          return;
        }
        else
          pDisposeHandler(SceneDisposeReason.USER_ACTION, null);

      }
      else if(name == "tutorail_Icon"){
        PlayerPrefsManager._PlayerPrefsManager.tutorial = "on";
        pDisposeHandler(SceneDisposeReason.USER_ACTION, null);
        return;
      }
      else
      if (name == "record_bt")
      {
        //設定一些東西
        GameObject slidertext = Root.transform.Find("RecordUI/ScreenSliderTextMesh").gameObject;
        slidertext.GetComponent<ScreenSliderTextMesh>().setText(getRecordText());
        ShowRecord();
      }
      else if (name == "staff_bt")
      {
        //設定一些東西
        GameObject text = Root.transform.Find("StaffUI/text").gameObject;
        text.GetComponent<TextMeshPro>().text = getStaffText();
        ShowStaff();
      }
      else if (name == "Staff_back_bt")
      {
        ShowLobby();
      }
      else if (name == "Record_back_bt")
      {
        ShowLobby();
      }
    }

  }

  GameObject instantiateObject(GameObject parent, string name)
  {
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  void updateUI(){

  }

  void InitBg(){

    int width = 9;
    int hight = 12;
    float cellsize = 200.0f;

    GameLogic.Point[,] bgpoints = new GameLogic.Point[hight, width];
    GameObject point_parent = Root.transform.Find("Bg").gameObject;
    Vector2 offset_pivot = UtilityHelper.MazePoint(width*cellsize, hight * cellsize);
    float pointsize = 0.8f;

      for (int i = 0; i < hight; i++){
        for (int j = 0; j < width; j++){
          int index = i * width + j;
        GameObject point = instantiateObject(point_parent, "Point");
        Vector2 pointposition = new Vector2(j * cellsize + cellsize * 0.5f, i * cellsize + cellsize * 0.5f) - offset_pivot;
          point.transform.localPosition = new Vector3(pointposition.x, pointposition.y);
          point.transform.localScale = new Vector3(cellsize* pointsize, cellsize * pointsize, 1.0f);
          point.transform.Find("Icon").GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f,1.0f,0f,1.0f,0.6f,1.0f);
        }
      }

    updateBg();
  }

 //保證隨機一個點在圈內，並且可以滿足設定的時間
  void updateBg(){
    float circle_radius = 100.0f;
    //float speed = 10.0f;
    float duration = UtilityHelper.Random(2.5f, 3.0f);
    float move_need_dis = /*speed **/ duration;

    //此計算目前都沒有管Z值
    Vector3 startposition = Vector3.zero;
    Vector3 currentposition = Root.transform.Find("Bg").localPosition;
    Vector2 circlepostion = Random.insideUnitCircle * circle_radius;
    Vector3 Targetpostion = new Vector3(circlepostion.x + startposition.x, circlepostion.y + startposition.y, startposition.z);
    float movedis = (Targetpostion - currentposition).magnitude;

    if(movedis < move_need_dis){
      //距離不夠嘗試拉長距離
      Vector3 newtargetposition = (Targetpostion - startposition).normalized * move_need_dis;
      float tartget_form_circle_poiont = (newtargetposition - startposition).magnitude;
      if(tartget_form_circle_poiont >= circle_radius){
        //但是假如這個點在圓外，那就將這個點拉回圓內
        newtargetposition = (startposition - newtargetposition).normalized * circle_radius;
        movedis = (newtargetposition - currentposition).magnitude;
        //在判斷一次距離==move_need_dis
        if (movedis < move_need_dis){
          //仍然不夠遠那就往taget方向移動到足夠距離
          newtargetposition = (newtargetposition - currentposition).normalized * move_need_dis;
        }
        else{
          //距離足夠可以使用
          Targetpostion = newtargetposition;
        }
      }
      else
      {
        //該點是在圓範圍內，那就可以使用這個點
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

  void ShowRecord(){
    AudioController._AudioController.crossFadeBGM("BGM_37", true);
    Root.transform.Find("LobbyUI").gameObject.SetActive(false);
    Root.transform.Find("RecordUI").gameObject.SetActive(true);
    Root.transform.Find("StaffUI").gameObject.SetActive(false);
  }

  void ShowLobby(){
    AudioController._AudioController.crossFadeBGM("BGM_25", true);
    Root.transform.Find("LobbyUI").gameObject.SetActive(true);
    Root.transform.Find("RecordUI").gameObject.SetActive(false);
    Root.transform.Find("StaffUI").gameObject.SetActive(false);
    Root.transform.Find("LobbyUI/record_bt").gameObject.SetActive(PlayerPrefsManager._PlayerPrefsManager.hasRecord());
  }
  void ShowStaff(){
    AudioController._AudioController.crossFadeBGM("BGM_51", true);
    Root.transform.Find("LobbyUI").gameObject.SetActive(false);
    Root.transform.Find("RecordUI").gameObject.SetActive(false);
    Root.transform.Find("StaffUI").gameObject.SetActive(true);
  }

  string getRecordText(){


    Dictionary<string,GameRecord> record = PlayerPrefsManager._PlayerPrefsManager.Record;

    float totalusedtime = 0.0f;
    float totallevelstime = 0.0f;
    int totallevelsamont = 0;
    int totalcorrect = 0;
    int levelmaxstar = 3;
    int totallevelstar = JsonLoader._JsonLoader.gettotallevelcount() * levelmaxstar;
    int totalgetstar = 0;

    string leveltexts = "<b>" + JsonLoader._JsonLoader.GetString("506") + "</b>\n";

    foreach (var v in record){
      GameDataConfig config = JsonLoader._JsonLoader.GetDataconfig(v.Value.level);
      totalusedtime += v.Value.time;
      totallevelstime += config.LevelTime;
      totallevelsamont += config.Amount;
      totalcorrect += v.Value.correct;

      float timerate = JsonLoader._JsonLoader.GetTimeRate(v.Value.level, v.Value.time);
      float correctrate = JsonLoader._JsonLoader.GetCorrectRate(v.Value.level, v.Value.correct);
      //totalgetstar += 

      leveltexts += "<size=50%>" + string.Format(JsonLoader._JsonLoader.GetString("507"), v.Value.level);
      leveltexts += string.Format(JsonLoader._JsonLoader.GetString("508"), v.Value.time);
      leveltexts += string.Format(JsonLoader._JsonLoader.GetString("509"), timerate);
      leveltexts += string.Format(JsonLoader._JsonLoader.GetString("510"), correctrate) + "\n";
    }


    float avg_time = totalusedtime / totallevelstime;
    float avg_correct = totalcorrect / totallevelsamont;
    //float avg_star = 

    string uptext = "<b>" +  JsonLoader._JsonLoader.GetString("501") + "</b>\n";
    uptext += string.Format(JsonLoader._JsonLoader.GetString("502"), record.Count) + "\n";
    uptext += string.Format(JsonLoader._JsonLoader.GetString("503"), avg_time) + "\n";
    uptext += string.Format(JsonLoader._JsonLoader.GetString("504"), avg_correct) + "\n\n";
    //uptext += string.Format(JsonLoader._JsonLoader.GetString("505"), avg_star) + "\n";

    uptext += leveltexts;
    return uptext;
  }

  string getStaffText(){
    int startindex = 601;
    int endindex = 609;
    string text = JsonLoader._JsonLoader.GetString("601") + "\n";
    text += JsonLoader._JsonLoader.GetString("602") + "\n\n";
    text += JsonLoader._JsonLoader.GetString("603") + "\n";
    text += JsonLoader._JsonLoader.GetString("604") + "\n\n";
    text += JsonLoader._JsonLoader.GetString("605") + "\n";
    text += JsonLoader._JsonLoader.GetString("606") + "\n\n";
    text += JsonLoader._JsonLoader.GetString("607") + "\n";
    text += JsonLoader._JsonLoader.GetString("608") + "\n";
    text += JsonLoader._JsonLoader.GetString("609");

    return text;
  }
}
