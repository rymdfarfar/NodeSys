using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour {
    public static AiManager instance;
    public GameObject player;
    public MoveToMouse playerScript;
    public Node playesCurrentNodeFlying;
    public Node playesCurrentNodeCharging;
    public float moveDistance;
    public float aiTimer;
    public bool AiMove;
    public int index;
    float t;
	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
        else if (instance!= this)
            Destroy(this);
        
        
	}
	
	// Update is called once per frame
	void Update () {

        
        

	}
   
}
