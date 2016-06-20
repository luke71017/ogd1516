﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

/**
* REMEMBER TO LOAD JOYPAD BUTTONS
**/

public class BtnManager : MonoBehaviour {

    private GameObject canvas;
    private Button[] buttons;
    private string firstButton = "Btn_NewGame";
    private List<string> interactibleButtonsNames = new List<string>();
    private Color activeColor;
    private Color inactiveColor;

	// Use this for initialization
	void Start () {
        canvas = this.gameObject;
        activeColor = new Color(1f, 1f, 1f, 1f);
        inactiveColor = new Color(148 / 255f, 148 / 255f, 148 / 255f, 1f);
        interactibleButtonsNames.Add("Btn_NewGame");
        interactibleButtonsNames.Add("Btn_Exit");
        interactibleButtonsNames.Add("Btn2_PrivateGame");
        buttons = canvas.GetComponentsInChildren<Button>(true);
        for(int i = 0; i < buttons.Length; i++)
        {
            if (interactibleButtonsNames.Contains(buttons[i].name))
                setButtonInteractible(buttons[i], true);
            else
                setButtonInteractible(buttons[i], false);

            if (buttons[i].name.Contains("Btn2"))
                buttons[i].gameObject.SetActive(false);

            if (buttons[i].name.Contains(firstButton))
                buttons[i].Select();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenNewGameMenu()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn2"))
            {
                buttons[i].gameObject.SetActive(true);
                if (buttons[i].name.Equals("Btn2_PrivateGame"))
                    buttons[i].Select();
            }
            else
                setButtonInteractible(buttons[i], false);
        }
    }

    public void CloseNewGameMenu()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn2"))
                buttons[i].gameObject.SetActive(false);
            else
            {
                if (interactibleButtonsNames.Contains(buttons[i].name))
                {
                    setButtonInteractible(buttons[i], true);
                    if (buttons[i].name.Contains(firstButton))
                        buttons[i].Select();
                }
            }
        }
    }

    private void setButtonInteractible(Button button, bool interactible)
    {
        button.interactable = interactible;
        Text text = button.gameObject.GetComponentInChildren<Text>();
        if (interactible)
            text.color = activeColor;
        else
            text.color = inactiveColor;
    }
}
