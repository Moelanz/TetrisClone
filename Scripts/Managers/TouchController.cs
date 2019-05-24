using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);
    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler SwipeEndEvent;

    public Text m_diagnosticText1;
    public Text m_diagnosticText2;
    public bool m_useDiagnostic = false;

    Vector2 m_touchMovement;
    int m_minSwipeDistance = 20;

	// Use this for initialization
	void Start ()
    {
        Diagnostic("", "");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.touches[0];

        if(touch.phase == TouchPhase.Began)
        {
            m_touchMovement = Vector2.zero;
            Diagnostic("", "");
        }
        else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            m_touchMovement += touch.deltaPosition;

            if (m_touchMovement.magnitude > m_minSwipeDistance)
            {
                OnSwipe();
                Diagnostic("Swipe Detected", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
            }
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            OnSwipeEnd();
        }
	}

    void OnSwipe()
    {
        if(SwipeEvent != null)
            SwipeEvent(m_touchMovement);
    }

    void OnSwipeEnd()
    {
        if (SwipeEvent != null)
            SwipeEndEvent(m_touchMovement);
    }

    void Diagnostic(string text1, string text2)
    {
        m_diagnosticText1.gameObject.SetActive(m_useDiagnostic);
        m_diagnosticText2.gameObject.SetActive(m_useDiagnostic);

        if (!m_diagnosticText1 && !m_diagnosticText2)
            return;

        m_diagnosticText1.text = text1;
        m_diagnosticText2.text = text2;
    }

    string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = "";

        if(Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            //Horizontal movement
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        }
        else
        {
            //Vertical movement
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        }

        return direction;
    }
}
