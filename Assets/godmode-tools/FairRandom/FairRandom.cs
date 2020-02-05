using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairRandom<T> {

    public T[] elements;
    public float[] w;
    public float factor = 2.0f;

    public FairRandom(T[] elements) {
        this.elements = elements;
        w = new float[elements.Length];
        for (int i = 0; i < w.Length; i++) {
            w[i] = 1.0f / (float)elements.Length;
        }
    }

    public T GetRandomElement() {
        //Get a weighted random value
        float random = Random.Range(0.0f, 1.0f);
        int r = 0;
        for (int i = 0; i < w.Length; i++) {
            if (random <= w[i]) {
                r = i;
                break;
            }
            random -= w[i];
        }
        //Distribute its chance between the others
        float remaining = w[r] / factor;
        float share = remaining / (w.Length - 1);
        for (int i = 0; i < w.Length; i++) {
            if (i == r) {
                w[i] = w[r] / factor;
            } else {
                w[i] = w[i] + share;
            }
        }
        return elements[r];
    }

}
