using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Sprite[] HPsprites;
    public Sprite[] MPsprites;

    public Image HPUI;
    public Image MPUI;

    private HERO_script hero;

    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<HERO_script>();
    }

    private void Update()
    {
        HPUI.sprite = HPsprites[hero.curHP];
        MPUI.sprite = MPsprites[hero.curMP];
    }
}
