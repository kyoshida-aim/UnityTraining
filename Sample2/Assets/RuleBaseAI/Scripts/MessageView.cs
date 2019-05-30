using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour {
    public GameObject battleLog;
    private string displayingMessage = string.Empty;
    private bool isContinuous;
    private readonly string waitText = "∇";

    private void Start() {
        isContinuous = false;
    }

    public void Set(string message, bool wait = false) {
        message = join(message);
        message = addWaitText(message, wait);
        message = Translate(message);
        updateLog(message);
    }

    public string join(string message) {
        if (isContinuous) {
            isContinuous = false;
            return displayingMessage.Replace(waitText, string.Empty) + "\n" +  message;
        }
        return displayingMessage;
    }

    public string addWaitText(string message, bool wait) {
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
        // message = message.Replace(
        //             "<PlayerName>", player.actorName).Replace(
        //                 "<EnemyName>", enemy.actorName).Replace(
        //                     "<Points>", this.points.ToString());
        return message;
    }
}