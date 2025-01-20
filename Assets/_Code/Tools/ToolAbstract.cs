using UI.Bottom;
using UnityEngine;

namespace Tools
{
    public abstract class ToolAbstract : MonoBehaviour
    {
        
        protected void ShowTooltip(string tooltip)
        {
            InfoPanel.Instance.SetInfoText(tooltip);
        }
        
        protected void ShowTooltip(string tooltip, float time)
        {
            InfoPanel.Instance.SetInfoText(tooltip, time);
        }

        protected void HideTooltip()
        {
            InfoPanel.Instance.ClearInfoText();
        }
    }
}
