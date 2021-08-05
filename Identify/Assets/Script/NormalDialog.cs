using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NormalDialog :  IDialogContext
{

  public NormalDialog(string text,InteractiveDiaLogHandler[] handlers){
    bt_handlers = handlers;
    this.text = text;
  }
  bool binited = false;
  GameObject dlgGO = null;
  InteractiveDiaLogHandler[] bt_handlers;
  string text;
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
      dlgGO = abl.InstantiatePrefab("NormalDialog");

    dlgGO.transform.Find("bg/text").GetComponent<TextMeshPro>().text = text;

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
      if(name == "yes_bt")
      {

        if (bt_handlers[0] != null)
          bt_handlers[0]();

        return DialogResponse.TAKEN_AND_DISMISS;
      }
      else if(name == "no_bt")
      {
        if (bt_handlers[1] != null)
          bt_handlers[1]();

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
