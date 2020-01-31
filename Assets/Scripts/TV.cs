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
    [SerializeField] private GameObject _screenNoise;
    [SerializeField] private GameObject _kafa;

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
                Debug.Log("NOW!");
                _pointLight.SetActive(true);
                _calledNow = true;
            }
            if (Time.time > (_nextFire + _fireMaximumTime))
            {
                Debug.LogError("You Lose!");
                _pointLight.SetActive(false);
                _gameOver = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Kafa();
                if (_currentScene < _tvScenes.Length)
                {
                    if(Time.time <= _nextFire + _fireRelevantTime)
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
        }
    }

    private void Kafa()
    {
        if (!_kafa.activeSelf)
        {
            Debug.Log("kafa routine is called");
            _screenNoise.SetActive(false);
            StartCoroutine(KafaAppear());
        }  
    }

    IEnumerator KafaAppear()
    {
        _kafa.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _kafa.SetActive(false);
    }

}
