//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour 
{
	public GameObject	m_target = null;
	public float		m_smooth = 1.0f;

	Vector3 m_lastTargetPosition = Vector3.zero;

	Vector3		m_defaultPosition = Vector3.zero;
	Quaternion	m_defaultRotation = Quaternion.identity;



	public void Follow(bool on)
	{
		if( on == false )
		{
			transform.position = m_defaultPosition;
			transform.rotation = m_defaultRotation;
			enabled = false;
		}
		else
		{
			enabled = true;
		}
	}


	// Use this for initialization
	void Start () 
	{
		if( m_target == null )
			return;

		m_lastTargetPosition = m_target.transform.position;

		m_defaultPosition = transform.position;
		m_defaultRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( m_target == null )
			return;

		m_lastTargetPosition = Vector3.Lerp(m_lastTargetPosition, m_target.transform.position, m_smooth * Time.deltaTime);
		transform.LookAt(m_lastTargetPosition );
	}
}
