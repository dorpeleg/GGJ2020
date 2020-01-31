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
    [SerializeField] private float _fireRate = 3f;
    private bool _calledNow = false;
    private bool _gameOver = false;
    private MeshRenderer _meshRenderer;

    void Start()
    {
        _gameOver = false;
        _currentScene = 0;
        _nextFire = 0;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(Time.time > _nextFire && !_gameOver)
        {
            if (!_calledNow)
            {
                Debug.Log("NOW!");
                _calledNow = true;
            }
            if (Time.time > (_nextFire + _fireRelevantTime + _fireMaximumTime))
            {
                Debug.LogError("You Lose!");
                _gameOver = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
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
                    _currentScene++;
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

}
