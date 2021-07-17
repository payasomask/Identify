using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinePosition : MonoBehaviour
{
  float timer = 0.0f;
  float duration;
  Vector3 current;
  Vector3 end;
  enum state {
    easing,
    done,
  }
  state currentstate = state.done;
  CommonAction Ondone;
  private void Start()
  {
    current = gameObject.transform.localPosition;
  }
  // Update is called once per frame
  void Update()
  {
    if (currentstate == state.done)
      return;

    timer += Time.deltaTime;

    float currentz = gameObject.transform.localPosition.z;

    if (timer>= duration){
      timer = 0.0f;
      currentstate = state.done;
      gameObject.transform.localPosition = new Vector3(end.x, end.y,currentz) ;
      current = end;
      if (Ondone != null)
        Ondone();
    }

    float currentx = (float)CurveUtil.Linear(timer, current.x, end.x - current.x, duration);
    float currenty = (float)CurveUtil.Linear(timer, current.y, end.y - current.y, duration);

    gameObject.transform.localPosition = new Vector3(currentx, currenty, currentz);
  }

  public void move(Vector3 end, float duration,CommonAction Ondone){
    if (currentstate == state.easing)
      return;

    this.Ondone = Ondone;
    timer = 0.0f;
    this.end = end;
    this.duration = duration;
    currentstate = state.easing;
  }
}
