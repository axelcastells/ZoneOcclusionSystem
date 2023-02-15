using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UISystemGUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI zoneRelationsText;
    [SerializeField] TMPro.TextMeshProUGUI hiddenCountText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetRelationsText(List<ZonePair> zonePairs)
    {
        StringBuilder stringBuilder = new StringBuilder("<b>-Zone Relations-</b>\n");
        
        for(int i = 0; i < zonePairs.Count; i++) 
            stringBuilder.AppendLine($"{zonePairs[i].zoneA.name}<b> - </b>{zonePairs[i].zoneB.name}");

        zoneRelationsText.text = stringBuilder.ToString();
    }
    public void SetHiddenCount(int hiddenCount, int totalCount)
    {
        hiddenCountText.text = $"<b>-Hidden Characters-</b>\n<b>{hiddenCount}</b>/<b>{totalCount}</b>";
    }
}
