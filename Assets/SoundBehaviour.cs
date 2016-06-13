﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundBehaviour : MonoBehaviour {
	private AudioSource driftAudio;
	private AudioSource weaponChangeAudio;

	private SimpleController scriptMovement;
	private Shooting scriptShooting;

	private int oldWeapon = 0;

	// Use this for initialization
	void Start () {
		scriptMovement = GetComponent<SimpleController> ();
		scriptShooting = GetComponent<Shooting> ();

		AudioSource[] audio = GetComponents<AudioSource> ();

		for (int i = 0; i < audio.Length; i++) {
			if(audio [i].clip.name.Equals("Drift"))
				driftAudio = audio [i];
			if(audio [i].clip.name.Equals("Weapon_Change"))
				weaponChangeAudio = audio [i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (scriptMovement.isDrifting) {
			if(!driftAudio.isPlaying)
				driftAudio.Play();
		}

		if (scriptShooting.currentWeapon != oldWeapon) {
			oldWeapon = scriptShooting.currentWeapon;

			weaponChangeAudio.Play ();
		}
	}
}