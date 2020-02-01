using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class TV : MonoBehaviour
{
    [SerializeField] private TVScenes _tvScenes;
    [SerializeField] private GameRules _rules;
    [SerializeField] private GameObject _gameOverObject;
    [SerializeField] private GameObject _kafaToLeft, _kafaToRight, _kafaDownwards, _kafaUpwards;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private MeshRenderer _snowMeshRenderer;
    [SerializeField] private string _hitSounds;
    [SerializeField] private Text _scoreLabel;

    private FMOD.Studio.EventInstance _tvAudio;
    private Dictionary<SwipeDirection, GameObject> _hitGraphic;
    private int _currentScene = 0;
    private bool _calledNow = false;
    private bool _gameOver = false;
    private float _nextFire = 0f;
    private SwipeDirection _neededDirection;
    private float _currenTimeBetweenScenes;
    private DateTime _startTime;
    private int _currentScore = 0;

    private const string ShaderKeyword = "_Direction";

    private float GameTime
    {
        get
        {
            var timePassed = DateTime.UtcNow - _startTime;
            return (float)timePassed.TotalSeconds;
        }
    }

    void Start()
    {
        PlayNextTVScene();
        string eventPath = _tvScenes.Scenes[_currentScene].AudioEvent;
        
        _gameOver = false;
        _currentScene = 0;
        _nextFire = 0;
        _rules.Initialize();
        _currenTimeBetweenScenes = _rules.GetTimeBetweenScenes();
        GestureManager.Instance.SwipeEvent += OnSwipeEvent;
        _startTime = DateTime.UtcNow;

        _hitGraphic = new Dictionary<SwipeDirection, GameObject>
        {
            {SwipeDirection.DOWN, _kafaDownwards },
            {SwipeDirection.LEFT, _kafaToLeft },
            {SwipeDirection.RIGHT, _kafaToRight },
            {SwipeDirection.UP, _kafaUpwards }
        };
    }

    private void OnSwipeEvent(object source, GestureEventArgs e)
    {
        Handheld.Vibrate();
        SceneCalculation(e.Direction);
    }

    void Update()
    {
        if(GameTime > _nextFire && !_gameOver)
        {
            if (GameTime > _nextFire + _rules.HitGoodThreshold)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("NOISE", 75f);
                // TODO: bad hit
            }
            if (!_calledNow)
            {
                int directionNumber = Random.Range(2, 5);
                _neededDirection = (SwipeDirection)directionNumber;
                Debug.Log("NOW! " + _neededDirection.ToString());
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("NOISE", 40f);
                UpdateTVOverlay(directionNumber);
                _calledNow = true;
            }
            if (GameTime > (_nextFire + _rules.HitLoseThreshold))
            {
                GameOver();
            }
#if UNITY_EDITOR
            WindowsInput();
#endif
        }
    }

    private void OnDestroy()
    {
        GestureManager.Instance.SwipeEvent -= OnSwipeEvent;
    }

    private void WindowsInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SceneCalculation(SwipeDirection.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SceneCalculation(SwipeDirection.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SceneCalculation(SwipeDirection.DOWN);
        }
    }

    private void SceneCalculation(SwipeDirection direction)
    {
        PlayKafaAnimation(direction);
        if(_neededDirection == direction)
        {
            if (GameTime <= _nextFire + _rules.HitGoodThreshold)
            {
                Debug.Log("current scene is " + _tvScenes.Scenes[_currentScene]);
            }
            else
            {
                Debug.Log("It's too late! current scene is " + _tvScenes.Scenes[_currentScene]);
            }
            _rules.IncreaseDifficulty();
            _currentScore++;
            ScoreManager.SaveHighestScore(_currentScore);
            _scoreLabel.text = _currentScore.ToString();
            PlayNextTVScene();
            _currenTimeBetweenScenes = _rules.GetTimeBetweenScenes();
            _nextFire = GameTime + _currenTimeBetweenScenes;
            _calledNow = false;
            Debug.Log("NEW ROUND");
            UpdateTVOverlay(0);
        }
        else
        {
            GameOver();
            Debug.Log("wrong move! " + direction.ToString());
        }
    }

    private void UpdateTVOverlay(int value)
    {
        _snowMeshRenderer.material.SetFloat(ShaderKeyword, value);
    }

    private void PlayNextTVScene()
    {
        var newScene = Random.Range(0, _tvScenes.Scenes.Count - 1);
        while(newScene == _currentScene)
        {
            newScene = Random.Range(0, _tvScenes.Scenes.Count - 1);
        }
        _currentScene = newScene;
        _videoPlayer.clip = _tvScenes.Scenes[_currentScene].VideoFile;
        _tvAudio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _tvAudio.release();
        string eventPath = _tvScenes.Scenes[_currentScene].AudioEvent;
        _tvAudio = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        _tvAudio.start();
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("NOISE", 0);
    }

    private void GameOver()
    {
        Debug.LogError("You Lose!");
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("NOISE", 100);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GameOver", 1);
        UpdateTVOverlay(10);
        _gameOver = true;
        _gameOverObject.SetActive(true);
    }

    private void PlayKafaAnimation(SwipeDirection direction)
    {
        if (!_hitGraphic[direction].activeSelf)
        {
            Debug.Log("kafa routine is called");
            StartCoroutine(KafaAppear(_hitGraphic[direction]));
        }
    }

    public void PlayHit()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_hitSounds);
    }

    private IEnumerator KafaAppear(GameObject kafaGraphic)
    {
        kafaGraphic.SetActive(true);
        if (kafaGraphic.GetComponent<Animator>() != null)
        {
            kafaGraphic.GetComponent<Animator>().Play("Kafa");
        }
        yield return new WaitForSeconds(0.45f);
        kafaGraphic.SetActive(false);
    }
}
