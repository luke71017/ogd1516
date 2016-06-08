﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour {
	//public bool Player = true;
	public GameObject shoot;
	public GameObject shootSecond;

	public Transform shooter;

	public int currentWeapon = 0;

	public float startTimerShootFirstWeapon = 2f;
	public float startTimerShootSecondWeapon = 2f;

	//[SyncVar]
	public int numBulletsFirstWeapon = 10;
	//[SyncVar]
	public int numBulletsSecondWeapon = 10;

	//[SyncVar]
	public float timerShootFirstWeapon = 0f;
	//[SyncVar]
	public float timerShootSecondWeapon = 0f;

	public int maxNumBulletsFirstWeapon = 10;
	public int maxNumBulletsSecondWeapon = 10;

	private SimpleController controller;



	//private Rigidbody body;

	void dropBullets(int whichWeapon, int num){
		if (whichWeapon == 0) {
			numBulletsFirstWeapon -= num;

			timerShootFirstWeapon = startTimerShootFirstWeapon;
		}else if (whichWeapon == 1) {
			numBulletsSecondWeapon -= num;

			timerShootSecondWeapon = startTimerShootSecondWeapon;
		}
	}

	[Command]
	void CmdDoFire(int weapon)//float rx, float ry, float rz
	{
		if (weapon == 0) {
			Vector3 offset = Vector3.zero;

			if (shoot.gameObject.name.Equals ("Mine")) {
				offset = shooter.forward * -5f;
			}

			GameObject shot = (GameObject)Instantiate (shoot, shooter.position + offset, shooter.rotation);//Quaternion.Euler(rx, ry, rz)
			if (gameObject.CompareTag ("VehicleTeam0"))
				shot.tag = "BulletTeam0";
			else if (gameObject.CompareTag ("VehicleTeam1"))
				shot.tag = "BulletTeam1";

			NetworkServer.Spawn(shot);
		} else if (weapon == 1) {
			Vector3 offset = Vector3.zero;

			if (shootSecond.name.Equals ("Laser"))
				offset = 15f * shooter.transform.forward;

			GameObject shot = (GameObject)Instantiate (shootSecond, shooter.position + offset, shooter.rotation);//Quaternion.Euler(rx, ry, rz)
			if (gameObject.CompareTag ("VehicleTeam0"))
				shot.tag = "BulletTeam0";
			else if (gameObject.CompareTag ("VehicleTeam1"))
				shot.tag = "BulletTeam1";

			if (shootSecond.gameObject.name.Equals ("Bomb")) {
				ShootBomb bombScript = shot.GetComponent<ShootBomb> ();

				bombScript.speedVehicle = controller.body.velocity.magnitude;
			}

			NetworkServer.Spawn(shot);
		}
		//Physics.IgnoreCollision(GetComponent<Collider>(), shot.GetComponent<Collider>());


	}

	// Use this for initialization
	void Start () {
		//body = GetComponent<Rigidbody> ();

		controller = GetComponent<SimpleController> ();
	}

	void FixedUpdate(){
		if (isLocalPlayer) {
			if (currentWeapon == 0) {
				timerShootFirstWeapon -= Time.fixedDeltaTime;

				if (timerShootFirstWeapon < 0f)
					timerShootFirstWeapon = 0f;
			}else if (currentWeapon == 1) {
				timerShootSecondWeapon -= Time.fixedDeltaTime;

				if (timerShootSecondWeapon < 0f)
					timerShootSecondWeapon = 0f;
			}
		}
	}

	void OnGUI()
	{
		if (!isLocalPlayer)
			return;

		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h);

		//if (!Player)
		//	rect.Set (w / 2f, 0, w / 2f, h);

		style.alignment = TextAnchor.LowerLeft;
		style.fontSize = h * 5 / 100;
		style.normal.textColor = Color.red;

		string text = "";

		if (currentWeapon == 0) {
			text = "Bullets: " + numBulletsFirstWeapon.ToString () + "/" + maxNumBulletsFirstWeapon.ToString ();
		} else if (currentWeapon == 1) {
			text = "Bullets: " + numBulletsSecondWeapon.ToString () + "/" + maxNumBulletsSecondWeapon.ToString ();
		}

		//text = timerShootFirstWeapon.ToString () + " " + numBulletsFirstWeapon + " " + transform.rotation;

		GUI.Label(rect, text, style);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("AmmoPickup")) {
			AmmoPickupBehaviour pickup = (AmmoPickupBehaviour)col.gameObject.GetComponent<AmmoPickupBehaviour> ();

			if (currentWeapon == 0) {
				numBulletsFirstWeapon += pickup.numBulletsPickup;

				if (numBulletsFirstWeapon > maxNumBulletsFirstWeapon)
					numBulletsFirstWeapon = maxNumBulletsFirstWeapon;
			}else if (currentWeapon == 1) {
				numBulletsSecondWeapon += pickup.numBulletsPickup;

				if (numBulletsSecondWeapon > maxNumBulletsSecondWeapon)
					numBulletsSecondWeapon = maxNumBulletsSecondWeapon;
			}

			Destroy (col.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown (KeyCode.A)) {
			if (currentWeapon == 0)
				currentWeapon = 1;
			else
				currentWeapon = 0;
		}
		if (Input.GetKey (KeyCode.Space)) {
			if (currentWeapon == 0) {
				if (shoot != null) {
					if (timerShootFirstWeapon <= 0f) {
						if (numBulletsFirstWeapon > 0) {
							//Quaternion rot = shooter.rotation;
							CmdDoFire (0);//rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z

							dropBullets (currentWeapon, 1);
						}
					}
				}
			}else if (currentWeapon == 1) {
				if (shootSecond != null) {
					if (timerShootSecondWeapon <= 0f) {
						if (numBulletsSecondWeapon > 0) {
							CmdDoFire (1);

							dropBullets (currentWeapon, 1);
						}
					}
				}
			}
		}
	}
}
