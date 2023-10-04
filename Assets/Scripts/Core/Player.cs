using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private Camera _mainCamera;
	
	private void Update()
	{
		var cameraPosition = new Vector2(_mainCamera.transform.position.x, transform.position.y);
		_mainCamera.transform.position = cameraPosition;
	}
}
