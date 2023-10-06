using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	[SerializeField] private AudioSource _music;
	[SerializeField] private AudioSource _ballDrop;
	[SerializeField] private AudioSource _coinCollect;
	[SerializeField] private AudioSource _playerDie;
	
	public float volume => _music.volume;
	
	private void Start()
	{
		AudioEvent.OnEvent += PlaySound;
	}
	
	private void PlaySound(AudioTypes type)
	{
		switch (type)
		{
			case AudioTypes.BallDrop:
				_ballDrop.Play();
				break;
				
			case AudioTypes.CoinCollect:
				_coinCollect.Play();
				break;
				
			case AudioTypes.PlayerDie:
				_playerDie.Play();
				break;
		}
		
	}
	
	public void ChangeVolume(float value)
	{
		_music.volume = value;
	}
}
