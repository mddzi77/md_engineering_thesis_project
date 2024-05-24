using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layer", menuName = "TheLayers/Layer")]
public class Layer : ScriptableObject
{
    [SerializeField] private string layerName;
    [SerializeField] private int order;
    [SerializeField] private Sprite sprite;
    
    public string LayerName => layerName;
    public int Order => order;
    public Sprite Sprite => sprite;
}
