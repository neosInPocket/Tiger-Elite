using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	[SerializeField] private Canvas _backgroundCanvas;
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private Camera _backGroundCamera;
	[SerializeField] private TutorialScreen _tutor;
	[SerializeField] private UIHealth _uiHealth; 
	[SerializeField] private FadeScreen _fadeScreen;
	[SerializeField] private GameScreen _gameScreen;
	[SerializeField] private WinScreen _countDownScreen; 
	[SerializeField] private WinScreen _defeatScreen; 
	[SerializeField] private WinScreenWithCoins _winScreen; 
	[SerializeField] private ProgressBar _levelProgress;
	[SerializeField] private Transform coinContainer;
	[SerializeField] private Transform platformContainer;
	[SerializeField] private PlayerBall player;	
	[SerializeField] private List<Platform> defaultPlatforms;
	[SerializeField] private Transform defaultPlayerPosition;
	[SerializeField] private PlatformCreateTrigger platformCreateTrigger;
	[SerializeField] private float particleOffset;
	[SerializeField] private float particleTriggerOffset;
	
	public static float[] rotations = new float[4] { -Platform.zRotationValue, 0, Platform.zRotationValue, 0 }; 
	private float _playDelay;
	public static int _levelCoins;
	private static int _levelMaxPoints;
	public static int _points;
	private bool _isSpawning;
	public static bool _isPlaying = false;
	private bool isTutor = false;
	public static int lives;
	public static bool isWon;
	public static int rotationPointer;
	private void Awake()
	{
		_isPlaying = false;
		GameEventHandler.OnEvent += OnEventHandler;
		player.TakeDamageEvent += PlayerTakeDamage;
		Initialize();
	}
	
	public void Initialize()
	{
		player.isDead = false;
		DeleteObjects();
		SetPlatformsIdentity();
		SpawnDefaultCoins();
		player.transform.position = defaultPlayerPosition.position;
		player.Rb.velocity = Vector2.zero;
		player.Rb.angularVelocity = 0;
		player.transform.rotation = Quaternion.identity;
		platformCreateTrigger.Initialize();
		player.Trail.Clear();
		
		rotationPointer = 1;
		_isPlaying = false;
		isWon = false;
		_backgroundCanvas.worldCamera = _backGroundCamera;
		
		lives = MainMenuController.CurrentLivesUpgrade;
		_levelMaxPoints = (int)(Mathf.Log(MainMenuController.CurrentLevel + 2) * 5);
		_levelCoins = (int)(Mathf.Log(MainMenuController.CurrentLevel + 2) * 10) + 50;
		_gameScreen.gameObject.SetActive(true);
		_gameScreen.Refresh();
		_levelProgress.Refresh(0);
		_points = 0;
		_playDelay = (int)_countDownScreen.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
		_uiHealth.RefreshLifes(MainMenuController.CurrentLivesUpgrade);
		isTutor = false;
		
		if (MainMenuController.IsFirstTime == "yes")
		{
			MainMenuController.IsFirstTime = "no";
			SaveLoad.Save();
			isTutor = true;
			_tutor.PlayTutor();
		}
		else
		{
			_countDownScreen.gameObject.SetActive(true);
			_countDownScreen.Show();
		}
		StartCoroutine(PlayDelay());
	}
	
	private void OnEventHandler(bool value)
	{
		if (!_isPlaying) return;
		
		if (!value)
		{
			_fadeScreen.ProcessTakeDamage();
			lives--;
			_uiHealth.RefreshLifes(lives);
		}
		
		_levelProgress.Refresh((float)_points / (float)_levelMaxPoints);
		
		if (_points >= _levelMaxPoints)
		{
			_isPlaying = false;
			isWon = true;
			MainMenuController.CurrentLevel++;
			MainMenuController.Coins += _levelCoins;
			SaveLoad.Save();
			_winScreen.gameObject.SetActive(true);
			_winScreen.Show(_levelCoins);
			DeleteCoins();
			SetPlayerDefaults();
			return;
		}
		
		if (lives <= 0)
		{
			_isPlaying = false;
			_points = 0;
			_defeatScreen.gameObject.SetActive(true);
			_defeatScreen.Show();
			DeleteCoins();
			SetPlayerDefaults();
			AudioEvent.RaiseEvent(AudioTypes.PlayerDie);
			return;
		}
	}
	
	private void SetPlayerDefaults()
	{
		player.Rb.velocity = Vector2.zero;
		player.Rb.gravityScale = 0;
	}
	
	public void ReturnToTheMainMenu()
	{
		_fadeScreen.Fade();
		_fadeScreen.OnFadeEnd += OnFadeMainMenuEnd;
	}
	
	private void OnFadeMainMenuEnd()
	{
		_fadeScreen.OnFadeEnd -= OnFadeMainMenuEnd;
		SceneManager.LoadScene(1);
	}
	
	private IEnumerator PlayDelay()
	{
		if (isTutor)
		{
			_playDelay = 18f;
		}
		yield return new WaitForSeconds(_playDelay + 0.5f);
		_countDownScreen.gameObject.SetActive(false);
		_isPlaying = true;
		player.Initialize();
	}
	
	public void UpdateUI()
	{
		var progress = _points / _levelMaxPoints;
		_levelProgress.Refresh(progress);
	}
	
	public void DeleteObjects()
	{
		foreach (Transform child in platformContainer)
		{
			Destroy(child.gameObject);
		}
	}
	
	public void DeleteCoins()
	{
		foreach (Transform child in coinContainer)
		{
			if (child.gameObject.TryGetComponent<Coin>(out Coin coin))
			{
				coin.PlayDeath(true);
			}
		}
	}
	
	public void SpawnDefaultCoins()
	{
		foreach (var platform in defaultPlatforms)
		{
			platform.SpawnCoin(coinContainer);
		}
	}
	
	private void SetPlatformsIdentity()
	{
		foreach (var platform in defaultPlatforms)
		{
			platform.SetIdentityRotation();
		}
		
		foreach (var platform in defaultPlatforms)
		{
			platform.SetIdentityRotation();
		}
	}
	
	public void PlayerTakeDamage()
	{
		SetPlatformsIdentity();
	}
}
