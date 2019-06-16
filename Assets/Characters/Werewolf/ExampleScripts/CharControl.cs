//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CharControl : MonoBehaviour 
{
	//---------------
	// public
	//---------------
	public bool		m_enableControl = true;
	public float	m_turnSpeed = 10.0f;
	public float	m_moveSpeed = 2.0f;
	public float	m_runSpeedScale = 2.0f;

	public Vector3	m_attackOffset = Vector3.zero;
	public float	m_attackRadius = 1.0f;

	public string[]	m_damageReaction;

	public GameObject	m_hitEffect = null;

	//---------------
	// private
	//---------------
	private Animator m_ani = null;
	private CharacterController m_char = null;
	private Vector3 m_moveDir = Vector3.zero;
	private bool	m_isRun = false;
	private float	m_moveSpeedScale = 1.0f;


	// Use this for initialization
	void Start () 
	{
		m_ani = GetComponent<Animator>();
		m_char = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//------------------
		//Parameters Reset
		//------------------
		m_moveDir = Vector3.zero;


		//------------------
		//Update Control
		//------------------
		if( m_enableControl == true )
		{
			m_moveSpeedScale = m_ani.GetFloat("SpeedScale");
			UpdateControl();
		}


		//------------------
		//Parameters sync
		//------------------

		float speed = m_moveDir.magnitude;
		if( m_isRun == true ) speed *= m_runSpeedScale;

		m_ani.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
	}

	private void UpdateControl()
	{
		UpdateMoveControl();
		UpdateActionControl();
	}

	private void UpdateMoveControl()
	{
		Vector3 dir = Vector3.zero;

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		dir.x = h;
		dir.z = v;


		dir = Camera.main.transform.TransformDirection(dir);
		dir.y = 0.0f;
		dir = Vector3.ClampMagnitude(dir, 1.0f);

		m_moveDir = dir;

		//Run key check
		m_isRun = Input.GetKey(KeyCode.LeftShift);

		if( m_isRun == true )
		{
			dir *= m_runSpeedScale;
		}

		if( dir.magnitude > 0.01f )
		{
			transform.forward = Vector3.RotateTowards(transform.forward, dir, m_turnSpeed * Time.deltaTime, m_turnSpeed);
			m_char.Move(dir * Time.deltaTime * m_moveSpeed * m_moveSpeedScale);
		}

		if( Input.GetButton("Jump") == true && m_char.isGrounded == true )
		{
			m_ani.SetTrigger("Jump");
		}

	}

	private void UpdateActionControl()
	{
		if( Input.GetKey(KeyCode.Z) == true )
		{
			m_ani.SetInteger("Action", 1);
		}
		else if( Input.GetKey(KeyCode.X) == true )
		{
			m_ani.SetInteger("Action",2);
		}
		else if( Input.GetKey(KeyCode.C) == true )
		{
			m_ani.SetInteger("Action", 3);
		}
		else if( Input.GetKey(KeyCode.V) == true )
		{
			m_ani.SetInteger("Action", 4);
		}
		else if( Input.GetKey(KeyCode.B) == true )
		{
			m_ani.SetInteger("Action", 5);
		}
		else
		{
			m_ani.SetInteger("Action", 0);
		}
	}

	void EventSkill(string skillName)
	{
		SendMessage(skillName, SendMessageOptions.DontRequireReceiver);
	}


	void EventAttack()
	{
		Vector3 center = transform.TransformPoint(m_attackOffset);
		float radius = m_attackRadius;


		Debug.DrawRay(center, transform.forward ,Color.red , 0.5f);

		Collider[] cols = Physics.OverlapSphere(center, radius);

		foreach( Collider col in cols )
		{
			CharControl charControl  = col.GetComponent<CharControl>();
			if( charControl == null )
				continue;

			if( charControl == this)
				continue;

			charControl.TakeDamage(this, center , transform.forward , 1.0f);
		}
	}

	public void TakeDamage(CharControl other,Vector3 hitPosition,  Vector3 hitDirection, float amount)
	{
		//-------------------------
		// Please enter your code.
		// hp calculation
		// animation reaction
		// ...
		//-------------------------

		//----------------------
		// For example
		//----------------------

		//--------------------
		// direction
		if( other != null )
		{
			transform.forward = -other.transform.forward;
		}
		else
		{
			hitDirection.y = 0.0f;
			transform.forward = -hitDirection.normalized;
		}

		//--------------------
		// animation
		string reaction = m_damageReaction[Random.Range(0, m_damageReaction.Length)];
		m_ani.CrossFade(reaction, 0.1f, 0, 0.0f);

		//--------------------
		// hitFX
		GameObject.Instantiate(m_hitEffect, hitPosition, Quaternion.identity);
	}

}
