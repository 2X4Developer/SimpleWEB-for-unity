using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.IO;
using System.Text;

namespace SimpleWEB{
    public class RequestQueue{
        public Dictionary<string, (Action<object>, reqType)> urlQueue = new Dictionary<string, (Action<object>, reqType)>();
        public MonoBehaviour coReq;
        public RequestQueue(MonoBehaviour coRq) => coReq = coRq;

        public void Add(string Url, reqType rc, Action<object> phone){
            urlQueue.Add(Url, (phone, rc));
        }

        public void Start(Action onFinish = null){
            coReq.StartCoroutine(runQueue(onFinish));
        }

        public IEnumerator runQueue(Action onFinish){
            foreach(KeyValuePair<string, (Action<object>, reqType)> kv in urlQueue){
                bool canGo = false;
                switch(kv.Value.Item2){
                    case reqType.get:
                        Request.get(kv.Key, (string ret) => {
                            kv.Value.Item1(ret);
                            canGo = true;
                        }, coReq);
                    break;
                    case reqType.texture:
                        Request.getImage(kv.Key, (Texture2D ret) => {
                            kv.Value.Item1(ret);
                            canGo = true;
                        }, coReq);
                    break;
                }
                yield return new WaitUntil(() => canGo == true);
            }
            if(onFinish != null){
                onFinish();
            }
            urlQueue.Clear();
            yield return 0;
        }
    }

    public enum reqType{
        get,
        post,
        getJson,
        texture
    }

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

        public static void getImage(string url, Action<Texture2D> callBack, MonoBehaviour coReq){
            coReq.StartCoroutine(imgRequest(url, callBack));
        }

        public static IEnumerator imgRequest(string url, Action<Texture2D> callBack){
            using(UnityWebRequest get = UnityWebRequestTexture.GetTexture(url)){
                yield return get.SendWebRequest();
                if(get.result == UnityWebRequest.Result.Success){
                    Texture2D getTex = DownloadHandlerTexture.GetContent(get);
                    callBack(getTex);
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
