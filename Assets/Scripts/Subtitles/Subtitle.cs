using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Subtitle", menuName = "Subtitle", order = 1)]
public class Subtitle: ScriptableObject
{
    public string text;
    public string name;
    public bool isFoe;
    public AudioClip clip = null;
}
