using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SerializableDate : ISerializationCallbackReceiver
{
    const string Format = "MM-dd";

    public DateTime Value;

    [SerializeField] string text;

    public SerializableDate() : this(new DateTime()) { }

    public SerializableDate(DateTime value)
    {
        Set(value);
    }

    public static implicit operator SerializableDate(DateTime value)
    {
        return new SerializableDate(value);
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
[CustomPropertyDrawer(typeof(SerializableDate))]
class SerializableDateDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property.FindPropertyRelative("text"), label);
    }
}
#endif