using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Platform : MonoBehaviour
{
	private void Awake()
	{
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();
	}
	
	public void Initialize()
	{
		Touch.onFingerDown += OnFingerDownHandler;
	}
	
	private void OnFingerDownHandler(Finger finger)
	{
		
	}
}
