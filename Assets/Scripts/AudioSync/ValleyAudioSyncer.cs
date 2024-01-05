using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyAudioSyncer : AudioSyncer 
{
    
    public List<Material> childShaders = new List<Material>();
    public string colorPropertyName = "";
    private Color _curr;
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer)
            {
                Material material = renderer.material;
                //Shader shader = material.shader;

                childShaders.Add(material);
            }
        }

        changeToColor(restColor);
    }
    private IEnumerator MoveToColor(Color _target)
    {
        //Debug.Log("On Beat");
        _curr = childShaders[0].GetColor(colorPropertyName);
        Color _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            changeToColor(_curr);

            yield return null;
        }

        m_isBeat = false;
    }

    private void changeToColor(Color target)
    {
        foreach (Material material in childShaders)
        {
            //Debug.Log(material.GetColor(colorPropertyName));
            material.SetColor(colorPropertyName, target);
        }
    }
    //private Color RandomColor()
    //{
    //    if (beatColors == null || beatColors.Length == 0) return Color.white;
    //    m_randomIndx = Random.Range(0, beatColors.Length);
    //    return beatColors[m_randomIndx];
    //}

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        //m_img.color = Color.Lerp(m_img.color, restColor, restSmoothTime * Time.deltaTime);
        _curr = Color.Lerp(_curr, restColor, restSmoothTime * Time.deltaTime);
        changeToColor(_curr);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        //Color _c = RandomColor();

        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", beatColor);
    }

    private void Start()
    {
        //m_img = GetComponent<Image>();
    }

    public Color beatColor;
    public Color restColor;

    //private int m_randomIndx;
    //private Image m_img;
}
