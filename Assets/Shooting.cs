﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

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

	private GuiVehicle gui;

	private GameObject ammo;
	private GameObject ammoAmount;
	private GameObject ammoTotal;

	private GameObject weaponImage;
	private GameObject weaponImageBG;

	public Sprite shootTexture;
	public Sprite driftTexture;
	public Sprite mineTexture;
	public Sprite bombTexture;
	public Sprite bazookaTexture;
	public Sprite laserTexture;

	private LoaderClass scriptClass;

	//private Rigidbody body;

	void dropBullets(int whichWeapon, int num){
		if (whichWeapon == 0) {
			numBulletsFirstWeapon -= num;

			timerShootFirstWeapon = startTimerShootFirstWeapon;

			needFlashWeapon = true;
		}else if (whichWeapon == 1) {
			numBulletsSecondWeapon -= num;

			timerShootSecondWeapon = startTimerShootSecondWeapon;

			needFlashWeapon = true;
		}
	}

    private GameObject shot;

	[Command]
	void CmdDoFire(int weapon, bool stationary)//float rx, float ry, float rz
	{
		if (weapon == 0) {
			Vector3 offset = Vector3.zero;

			if (shoot.gameObject.name.Equals ("Mine")) {
				offset = shooter.forward * -2.5f + shooter.up * -2f;
			}else if (shoot.gameObject.name.Equals ("shoot")) {
				if(!stationary)
					offset = shooter.forward * 8.5f;
			}

			shot = (GameObject)Instantiate (shoot, shooter.position + offset, shooter.rotation);//Quaternion.Euler(rx, ry, rz)
			if (gameObject.CompareTag ("VehicleTeam0")){
				shot.tag = "BulletTeam0";
				shot.layer = 8;
            }else if (gameObject.CompareTag ("VehicleTeam1")){
				shot.tag = "BulletTeam1";
				shot.layer = 9;
            }
			NetworkServer.Spawn(shot);
        } else if (weapon == 1) {
			Vector3 offset = Vector3.zero;

			if (shootSecond.name.Equals ("Laser"))
				offset = 10f * shooter.transform.forward;

			GameObject shot = (GameObject)Instantiate (shootSecond, shooter.position + offset, shooter.rotation);//Quaternion.Euler(rx, ry, rz)
			if (gameObject.CompareTag ("VehicleTeam0")) {
				shot.tag = "BulletTeam0";
				//if (shootSecond.name.Equals ("Laser"))
					shot.layer = 8;
			}
			else if (gameObject.CompareTag ("VehicleTeam1")){
				shot.tag = "BulletTeam1";
				//if (shootSecond.name.Equals ("Laser"))
					shot.layer = 9;
			}
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

		gui = gameObject.GetComponent<GuiVehicle> ();

		ammo = GameObject.Find ("Ammo");
		ammoAmount = GameObject.Find ("AmmoAmount");
		ammoTotal = GameObject.Find ("AmmoTotal");

		weaponImage = GameObject.Find ("WeaponImage");
		weaponImageBG = GameObject.Find ("WeaponImageBG");

		scriptClass = GetComponent<LoaderClass> ();
	}

	private float flashWeapon = 0;
	private bool needFlashWeapon = false;

	void FixedUpdate(){
		if (isLocalPlayer) {
			if (currentWeapon == 0) {
				timerShootFirstWeapon -= Time.deltaTime;

				if (timerShootFirstWeapon < 0f)
					timerShootFirstWeapon = 0f;

				weaponImage.GetComponent<Image>().fillAmount = 1f - timerShootFirstWeapon / startTimerShootFirstWeapon;

				if (Mathf.Approximately(timerShootFirstWeapon,0f)) {
					if (needFlashWeapon) {
						if (flashWeapon < 1f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (Color.white, scriptClass.teamColor, flashWeapon);
						} else if (flashWeapon < 2f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (scriptClass.teamColor, Color.white, flashWeapon - 1f);
						} else if (flashWeapon < 3f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (Color.white, scriptClass.teamColor, flashWeapon - 2f);
						} else if (flashWeapon < 4f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (scriptClass.teamColor, Color.white, flashWeapon - 3f);
						}

						flashWeapon += Time.fixedDeltaTime * 5f;

						if (flashWeapon > 4f) {
							flashWeapon = 0f;

							needFlashWeapon = false;
						}
					}
				}
			}else if (currentWeapon == 1) {
				timerShootSecondWeapon -= Time.deltaTime;

				if (timerShootSecondWeapon < 0f)
					timerShootSecondWeapon = 0f;

				weaponImage.GetComponent<Image>().fillAmount = 1f - timerShootSecondWeapon / startTimerShootSecondWeapon;

				if (Mathf.Approximately(timerShootSecondWeapon,0f)) {
					if (needFlashWeapon) {
						if (flashWeapon < 1f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (Color.white, scriptClass.teamColor, flashWeapon);
						} else if (flashWeapon < 2f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (scriptClass.teamColor, Color.white, flashWeapon - 1f);
						} else if (flashWeapon < 3f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (Color.white, scriptClass.teamColor, flashWeapon - 2f);
						} else if (flashWeapon < 4f) {
							weaponImage.GetComponent<Image> ().color = Color.Lerp (scriptClass.teamColor, Color.white, flashWeapon - 3f);
						}

						flashWeapon += Time.fixedDeltaTime * 5f;

						if (flashWeapon > 4f) {
							flashWeapon = 0f;

							needFlashWeapon = false;
						}
					}
				}
			}
		}
	}

	void OnGUI()
	{
		if (!isLocalPlayer)
			return;

		if (currentWeapon == 0) {
			ammo.SetActive (true);

			ammoAmount.GetComponent<Text> ().text = numBulletsFirstWeapon.ToString ();
			ammoTotal.GetComponent<Text> ().text = maxNumBulletsFirstWeapon.ToString ();

			if (scriptClass.vehicleTypeClass == 0) {
				weaponImage.GetComponent<Image> ().sprite = shootTexture;
				weaponImageBG.GetComponent<Image> ().sprite = shootTexture;
			} else if (scriptClass.vehicleTypeClass == 1) {
				weaponImage.GetComponent<Image> ().sprite = mineTexture;
				weaponImageBG.GetComponent<Image> ().sprite = mineTexture;
			} else if (scriptClass.vehicleTypeClass == 2) {
				weaponImage.GetComponent<Image> ().sprite = bazookaTexture;
				weaponImageBG.GetComponent<Image> ().sprite = bazookaTexture;
			}
		} else if (currentWeapon == 1) {
			if (scriptClass.vehicleTypeClass == 0) {
				ammo.SetActive (false);
			}else{
				ammo.SetActive (true);
				ammoAmount.GetComponent<Text> ().text = numBulletsSecondWeapon.ToString ();
				ammoTotal.GetComponent<Text> ().text = maxNumBulletsSecondWeapon.ToString ();
			}

			if (scriptClass.vehicleTypeClass == 0) {
				weaponImage.GetComponent<Image> ().sprite = driftTexture;
				weaponImageBG.GetComponent<Image> ().sprite = driftTexture;
			} else if (scriptClass.vehicleTypeClass == 1) {
				weaponImage.GetComponent<Image> ().sprite = bombTexture;
				weaponImageBG.GetComponent<Image> ().sprite = bombTexture;
			} else if (scriptClass.vehicleTypeClass == 2) {
				weaponImage.GetComponent<Image> ().sprite = laserTexture;
				weaponImageBG.GetComponent<Image> ().sprite = laserTexture;
			}
		}

		/*int w = Screen.width, h = Screen.height;

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

		GUI.Label(rect, text, style);*/
	}

	void OnCollisionEnter(Collision col){
		/*if (col.gameObject.CompareTag ("AmmoPickup")) {
            if (currentWeapon == 0 && numBulletsFirstWeapon < maxNumBulletsFirstWeapon || currentWeapon == 1 && numBulletsSecondWeapon < maxNumBulletsSecondWeapon)
            {
                AmmoPickupBehaviour pickup = (AmmoPickupBehaviour)col.gameObject.GetComponent<AmmoPickupBehaviour>();

                if (currentWeapon == 0)
                {
                    numBulletsFirstWeapon += pickup.numBulletsPickup;

                    if (numBulletsFirstWeapon > maxNumBulletsFirstWeapon)
                        numBulletsFirstWeapon = maxNumBulletsFirstWeapon;
                }
                else if (currentWeapon == 1)
                {
                    numBulletsSecondWeapon += pickup.numBulletsPickup;

                    if (numBulletsSecondWeapon > maxNumBulletsSecondWeapon)
                        numBulletsSecondWeapon = maxNumBulletsSecondWeapon;
                }

                //Destroy (col.gameObject);
                pickup.getPickup = true;
            }
		}*/
	}

	// Update is called once per frame
	void Update () {
		if (gui.life <= 0)
			return;

        if (currentWeapon == 0 && numBulletsFirstWeapon < maxNumBulletsFirstWeapon || currentWeapon == 1 && numBulletsSecondWeapon < maxNumBulletsSecondWeapon)
        {
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("AmmoPickup");

           for (int i = 0; i < pickups.Length; i++)
            {
                AmmoPickupBehaviour pickup = (AmmoPickupBehaviour)pickups[i].GetComponent<AmmoPickupBehaviour>();

                if (Vector3.Distance(pickups[i].transform.position, transform.position) <= pickup.RADIUS_PICKUP)
                {
                    if (currentWeapon == 0)
                    {
                        numBulletsFirstWeapon += pickup.numBulletsPickup;

                        if (numBulletsFirstWeapon > maxNumBulletsFirstWeapon)
                            numBulletsFirstWeapon = maxNumBulletsFirstWeapon;
                    }
                    else if (currentWeapon == 1)
                    {
                        numBulletsSecondWeapon += pickup.numBulletsPickup;

                        if (numBulletsSecondWeapon > maxNumBulletsSecondWeapon)
                            numBulletsSecondWeapon = maxNumBulletsSecondWeapon;
                    }

                    break;
                }
            }
        }

        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown (KeyCode.A)) {
			if (scriptClass.vehicleTypeClass != 2) {
				if (currentWeapon == 0)
					currentWeapon = 1;
				else
					currentWeapon = 0;
			}
		}
  
		if (Input.GetKey (KeyCode.Space)) {
			if (currentWeapon == 0) {
				if (shoot != null) {
					if (timerShootFirstWeapon <= 0f) {
						if (numBulletsFirstWeapon > 0) {
							//Quaternion rot = shooter.rotation;
							CmdDoFire (0, Vector3.Distance(controller.body.velocity, Vector3.zero) <= 0.1f);//rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z

							dropBullets (currentWeapon, 1);
						}
					}
				}
			}else if (currentWeapon == 1) {
				if (shootSecond != null) {
					if (timerShootSecondWeapon <= 0f) {
						if (numBulletsSecondWeapon > 0) {
							CmdDoFire (1, Vector3.Distance(controller.body.velocity, Vector3.zero) <= 0.1f);

							dropBullets (currentWeapon, 1);
						}
					}
				}
			}
		}
	}
}
