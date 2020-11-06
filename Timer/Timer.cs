//@FiloGCS 2017, all rights reserved. Don't do illegal shit with this.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
 * Restart()...........Resets time without stopping counting
 */

[AddComponentMenu("Godmode/Timer")]
public class Timer : MonoBehaviour {

    [System.Serializable]
    public class ReachedTargetTimeEvent : UnityEvent { }
    public enum DisplayMode { Float, RoundedUp, RoundedDown, Fancy }
    public enum TargetReachedBehaviour { Pause, Stop, Restart, Continue }

    //Parameters
    [Header("Timer Options")]
    [Tooltip("Starting timer value")]
    public float startingValue = 10;
    [Tooltip("Time multiplier. [1=Count up] [-1=Count down] [2=Double speed count up]")]
    public float timeScale = -1;
    [Tooltip("[raw=302.69] [truncated=302] [rounded=303] [fancy=5:02]")]
    public DisplayMode displayMode = DisplayMode.Fancy;

    private float time;
    public bool isRunning = false;
    [SerializeField]
    public float Time {
        get => time;
        set {
            time = value;
            UpdateText(time);
        }
    }

    //Private attributes
    [Space(10)]
    [Header("Timer Text Components")]
    [Tooltip("If true, any suitable component will be automatically added to the list on Awake()")]
    public bool autoLoad = true;
    public List<Text> timerTextList = null;
    public List<TMPro.TextMeshPro> timerTextMeshProList= null;
    public List<TMPro.TextMeshProUGUI> timerTextMeshProUGUIList = null;

    [Space(10)]
    [Header("Target Time")]
    [Tooltip("When the target time is reached the OnTargetTime() event will ve invoked")]
    public float targetTime = 0;
    [Tooltip("What should the timer do whenever the target time is reached")]
    public TargetReachedBehaviour targetReachedBehaviour;
    [Tooltip("The OnTargetTime() event is invoked when the target time is reached")]
    public ReachedTargetTimeEvent m_OnTargetTime = new ReachedTargetTimeEvent();

    private void Awake() {
        //Load any text components in this gameobject
        if (autoLoad) {
            Text myText = this.GetComponent<Text>();
            TMPro.TextMeshPro myTextMeshPro = this.GetComponent<TMPro.TextMeshPro>();
            TMPro.TextMeshProUGUI myTextMeshProUGUI = this.GetComponent<TMPro.TextMeshProUGUI>();
            if (myText != null && !timerTextList.Contains(myText)) {
                timerTextList.Add(myText);
            }
            if (myTextMeshPro != null && !timerTextMeshProList.Contains(myTextMeshPro)) {
                timerTextMeshProList.Add(myTextMeshPro);
            }
            if (myTextMeshProUGUI != null && !timerTextMeshProUGUIList.Contains(myTextMeshProUGUI)) {
                timerTextMeshProUGUIList.Add(myTextMeshProUGUI);
            }
        }
        //Set the starting time value
        Time = startingValue;
    }
    
    void FixedUpdate() {
        if (isRunning) {
            float previous = Time;
            Time += UnityEngine.Time.fixedDeltaTime * timeScale;
            if( (previous>targetTime && Time<targetTime) || (previous<targetTime && Time>targetTime) ) {
                //We just crossed the targetTime
                //Invoke OnTargetTime()
                m_OnTargetTime.Invoke();
                //Act depending on the current TargetReachedBehaviour
                switch (targetReachedBehaviour) {
                    case TargetReachedBehaviour.Pause:
                        this.Pause();
                        break;
                    case TargetReachedBehaviour.Stop:
                        this.Stop();
                        break;
                    case TargetReachedBehaviour.Restart:
                        this.Restart();
                        break;
                    case TargetReachedBehaviour.Continue:
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    //Timer Methods
    public void Restart() {
        Time = startingValue;
    }
    public void Pause() {
        isRunning = false;
    }
    public void Unpause() {
        isRunning = true;
    }
    public void PauseToggle() {
        isRunning = !isRunning;
    }
    public void Play() {
        Restart();
        Unpause();
    }
    public void Stop() {
        Restart();
        Pause();
    }
    public string GetTimeAsString() {
        return TimeToString(Time);
    }

    //Utils
    private void UpdateText(float t) {
        string s = TimeToString(t);
        foreach (Text text in timerTextList) {
            text.text = s;
        }
        foreach (TMPro.TextMeshPro text in timerTextMeshProList) {
            text.text = s;
        }
        foreach (TMPro.TextMeshProUGUI text in timerTextMeshProUGUIList) {
            text.text = s;
        }
    }
    private string TimeToString(float t) {
        string result = "";
        switch (displayMode) {
            case DisplayMode.Float:
                result = Time.ToString("N2");
                break;
            case DisplayMode.RoundedUp:
                result = Mathf.CeilToInt(Time).ToString();
                break;
            case DisplayMode.RoundedDown:
                result = Mathf.FloorToInt(Time).ToString();
                break;
            case DisplayMode.Fancy:
                result = Mathf.Ceil((Time / 60)).ToString("00") + ":" + Mathf.Ceil((Time % 60)).ToString("00");
                break;
            default:
                break;
        }
        return result;
    }
}
