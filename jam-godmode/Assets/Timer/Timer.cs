//@FiloGCS 2017, all rights reserved. Don't do illegal shit with this.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Attach this script to a GameObject with a Text component.
 * Or don't, what do I care? It will work either way.
 * 
 * TIMER_STARTING_VALUE.......Starting time value in seconds
 * TIMER_SCALE................Amount of seconds the time value changes each second.
 * DISPLAY_MODE...............Time display format. Examples:
 *          raw...........302.69
 *          truncated.....302
 *          rounded.......303
 *          fancy.........5:02
 *      
 * Play()..............Starts counting
 * Pause().............Stops counting
 * Unpause()...........Resumes counting
 * PauseToggle().......Stops/resumes counting
 * Stop()..............Resets time and stops counting
 * Restart()...........Resets time without starting counting
 */

public class Timer : MonoBehaviour {

    //Parameters
    public float TIMER_STARTING_VALUE = 10;
    public float TIMER_SCALE = -1;
    public string DISPLAY_MODE = "fancy";
    //Public attributes
    public float time;
    public bool running = false;
    //Private attributes
    private Text timerText;

    //Unity Methods
    void Start() {
        timerText = GetComponent<Text>();
        time = TIMER_STARTING_VALUE;
    }
    void Update() {
        if (timerText != null) {
            switch (DISPLAY_MODE) {
                case "raw":
                    timerText.text = time.ToString("N2");
                    break;
                case "rounded":
                    timerText.text = Mathf.RoundToInt(time).ToString();
                    break;
                case "truncated":
                    timerText.text = ((int)time).ToString();
                    break;
                case "fancy":
                    timerText.text = Mathf.Floor((time / 60)).ToString("00") + ":" + Mathf.Floor((time % 60)).ToString("00");
                    break;
                default:
                    break;
            }
        }
    }
    void FixedUpdate() {
        if (running) {
            time += Time.fixedDeltaTime * TIMER_SCALE;
        }
    }
    
    //Timer Methods
    public void Restart() {
        time = TIMER_STARTING_VALUE;
    }
    public void Pause() {
        running = false;
    }
    public void Unpause() {
        running = true;
    }
    public void PauseToggle() {
        running = !running;
    }
    public void Play() {
        Restart();
        Unpause();
    }
    public void Stop() {
        Restart();
        Pause();
    }
}
