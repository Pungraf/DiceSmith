using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    [SerializeField]
    private Color blinkColor;
    [SerializeField]
    private Color shadowColor;
    [SerializeField]
    private float colorTransitionSpeed;

    public int FaceIndex;
    public int tier;
    public string FaceName;
    public string type;
    public bool blinking;
    public bool shadow;

    private Renderer renderer;
    private Color startingColor;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        startingColor = renderer.material.color;
    }

    private void Update()
    {
        if (blinking)
        {
            Blinking();
        }
        else if (shadow)
        {
            Shadow();
        }
    }

    private void Blinking()
    {
        renderer.material.color = Color.Lerp(startingColor, blinkColor, Mathf.PingPong(Time.time * colorTransitionSpeed, 1));
    }

    public void EndBlinking()
    {
        renderer.material.color = startingColor;
    }

    private void Shadow()
    {
        renderer.material.color = Color.Lerp(startingColor, shadowColor, Time.time * colorTransitionSpeed);
    }
}
