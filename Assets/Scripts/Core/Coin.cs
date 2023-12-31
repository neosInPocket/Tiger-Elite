using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private GameObject effect;
	[SerializeField] private ParticleSystem ps;
	private bool isCollected;
	
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (isCollected) return;
		
		if (collider.gameObject.TryGetComponent<PlayerBall>(out PlayerBall player))
		{
			isCollected = true;
			GameController._points += 2;
			GameEventHandler.RaiseEvent(true);
			PlayDeath(false);
		}
	}
	
	public void PlayDeath(bool isWon)
	{
		if (!isWon)
		{
			AudioEvent.RaiseEvent(AudioTypes.CoinCollect);
		}
		StartCoroutine(PlayEffect());
	}
	
	private IEnumerator PlayEffect()
	{	
		ps.Stop();
		spriteRenderer.color = new Color(0, 0, 0, 0);
		var deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(1f);
		Destroy(deathEffect);
		Destroy(this.gameObject);
	}
}
