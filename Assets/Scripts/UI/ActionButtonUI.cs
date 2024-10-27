using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;

        textMeshPro.text = baseAction.GetActionName().ToUpper();
    }
}
