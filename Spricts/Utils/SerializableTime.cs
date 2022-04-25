using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SerializableTime : ISerializationCallbackReceiver
{
    const string Format = "HH:mm:ss";

    public DateTime Value;

    [SerializeField] string text;

    public SerializableTime() : this(new DateTime()) { }

    public SerializableTime(DateTime value)
    {
        Set(value);
    }

    public static implicit operator SerializableTime(DateTime value)
    {
        return new SerializableTime(value);
    }

    void Set(DateTime value)
    {
        Value = value;
        text = ToString();
    }

    public void OnAfterDeserialize()
    {
        DateTime newValue;
        if (DateTime.TryParse(text, out newValue))
        {
            Set(newValue);
        }
        else
        {
            Set(Value);
        }
    }

    public void OnBeforeSerialize() { }

    public override string ToString()
    {
        return Value.ToString(Format);
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SerializableTime))]
class SerializableTimeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property.FindPropertyRelative("text"), label);
    }
}
#endif