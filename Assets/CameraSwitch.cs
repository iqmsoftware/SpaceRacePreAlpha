using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera Inside;
    public Camera Outside;

    private Camera _currentCamera;

    void Awake()
    {
        if (Inside != null)
        {
            _currentCamera = Inside;
        }
    }

    // Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyUp(KeyCode.F))
	    {
	        if (Inside != null && Outside != null)
	        {
	            _currentCamera = _currentCamera != Inside ? Inside : Outside;

	            if (_currentCamera == Inside)
	            {
                    Outside.gameObject.SetActive(false);
                    Inside.gameObject.SetActive(true);
                }
	            else
	            {
                    Outside.gameObject.SetActive(true);
                    Inside.gameObject.SetActive(false);
                }
            }
	    }
	}
}
