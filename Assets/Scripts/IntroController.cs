using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Text _scoreLabel;

    // Start is called before the first frame update
    void Start()
    {
        var highScore = ScoreManager.GetScore();
        _scoreLabel.text = highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartClicked()
    {
        GameObject.Find("Main Camera").GetComponent<MainMenu>().StopMusic();
        SceneManager.LoadScene(1);
    }
}
