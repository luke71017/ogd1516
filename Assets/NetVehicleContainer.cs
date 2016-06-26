﻿using UnityEngine;
using System.Collections;

public class NetVehicleContainer : MonoBehaviour {
	public int classType = 0;
	public int team = 0;
    public int numberOfPlayers = 2;
	public string player = "Player";
	public string ipAddress = "";
	public bool host = false;

	void Awake()
	{
		//this avoid the destruction of network manager
		DontDestroyOnLoad(transform.gameObject); 
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
