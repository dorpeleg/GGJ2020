using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _menuMusic;
    FMOD.Studio.EventInstance bgMusic;
    // Start is called before the first frame update
    void Start()
    {
        bgMusic = FMODUnity.RuntimeManager.CreateInstance(_menuMusic);
        bgMusic.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMusic()
    {
        bgMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgMusic.release();
    }
}
