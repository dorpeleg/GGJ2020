using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{
    [SerializeField] private Color[] _tvScenes = new Color[5];
    [SerializeField] private int _currentScene = 0;
    [SerializeField] private float _nextFire = 0f;
    [SerializeField] private float _fireRelevantTime = 3f;
    [SerializeField] private float _fireMaximumTime = 3f;
    [SerializeField] private float _fireRateMin = 1f;
    [SerializeField] private float _fireRateMax = 5f;
    [SerializeField] private float _fireRate = 3f;
    private bool _calledNow = false;
    private bool _gameOver = false;
    private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _pointLight;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _screenNoise;
    [SerializeField] private GameObject _kafaToLeft, _kafaToRight, _kafaDownwards;
    SwipeDirection neededDirection;
    //SwipeDirection neededDirection;

    void Start()
    {
        if (_pointLight == null)
            Debug.LogError("Light is null");
        _pointLight.SetActive(false);
        _gameOver = false;
        _currentScene = 0;
        _nextFire = 0;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(Time.time > _nextFire && !_gameOver)
        {
            if(Time.time > _nextFire + _fireRelevantTime)
                _screenNoise.SetActive(true);
            if (!_calledNow)
            {
                int directionNumber = Random.Range(2, 5);
                neededDirection = (SwipeDirection)directionNumber;
                Debug.Log("NOW! " + neededDirection.ToString());
                _pointLight.SetActive(true);
                _calledNow = true;
            }
            if (Time.time > (_nextFire + _fireMaximumTime))
            {
                GameOver();
            }
#if UNITY_EDITOR && !UNITY_ANDROID
            WindowsInput();
#endif
        }
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
                if (Time.time <= _nextFire + _fireRelevantTime)
                {
                    Debug.Log("current scene is " + _tvScenes[_currentScene]);
                }
                else
                {
                    Debug.Log("It's too late! current scene is " + _tvScenes[_currentScene]);
                }
                _meshRenderer.material.color = _tvScenes[_currentScene];
                _pointLight.SetActive(false);
                _currentScene++;
                _fireRate = Random.Range(_fireRateMin, _fireRateMax);
                _nextFire = Time.time + _fireRate;
                _calledNow = false;
            }
            else
            {
                Debug.Log("You Won!");
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
            _screenNoise.SetActive(false);
            StartCoroutine(KafaAppear(_kafaToLeft));
        }  
    }

    private void GameOver()
    {
        Debug.LogError("You Lose!");
        _pointLight.SetActive(false);
        _gameOver = true;
        _gameOverText.SetActive(true);
    }

    private void KafaToTheRight()
    {
        if (!_kafaToRight.activeSelf)
        {
            Debug.Log("kafa routine is called");
            _screenNoise.SetActive(false);
            StartCoroutine(KafaAppear(_kafaToRight));
        }
    }

    private void KafaDownwards()
    {
        if (!_kafaDownwards.activeSelf)
        {
            Debug.Log("kafa routine is called");
            _screenNoise.SetActive(false);
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
