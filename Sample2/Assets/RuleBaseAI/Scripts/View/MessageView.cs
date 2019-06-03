using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour {
    [SerializeField] private Text battleLog;
    private string log = string.Empty;

    public void UpdateLog(string message) {
        log = message;
        battleLog.text = message;
    }

    public string Log { get { return log; } }

}