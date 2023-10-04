using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	[SerializeField] private Canvas _backgroundCanvas;
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private Camera _backGroundCamera;
	[SerializeField] private TutorialScreen _tutor;
	[SerializeField] private UIHealth _uiHealth; 
	[SerializeField] private FadeScreen _fadeScreen;
	[SerializeField] private MainMenuController _mainMenuController;
	[SerializeField] private GameScreen _gameScreen;
	[SerializeField] private WinScreen _countDownScreen; 
	[SerializeField] private WinScreen _defeatScreen; 
	[SerializeField] private WinScreenWithCoins _winScreen; 
	[SerializeField] private ProgressBar _levelProgress;
	[SerializeField] private Transform enemySpawnPosition;
	[SerializeField] private List<Transform> _coinsSpawnPoints;
	[SerializeField] private Transform coinContainer;
	[SerializeField] private ParticleSystem _particleSystem;
	
	private float _spawnDelay = 3f;
	private float _playDelay;
	public static int _levelCoins;
	private static int _levelMaxPoints;
	public static int _points;
	private bool _isSpawning;
	public static bool _isPlaying = false;
	private bool isTutor = false;
	public static int lives;
	public static bool isWon;
	
	private void Awake()
	{
		_isPlaying = false;
		GameEventHandler.OnEvent += OnEventHandler;
	}
	
	private void Update()
	{
		if (!_isPlaying) return;
		if (_isSpawning) return;
	}
	
	public void Initialize()
	{
		_particleSystem.gameObject.SetActive(false);
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
			_particleSystem.gameObject.SetActive(true);
			return;
		}
		
		if (lives <= 0)
		{
			_isPlaying = false;
			_points = 0;
			_defeatScreen.gameObject.SetActive(true);
			_defeatScreen.Show();
			return;
		}
	}
	
	public void ReturnToTheMainMenu()
	{
		_fadeScreen.Fade();
		_fadeScreen.OnFadeEnd += OnFadeMainMenuEnd;
	}
	
	private void OnFadeMainMenuEnd()
	{
		_fadeScreen.OnFadeEnd -= OnFadeMainMenuEnd;
		_gameScreen.gameObject.SetActive(false);
		_mainMenuController.Initialize();
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
	}
	
	public void UpdateUI()
	{
		var progress = _points / _levelMaxPoints;
		_levelProgress.Refresh(progress);
	}
}
