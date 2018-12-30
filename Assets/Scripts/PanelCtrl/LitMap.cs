using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitMap : MonoBehaviour {
    Vector3 pos;
    Vector2 litMap;
    float posX;
    float posY;
    RectTransform Map;
    Terrain plan;
	void Start ()
    {
        plan = GameObject.FindGameObjectWithTag("Plane").GetComponent<Terrain>();
        Map = transform.parent.GetComponent<RectTransform>();
	}
	
	void Update ()
    {
        pos = PlayerCtrl.Instance.transform.position - plan.transform.position;
        posX = pos.x / plan.terrainData.size.x;
        posY = pos.z / plan.terrainData.size.z;
        litMap.x = posX * Map.sizeDelta.x;
        litMap.y = posY * Map.sizeDelta.y;
        transform.localPosition = litMap;
	}
}
