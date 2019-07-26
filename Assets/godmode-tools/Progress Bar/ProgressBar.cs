//@FiloGCS 2018, all rights reserved. Don't do illegal shit with this.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Godmode/Progress Bar")]
public class ProgressBar : MonoBehaviour {

    //Parameters
    [Header("Fill Value")]
    [Range(0, 1)]
    public float value = 1.0f;
    [Space(5)]
    [Header("Progress Bar Options")]
    [Tooltip("Fill Color")]
    public Color color = new Color(0.3f,0.8f,0.0f,1.0f);
    [Tooltip("Empty zone Color")]
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
    public Sprite sprite;
    [Range(0, 10)]
    public float padding = 2.0f;
    [Range(0, 10)]
    public float border = 2.0f;

    private GameObject fill;
    private GameObject back;
    private RectTransform fillRT;
    private RectTransform backRT;
    private RectTransform myRT;

    // Use this for initialization
    void Start() {
        //Grab Component References
        back = this.transform.GetChild(0).gameObject;
        fill = this.transform.GetChild(1).gameObject;
        backRT = back.GetComponent<RectTransform>();
        fillRT = fill.GetComponent<RectTransform>();
        myRT = this.GetComponent<RectTransform>();

        //Set Image
        fill.GetComponent<Image>().sprite = this.sprite;
        fill.GetComponent<Image>().type = Image.Type.Sliced;


    }

    // Update is called once per frame
    void Update() {

        //Set Colors
        back.GetComponent<Image>().color = this.backgroundColor;
        fill.GetComponent<Image>().color = this.color;
        //Set Position
        backRT.anchoredPosition = new Vector2(padding, 0);
        fillRT.anchoredPosition = new Vector2((border + padding), 0);
        //Set Sizes
        Vector2 backDelta = new Vector2(padding * 2, padding * 2);
        backRT.sizeDelta = myRT.sizeDelta - backDelta;

        float width = myRT.sizeDelta.x * (1 - value);
        Vector2 fillDelta = new Vector2((border + padding) * 2 + width, (border + padding) * 2);
        fillRT.sizeDelta = myRT.sizeDelta - fillDelta;
        

    }
}
