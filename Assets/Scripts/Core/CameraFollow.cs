using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private PlayerBall player;
	[SerializeField] private float offset;
	public PlayerBall Player 
	{
		get => player;
		set => player = value;
	} 
	
	
	private void Update()
	{
		if (player == null || player.isDead) return;
		transform.position = new Vector3(transform.position.x, player.transform.position.y - offset, -10);
	}
}
