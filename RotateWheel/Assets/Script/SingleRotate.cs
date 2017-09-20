using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRotate : MonoBehaviour
{

	#region Motor

	//public float m_Speed = 50f;
	public float m_MaxSpeed = 1000f;
	public float m_MinSpeed = -1000f;
	public float m_CelerationRate = 500f;
	public float m_BrakeSpeed = 500f;

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
			float temp = Mathf.Clamp (m_Motor.motorSpeed - (m_Dir * m_CelerationRate - Mathf.Sin ((m_Slope * Mathf.PI) / 180) * 160) * Time.deltaTime, m_MinSpeed, m_MaxSpeed);
			if (m_Dir > 0)
				m_Motor.motorSpeed = Mathf.Abs (temp);
			else
				m_Motor.motorSpeed = 0 - Mathf.Abs (temp);
		}

		if ((m_Dir == 0 && m_Motor.motorSpeed < 0) || (m_Dir == 0 && m_Motor.motorSpeed == 0 && m_Slope < 0)) {
			m_Motor.motorSpeed = Mathf.Clamp (m_Motor.motorSpeed - ((-m_CelerationRate) - Mathf.Sin ((m_Slope * Mathf.PI) / 180) * 80) * Time.deltaTime, m_MinSpeed, 0);
		} else if ((m_Dir == 0 && m_Motor.motorSpeed > 0) || (m_Dir == 0 && m_Motor.motorSpeed == 0 && m_Slope > 0)) {
			m_Motor.motorSpeed = Mathf.Clamp (m_Motor.motorSpeed - (m_CelerationRate - Mathf.Sin ((m_Slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, m_MaxSpeed);
		}

		//apply brakes to the car
		if (Input.GetKey(KeyCode.RightArrow) && m_Motor.motorSpeed > 0){
			m_Motor.motorSpeed = Mathf.Clamp(m_Motor.motorSpeed - m_BrakeSpeed*Time.deltaTime, 0, m_MaxSpeed); 
		}
		else if(Input.GetKey(KeyCode.LeftArrow) && m_Motor.motorSpeed < 0){ 
			m_Motor.motorSpeed = Mathf.Clamp(m_Motor.motorSpeed + m_BrakeSpeed*Time.deltaTime, m_MinSpeed, 0);
		}

		m_Wheel.motor = m_Motor;
	}

	// Update is called once per frame
	void Update ()
	{
//		if (Input.GetKey (KeyCode.RightArrow)) {
//			m_Motor = m_Wheel.motor;
//			m_Motor.motorSpeed += m_Speed;
//			if (m_Motor.motorSpeed >= m_Motor.maxMotorTorque)
//				m_Motor.motorSpeed = m_Motor.maxMotorTorque;
//			m_Wheel.motor = m_Motor;
//		}
//				
//		if (Input.GetKey (KeyCode.LeftArrow)) {
//			m_Motor = m_Wheel.motor;
//			m_Motor.motorSpeed -= m_Speed;
//			if (Mathf.Abs (m_Motor.motorSpeed) >= m_Motor.maxMotorTorque)
//				m_Motor.motorSpeed = 0 - m_Motor.maxMotorTorque;
//			m_Wheel.motor = m_Motor;
//		}	
	}
}
