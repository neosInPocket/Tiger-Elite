using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Random = UnityEngine.Random;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Platform : MonoBehaviour
{
	[SerializeField] private Coin coinPrefab;
	[SerializeField] private Transform coinSpawnPosition;
	[Range(0, 1f)]
	[SerializeField] private float coinSpawnChance; 
	[SerializeField] private bool isSpawnCoin = true; 
	public const float zRotationValue = 30f;
	public Transform CoinSpawnPosition => coinSpawnPosition;
	
	private float[] rotations = new float[4] { -zRotationValue, 0, zRotationValue, 0 }; 
	private int rotationsPointer;
	private Platform lastPlatform;
	public Transform coinContainer;
	
	private void Awake()
	{
		rotationsPointer = GameController.rotationPointer;
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();
		coinContainer = GameObject.FindGameObjectWithTag("coinContainer").transform;
		Initialize();
	}
	
	public void Initialize()
	{
		SpawnCoin(coinContainer);
		Touch.onFingerDown += OnFingerDownHandler;
		
	}
	
	public void SpawnCoin(Transform coinContainer)
	{
		if (!isSpawnCoin) return;
		
		var random = Random.Range(0, 1f);
		if (random < coinSpawnChance)
		{
			Instantiate(coinPrefab, coinSpawnPosition.transform.position, Quaternion.identity, coinContainer.transform);
		}
	}
	
	public void SetIdentityRotation()
	{
		transform.rotation = Quaternion.identity;
	}
	
	private void OnFingerDownHandler(Finger finger)
	{
		if (!GameController._isPlaying) return;
		
		rotationsPointer += 1;
		if (rotationsPointer == 4)
		{
			rotationsPointer = 0;
		}
		GameController.rotationPointer = rotationsPointer;
		
		var rotation = rotations[rotationsPointer];
		transform.rotation = Quaternion.Euler(0, 0, rotation);
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Platform>(out Platform platform))
		{
			lastPlatform = platform;
		}
	}
	
	private void OnDestroy()
	{
		Touch.onFingerDown -= OnFingerDownHandler;
	}
	
	public void PlayDeath()
	{
		StopCoroutine(PlayEffect());
		StartCoroutine(PlayEffect());
	}
	
	private IEnumerator PlayEffect()
	{
		yield break;
	}
}
