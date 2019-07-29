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
    private float secondsF;
    private float miliSeconds;

    private bool setOnce;
    private bool playOnce;

    private void Start()
    {
        initialTimer = timer;
        timer = initialTimer;
        ResetTime();
        SetRandomShootValue();
    }

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
            buttonText.text = "Start";
            timer = initialTimer;
            isSleepON = false;
            ResetTime();
            ResetText();
            SetRandomShootValue();
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
        ResetTimer(Mathf.RoundToInt(initialTimer));
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
            isTimerON = true;
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
    }

    public void Set15SecsTimer()
    {
        initialTimer = 15;
        timer = initialTimer;
        ResetTime();
        SetRandomShootValue();
    }
    public void Set10SecsTimer()
    {
        initialTimer = 10;
        timer = initialTimer;
        ResetTime();
        SetRandomShootValue();
    }
}
