using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	[SerializeField] private PlayerBall player;
	
	public PlayerBall Player 
	{
		get => player;
		set => player = value;
	}
	
	public void Initialize(PlayerBall player)
	{
		Player = player;
	}
	
	private void Update()
	{
		if (player == null || player.isDead) return;
		var position = new Vector2(transform.position.x, player.transform.position.y);
		transform.position = position;
	}
}
