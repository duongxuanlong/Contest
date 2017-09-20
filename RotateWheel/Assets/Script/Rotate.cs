using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	#region Motor

	public int m_Speed = 50;
	public float m_MaxSpeed = 5000;
	public float m_CelerationRate = 500;
	public float m_BrakeSpeed = 2500;

	float m_Slope = 0;
	float m_TorqueDir = 0;
	float m_Dir = 0;
	Rigidbody2D m_Body;
	WheelJoint2D m_Wheel;
	JointMotor2D m_Motor;

	#endregion

	// Use this for initialization
	void Start ()
	{
		m_Body = GetComponent<Rigidbody2D> ();
		m_Wheel = GetComponent<WheelJoint2D> ();
		m_Motor = m_Wheel.motor;
	}

	void FixedUpdate ()
	{
		m_TorqueDir = Input.GetAxis ("Horizontal");
		if (m_TorqueDir != 0)
			m_Body.AddTorque (3 * Mathf.PI * m_TorqueDir, ForceMode2D.Force);
		else
			m_Body.AddTorque (0);

		m_Slope = transform.localEulerAngles.z;

		if (m_Slope >= 180)
			m_Slope -= 360;

		m_Dir = Input.GetAxis ("Horizontal");
		if (m_Dir != 0) {
			m_Motor.motorSpeed = Mathf.Clamp (m_Motor.motorSpeed - (m_Dir * m_CelerationRate - Mathf.Sin ((m_Slope * Mathf.PI) / 180) * 80) * Time.deltaTime, -m_MaxSpeed, m_MaxSpeed);
		}

		if ((m_Dir == 0 && m_Motor.motorSpeed < 0) || (m_Dir==0 && m_Motor.motorSpeed == 0 && m_Slope < 0)){
			m_Motor.motorSpeed = Mathf.Clamp(m_Motor.motorSpeed - ((-m_CelerationRate) - Mathf.Sin((m_Slope * Mathf.PI) / 180)* 80)*Time.deltaTime, -m_MaxSpeed, 0);
		}
		else if ((m_Dir == 0 && m_Motor.motorSpeed > 0) || (m_Dir == 0 && m_Motor.motorSpeed == 0 && m_Slope > 0)){
			m_Motor.motorSpeed = Mathf.Clamp(m_Motor.motorSpeed - (m_CelerationRate - Mathf.Sin((m_Slope * Mathf.PI)/180)*80)*Time.deltaTime, 0, m_MaxSpeed);
		}

		m_Wheel.motor = m_Motor;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		if (Input.GetKey(KeyCode.RightArrow))
//		{
//			m_Motor = m_Wheel.motor;
//			m_Motor.motorSpeed += m_Speed;
//			if (m_Motor.motorSpeed >= m_Motor.maxMotorTorque)
//				m_Motor.motorSpeed = m_Motor.maxMotorTorque;
//			m_Wheel.motor = m_Motor;
//		}
//		
//		if (Input.GetKey(KeyCode.LeftArrow))
//		{
//			m_Motor = m_Wheel.motor;
//			m_Motor.motorSpeed -= m_Speed;
//			if (Mathf.Abs (m_Motor.motorSpeed) >= m_Motor.maxMotorTorque)
//				m_Motor.motorSpeed = 0 - m_Motor.maxMotorTorque;
//			m_Wheel.motor = m_Motor;
//		}	
	}
}
