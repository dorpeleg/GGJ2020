using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] float _power;

    private Vector3 _originalPos;

    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;
    }

    private void OnDisable()
    {
        transform.position = _originalPos;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Random.Range(-_power, _power);
        var y = Random.Range(-_power, _power);
        transform.position = new Vector3(x, y, _originalPos.z);
    }
}
