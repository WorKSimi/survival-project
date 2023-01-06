using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.Rendering.Universal; 

public class WorldTime : MonoBehaviour
{
    //public TextMeshProUGUI timeDisplay; // Display Time
    //public TextMeshProUGUI dayDisplay; // Display Day

    public float tick; // Increasing the tick, increases second rate
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    public Light2D _light;

    [SerializeField] private Gradient gradient;

    [SerializeField] Color[] myColors;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        hours = 12; //Have the game start at hour 12 (day)
    }

    void FixedUpdate() 
    {
        CalcTime();
        //DisplayTime();       
    }

    private void Update()
    {
        ChangeColor();
    }

    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        
    }
    //public void DisplayTime() // Shows time and day in ui
    //{
    //    timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
    //    dayDisplay.text = "Day: " + days; // display day counter
    //}

    private void ChangeColor() 
    {
        if (hours == 0) //Hour 0, Night [Color 0]
        {
            _light.color = myColors[0];
        }
        if (hours == 1) //Hour 1, Night [Color 0]
        {
            _light.color = myColors[0];
        }
        if (hours == 2) //Hour 2, Night [Color 0]
        {
            _light.color = myColors[0];
        }
        if (hours == 3) //Hour 3, Night [Color 0]
        {
            _light.color = myColors[0];
        }
        if (hours == 4) //Hour 4, Night [Color 0]
        {
            _light.color = myColors[0];
        }
        if (hours == 5) //Hour 5, Night Ending [Color 1]
        {
            _light.color = myColors[1];
        }
        if (hours == 6) //Hour 6, Day Starts [Color 2]
        {
            _light.color = myColors[2];
        }
        if (hours == 7) //Hour 7, Day [Color 3]
        {
            _light.color = myColors[3];
        }
        if (hours == 8) //Hour 8, Day [Color 4]
        {
            _light.color = myColors[4];
        }
        if (hours == 9) //Hour 9, Day [Color 5]
        {
            _light.color = myColors[5];
        }
        if (hours == 10) // Day [Color 6]
        {
            _light.color = myColors[6];
        }
        if (hours == 11) // Day [Color 6]
        {
            _light.color = myColors[6];
        }
        if (hours == 12) // Day [Color 6]
        {
            _light.color = myColors[6];
        }
        if (hours == 13) // Day [Color 6]
        {
            _light.color = myColors[6];
        }
        if (hours == 14) //Color 6
        {
            _light.color = myColors[6];
        }
        if (hours == 15) //Color 6
        {
            _light.color = myColors[6];
        }
        if (hours == 16) //Color 5
        {
            _light.color = myColors[5];
        }
        if (hours == 17) //Color 4
        {
            _light.color = myColors[4];
        }
        if (hours == 18) //Color 3
        {
            _light.color = myColors[3];
        }
        if (hours == 19) //[Color 2]
        {
            _light.color = myColors[2];
        }
        if (hours == 20) //Night Starts [Color 1]
        {
            _light.color = myColors[1];
        }
        if (hours == 21) //Night
        {
            _light.color = myColors[0];
        }
        if (hours == 22) //Night
        {
            _light.color = myColors[0];
        }
        if (hours == 23) //Hour 23, Night
        {
            _light.color = myColors[0];
        }            
    }        
}