using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

public class UIDisplayTime : MonoBehaviour
{
    public InputField breakTimeSet;
    public AudioSource goSound;
    public AudioSource countdownSound;
    public Text infoText;
    public Text timeText;
    public Text buttonText;
    public float timer;
    public int sleepTime;
    public bool isTimerON;
    public bool isSleepON;

    private float initialTimer;
    private float shootTime;

    private string time;
    private int minutes;
    private int seconds;
    private int newTimerValue;
    private float secondsF;
    private float miliSeconds;

    public bool setOnce;
    private bool playOnce;
    private bool setNewValueOnce;
    private bool canSetTimerOn;

    private void Start()
    {
        newTimerValue = (int)timer;
        initialTimer = timer;
        timer = initialTimer;
        ResetTime();
        SetRandomShootValue();
        SetSleepTime();
        setOnce = false;
        isTimerON = false;
    }
    //
    // Update is called once per frame
    private void Update()
    {
        if (!isTimerON) return;
        UpdateTime();

        if (!isSleepON)
        {
            if (timer <= shootTime)
            {
                Alert();
            }
        }
        else
        {
            if (timer <= 3)
            {
                if (!playOnce)
                {
                    countdownSound.Play();
                    playOnce = true;
                }
                
            }

            if (timer <= 0)
            {
                playOnce = false;
                timer = initialTimer;
                isSleepON = false;
                ResetText();
                ResetTime();
            }
        }
        
    }

    public void SwitchTimer()
    {
        isTimerON = !isTimerON;
        if (!isTimerON)
        {
            isTimerON = false;
            playOnce = false;
            setOnce = false;
            isSleepON = true;
            canSetTimerOn = false;
            ResetTime();
            SetRandomShootValue();
            SetSleepTime();
            buttonText.text = "Start";
            goSound.Stop();
            countdownSound.Stop();
        }
        else
        {
            buttonText.text = "Stop";
        }
    }

    private void UpdateTime()
    {
        timer -= Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60F);
        seconds = Mathf.FloorToInt(timer - minutes * 60);
        secondsF = timer - minutes * 60;
        miliSeconds = (secondsF * 1000) - (seconds * 1000);
        time = string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, miliSeconds);

        timeText.text = time;
    }

    private void ResetTime()
    {
        if (!isSleepON)
        {
            initialTimer = newTimerValue;
            timer = initialTimer;
            ResetTimer(Mathf.RoundToInt(initialTimer));
        }
        else
        {
            SetSleepTime();
        }
    }

    private void SetRandomShootValue()
    {
        shootTime = Random.Range(0.1f, initialTimer);
    }

    private void Alert()
    {
        goSound.Play();
        infoText.color = Color.green;
        infoText.text = "SHOOT";
        setOnce = false;
        isTimerON = false;
        canSetTimerOn = true;
        Invoke("SetSleepTime", 0.6f);
    }

    private void ResetText()
    {
        timeText.color = Color.white;
        infoText.color = Color.white;
        infoText.text = "PREPARE";
    }

    private void SetSleepTime()
    {
        if (!setOnce)
        {
            SetRandomShootValue();
            ResetTimer(sleepTime);

            infoText.text = "BREAK";
            infoText.color = Color.yellow;
            timeText.color = Color.yellow;

            timer = sleepTime;

            isSleepON = true;
            setOnce = true;
            if (canSetTimerOn)
            {
                isTimerON = true;
            }
            
        }
    }

    private void ResetTimer(int value)
    {
        minutes = 00;
        seconds = Mathf.RoundToInt(value);
        secondsF = 00;
        miliSeconds = 00;
        time = string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, miliSeconds);

        timeText.text = time;
    }

    public void SetBreakTime()
    {
        if (!isTimerON)
        {
            int newSleepTime = int.Parse(breakTimeSet.text);
            if (newSleepTime < 1)
            {
                newSleepTime = 1;
            }
            else if (newSleepTime > 15)
            {
                newSleepTime = 15;
            }

            sleepTime = newSleepTime;
            breakTimeSet.text = sleepTime.ToString();

            if (sleepTime < 3)
            {
                countdownSound.volume = 0;
            }
            else
            {
                countdownSound.volume = 1;
            }
            setOnce = false;
            SetSleepTime();
        }
        else
        {
            breakTimeSet.text = "";
        }
    }

    public void Set15SecsTimer()
    {
        if (!isTimerON)
        {
            newTimerValue = 15;
        }
    }
    public void Set10SecsTimer()
    {
        if (!isTimerON)
        {
            newTimerValue = 10;
        }
    }
}
