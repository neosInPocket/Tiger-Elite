using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour
{
	[SerializeField] private Transform object1;
	[SerializeField] private Transform object2;
	
	private void Start()
	{
		Debug.Log($"Position distance {Mathf.Abs(object1.position.y - object2.position.y)}");
	}
}
