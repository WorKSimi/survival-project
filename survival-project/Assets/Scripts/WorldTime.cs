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
    public int totalMins; //Mins used for gradiant
    public int hours;
    public int days = 1;

    public Light2D _light;

    [SerializeField] private Gradient gradient;

    [SerializeField] Color[] myColors;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        hours = 10; //Have the game start at hour 12 (day)
        totalMins = 600; //Set total mins equal to hour 10
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
            totalMins += 1; //Add 1 to total mins counter
            ChangeColor(); //Change color of the world
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            totalMins = 0; //Reset total mins back to 0
            days += 1;
        }
        
    }

    private float PercentOfDay(int totalMins)
    {
        return (float)totalMins % 1440 / 1440; //Total minutes past divded by minutes in a day
    }
    //public void DisplayTime() // Shows time and day in ui
    //{
    //    timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
    //    dayDisplay.text = "Day: " + days; // display day counter
    //}

    private void ChangeColor() 
    {
        _light.color = gradient.Evaluate(PercentOfDay(totalMins));
        Debug.Log("Changing Time!");
        Debug.Log(PercentOfDay(totalMins));    
    }        
}