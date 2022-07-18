using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[AddComponentMenu("Godmode/OnlineScores")]
public class OnlineScores : MonoBehaviour {

    public string URL_POST_SCORE = "";
    public string URL_GET_ALL_SCORES = "";

    public string username = "anonymous";

    //POST SCORE
    public void PostTime(int score) {
        StartCoroutine(PostScoreCoroutine(score));
    }
    IEnumerator PostScoreCoroutine(int score) {
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("score", score);
        using(UnityWebRequest request = UnityWebRequest.Post(URL_POST_SCORE, form)) {
            yield return request.SendWebRequest();
            if((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError)) {
                Debug.Log(request.error);
            } else {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    //GET SCORES
    IEnumerator GetAllScoresCoroutine() {
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GET_ALL_SCORES)) {
            yield return request.SendWebRequest();
            if((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError)) {
                Debug.Log(request.error);
            } else {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

}
