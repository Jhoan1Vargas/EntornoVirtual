using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="Items/Item", order=1)]
public class Item : ScriptableObject
{
    [SerializeField]
    string labelName = "My Item";
    [SerializeField]
    Sprite icon;
    [SerializeField, Range(1, 10)]
    int value = 1;
    [SerializeField]
    Color color = Color.blue;

    public Sprite Icon => icon;
    public string LabelName => labelName;
    public int Value => value;
    public Color Color => color;
}
