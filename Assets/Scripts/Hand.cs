using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private TV _tv;
    // Start is called before the first frame update
    void Start()
    {
        _tv = GameObject.Find("TV").GetComponent<TV>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBoom()
    {
        Debug.Log("trying to play boom");
        _tv.PlayHit();
    }
}
