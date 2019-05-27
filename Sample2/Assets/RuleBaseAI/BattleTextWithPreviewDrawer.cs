using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 後藤さんのサンプルほぼそのまま使用しています
[CustomPropertyDrawer (typeof(BattleTextWithPreviewAttribute))]
public class BattleTextWithPreviewDrawer : PropertyDrawer
{
    private readonly float labelHeight = EditorGUIUtility.singleLineHeight;
    private readonly float textAreaHeight = EditorGUIUtility.singleLineHeight * 2;
    private readonly float previewerHeight = EditorGUIUtility.singleLineHeight * 3;

    public override void OnGUI (Rect position,
        SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField (position, property, label);
            return;
        }

        var messageValue = property.stringValue;
        EditorGUI.LabelField(position, label);

        var textAreaRect = position;
        textAreaRect.center += new Vector2(0, labelHeight);
        textAreaRect.height = textAreaHeight;
        messageValue = EditorGUI.TextArea(textAreaRect, messageValue);

        property.stringValue = messageValue;

        var previewPosition = position;
        previewPosition.center += new Vector2(0, labelHeight + textAreaHeight);
        previewPosition.height = previewerHeight;
        // var translatedMessage = MessageTranslator.Execute(messageValue);    // ここの実装は省略(特定文字列の置換処理)
		string translatedMessage = MessageTranslator(messageValue);
        EditorGUI.HelpBox(previewPosition, string.Format("[preview]\n{0}", translatedMessage), MessageType.None);
    }

    public override float GetPropertyHeight(SerializedProperty property,
        GUIContent label)
    {
        var height = base.GetPropertyHeight(property, label);
        var isTargetType = property.propertyType != SerializedPropertyType.String;

        return isTargetType
            ? height
            : labelHeight + textAreaHeight + previewerHeight;
    }

    private string MessageTranslator(string message) {
        // プレビュー変換
        // 配列を使って変換用のリストを作成したかったがC#の配列はまだよくわかってないので泥臭くReplaceする
        message = message.Replace("<PlayerName>", "\"プレイヤー\"");
        message = message.Replace("<EnemyName>", "\"敵\"");
        message = message.Replace("<Points>", "(ダメージ/回復量)");
        return message;
    }
}