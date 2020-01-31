using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventTest : MonoBehaviour
{
    public UnityEngine.UI.Text LABEL;

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.SwipeEvent += Instance_SwipeEvent;
    }

    private void OnDestroy()
    {
        GestureManager.Instance.SwipeEvent -= Instance_SwipeEvent;
    }

    private void Instance_SwipeEvent(object source, GestureEventArgs e)
    {
        Log(e.Direction.ToString());
    }

    private void Log(string str)
    {
        LABEL.text += $"\n" + str;
    }
}
