using UnityEngine;

[CreateAssetMenu(fileName = "Attribute", menuName = "Spell/Attribute")]
public class AttributeSpell : ScriptableObject
{
    [Header("Description")]
    public string Name;
    public Sprite icone;

    [Header("Attribute")]
    public float cooldown;
}