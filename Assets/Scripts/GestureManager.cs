using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
	public static GestureManager Instance
	{
		if(_instance == null)
		{
			var go = new GameObject();
			_instance = go.AddComponent<GestureManager>();
		}
		return _instance;
	}
	private static GestureManager _instance;
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
