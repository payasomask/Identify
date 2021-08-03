using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public delegate void CommonAction(); //the CommonAction should works like C# Action
//針對InteractiveDiaLogHandler做的行為
public delegate void InteractiveDiaLogHandler();
public class PlayerPrefsManager
{
  public static PlayerPrefsManager _PlayerPrefsManager = null;
  string _Audio_T = "on";
  string audioKey = "AudioToggle";
  public string Audio_T
  {
    get { return _Audio_T; }
    set{
      _Audio_T = value;
      savePlayerPrefsString("AudioToggle", value);
      AudioController._AudioController.toggleAudio(convertOnOff2bool(value));
    }
  }
  string _Music_T = "on";
  string musicKey = "MusicToggle";
  public string Music_T
  {
    get { return _Music_T; }
    set{
      _Music_T = value;
      savePlayerPrefsString("MusicToggle", value);
      AudioController._AudioController.toggleMusic(convertOnOff2bool(value));
    }
  }
  string _Vibrate_T = "on";
  string vibrateKey = "VibrateToggle";
  public string Vibrate_T
  {
    get { return _Vibrate_T; }
    set{
      _Vibrate_T = value;
      savePlayerPrefsString("VibrateToggle", value);
    }
  }

  string PlayerPrefs_Ver = "20210120";


  string _Language = "US";
  string LanguageKey = "Language";
  public string Language
  {
    get { return _Language; }
    set
    {
      if(value == "default")
        _Language = Application.systemLanguage == SystemLanguage.ChineseTraditional ? "TW" : "US";
      else
        _Language = value;
      savePlayerPrefsString("Language", value);
    }
  }

  string _AdministationVersion = "off";
  string _AdministationVersionKey = "AdministationVersion";
  public string AdministationVersion
  {
    get { return _AdministationVersion; }
    set
    {
      _AdministationVersion = value;
      savePlayerPrefsString(_AdministationVersionKey, value);
    }
  }

  string _tutorial = "off";
  string tutorialKey = "tutorial";
  public string tutorial
  {
    get { return _tutorial; }
    set
    {
      _tutorial = value;
      savePlayerPrefsString(tutorialKey, value);
    }
  }

  int _currentlevel = 1;
  string currentlevelKey = "currentlevel";
  public int currentlevel
  {
    get { return _currentlevel; }
    set
    {
      _currentlevel = value;
      savePlayerPrefsInt(currentlevelKey, value);
    }
  }


  int _currentmaxlevel = 1;
  string currentmaxlevelKey = "currentmaxlevel";
  //玩家玩到的最大關卡
  public int currentmaxlevel
  {
    get { return _currentmaxlevel; }
    set
    {
      _currentmaxlevel = value;
      savePlayerPrefsInt(currentmaxlevelKey, value);
    }
  }

  Dictionary<string, GameRecord> _Record = null;
  string RecordKey = "Record";
  //玩家玩到的最大關卡
  public Dictionary<string, GameRecord> Record
  {
    get { return _Record; }
    set
    {
      _Record = value;
      //Dictionary<string, string> tmp = new Dictionary<string, string>(_Record.Count);
      string record_dic_string = Newtonsoft.Json.JsonConvert.SerializeObject(_Record);
      savePlayerPrefsString(RecordKey, record_dic_string);
      //_Record = value;
      //Dictionary<string, string> tmp = new Dictionary<string, string>(_Record.Count);
      //foreach(var v in _Record){
      //  tmp.Add(v.Key, MiniJSON.Json.Serialize(v.Value));
      //}
      //string record_dic_string = MiniJSON.Json.Serialize(tmp);
      //savePlayerPrefsString(RecordKey, record_dic_string);
    }
  }

  public PlayerPrefsManager()
  {
    _Audio_T = PlayerPrefs.GetString(audioKey, "on");
    _Music_T = PlayerPrefs.GetString(musicKey, "on");
    _Vibrate_T = PlayerPrefs.GetString(vibrateKey, "on");
    _tutorial = PlayerPrefs.GetString(vibrateKey, "off");
    _currentlevel = PlayerPrefs.GetInt(currentlevelKey, 1);
    _currentmaxlevel = PlayerPrefs.GetInt(currentmaxlevelKey, 1);
    //_Language = PlayerPrefs.GetString(LanguageKey, "US");
    _AdministationVersion = PlayerPrefs.GetString(_AdministationVersionKey, "");

    //PlayerPrefs.DeleteKey(RecordKey);

    if (PlayerPrefs.HasKey(RecordKey)){
      string record_string = PlayerPrefs.GetString(RecordKey);
      var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,GameRecord>>(record_string);
      _Record = new Dictionary<string, GameRecord>(tmp.Count);
      foreach (var v in tmp){
        GameRecord re = v.Value;
        _Record.Add(v.Key, re);
      }
      Debug.Log("548 - Record count : " + _Record.Count);
    }
  }

  public bool hasRecord(){
    if (_Record == null)
      return false;
    if (_Record.Count == 0)
      return false;

    return true;
  }

  public string GetRecordStar(string formatid, string level){
    int star = 0;
    if (hasRecord() == false)
      return JsonLoader._JsonLoader.Star_string(formatid,star);

    if(_Record.ContainsKey(level) == false)
      return JsonLoader._JsonLoader.Star_string(formatid,star);

    return JsonLoader._JsonLoader.Star_string(formatid,_Record[level].star);
  }
  public int GetRecordStar(string level)
  {
    int star = 0;
    if (hasRecord() == false)
      return 0;

    if (_Record.ContainsKey(level) == true)
      return _Record[level].star;

    return star;
  }
  public void updateRecord(string level,float time,int correct){
    if (_Record == null)
      _Record = new Dictionary<string, GameRecord>();

    GameDataConfig config =  JsonLoader._JsonLoader.GetDataconfig(level);
    float timerate = JsonLoader._JsonLoader.GetTimeRate(level, time);
    float corrrctrate = JsonLoader._JsonLoader.GetCorrectRate(level, correct);
    float current_record = 1 - timerate + corrrctrate;
    if (_Record.ContainsKey(level)){
      float record_timerate = JsonLoader._JsonLoader.GetTimeRate(level, _Record[level].time);
      float record_corrrctrate = JsonLoader._JsonLoader.GetCorrectRate(level, _Record[level].correct);
      float record = 1 - record_timerate + record_corrrctrate;

      if (current_record > record){
        //更新紀錄
        _Record[level].time = time;
        _Record[level].correct = correct;
        _Record[level].star = JsonLoader._JsonLoader.GetStar(level, time, correct);
        Record = _Record;
      }
    }
    else{
      _Record.Add(level, new GameRecord() { level = level, time = time, correct = correct ,
        star = JsonLoader._JsonLoader.GetStar(level,time, correct) });;
      Record = _Record;
    }
  }
  public void updateMaxlevel(){

    int nextlevel =  _PlayerPrefsManager.currentlevel+1;

    if (nextlevel > _currentmaxlevel)
      currentmaxlevel = nextlevel;
  }

  public void resetplayerprefs(){
    _PlayerPrefsManager = new PlayerPrefsManager();
  }

  //檢查暫存資料有沒有異常..
  public bool checkPlayerPrefsDATA(){
    if (PlayerPrefs.HasKey("PlayerPrefs_Ver")){
      string ver = PlayerPrefs.GetString("PlayerPrefs_Ver");
      Debug.Log("Local PlayerPrefs_Ver : " + ver + ", Embedded PlayerPrefs_Ver : " + PlayerPrefs_Ver);
      if (ver != PlayerPrefs_Ver)
        return true;

      return false;
    }
    PlayerPrefs.SetString("PlayerPrefs_Ver", PlayerPrefs_Ver);
    return true;
  }

  public void loginsetting()//登陸遊戲後需要設定的參數
  {
    //if (PlayerPrefs.HasKey(GetFriendsCardsTimeKey))
    //  _GetFriendsCardsTime = new DateTime().ToString();

  }

  void savePlayerPrefsString(string keyname , string OnOff)
  {
    PlayerPrefs.SetString(keyname, OnOff);
    PlayerPrefs.Save();
  }
  void savePlayerPrefsFloat(string keyname, float value)
  {
    PlayerPrefs.SetFloat(keyname, value);

    PlayerPrefs.Save();
  }
  void savePlayerPrefsInt(string keyname, int value)
  {
    PlayerPrefs.SetInt(keyname, value);

    PlayerPrefs.Save();
  }
  public bool convertOnOff2bool(string name)
  {
    if (name.Equals("on"))
      return true;

    return false;
  }
  public string convertbool2OnOff(bool OnOff)
  {
    if (OnOff)
      return "on";

    return "off";
  }

  Dictionary<string, object> pauseSaveDic = new Dictionary<string, object>();
  public void OnPause()
  {
    Debug.Log("app paused by user...");
    //AddpauseSaveDic(GetFriendsCardsTimeKey, GetFriendsCardsTime);//暫存參數
    //GetFriendsCardsTime = new DateTime().ToString();//重置取得FB好友隊伍的時間
  }
  public void OnFocus()
  {
    Debug.Log("user back to the app...");
    //GetFriendsCardsTime = (string)LoadpauseSaveDic(GetFriendsCardsTimeKey);
  }

  void AddpauseSaveDic(string name, object value)
  {
    Debug.Log("將 " + name + " value : " + (string)value+ "存入暫存Dic");
    if (pauseSaveDic.ContainsKey(name))
      pauseSaveDic[name] = value;
    else
      pauseSaveDic.Add(name, value);
  }

  object LoadpauseSaveDic(string key)
  {
    if (pauseSaveDic.ContainsKey(key)){
      Debug.Log("從 存入暫存Dic 讀出 Key : " + key + " value : " + (string)pauseSaveDic[key]);
      return pauseSaveDic[key];
    }
    return null;
  }

  //bulltin part
  Dictionary<string, object> bulltin_dic =null;
  void resume_bulltin_dic(){
    if (bulltin_dic ==null){
      if (PlayerPrefs.HasKey("blocked_bulltin_list")){
        string raw_str =PlayerPrefs.GetString("blocked_bulltin_list");
        bulltin_dic =(Dictionary<string, object>)MiniJSON.Json.Deserialize(raw_str);
      }else{
        bulltin_dic =new Dictionary<string, object>();
      }
    }
  }

  public void block_bulltin_today(string id){
    resume_bulltin_dic();

    DateTime block_until =DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddTicks(-1).AddDays(1);
    Debug.Log("308 - bulltin "+id+" blocked until "+block_until.ToString("yyyy/MM/dd HH:mm:ss"));
    if (bulltin_dic.ContainsKey(id)==true){
      bulltin_dic[id] =block_until.ToString("yyyy/MM/dd HH:mm:ss");
    }else{
      bulltin_dic.Add(id, block_until.ToString("yyyy/MM/dd HH:mm:ss"));
    }

    PlayerPrefs.SetString("blocked_bulltin_list", MiniJSON.Json.Serialize(bulltin_dic));

  }

  public void remove_obsoleted_bulltin_data(List<string> bulltin_id){
    resume_bulltin_dic();

    List<string> raw_dic_keys =new List<string>(bulltin_dic.Keys);
    for (int i=0;i<raw_dic_keys.Count;){
      if (bulltin_id.Contains(raw_dic_keys[i])==true){
        raw_dic_keys.RemoveAt(i);
      }else{
        ++i;
      }
    }

    for (int i=0;i<raw_dic_keys.Count;++i){
      Debug.Log("342 - bulltin ("+raw_dic_keys[i]+") record deleted");
      bulltin_dic.Remove(raw_dic_keys[i]);
    }

    PlayerPrefs.SetString("blocked_bulltin_list", MiniJSON.Json.Serialize(bulltin_dic));

  }

  public bool is_bulltin_blocked(string id){
    resume_bulltin_dic();
    if (bulltin_dic.ContainsKey(id)){
      System.DateTime ed =System.DateTime.ParseExact((string)bulltin_dic[id], "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
      int curr_ts = (int)(System.DateTime.Now.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
      int ed_ts =(int)(ed.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
      if (curr_ts<=ed_ts){
        Debug.Log("357 - bulltin id "+id+" was blocked");
        return true;
      }
    }else{
      Debug.Log("361 - bulltin id "+id+" not found");
    }
    return false;
  }

  //bulltin part
  Dictionary<string, object> dialog_dic = null;
  void resume_dialog_dic()
  {
    if (dialog_dic == null)
    {
      if (PlayerPrefs.HasKey("blocked_dialog_list"))
      {
        string raw_str = PlayerPrefs.GetString("blocked_dialog_list");
        dialog_dic = (Dictionary<string, object>)MiniJSON.Json.Deserialize(raw_str);
      }
      else
      {
        dialog_dic = new Dictionary<string, object>();
      }
    }
  }

  public void block_dialog_today(string id)
  {
    resume_dialog_dic();

    DateTime dialog_until = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddTicks(-1).AddDays(1);
    Debug.Log("308 - dialog " + id + " dialog until " + dialog_until.ToString("yyyy/MM/dd HH:mm:ss"));
    if (dialog_dic.ContainsKey(id) == true)
    {
      dialog_dic[id] = dialog_until.ToString("yyyy/MM/dd HH:mm:ss");
    }
    else
    {
      dialog_dic.Add(id, dialog_until.ToString("yyyy/MM/dd HH:mm:ss"));
    }

    PlayerPrefs.SetString("blocked_dialog_list", MiniJSON.Json.Serialize(dialog_dic));

  }

  public void remove_obsoleted_dialog_data(List<string> dialog_id)
  {
    resume_dialog_dic();

    List<string> raw_dic_keys = new List<string>(dialog_dic.Keys);
    for (int i = 0; i < raw_dic_keys.Count;)
    {
      if (dialog_id.Contains(raw_dic_keys[i]) == true)
      {
        raw_dic_keys.RemoveAt(i);
      }
      else
      {
        ++i;
      }
    }

    for (int i = 0; i < raw_dic_keys.Count; ++i)
    {
      Debug.Log("342 - dialog (" + raw_dic_keys[i] + ") record deleted");
      dialog_dic.Remove(raw_dic_keys[i]);
    }

    PlayerPrefs.SetString("blocked_dialog_list", MiniJSON.Json.Serialize(dialog_dic));

  }

  public bool is_dialog_blocked(string id)
  {
    resume_dialog_dic();
    if (dialog_dic.ContainsKey(id))
    {
      System.DateTime ed = System.DateTime.ParseExact((string)dialog_dic[id], "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
      int curr_ts = (int)(System.DateTime.Now.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
      int ed_ts = (int)(ed.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
      if (curr_ts <= ed_ts)
      {
        Debug.Log("357 - dialog id " + id + " was blocked");
        return true;
      }
    }
    else
    {
      Debug.Log("361 - dialog id " + id + " not found");
    }
    return false;
  }

}

public class MainLogic : MonoBehaviour
{

  public static MainLogic _MainLogic = null;
  private GameObject camera_go = null;
  public IScene mS = null;
  class SceneTransition
  {
    public delegate void TransitionDoneHandler();
    // IScene fromScene;
    Type toScene; //IScene Type
    string toScene_name;

    AsyncOperation ao = null;
    float ao_start_time = 0f;

    TransitionDoneHandler pTransitionDoneHandler = null;

    bool force_transition = false;

    enum Status
    {
      IDLE,
      DISPOSE_CURRENT_SCENE,
      WAIT_DISPOSE,
      INIT_NEW_SCENE,
      WAIT_RELEASE_UNUSEDASSET1,
      INIT_NEW_SCENE_PHASE2,
      WAIT_INIT,
      DUPLICATED_LOGIN_SHOWING,
      DONE
    }
    Status mCurrentStatus = Status.IDLE;

    object[] extra_param_for_init = null;

    public SceneTransition(string pto_name, object[] extra_param, TransitionDoneHandler pHandler, bool forceTransition = false)
    {
      force_transition = forceTransition;

      //get gametype from pto_name
      toScene = (Type)_MainLogic.mER.getSettings(pto_name, SceneSettingsType.SCENE_TYPE);
      toScene_name = pto_name;
      pTransitionDoneHandler = pHandler;

      extra_param_for_init = extra_param;
    }

    public void update()
    {
      if (mCurrentStatus == Status.IDLE)
      {
        //start transition...
        mCurrentStatus = Status.DISPOSE_CURRENT_SCENE;

      }
      else
      if (mCurrentStatus == Status.DISPOSE_CURRENT_SCENE)
      {
        if (_MainLogic.mS == null)
        {
          mCurrentStatus = Status.INIT_NEW_SCENE;
          return;
        }
        Debug.Log("66 - disposeScene with force transition =" + force_transition);
        _MainLogic.mS.disposeScene(force_transition);

        mCurrentStatus = Status.WAIT_DISPOSE;

      }
      else
      if (mCurrentStatus == Status.WAIT_DISPOSE)
      {
        if (_MainLogic.mS.isSceneDisposed())
        {

          GameObject.Destroy(((MonoBehaviour)_MainLogic.mS).gameObject);
          _MainLogic.mS = null;

          AssetbundleLoader._AssetbundleLoader.PurgeAudioClipCache();
          AssetbundleLoader._AssetbundleLoader.PurgeAudioClipCache();
          AssetbundleLoader._AssetbundleLoader.PurgeSpriteCache();
          AssetbundleLoader._AssetbundleLoader.PurgeSpriteCache();
          AssetbundleLoader._AssetbundleLoader.PurgeAssetbundles();

          Debug.Log(">>>>>>>>> UNLOAD UNUSED ASSETS - 1 <<<<<<<<<");
          ao_start_time = Time.realtimeSinceStartup;
          ao = Resources.UnloadUnusedAssets();

          System.GC.Collect();
          System.GC.WaitForPendingFinalizers();

          mCurrentStatus = Status.WAIT_RELEASE_UNUSEDASSET1;
        }

      }
      else
      if (mCurrentStatus == Status.WAIT_RELEASE_UNUSEDASSET1)
      {
        if (ao.isDone)
        {
          Debug.Log(">>>>>>>>> UNLOAD UNUSED ASSETS :" + (Time.realtimeSinceStartup - ao_start_time) + " SECONDS TAKEN <<<<<<<<<");
          mCurrentStatus = Status.INIT_NEW_SCENE;
          ao = null;
        }
      }
      else
      if (mCurrentStatus == Status.INIT_NEW_SCENE)
      {
        Debug.Log("82 - init new scene (extra_param.Length=" + extra_param_for_init.Length + ")...");
        GameObject gameObj = new GameObject();
        gameObj.name = toScene_name;

        //EP Type
        //Type ep_type = (Type)_MainLogic.mER.getSettings(toScene_name, SceneSettingsType.PROBABILITY_TYPE);
        //if (ep_type != null)
        //{
        //  mCxt.mEP = (IProbability)System.Activator.CreateInstance(ep_type);
        //  mCxt.mEP.setContext(mCxt);
        //}

        _MainLogic.mS = (IScene)gameObj.AddComponent(toScene);
        _MainLogic.mS.registerSceneDisposeHandler(_MainLogic.SceneDisposeHandler);
        _MainLogic.mS.initLoadingScene(toScene_name, extra_param_for_init);

        mCurrentStatus = Status.INIT_NEW_SCENE_PHASE2;//WAIT_INIT;

      }
      else
      if (mCurrentStatus == Status.INIT_NEW_SCENE_PHASE2)
      {
        _MainLogic.mS.initScene(toScene_name, extra_param_for_init);
        mCurrentStatus = Status.WAIT_INIT;

      }
      else
      if (mCurrentStatus == Status.WAIT_INIT)
      {
        if (_MainLogic.mS.isSceneInitialized())
        {

          if (pTransitionDoneHandler != null)
          {
            pTransitionDoneHandler();
          }

          mCurrentStatus = Status.DONE;
        }

      }

    }
  }
  SceneTransition mCurrentSceneTransition = null;

  private void Awake()
  {
    _MainLogic = this;
    PlayerPrefsManager._PlayerPrefsManager = new PlayerPrefsManager();
    JsonLoader._JsonLoader.Init();
    AssetbundleLoader._AssetbundleLoader = new AssetbundleLoader();
    mER = new EmbeddedSceneSettings();

    //AudioManager._AudioManager = new AudioManager();
    //MaskManager._MaskManager = new MaskManager();
  }

  public ISceneSettings mER = null;

  // Use this for initialization
  void Start()
  {

    //
    // SETUP UNITY ENGINE PARAMETERS
    //
    //Application.targetFrameRate = 60;
    QualitySettings.vSyncCount = 1;

    //
    //Create Common Object
    //
    Debug.Log("init MainLogic Environment...");

    //keep screen always on
    Screen.sleepTimeout = SleepTimeout.NeverSleep;

    GameObject dlgmgr = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab("DialogManager");
    dlgmgr.name = "DialogManager";
    dlgmgr.GetComponent<TouchEventHandler>().update_priority(-1);

    UIDialog tmpDLG = dlgmgr.GetComponent<UIDialog>();
    tmpDLG.setupAssetbundleLoader(AssetbundleLoader._AssetbundleLoader);

    //AUDIO
    GameObject audio_obj = new GameObject();
    audio_obj.name = "Audio";
    AudioController mAC = audio_obj.AddComponent<AudioController>();
    mAC.init();

    GameObject dynamicObj = new GameObject();
    dynamicObj.name = "Main Cameras";
    //TouchEventHandler teh =dynamicObj.AddComponent<TouchEventHandler>(); //HANDLE EVENT FOR SLEEPING SCREEN WAKE
    //teh.update_priority(-3);//
    //BoxCollider2D tehbc2d =dynamicObj.AddComponent<BoxCollider2D>();
    //tehbc2d.size =new Vector2(1366f, 768f);

    FontManager._FontManager.init();

    camera_go = instantiateObject(dynamicObj, "Common_Camera");

    GameObject extendBGCamera = new GameObject();
    extendBGCamera.transform.SetParent(dynamicObj.transform);
    extendBGCamera.transform.localPosition = Vector3.forward * -2000f;
    extendBGCamera.name = "ExtCamera";
    Camera ec = extendBGCamera.AddComponent<Camera>();
    ec.cullingMask = 1 << 31;
    ec.orthographic = true;
    ec.clearFlags = CameraClearFlags.SolidColor;
    ec.backgroundColor = Color.black;
    ec.farClipPlane = 4000f;
    ec.depth = -2;
    ec.allowMSAA = false;

    if (((float)Screen.width / (float)Screen.height) < (768f / 1366))
    {
      ec.orthographicSize = 683f * Screen.height / Screen.width * (768f / 1366);

      GameObject ext_bgv1 = new GameObject();
      ext_bgv1.transform.SetParent(ec.transform, false);
      ext_bgv1.transform.localPosition = new Vector3(0f, -683 - (116F / 2F), 2500f);
      ext_bgv1.name = "ext_bg";
      ext_bgv1.layer = 31;
      SpriteRenderer sr1 = ext_bgv1.AddComponent<SpriteRenderer>();
      sr1.sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("embedded2", "ext_bg_h");

      GameObject ext_bgv2 = new GameObject();
      ext_bgv2.transform.SetParent(ec.transform, false);
      ext_bgv2.transform.localPosition = new Vector3(0f, 683 + (116F / 2F), 2500f);
      ext_bgv2.name = "ext_bg";
      ext_bgv2.layer = 31;
      SpriteRenderer sr2 = ext_bgv2.AddComponent<SpriteRenderer>();
      sr2.flipY = true;
      sr2.sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("embedded2", "ext_bg_h");


    }
    else
    {
      ec.orthographicSize = 683f;

      GameObject ext_bgv1 = new GameObject();
      ext_bgv1.transform.SetParent(ec.transform, false);
      ext_bgv1.transform.localPosition = new Vector3(-766.5f, 0f, 2500f);
      ext_bgv1.name = "ext_bg";
      ext_bgv1.layer = 31;
      SpriteRenderer sr1 = ext_bgv1.AddComponent<SpriteRenderer>();
      sr1.sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("embedded2", "ext_bg_v");

      GameObject ext_bgv2 = new GameObject();
      ext_bgv2.transform.SetParent(ec.transform, false);
      ext_bgv2.transform.localPosition = new Vector3(766.5f, 0f, 2500f);
      ext_bgv2.name = "ext_bg";
      ext_bgv2.layer = 31;
      SpriteRenderer sr2 = ext_bgv2.AddComponent<SpriteRenderer>();
      sr2.flipX = true;
      sr2.sprite = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("embedded2", "ext_bg_v");
    }




    //
    //Create First Scene
    //
    Debug.Log("transit to first scene...");
    mCurrentSceneTransition = new SceneTransition("IntroScene", new object[] { }, delegate ()
    {
      mCurrentSceneTransition = null;
    });
    //mCurrentSceneTransition = new SceneTransition("GameScene", new object[] { }, delegate ()
    //{
    //  mCurrentSceneTransition = null;
    //});
    //mCurrentSceneTransition = new SceneTransition("LobbyScene", new object[] { }, delegate ()
    //{
    //  mCurrentSceneTransition = null;
    //});
    //mCurrentSceneTransition = new SceneTransition("LevelListScene", new object[] { }, delegate ()
    //{
    //  mCurrentSceneTransition = null;
    //});

    //#if UNITY_ANDROID && !UNITY_EDITOR
    //          if (android_utility_class ==null)
    //            android_utility_class =(new AndroidJavaClass("com.powergameranger.androidnative.ADSHelper"));

    //          if (android_utility_class !=null)
    //            android_utility_class.CallStatic("Log","test call native");

    //#endif

    AdsHelper._AdsHelper.init();
    
    //AdsHelper._AdsHelper.RequestInterstitialAds();
  }

  GameObject instantiateObject(GameObject parent, string name)
  {
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  void Update()
  {

    //process scene transition
    if (mCurrentSceneTransition != null)
    {
      mCurrentSceneTransition.update();
      return;
    }

    if (Input.GetKeyUp(KeyCode.Escape))
    {//裝置KeyCode.Escape統一從這邊觸發
      setUIEvent("back_bt", UIEventType.BUTTON, null);
      return;
    }

  }

  virtual public void setUIEvent(string name, UIEventType type, object[] extra_info)
  {
    if (type == UIEventType.BUTTON)
      AudioController._AudioController.playOverlapEffect("Sound_44");

    if (mS != null)
    {
      Debug.Log("515 Scene name : " + mS.getSceneName() + "，setUIEvent : " + type + "，name : " + name);
      if (UIDialog._UIDialog.setUIEvent(name, type, extra_info)){
        return;
      }

      mS.setUIEvent(name, type, extra_info);

    }
  }

  virtual public bool getSandboxMode()
  {
    return false;
  }

  void SceneDisposeHandler(SceneDisposeReason sdr, object[] extra_info)
  {
    if (mCurrentSceneTransition != null)
    {
      //假如正在換 scene 則取消指令
      Debug.Log("181 - scene transition is processing...");
      return;
    }

    Debug.Log("SceneDisposed, reason =" + sdr + ", current scene=" + mS.getSceneName());


    //transit to new scene with random mission
    if (mS.getSceneName() == "IntroScene")
    {
      if (sdr == SceneDisposeReason.USER_EXIT)
      {
        mCurrentSceneTransition = new SceneTransition("LobbyScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);
      }

    }
    else
    //if (mS.getSceneName() == "LoginScene")
    //{
    //  if (sdr == SceneDisposeReason.USER_ACTION)
    //  {
    //    int type = (int)extra_info[0];

    //    if (type == 0)
    //    {
    //      mCurrentSceneTransition = new SceneTransition("LobbyScene", new object[] { extra_info[1], extra_info[2] }, delegate () {
    //        mCurrentSceneTransition = null;
    //      }, true);
    //    }
    //  }
    //}
    //else
    if (mS.getSceneName() == "LobbyScene")
    {
      if (sdr == SceneDisposeReason.USER_ACTION)
      {
        mCurrentSceneTransition = new SceneTransition("LevelListScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);

      }
      else
      if (sdr == SceneDisposeReason.USER_EXIT)
      {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("move task to back !");
        using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
            unityActivity.Call<bool>("moveTaskToBack", true);
        }
#endif
      }
    }
    else
    if (mS.getSceneName() == "LevelListScene")
    {
      if (sdr == SceneDisposeReason.USER_ACTION)
      {
        mCurrentSceneTransition = new SceneTransition("GameScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);
      }
      else
      if (sdr == SceneDisposeReason.USER_EXIT)
      {
        mCurrentSceneTransition = new SceneTransition("LobbyScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);
      }
    }
    else
    if (mS.getSceneName() == "GameScene")
    {
      if (sdr == SceneDisposeReason.USER_EXIT)
      {
        mCurrentSceneTransition = new SceneTransition("LevelListScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);
      }
      else if (sdr == SceneDisposeReason.USER_ACTION){
        mCurrentSceneTransition = new SceneTransition("AllClearScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);
      }
    }
    else if (mS.getSceneName() == "AllClearScene")
    {
      if (sdr == SceneDisposeReason.USER_EXIT)
      {
        mCurrentSceneTransition = new SceneTransition("LevelListScene", new object[] { }, delegate ()
        {
          mCurrentSceneTransition = null;
        }, true);
      }
    }

  }


  AndroidJavaClass android_utility_class = null;
  public static void vibrate(int duration_ms)
  {
    MainLogic ml = GameObject.Find("MainLogic").GetComponent<MainLogic>();
    if (ml == null)
      return;

    //#if UNITY_ANDROID && !UNITY_EDITOR
    //    if (ml.mCxt.mPPM.Vibrate_T =="on"){
    //      if (ml.android_utility_class ==null)
    //        ml.android_utility_class =(new AndroidJavaClass("com.phardera.support.Utility"));

    //      if (ml.android_utility_class !=null)
    //        ml.android_utility_class.CallStatic("vibrate", duration_ms); //vibrate with duration 500 ms
    //    }
    //#endif
  }


  //Android 測試 開啟分頁模式離開遊戲 會先呼叫OnApplicationFocus(false)接著呼叫OnApplicationPause(true)
  //Android 測試 從分頁模式返回遊戲 會先呼叫OnApplicationFocus(true)接著呼叫OnApplicationPause(false)
  //mono 模擬器測試 開啟分頁模式離開遊戲 會先呼叫OnApplicationPause(true)接著呼叫OnApplicationFocus(false)
  //mono 模擬器測試 從分頁模式返回遊戲 會先呼叫OnApplicationFocus(true)接著呼叫OnApplicationPause(false)
  bool ispause = false;
  bool isfocus = true;
  void OnApplicationPause(bool pause)
  {
    //Debug.Log("OnApplicationPause" + pause);
    ispause = pause;
    if (/*!isfocus &&*/ ispause)
    {
      //玩家將遊戲暫停...
      PlayerPrefsManager._PlayerPrefsManager.OnPause();
    }
  }

  void OnApplicationFocus(bool focus)
  {
    //Debug.Log("OnApplicationFocus" + focus);
    isfocus = focus;
    if (isfocus && ispause)
    {
      PlayerPrefsManager._PlayerPrefsManager.OnFocus();
    }
  }

  //處理需要在遊戲關閉後重置的參數
  void OnApplicationQuit()
  {
    //這個只能用在Unity Editor 或是經由 程式碼的Application.Quit(); 才會被呼叫，若Android平台用分頁模式直接關閉遊戲 則不會有任何反應
    PlayerPrefsManager._PlayerPrefsManager.OnPause();
  }

  string ReplaceFirst(string text, string search, string replace)
  {
    int pos = text.IndexOf(search);
    if (pos < 0)
    {
      return text;
    }
    return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
  }

  string stage_id_to_stage_no(string stage_id)
  {
    return int.Parse(stage_id.Substring(stage_id.Length - 2, 2)).ToString();
  }

  public float getCameraWidth()
  {
    if (camera_go == null)
      return 0.0f;
    return camera_go.GetComponent<ConfigCamera>().desiredWidth;
  }

  public float getCameraHight()
  {
    if (camera_go == null)
      return 0.0f;
    return camera_go.GetComponent<ConfigCamera>().desiredHeight;
  }

  public Camera getMainCamera()
  {
    if (camera_go == null)
      return null;
    return camera_go.GetComponent<Camera>();
  }
}
