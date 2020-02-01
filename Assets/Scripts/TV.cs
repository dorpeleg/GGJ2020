using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour
{
    [SerializeField] private TVScenes _tvScenes;
    [SerializeField] private float _hitGoodThreshold = 3f;
    [SerializeField] private float _hitLoseThreshold = 3f;
    [SerializeField] private float _minTimeBetweenScenes = 1f;
    [SerializeField] private float _maxTimeBetweenScenes = 5f;
    [SerializeField] private float _timeBetweenScenes = 3f;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _kafaToLeft, _kafaToRight, _kafaDownwards, _kafaUpwards;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private MeshRenderer _snowMeshRenderer;
    //[SerializeField] private string[] _hitSounds;
    [SerializeField] private string _hitSounds;
    FMOD.Studio.EventInstance tvAudio;

    private Dictionary<SwipeDirection, GameObject> _hitGraphic;
    private int _currentScene = 0;
    private bool _calledNow = false;
    private bool _gameOver = false;
    private float _nextFire = 0f;
    private SwipeDirection _neededDirection;
    private AudioSource _audio;

    private const string ShaderKeyword = "_Direction";

    void Start()
    {
        string eventPath = _tvScenes.Scenes[_currentScene].AudioEvent;
        tvAudio = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        tvAudio.start();
        //_audio = GetComponent<AudioSource>();
        _gameOver = false;
        _currentScene = 0;
        _nextFire = 0;
        GestureManager.Instance.SwipeEvent += OnSwipeEvent;

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
        if(Time.time > _nextFire && !_gameOver)
        {
            if (Time.time > _nextFire + _hitGoodThreshold)
            {
                // TODO: bad hit
            }
            if (!_calledNow)
            {
                int directionNumber = Random.Range(2, 5);
                _neededDirection = (SwipeDirection)directionNumber;
                Debug.Log("NOW! " + _neededDirection.ToString());
                UpdateTVOverlay(directionNumber);
                _calledNow = true;
            }
            if (Time.time > (_nextFire + _hitLoseThreshold))
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
            if (Time.time <= _nextFire + _hitGoodThreshold)
            {
                Debug.Log("current scene is " + _tvScenes.Scenes[_currentScene]);
            }
            else
            {
                Debug.Log("It's too late! current scene is " + _tvScenes.Scenes[_currentScene]);
            }
            PlayNextTVScene();
            _timeBetweenScenes = Random.Range(_minTimeBetweenScenes, _maxTimeBetweenScenes);
            _nextFire = Time.time + _timeBetweenScenes;
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
        string eventPath = _tvScenes.Scenes[_currentScene].AudioEvent;
        tvAudio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        tvAudio = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        tvAudio.start();
        //_audioSource.Stop();
        //_audioSource.PlayOneShot(_tvScenes.Scenes[_currentScene].AudioFile);
    }

    private void GameOver()
    {
        Debug.LogError("You Lose!");
        UpdateTVOverlay(10);
        _gameOver = true;
        _gameOverText.SetActive(true);
    }

    private void PlayKafaAnimation(SwipeDirection direction)
    {
        if (!_kafaDownwards.activeSelf)
        {
            Debug.Log("kafa routine is called");
            StartCoroutine(KafaAppear(_hitGraphic[direction]));
        }
    }

    public void PlayHit()
    {
        //var index = Random.Range(0, _hitSounds.Length - 1);
        //_audio.PlayOneShot(_hitSounds[index]);
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
