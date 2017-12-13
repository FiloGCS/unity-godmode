using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    //Starting time in seconds
    public float TIMER_STARTING_VALUE = 300;
    //Time scale and direction
    public float TIMER_SCALE = -1;

    public float time;
    bool running = false;

    void Update() {
        //TODO Output value somewhere
    }

    void FixedUpdate() {
        if (running) {
            time += Time.fixedDeltaTime * TIMER_SCALE;
        }
    }

    //Sets the timer to the starting value
    public void ResetTimer() {
        time = TIMER_STARTING_VALUE;
    }

    //Pauses the timer
    public void PauseTimer() {
        running = false;
    }

    //Unpauses the timer
    public void UnpauseTimer() {
        running = true;
    }

    //Pauses/unpauses the timer
    public void PauseTimerToggle() {
        running = !running;
    }


    //Resets and Unpauses the timer
    public void StartTimer() {
        ResetTimer();
        UnpauseTimer();
    }

    //Resets and Pauses the timer
    public void StopTimer() {
        ResetTimer();
        PauseTimer();
    }

}
