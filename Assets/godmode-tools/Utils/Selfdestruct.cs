using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Godmode/Selfdestruct")]
public class Selfdestruct : MonoBehaviour {

    [SerializeField]
    private float lifetime = 0f;

    private void Start() {
        if (lifetime > 0) {
            TimedKill(lifetime);
        }
    }

    public void TimedKill(float ttl) {
        StartCoroutine(WaitAndKill(ttl));
    }

    private IEnumerator WaitAndKill(float t) {
        yield return new WaitForSeconds(t);
        Kill();
    }

    public void Kill() {
        Destroy(this.gameObject);
    }
}