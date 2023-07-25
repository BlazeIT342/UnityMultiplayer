using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class SpellHUD : MonoBehaviour
{
    public List<Image> slotIcon;
    public List<Text> reloadText;

    private CastSystem castSystem;

    private void Start()
    {
        castSystem = GetComponent<CastSystem>();
        for (int i = 0; i < castSystem.Spells.Count; i++)
        {
            slotIcon[i].sprite = castSystem.Spells[i].attribute.icone;
        }
    }
}
