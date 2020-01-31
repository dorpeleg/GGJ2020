using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{
    [SerializeField] private Color[] _tvScenes = new Color[5];
    [SerializeField] private int _currentScene = 0;
    private float _nextFire = 0f;
    [SerializeField] private float _hitGoodThreshold = 3f;
    [SerializeField] private float _hitLoseThreshold = 3f;
    [SerializeField] private float _minTimeBetweenScenes = 1f;
    [SerializeField] private float _maxTimeBetweenScenes = 5f;
    [SerializeField] private float _timeBetweenScenes = 3f;
    private bool _calledNow = false;
    private bool _gameOver = false;
    private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _kafaToLeft, _kafaToRight, _kafaDownwards;
    SwipeDirection neededDirection;
    public UnityEngine.UI.Text Label;

    private const string ShaderKeyword = "_Direction";

    void Start()
    {
        _gameOver = false;
        _currentScene = 0;
        _nextFire = 0;
        _meshRenderer = GetComponent<MeshRenderer>();
        GestureManager.Instance.SwipeEvent += OnSwipeEvent;
    }

    private void OnSwipeEvent(object source, GestureEventArgs e)
    {
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
                neededDirection = (SwipeDirection)directionNumber;
                Debug.Log("NOW! " + neededDirection.ToString());
                Label.text = neededDirection.ToString();
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
        if(neededDirection == direction)
        {
            if (_currentScene < _tvScenes.Length)
            {
                if (Time.time <= _nextFire + _hitGoodThreshold)
                {
                    Debug.Log("current scene is " + _tvScenes[_currentScene]);
                }
                else
                {
                    Debug.Log("It's too late! current scene is " + _tvScenes[_currentScene]);
                }
                //_meshRenderer.material.color = _tvScenes[_currentScene];
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
                _gameOver = true;
            }
        }
        else
        {
            GameOver();
            Debug.Log("wrong move! " + direction.ToString());
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

    IEnumerator KafaAppear(GameObject kafaGraphic)
    {
        kafaGraphic.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        kafaGraphic.SetActive(false);
    }

}
