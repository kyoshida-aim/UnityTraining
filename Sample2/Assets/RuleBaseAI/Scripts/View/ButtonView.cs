using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonView : MonoBehaviour {
    [SerializeField] private Button attackButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button resetButton;

    public UnityEvent OnAttackClick {
        get { return attackButton.onClick; }
    }
    public UnityEvent OnHealClick {
        get { return healButton.onClick; }
    }
    public UnityEvent OnResetClick {
        get { return resetButton.onClick; }
    }
}