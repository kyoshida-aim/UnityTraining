using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour {
    private Text battleLog;
    private string log = string.Empty;

    public void UpdateLog(string message) {
        battleLog = GetComponent<Text>();
        log = message;
        battleLog.text = message;
    }

    public string Log { get { return log; } }

}