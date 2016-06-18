﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ControllerNet : NetworkManager {
	public int maxPlayers = 2;

	//private bool firstTeam = true;

	/*public override void OnServerConnect(NetworkConnection conn){
		
	}*/

	/*public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);


	}*/

	/*public override void OnServerConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);

		LoaderClass loaderScript = GameObject.Find ("Player").GetComponent<LoaderClass> ();

		RpcChooseClass (loaderScript.typeClass);

		print ("ON SERVER CONNNECT: " + loaderScript.typeClass);
	}*/

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (GameObject.Find("Lobby").GetComponent<Lobby>().activePlayers == maxPlayers)
			return;

		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		//SpawnPoints scriptSpawnPoints = player.GetComponent<SpawnPoints> ();
		//string whichTagTeam = "SpawnTeam0";

		//PlayerController[] players = conn.playerControllers.ToArray ();
		//print (players [0].gameObject.GetComponent<LoaderClass> ().teamPlayer);

		//if (firstTeam){
		//	firstTeam = false;

			/*player.GetComponent<SimpleController> ().team = 0;
			player.gameObject.tag = "VehicleTeam0";
			player.gameObject.GetComponent<LoaderClass>().tagTeam = "VehicleTeam0";
			player.gameObject.layer = 8;

			for (int i = 0; i < player.transform.childCount; i++) {
				player.transform.GetChild (i).gameObject.layer = 8;
			}*/
		//	whichTagTeam = "SpawnTeam0";
		//} else {
		/*	player.GetComponent<SimpleController> ().team = 1;
			//player.gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
			player.gameObject.tag = "VehicleTeam1";

			player.gameObject.GetComponent<LoaderClass>().tagTeam = "VehicleTeam1";
			player.gameObject.layer = 9;
			for (int i = 0; i < player.transform.childCount; i++) {
				player.transform.GetChild (i).gameObject.layer = 9;
			}
			whichTagTeam = "SpawnTeam1";
		}*/

		//GameObject[] spawns = GameObject.FindGameObjectsWithTag (whichTagTeam);

		//int randomRange = Random.Range (0, spawns.Length);

		//player.transform.position = spawns [randomRange].transform.position;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		GameObject.Find("Lobby").GetComponent<Lobby>().activePlayers++;
	}

	public bool canPlay(){
		return GameObject.Find("Lobby").GetComponent<Lobby>().activePlayers == maxPlayers;
	}	
}
