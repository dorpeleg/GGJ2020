using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour
{
    [SerializeField] private TVScenes _tvScenes;
    [SerializeField] private int _currentScene = 0;
    [SerializeField] private float _hitGoodThreshold = 3f;
    [SerializeField] private float _hitLoseThreshold = 3f;
    [SerializeField] private float _minTimeBetweenScenes = 1f;
    [SerializeField] private float _maxTimeBetweenScenes = 5f;
    [SerializeField] private float _timeBetweenScenes = 3f;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _kafaToLeft, _kafaToRight, _kafaDownwards;
    [SerializeField] private VideoPlayer _videoPlayer;
    private bool _calledNow = false;
    private bool _gameOver = false;
    private MeshRenderer _meshRenderer;
    private float _nextFire = 0f;
    private SwipeDirection _neededDirection;
    private AudioSource _audio;
    //Vector3 _kafaToLeftPosition, _kafaToRightPosition, _kafaDownwardsPosition;
    //Vector3 _kafaToLeftEndposition = new Vector3(5.74f, 2.489f, 0);
    //Vector3 _kafaToRightEndposition = new Vector3(-6.299f, 2.489f, 0);
    //Vector3 _kafaDownwardsEndposition = new Vector3(-1.27f, 4.08f, 0);

    // TEMP
    public UnityEngine.UI.Text Label;

    private const string ShaderKeyword = "_Direction";

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _gameOver = false;
        _currentScene = 0;
        _nextFire = 0;
        _meshRenderer = GetComponent<MeshRenderer>();
        //_kafaToLeftPosition = _kafaToLeft.transform.position;
        //_kafaToRightPosition = _kafaToRight.transform.position;
        //_kafaDownwardsPosition = _kafaDownwards.transform.position;
        GestureManager.Instance.SwipeEvent += OnSwipeEvent;
    }

    private void OnSwipeEvent(object source, GestureEventArgs e)
    {
        SceneCalculation(e.Direction);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (_kafaToLeft.GetComponent<Animation>() != null)
        //        _kafaToLeft.GetComponent<Animation>().Play("KafaToLeftAnimation");
        //}

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
                Label.text = _neededDirection.ToString();
                _meshRenderer.material.SetFloat(ShaderKeyword, directionNumber);
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

    private void AndroidInput()
    {

    }

    private void WindowsInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            KafaToTheRight();
            SceneCalculation(SwipeDirection.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            KafaToTheLeft();
            SceneCalculation(SwipeDirection.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            KafaDownwards();
            SceneCalculation(SwipeDirection.DOWN);
        }
    }

    private void SceneCalculation(SwipeDirection direction)
    {
        if(_neededDirection == direction)
        {
            if (_currentScene < _tvScenes.Scenes.Count)
            {
                if (Time.time <= _nextFire + _hitGoodThreshold)
                {
                    Debug.Log("current scene is " + _tvScenes.Scenes[_currentScene]);
                }
                else
                {
                    Debug.Log("It's too late! current scene is " + _tvScenes.Scenes[_currentScene]);
                }
                _videoPlayer.clip = _tvScenes.Scenes[_currentScene].VideoFile;
                _currentScene++;
                _timeBetweenScenes = Random.Range(_minTimeBetweenScenes, _maxTimeBetweenScenes);
                _nextFire = Time.time + _timeBetweenScenes;
                _calledNow = false;
                Debug.Log("NEW ROUND");
                Label.text = "";
                _meshRenderer.material.SetFloat(ShaderKeyword, 0);
            }
            else
            {
                Debug.Log("You Won!");
                Label.text = "";
                _gameOver = true;
            }
        }
        else
        {
            GameOver();
            Debug.Log("wrong move! " + direction.ToString());
            Label.text = "";
        }
    }

    private void KafaToTheLeft()
    {
        if (!_kafaToLeft.activeSelf)
        {
            Debug.Log("kafa routine is called");
            StartCoroutine(KafaAppear(_kafaToLeft));
        }  
    }

    private void GameOver()
    {
        Debug.LogError("You Lose!");
        _gameOver = true;
        _gameOverText.SetActive(true);
    }

    private void KafaToTheRight()
    {
        if (!_kafaToRight.activeSelf)
        {
            Debug.Log("kafa routine is called");
            StartCoroutine(KafaAppear(_kafaToRight));
        }
    }

    private void KafaDownwards()
    {
        if (!_kafaDownwards.activeSelf)
        {
            Debug.Log("kafa routine is called");
            StartCoroutine(KafaAppear(_kafaDownwards));
        }
    }

    public void PlayBoom()
    {
        _audio.Play();
    }
    

    IEnumerator KafaAppear(GameObject kafaGraphic)
    {
        kafaGraphic.SetActive(true);
        if (kafaGraphic.GetComponent<Animator>() != null)
            kafaGraphic.GetComponent<Animator>().Play("Kafa");
        yield return new WaitForSeconds(0.45f);
        kafaGraphic.SetActive(false);
    }

}
