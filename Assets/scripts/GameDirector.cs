using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject car; GameObject flag; GameObject distance;
    void Start()
    {
        this.car = GameObject.Find("car"); 
        this.flag = GameObject.Find("flag"); 
        this.distance = GameObject.Find("Distance");
    }
    void Update()
    {
        float length = this.flag.transform.position.x - this.car.transform.position.x;
        if (length > 0.5f)
            this.distance.GetComponent<Text>().text = "목표 지점까지 " + length.ToString("F2") + "m";
        else if (length <= 0.5f && length >= -0.5f)
            this.distance.GetComponent<Text>().text = "youwin";
        else
            this.distance.GetComponent<Text>().text = "GAMEOVER";

    }
}
