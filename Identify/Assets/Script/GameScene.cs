using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScene : MonoBehaviour,IScene
{
  GameObject mRoot = null;
  string secneName;
  SceneDisposeHandler pDisposeHandler = null;
  bool mInited = false;

  //game
  GameDataConfig config;
  int currentquestion = 1;
  float time;
  Vector2 area = new Vector2(384, 700);
  float cellsize;
  List<GameObject> right_point_list;
  List<GameObject> left_point_list;
  TextMeshPro timer_text = null;
  GameObject correct_go= null;//正確不正確共用
  summaryData msd = null;
  string level;
  class summaryData{
    public float time;//使用多久時間
    public int correct;
    public int incorrect;
  }

  float logic_timer = 0.0f;
  enum state
  {
    NULL,
    IDLE,

    SAME,
    DIFFERENT,

    determination_start,
    correct,//判定對錯
    Incorrect,
    determination_done,

    check_next_question,
    pre_next_question,
    summary,

    wait_player,
  }
  state currentstate = state.NULL;

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

    GameObject dynamicObj = gameObject;
    //int level = (int)extra_param[0];

    level = PlayerPrefsManager._PlayerPrefsManager.currentlevel.ToString();
    config = JsonLoader._JsonLoader.GetDataconfig(level);

    //
    // Intro prefab
    //
    mRoot = GameObject.Find("Game(Clone)");
    if (mRoot == null)
    {
      mRoot = instantiateObject(dynamicObj, "Game");
    }

    currentquestion = 1;
    timer_text = mRoot.transform.Find("time").GetComponent<TextMeshPro>();

    resetTimer();
    updateAmount(currentquestion);
    //correct_go = mRoot.transform.Find("correct").gameObject;
    //incorrect_go = mRoot.transform.Find("Incorrect").gameObject;


    AudioController._AudioController.play("BGM_44", true);

    AdsHelper._AdsHelper.RequestBannerAds(GoogleMobileAds.Api.AdPosition.Top, null);


    float cellwidth = area.x / config.W;
    float cellhight = area.y / config.H;

    //取最小的一個當作size
    cellsize = UtilityHelper.GetSmallValue(cellwidth, cellhight);

    GameLogic._GameLogic.SetConfig(config);
    GameLogic._GameLogic.InitLevel(cellsize);

    updatePoint();

    msd = new summaryData()
    {
      correct = 0,
      incorrect = 0,
      time = 0.0f
    };

    //設定圓圈
    //圓圈看起來必須是正圓
    //所以必須將寬高取得

    AudioController._AudioController.crossFadeBGM("BGM_44", true);

    currentstate = state.IDLE;
    //UIDialog._UIDialog.show(new summaryDialog(null));

    mInited = true;
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
    
    if(currentstate == state.IDLE){
      if(type == UIEventType.BUTTON){
        if(name == "same_bt"){
          currentstate = state.SAME;
        }
        else if(name == "different_bt"){
          currentstate = state.DIFFERENT;
        }
        else if(name == "back_bt"){
          pDisposeHandler(SceneDisposeReason.USER_EXIT, null);
          return;
        }
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
    //if (Input.GetKeyUp(KeyCode.R)){
    //  resetQuestion();
    //  resetTimer();
    //}

    if (Input.GetKeyUp(KeyCode.C))
    {
      //強制答對題目
      bool correct = GameLogic._GameLogic.Correct(QuestionAnswer.Same);
      if (correct)
        MainLogic._MainLogic.setUIEvent("same_bt", UIEventType.BUTTON, null);
      else
        MainLogic._MainLogic.setUIEvent("different_bt", UIEventType.BUTTON, null);
    }

    if (Input.GetKeyUp(KeyCode.E))
    {
      //強制答錯題目
      bool correct = GameLogic._GameLogic.Correct(QuestionAnswer.Same);
      if (correct)
        MainLogic._MainLogic.setUIEvent("different_bt", UIEventType.BUTTON, null);
      else
        MainLogic._MainLogic.setUIEvent("same_bt", UIEventType.BUTTON, null);
    }

    if (currentstate == state.IDLE){
      time -= Time.deltaTime;
      msd.time += Time.deltaTime;

      updateTime(time);
      if (time <= 0.0f){
        time = 0.0f;
        currentstate = state.Incorrect;
      }

    }

    else if (currentstate == state.SAME)
    {
      bool correct = GameLogic._GameLogic.Correct(QuestionAnswer.Same);
      currentstate = correct ? state.correct : state.Incorrect;
      return;
    }

    else if (currentstate == state.DIFFERENT)
    {
      bool correct = GameLogic._GameLogic.Correct(QuestionAnswer.Different);
      currentstate = correct ? state.correct : state.Incorrect;
      return;
    }

    else if (currentstate == state.correct)
    {
      //對
      AudioController._AudioController.playOverlapEffect("Sound_13");
      showcorrect(true);
      logic_timer = 0.5f;
      msd.correct++;
      currentstate = state.determination_done;
    }

    else if (currentstate == state.Incorrect)
    {
      //錯誤
      AudioController._AudioController.playOverlapEffect("Sound_32");
      showcorrect(false);
      logic_timer = 0.5f;
      msd.incorrect++;
      currentstate = state.determination_done;
    }

    else if (currentstate == state.determination_done)
    {
      logic_timer -= Time.deltaTime;
      if (logic_timer <= 0.0f)
      {
        closecorrect();
        currentstate = state.check_next_question;
      }
    }

    else if (currentstate == state.check_next_question)
    {
      if (currentquestion == config.Amount)
      {
        currentstate = state.summary;
        return;
      }
      else
      {
        currentstate = state.pre_next_question;
      }
    }

    else if (currentstate == state.pre_next_question)
    {

      nextQuestion();
      currentstate = state.IDLE;
    }

    else if (currentstate == state.summary)
    {
      AudioController._AudioController.crossFadeBGM("BGM_02");

      //判斷是不是更好紀錄
      PlayerPrefsManager._PlayerPrefsManager.updateRecord(level, msd.time, msd.correct);
      PlayerPrefsManager._PlayerPrefsManager.updateMaxlevel();

      UIDialog._UIDialog.show(new summaryDialog(msd.time, msd.correct, new InteractiveDiaLogHandler[] {
        ()=>{ 
          //返回關卡選擇
          pDisposeHandler( SceneDisposeReason.USER_EXIT,null);
        },
        ()=>{ 
          //重新挑戰
          resetLevel();
        },
        ()=>{
        //下一關
        nextLevel();
        }
        }));

      currentstate = state.wait_player;
    }
  }

  GameObject instantiateObject(GameObject parent, string name)
  {
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  public void updateTime(float time){
    if (timer_text == null)
      return;

    timer_text.text = time.ToString("F02");

  }

  public void resetTimer(){
    time = config.Time;
    updateTime(time);
  }

  public void resetQuestion(){
    GameLogic._GameLogic.NextQuest();
    updatePoint();
  }

  public void resetLevel(){
    resetTimer();
    resetQuestion();
    updateAmount(currentquestion);

    currentquestion = 1;
    msd = new summaryData()
    {
      correct = 0,
      incorrect = 0,
      time = 0.0f
    };
    currentstate = state.IDLE;
  }

  public void nextLevel()
  {
    PlayerPrefsManager._PlayerPrefsManager.currentlevel++;
    level = PlayerPrefsManager._PlayerPrefsManager.currentlevel.ToString();
    config = JsonLoader._JsonLoader.GetDataconfig(level);
    PlayerPrefsManager._PlayerPrefsManager.updateMaxlevel();


    float cellwidth = area.x / config.W;
    float cellhight = area.y / config.H;

    //取最小的一個當作size
    cellsize = UtilityHelper.GetSmallValue(cellwidth, cellhight);

    GameLogic._GameLogic.SetConfig(config);
    GameLogic._GameLogic.InitLevel(cellsize);

    for (int i = 0; i < right_point_list.Count; i++){
      Destroy(right_point_list[i]);
      Destroy(left_point_list[i]);
    }
    right_point_list = null;
    left_point_list = null;

    updatePoint();

    msd = new summaryData(){
      correct = 0,
      incorrect = 0,
      time = 0.0f
    };


    currentquestion = 1;

    resetTimer();
    updateAmount(currentquestion);

    currentstate = state.IDLE;
  }

  public void nextQuestion(){
    resetQuestion();
    resetTimer();

    currentquestion++;
    updateAmount(currentquestion);
  }

  void updateAmount(int currentquestion){
    mRoot.transform.Find("amount").GetComponent<TextMeshPro>().text = currentquestion + "/" + config.Amount;
  }

  void showcorrect(bool correct){
    string name = correct ? "correct" : "incorrect";
    correct_go = mRoot.transform.Find(name).gameObject;
    correct_go.SetActive(true);
  }

  void closecorrect(){
    correct_go.SetActive(false);
  }

  void updatePoint(){
    GameLogic.Point[,] rightpoints = GameLogic._GameLogic.GetRightPoints();
    GameLogic.Point[,] leftpoints = GameLogic._GameLogic.GetLeftPoints();

    float point_icon_size = 0.9f;
    float area_size = 0.9f;


    if (right_point_list == null){
      right_point_list = new List<GameObject>(rightpoints.Length);
      for (int i = 0; i < rightpoints.Length; i++)
        right_point_list.Add(instantiateObject(mRoot.transform.Find("rightarea").gameObject, "Point"));
    }
    if (left_point_list == null){
      left_point_list = new List<GameObject>(leftpoints.Length);
      for (int i = 0; i < leftpoints.Length; i++)
        left_point_list.Add(instantiateObject(mRoot.transform.Find("leftarea").gameObject, "Point"));
    }

    for (int i = 0; i < config.H; i++){
      for (int j = 0; j < config.W; j++){
        int index = i * config.W + j;

        GameObject point = right_point_list[index];

        point.transform.localPosition = new Vector3(rightpoints[i, j].position.x + area.x * 0.5f, rightpoints[i, j].position.y + 75);
        point.transform.localScale = new Vector3(cellsize * point_icon_size, cellsize * point_icon_size, 1.0f);
        point.transform.Find("Icon").GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Mathf.InverseLerp(0,360, rightpoints[i, j].HSV.H) , Mathf.InverseLerp(0, 100, rightpoints[i, j].HSV.S), Mathf.InverseLerp(0, 100, rightpoints[i, j].HSV.V));

        point = left_point_list[index];

        point.transform.localPosition = new Vector3(leftpoints[i, j].position.x - area.x * 0.5f, leftpoints[i, j].position.y + 75);
        point.transform.localScale = new Vector3(cellsize * point_icon_size, cellsize * point_icon_size, 1.0f);
        point.transform.Find("Icon").GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Mathf.InverseLerp(0, 360, leftpoints[i, j].HSV.H), Mathf.InverseLerp(0, 100, leftpoints[i, j].HSV.S), Mathf.InverseLerp(0, 100, leftpoints[i, j].HSV.V));

      }
    }


    mRoot.transform.Find("rightarea").localScale = new Vector3(area_size, area_size, 1.0f);
    mRoot.transform.Find("leftarea").localScale = new Vector3(area_size, area_size, 1.0f);

  }
}
