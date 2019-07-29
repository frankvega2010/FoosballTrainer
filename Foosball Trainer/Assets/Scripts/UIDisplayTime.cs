using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIDisplayTime : MonoBehaviour
{
    public AudioSource goSound;
    public AudioSource countdownSound;
    public Text infoText;
    public Text timeText;
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
            timer = initialTimer;
            isSleepON = false;
            ResetTime();
            ResetText();
            SetRandomShootValue();
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
        minutes = 00;
        seconds = Mathf.RoundToInt(initialTimer);
        secondsF = 00;
        miliSeconds = 00;
        time = string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, miliSeconds);

        timeText.text = time;
    }

    private void SetRandomShootValue()
    {
        shootTime = Random.Range(0.0f, timer);
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

            minutes = 00;
            seconds = sleepTime;
            secondsF = 00;
            miliSeconds = 00;
            time = string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, miliSeconds);

            timeText.text = time;
            infoText.text = "BREAK";
            infoText.color = Color.yellow;
            timeText.color = Color.yellow;
            timer = sleepTime;
            isSleepON = true;
            setOnce = true;
            isTimerON = true;
        }
    }
}
