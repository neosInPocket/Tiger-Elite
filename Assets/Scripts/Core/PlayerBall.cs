using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private TrailRenderer trailRenderer;
	[SerializeField] private GameObject effect;
	[SerializeField] private Rigidbody2D rb;
	
	public Rigidbody2D Rb => rb;
	public TrailRenderer Trail => trailRenderer;
	public bool isDead;
	private float[] gravityScales = new float[4] { 1, 0.7f, 0.5f, 0.3f };
	private Platform lastPlatform;
	public event Action TakeDamageEvent;
	
	public void Initialize()
	{
		rb.gravityScale = gravityScales[MainMenuController.CurrentGravityUpgrade];
	}
	
	public void PlayDeath(bool isWon)
	{
		if (isWon)
		{
			StartCoroutine(PlayEffect());
			return;
		}
		
		GameEventHandler.RaiseEvent(false);
		if (GameController.lives != 0)
		{
			StartCoroutine(TakeDamage());
		}
	}
	
	private IEnumerator PlayEffect()
	{
		isDead = true;
		spriteRenderer.color = new Color(0, 0, 0, 0);
		var deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(1f);
		Destroy(deathEffect);
	}
	
	private IEnumerator TakeDamage()
	{
		TakeDamageEvent?.Invoke();
		transform.position = lastPlatform.CoinSpawnPosition.transform.position;
		rb.angularVelocity = 0;
		rb.velocity = Vector2.zero;
		
		for (int i = 0; i < 9; i++)
		{
			spriteRenderer.color = spriteRenderer.color = new Color(1f, 1f, 1f, 0);
			trailRenderer.startColor = new Color(1f, 0f, 0f, 0f);
			trailRenderer.endColor = new Color(1f, 1f, 0f, 0f);
			yield return new WaitForSeconds(0.1f);
			spriteRenderer.color = spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
			trailRenderer.startColor = new Color(1f, 0f, 0f, 1f);
			trailRenderer.endColor = new Color(1f, 1f, 0f, 1f);
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Platform>(out Platform platform))
		{
			lastPlatform = platform;
		}
	}
	
}
