using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class QRCode : MonoBehaviour
{
    public string Player;
    void Start()
    {
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture() {
        RawImage i = GetComponent<RawImage>();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://zxing.org/w/chart?cht=qr&chs=350x350&chld=L&choe=UTF-8&chl=https%3A%2F%2Fshopping-mopup-server.herokuapp.com%2F" + Player);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            i.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
