using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private bool isRight;
	private Vector2 screenBounds;
	private float objectWidth;
	
	private void Start()
	{
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		objectWidth = spriteRenderer.bounds.size.x / 2;
		if (isRight)
		{
			transform.position = new Vector2(screenBounds.x + objectWidth, transform.position.y);
		}
		else
		{
			transform.position = new Vector2(-screenBounds.x - objectWidth, transform.position.y);
		}
	}
	
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.TryGetComponent<PlayerBall>(out PlayerBall player))
		{
			player.PlayDeath(false);
		}
	}
}
