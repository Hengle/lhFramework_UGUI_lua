using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //var obj=Resources.Load("LuaScripts/framework/luaInfrastrutureManager");
        //var obj=Resources.Load("ConfigData/123");
        //Debug.Log(obj);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10));
	}
}
