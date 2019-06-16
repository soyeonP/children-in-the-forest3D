//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class DestroyTime : MonoBehaviour 
{
	public float m_lifeTime;

	// Use this for initialization
	void Start () 
	{
		Destroy(gameObject, m_lifeTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
