//@FiloGCS 2022, all rights reserved. Don't do illegal shit with this.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Holds a list of elements that can be randomly retrieved with bad luck protection.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RandomContainer<T> {

    public T[] elements;
    public float[] w;
    public float factor = 0.5f;

    public RandomContainer(T[] elements) {
        this.elements = elements;
        w = new float[elements.Length];
        for (int i = 0; i < w.Length; i++) {
            w[i] = 1.0f / (float)elements.Length;
        }
    }

    public T GetRandom() {
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
        float amountToShare = w[r] * factor;
        float share = amountToShare / (w.Length - 1);
        for (int i = 0; i < w.Length; i++) {
            if (i == r) {
                w[i] -= amountToShare;
            } else {
                w[i] += share;
            }
        }
        return elements[r];
    }

    public float GetAverageWeight() {
        float sum = 0f;
        foreach (float weight in w) {
            sum += weight;
        }
        return (sum / (float)w.Length);
    }

}
