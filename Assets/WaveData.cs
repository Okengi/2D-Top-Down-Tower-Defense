using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Wave", menuName ="Custom/Wave")]
public class WaveData : ScriptableObject
{
	[SerializeField]
	public int enemyCount;
}
