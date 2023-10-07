using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	[SerializeField] private int sceneNumber;
	
	public void ChangeScene()
	{
		SceneManager.LoadScene(sceneNumber);
	}
}
