﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootMineBehavior : NetworkBehaviour {
	private Rigidbody body;
	public int hitPoints = 40;
	public float radius = 15f;

	public GameObject explosionHitPlayer;
	public GameObject soundExplosion;

    public float timerMine = 10f;

    private float timerMineExplosion = 0f;

    [Command]
	void CmdDoExplosionHitPlayer(){
		GameObject shotExplosionHitPlayer = (GameObject)Instantiate (explosionHitPlayer, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosionHitPlayer);

		GameObject shotExplosionHitPlayerSound = (GameObject)Instantiate (soundExplosion, transform.position, transform.rotation);
        
		NetworkServer.Spawn (shotExplosionHitPlayerSound);
	}

    [ClientRpc]
    void RpcAssignTagLayer(string tag, int layer)
    {
        gameObject.tag = tag;
        gameObject.layer = layer;
    }
    
    // Use this for initialization
    void Start () {
		if (!isServer)
			return;

		body = GetComponent<Rigidbody> ();

        RpcAssignTagLayer(gameObject.tag, gameObject.layer);

        //body.velocity = transform.forward * speed;

        //Destroy (this.gameObject, 1f);
    }

	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

        timerMineExplosion += Time.deltaTime;

        if (timerMineExplosion >= timerMine)
        {
            CmdDoExplosionHitPlayer();
            Destroy(gameObject);
        }
        else
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;

            if (gameObject.CompareTag("BulletTeam1"))
            {
                bool damage = false;

                GameObject[] playersEnemy = GameObject.FindGameObjectsWithTag("VehicleTeam0");

                for (int i = 0; i < playersEnemy.Length; i++)
                {
                    if (Vector3.Distance(playersEnemy[i].transform.position, transform.position) <= radius)
                    {
                        GuiVehicle gui = playersEnemy[i].GetComponent<GuiVehicle>();
                        if (gui != null)
                        {
                            gui.TakeDamage(hitPoints);

                            damage = true;
                        }

                        //break;
                    }
                }

                if (damage)
                {
                    CmdDoExplosionHitPlayer();
                    Destroy(gameObject);
                }

            }
            else if (gameObject.CompareTag("BulletTeam0"))
            {
                bool damage = false;

                GameObject[] playersEnemy = GameObject.FindGameObjectsWithTag("VehicleTeam1");

                for (int i = 0; i < playersEnemy.Length; i++)
                {
                    if (Vector3.Distance(playersEnemy[i].transform.position, transform.position) <= radius)
                    {
                        GuiVehicle gui = playersEnemy[i].GetComponent<GuiVehicle>();
                        if (gui != null)
                        {
                            gui.TakeDamage(hitPoints);

                            damage = true;
                        }

                        //break;
                    }
                }

                if (damage)
                {
                    CmdDoExplosionHitPlayer();
                    Destroy(gameObject);
                }
            }
        }
	}

	/*void OnCollisionEnter(Collision col){
		if (!isServer)
			return;

		if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag("BulletTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag("BulletTeam0")) {
			GuiVehicle gui = col.gameObject.GetComponent<GuiVehicle> ();
			//gui.life -= hitPoints;

			gui.TakeDamage (hitPoints);
		}

		Destroy (gameObject);
	}*/
}
