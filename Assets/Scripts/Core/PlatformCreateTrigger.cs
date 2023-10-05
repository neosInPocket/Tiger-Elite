using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformCreateTrigger : MonoBehaviour
{
	[SerializeField] private GameObject doublePlatform;
	[SerializeField] private GameObject singlePlatform;
	[SerializeField] private float maxPlatformSpawnDistance;
	private Transform platformContainer;
	private Transform currentPlatform;
	private float currentPoint;
	private float dy;
	private bool isSinglePlatform = false;
	
	private void Start()
	{
		Initialize();	
	}
	
	public void Initialize()
	{
		platformContainer = GameObject.FindGameObjectWithTag("platformContainer").transform;
		currentPlatform = GameObject.FindGameObjectWithTag("lastPlatform").transform;
		
		dy = Mathf.Abs(transform.position.y - currentPlatform.position.y);
	}
	
	private void Update()
	{
		if (currentPlatform == null || !GameController._isPlaying) return;
		currentPoint = transform.position.y - dy;
		
		if (Mathf.Abs(currentPlatform.transform.position.y - currentPoint) > maxPlatformSpawnDistance)
		{
			GameObject platform;
			if (isSinglePlatform)
			{
				platform = singlePlatform;
			}
			else
			{
				platform = doublePlatform;
			}
			
			isSinglePlatform = !isSinglePlatform;
			
			currentPlatform = Instantiate(
				platform, 
				new Vector2(0, currentPoint), Quaternion.Euler(0, 0, GameController.rotations[GameController.rotationPointer]), 
				platformContainer
				).transform;
		}
	}
	
	void OnDrawGizmosSelected()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(new Vector2(0, currentPoint), 0.55f);
	}
}
