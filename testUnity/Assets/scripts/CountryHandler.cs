﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PolygonCollider2D))]

public class CountryHandler : MonoBehaviour
{
    public Country country;
    
    public Text nbTroupeUI;
    private SpriteRenderer sprite;
    public Color32 oldColor;
    public Color32 hoverColor;
    public List<CountryHandler> voisins;
    //public Color32 startColor;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        showTroupe();
        //sprite.color = startColor;
    }

    public void showTroupe()
    {
        nbTroupeUI.text = country.nbTroupe.ToString();
    }


    private void OnMouseEnter()
    {

        oldColor = sprite.color;
        /*if (country.tribe == Country.theTribes.CLONE)
        {
            hoverColor = oldColor;
        }
        else
        {*/
            hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 255);

        //}
        sprite.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sprite.color = oldColor;

    }

    void OnMouseUpAsButton()
    {
        switch (CountryManager.instance.PhaseEnCours)
        {
            case 1 :
                this.PhaseUn();
                break;
            case 2 :
                this.PhaseDeux();
                break;
        }
        
    }

    private void OnDrawGizmos()
    {
        country.name = name;
        this.tag = "Country";
    }

    public void TintColor(Color32 color)
    {
        sprite.color = color;
    }

    void ShowGUI()
    {
        CountryManager.instance.ShowAttackPanel(country.name + " appartient aux " + country.tribe.ToString() + ". Est-ce que vous voulez vraiment les attaquer?", country.moneyReward, country.expReward);
        GameManager.instance.attackedCountry = country.name;
        GameManager.instance.battleHasEnded = false;
        GameManager.instance.battleWon = false;

        CountryManager.instance.CountrySelectedAttacked = this;

    }

    private void PhaseUn()
    {
        if (country.tribe == (Country.theTribes) CountryManager.instance.TourJoueur)
        {
            if (!CountryManager.instance.CountryIsSelected)
            {
                CountryManager.instance.CountryIsSelected = true;
                
                oldColor = sprite.color;
                hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 190);
                sprite.color = hoverColor;
                
                CountryManager.instance.ShowSliderTroupe(CountryManager.instance.NbTroupePhase1);
                CountryManager.instance.TintThisCountries(this);


            }
            else
            {
                CountryManager.instance.CountryIsSelected = false;
                CountryManager.instance.DisableSliderTroupe();
                CountryManager.instance.TintCountries();
            }
            
            
            //this.country.nbTroupe += CountryManager.instance.nbTerritoire(this.country.tribe);
            
        }
        
    }

    private void PhaseDeux()
    {
        //print("Pressed");
        if (country.tribe == (Country.theTribes)CountryManager.instance.TourJoueur && country.nbTroupe >1)
        {
            if (!CountryManager.instance.CountryIsSelected)
            {
                bool res = false;
                foreach (CountryHandler c in this.voisins)
                {
                    if (c.country.tribe != (Country.theTribes)CountryManager.instance.TourJoueur)
                    {
                        res = true;
                    }
                }

                if (res)
                {
                    CountryManager.instance.CountryIsSelected = true;

                    oldColor = sprite.color;
                    hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 190);
                    sprite.color = hoverColor;


                    CountryManager.instance.TintAttaqueCountries(this);

                    CountryManager.instance.ShowAttaqueText();
                }
            }
            else
            {
                CountryManager.instance.CountryIsSelected = false;
                CountryManager.instance.DisableAttaqueText();
                CountryManager.instance.TintCountries();
            }

        }
        else if (country.tribe != (Country.theTribes)CountryManager.instance.TourJoueur && CountryManager.instance.CountryIsSelected)
        {
            CountryManager.instance.CountryIsSelected = false;

            CountryManager.instance.DisableAttaqueText();
            ShowGUI();

        }
    }
}
