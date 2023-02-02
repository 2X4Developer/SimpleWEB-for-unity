using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.IO;
using System.Text;

namespace SimpleWEB{
    public class Request : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        /// <summary>
        /// Returns a SimpleJSON JSONNode from the url's response
        /// </summary>
        public static void getJson(string url, Action<JSONNode> callBack, MonoBehaviour coReq){
            coReq.StartCoroutine(jsonRequest(url, callBack));
        }

        public static IEnumerator jsonRequest(string url, Action<JSONNode> callBack){
            using(UnityWebRequest getJson = UnityWebRequest.Get(url)){
                yield return getJson.SendWebRequest();
                if(getJson.result == UnityWebRequest.Result.Success){
                    string downloadText = getJson.downloadHandler.text;
                    JSONNode jNo = JSON.Parse(downloadText);
                    callBack(jNo);
                }else{
                    Debug.LogError("Error getting JSON!");
                }
            }
            yield return 0;
        }

        /// <summary>
        /// Returns a string from the url's response
        /// </summary>
        public static void get(string url, Action<string> callBack, MonoBehaviour coReq){
            coReq.StartCoroutine(strRequest(url, callBack));
        }

        public static IEnumerator strRequest(string url, Action<string> callBack){
            using(UnityWebRequest get = UnityWebRequest.Get(url)){
                yield return get.SendWebRequest();
                if(get.result == UnityWebRequest.Result.Success){
                    string downloadText = get.downloadHandler.text;
                    callBack(downloadText);
                }else{
                    Debug.LogError("Error getting string!");
                }
            }
            yield return 0;
        }

        /// <summary>
        /// Posts the data you input and returns a string from the response
        /// </summary>
        public static void post(string url, WWWForm postData, Action<string> callBack, MonoBehaviour coReq){
            coReq.StartCoroutine(strPost(url, postData, callBack));
        }

        public static IEnumerator strPost(string url, WWWForm postData, Action<string> callBack){
            using (UnityWebRequest www = UnityWebRequest.Post(url, postData))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    callBack(www.downloadHandler.text);
                }else{
                    Debug.LogError("Error posting!");
                }
            }
            yield return 0;
        }

        /// <summary>
        /// Posts the data you input and returns a SimpleJSON JSONNode from the response
        /// </summary>
        public static void postJson(string url, WWWForm postData, Action<JSONNode> callBack, MonoBehaviour coReq){
            coReq.StartCoroutine(jsonPost(url, postData, callBack));
        }

        public static IEnumerator jsonPost(string url, WWWForm postData, Action<JSONNode> callBack){
            using (UnityWebRequest www = UnityWebRequest.Post(url, postData))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string downloadText = www.downloadHandler.text;
                    JSONNode jsonify = JSON.Parse(downloadText);
                    callBack(jsonify);
                }else{
                    Debug.LogError("Error posting!");
                }
            }
            yield return 0;
        }
    }
}