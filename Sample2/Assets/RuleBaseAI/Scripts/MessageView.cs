using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour {
    private Text battleLog;
    private string displayingMessage = string.Empty;
    private bool isContinuous;
    private readonly string waitText = "∇";

    [HideInInspector]public string PlayerName;
    [HideInInspector]public string EnemyName;
    [HideInInspector]public int EffectQuantity;

    private void Awake() {
        battleLog = GetComponent<Text>();
    }

    public void DisplayMessage(string message, bool wait = false) {
        message = join(message);
        message = addWaitText(message, wait);
        message = Translate(message);
        updateLog(message);
    }

    private string join(string message) {
        if (isContinuous) {
            isContinuous = false;
            return displayingMessage.Replace(waitText, string.Empty) + "\n" +  message;
        }
        return message;
    }

    private string addWaitText(string message, bool wait) {
        if (wait) {
            isContinuous = true;
            return message + waitText;
        }
        return message;
    }

    private void updateLog(string message) {
        displayingMessage = message;
        battleLog.GetComponent<Text>().text = message;
    }

    // TODO : モデルやコントローラーを作成後参照しメッセージの変換ができるようにする
    private string Translate(string message) {
        message = message.Replace("<PlayerName>", PlayerName)
                            .Replace("<EnemyName>", EnemyName)
                            .Replace("<Points>", EffectQuantity.ToString());
        return message;
    }
}