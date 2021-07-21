using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class summaryDialog :  IDialogContext
{

  public summaryDialog(float time, int correct, InteractiveDiaLogHandler[] handlers){
    bt_handlers = handlers;
    this.time = time;
    this.correct = correct;
  }
  bool binited = false;
  GameObject dlgGO = null;
  InteractiveDiaLogHandler[] bt_handlers;
  float time;
  int correct;
  public bool dismiss()
  {
    GameObject.Destroy(dlgGO);
    return true;
  }

  public DialogEscapeType getEscapeType()
  {
    return DialogEscapeType.NOTHING;
  }

  public DialogType getType()
  {
    return DialogType.NORMAL;
  }

  public GameObject init(int dlgIdx, AssetbundleLoader abl){
      dlgGO = abl.InstantiatePrefab("summaryDialog");

    int level = PlayerPrefsManager._PlayerPrefsManager.currentlevel;

    GameDataConfig config = JsonLoader._JsonLoader.GetDataconfig(level.ToString());
    float timeRate = JsonLoader._JsonLoader.GetTimeRate(PlayerPrefsManager._PlayerPrefsManager.currentlevel.ToString(),time)*100.0f;
    float correctRate = JsonLoader._JsonLoader.GetCorrectRate(PlayerPrefsManager._PlayerPrefsManager.currentlevel.ToString(), correct) * 100.0f;
    int star = JsonLoader._JsonLoader.GetStar(PlayerPrefsManager._PlayerPrefsManager.currentlevel.ToString(), time, correct);

    string summary_text = string.Format(JsonLoader._JsonLoader.GetString("401"), time, timeRate) + "\n";
    summary_text += JsonLoader._JsonLoader.GetString("402") +"    " + correct +"\n";
    summary_text += JsonLoader._JsonLoader.GetString("403") +"    " + (config.Amount - correct) + "\n";
    summary_text += string.Format(JsonLoader._JsonLoader.GetString("404"), correctRate) + "\n";
    summary_text += JsonLoader._JsonLoader.Star_string(star);

    TextMeshPro t = dlgGO.transform.Find("bg/text").GetComponent<TextMeshPro>();
    t.text = summary_text;

    bool hasnextleveldata = JsonLoader._JsonLoader.GetDataconfig((level + 1).ToString()) != null;
    bool played = level + 1 <= PlayerPrefsManager._PlayerPrefsManager.currentmaxlevel;
    bool next_level_active = star >= 1;
    if (played)
      next_level_active = true;
    if (hasnextleveldata == false)
      next_level_active = false;
    dlgGO.transform.Find("bg/next_level_bt").gameObject.SetActive(next_level_active);

    binited = true;

    return dlgGO;
  }

  public bool inited()
  {
    return binited;
  }

  public DialogResponse setUIEvent(string name, UIEventType type, object[] extra_info)
  {
    if(type == UIEventType.BUTTON){
      if(name == "level_list_bt")
      {
        //AudioController._AudioController.playOverlapEffect("yes_no_使用道具_按鍵音效");
        if (bt_handlers[0] != null)
          bt_handlers[0]();
        return DialogResponse.TAKEN_AND_DISMISS;
      }
      else if(name == "restart_bt")
      {
        //AudioController._AudioController.playOverlapEffect("yes_no_使用道具_按鍵音效");
        if (bt_handlers[1] != null)
          bt_handlers[1]();



        return DialogResponse.TAKEN_AND_DISMISS;
      }
      else if (name == "next_level_bt")
      {
        //AudioController._AudioController.playOverlapEffect("yes_no_使用道具_按鍵音效");
        if (bt_handlers[2] != null)
          bt_handlers[2]();



        return DialogResponse.TAKEN_AND_DISMISS;
      }

    }

    return DialogResponse.PASS;
  }



  public DialogNetworkResponse setNetworkResponseEvent(string name, object payload)
  {
    return DialogNetworkResponse.PASS;
  }
}
