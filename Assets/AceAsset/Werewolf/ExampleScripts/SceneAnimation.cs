//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SceneAnimation : MonoBehaviour
{

	public string[]		m_animationList;

	static Vector2		m_scrollPos = Vector2.zero;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if( Input.GetKeyDown(KeyCode.R) == true )
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}


	void OnGUI()
	{
		GUILayout.Space(10);

		if( GUILayout.Button("Reset Scene : R", GUILayout.Height(30)) == true )
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		GUILayout.Space(10);

		m_scrollPos = GUILayout.BeginScrollView(m_scrollPos);

		foreach( string ani in m_animationList )
		{
			if( GUILayout.Button(ani, GUILayout.Height(30)) == true )
			{
				Animator[] animators = GameObject.FindObjectsOfType<Animator>();

				foreach( Animator animator in animators )
				{
					animator.CrossFade(ani, 0.1f);
				}
			}
		}

		GUILayout.EndScrollView();

	}
}
