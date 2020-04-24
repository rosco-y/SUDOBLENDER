using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sudoCube : MonoBehaviour
{

    private Transform _lookAtCamera;
    // Start is called before the first frame update
    void Start()
    {
        _lookAtCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_lookAtCamera);
        transform.rotation = _lookAtCamera.rotation;
    }
}
