using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using SimpleJSON;
using SimpleWEB;

public class jsonthing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Get Json Response (Requires "using SimpleJSON;" at the top of your script which comes with this library)
        Request.getJson("https://jsonplaceholder.typicode.com/todos/1", (JSONNode data) => {
            Debug.Log(data["title"]);
        }, this);

        //Get String Response
        Request.get("https://jsonplaceholder.typicode.com/todos/1", (string data) => {
            Debug.Log(data);
        }, this);

        //Post and get String Response
        WWWForm postDat = new WWWForm();
        postDat.AddField("username", "john");

        Request.post("https://example.com/api/v1/post.php", postDat, (string data) => {
            Debug.Log(data);
        }, this);

        //Post and get Json Response (Requires "using SimpleJSON;" at the top of your script which comes with this library)
        WWWForm postDatJSON = new WWWForm();
        postDatJSON.AddField("username", "john");

        Request.postJson("https://example.com/api/v1/postJson.php", postDatJSON, (JSONNode data) => {
            Debug.Log(data["Property Key"]);
        }, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
