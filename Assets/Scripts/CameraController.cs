using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame

    void Update()
    {
         Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
         if (hit != null && hit.collider != null && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
         {
            highlightTile(hit);
        }

#if UNITY_EDITOR
        if (hit != null && hit.collider != null)
        {
            highlightTile(hit);
        }
#endif
    }

    private void highlightTile(RaycastHit2D hit)
    {
        Debug.Log("I'm hitting " + hit.collider.name);
        GameObject hitObject = hit.collider.GetComponent<GameObject>();
        hit.collider.GetComponent<HexagonManager>().selectHexa();
        
    }
}
