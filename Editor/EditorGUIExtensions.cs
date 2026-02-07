

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace mdz.Editor
{
    public struct EditorGUIData
    {
        public bool isVertical;
        public bool isReverse;
        public Rect rect;

        public EditorGUIData(bool isVertical, Rect rect, bool isReverse = false)
        {
            this.isVertical = isVertical;
            this.rect = rect;
            this.isReverse = isReverse;
        }

        public static implicit operator Rect(EditorGUIData drawer)
        {
            return drawer.rect;
        }
    }

    public static class EditorGUIExtensions
    {
        const float OBJECT_REFERENCE_HEIGHT = 16;
        const float LABEL_FIELD_HEIGHT = 16;
        const float PROPERTY_FIELD_HEIGHT = 16;
        const float BUTTON_HEIGHT = 16;
        const float FOLD_HEIGHT = 16;
        static EditorGUIExtensions()
        {

        }

        public static EditorGUIData BeginVertical(this Rect rect, bool isReverse = false)
        {
            return new EditorGUIData(true, rect, isReverse);
        }

        public static EditorGUIData BeginHorizontal(this Rect rect, bool isReverse = false)
        {
            return new EditorGUIData(false, rect, isReverse);
        }
        
        public static EditorGUIData BeginVertical(this EditorGUIData rect, bool isReverse = false)
        {
            return new EditorGUIData(true, rect, isReverse);
        }

        public static EditorGUIData BeginHorizontal(this EditorGUIData rect, bool isReverse = false)
        {
            return new EditorGUIData(false, rect, isReverse);
        }

        public static EditorGUIData Reverse(this EditorGUIData data)
        {
            return new EditorGUIData(data.isVertical, data.rect, !data.isReverse);
        }

        public static EditorGUIData PrefixLabel(this EditorGUIData data, GUIContent label)
        {
            var rect = data.rect;
            rect = EditorGUI.PrefixLabel(rect, label);
            return new EditorGUIData(data.isVertical, rect, data.isReverse);
        }

        public static EditorGUIData Slice(this EditorGUIData data, float size, Action<EditorGUIData> action)
        {
            var space = data;
            if (data.isVertical)
            {
                if (data.isReverse)
                {
                    space.rect.height = size;
                    space.rect.y = data.rect.y + data.rect.height - size;
                    data.rect.height -= size;
                }
                else
                {
                    space.rect.height = size;
                    data.rect.y += size;
                    data.rect.height -= size;
                }
            }
            else
            {
                if (data.isReverse)
                {
                    space.rect.width = size;
                    space.rect.x = data.rect.x + data.rect.width - size;
                    data.rect.width -= size;
                }
                else
                {
                    space.rect.width = size;
                    data.rect.x += size;
                    data.rect.width -= size;
                }
            }
            action(space);
            return data;
        }

        public static EditorGUIData ObjectReference(this EditorGUIData data, GUIContent label, ref UnityEngine.Object obj, bool allowSceneObjects, Type type, float directionSize = OBJECT_REFERENCE_HEIGHT)
        {
            var localObj = obj;
            var newData = data.Slice(directionSize, (space) =>
            {
                localObj = UnityEditor.EditorGUI.ObjectField(space, label, localObj, type, allowSceneObjects);
            });
            obj = localObj;
            return newData;
        }

        public static EditorGUIData ObjectReference<T>(this EditorGUIData data, GUIContent label, ref T obj, bool allowSceneObjects, float directionSize = OBJECT_REFERENCE_HEIGHT) where T : UnityEngine.Object
        {
            var localObj = obj;
            var newData = data.Slice(directionSize, (space) =>
            {
                localObj = (T)UnityEditor.EditorGUI.ObjectField(space, label, localObj, typeof(T), allowSceneObjects);
            });
            obj = localObj;
            return newData;
        }

        public static EditorGUIData LabelField(this EditorGUIData data, string label, float directionSize = LABEL_FIELD_HEIGHT)
        {
            var newData = data.Slice(directionSize, (space) =>
            {
                UnityEditor.EditorGUI.LabelField(space, label);
            });
            return newData;
        }

        public static EditorGUIData LabelField(this EditorGUIData data, string label, GUIStyle style, float directionSize = LABEL_FIELD_HEIGHT)
        {
            var newData = data.Slice(directionSize, (space) =>
            {
                UnityEditor.EditorGUI.LabelField(space, label, style);
            });
            return newData;
        }

        public static EditorGUIData LabelField(this EditorGUIData data, GUIContent label, float directionSize = LABEL_FIELD_HEIGHT)
        {
            var newData = data.Slice(directionSize, (space) =>
            {
                UnityEditor.EditorGUI.LabelField(space, label);
            });
            return newData;
        }

        public static EditorGUIData LabelField(this EditorGUIData data, GUIContent label, GUIStyle style, float directionSize = LABEL_FIELD_HEIGHT)
        {
            var newData = data.Slice(directionSize, (space) =>
            {
                UnityEditor.EditorGUI.LabelField(space, label, style);
            });
            return newData;
        }

        public static EditorGUIData PropertyField(this EditorGUIData data, SerializedProperty property, GUIContent label, bool includeChildren, float directionSize = PROPERTY_FIELD_HEIGHT)
        {
            var newData = data.Slice(directionSize, (space) =>
            {
                UnityEditor.EditorGUI.PropertyField(space, property, label, includeChildren);
            });
            return newData;
        }

        public static EditorGUIData SliceRatio(this EditorGUIData data, float ratio, Action<EditorGUIData> action)
        {
            var newData = data;
            if (data.isVertical)
            {
                if (data.isReverse)
                {
                    newData.rect.height = data.rect.height * ratio;
                    data.rect.y = data.rect.y + data.rect.height - newData.rect.height;
                    data.rect.height -= newData.rect.height;
                }
                else
                {
                    newData.rect.height = data.rect.height * ratio;
                    data.rect.y += newData.rect.height;
                    data.rect.height -= newData.rect.height;
                }
            }
            else
            {
                if (data.isReverse)
                {
                    newData.rect.width = data.rect.width * ratio;
                    data.rect.x += newData.rect.width;
                    data.rect.width -= newData.rect.width;
                }
                else
                {
                    newData.rect.width = data.rect.width * ratio;
                    data.rect.x = data.rect.x + data.rect.width - newData.rect.width;
                    data.rect.width -= newData.rect.width;
                }
            }
            action(newData);
            return data;
        }

        public static EditorGUIData SplitRatio(this EditorGUIData data, IEnumerable<(float, Action<EditorGUIData>)> ratios)
        {
            var newData = data;
            var startRect = data.rect;
            foreach (var (ratio, action) in ratios)
            {
                var size = data.isVertical ? data.rect.height * ratio : data.rect.width * ratio;
                newData = newData.Slice(size, action);
            }
            return newData;
        }

        public static EditorGUIData Split(this EditorGUIData data, IEnumerable<(float, Action<EditorGUIData>)> ratios)
        {
            var newData = data;
            var startRect = data.rect;
            foreach (var (size, action) in ratios)
            {
                newData = newData.Slice(size, action);
            }
            return newData;
        }

        public static EditorGUIData Button(this EditorGUIData data, string text, Action onClick, float directionSize = BUTTON_HEIGHT)
        {
            return data.Slice(directionSize, (space) =>
            {
                if (GUI.Button(space, text))
                {
                    onClick();
                }
            });
        }

        public static EditorGUIData IntentButton(this EditorGUIData data, string text, Action onClick, float directionSize = BUTTON_HEIGHT)
        {
            return data.Slice(directionSize, (space) =>
            {
                if (GUI.Button(EditorGUI.IndentedRect(space), text))
                {
                    onClick();
                }
            });
        }

        public static EditorGUIData Foldout(this EditorGUIData data, ref bool foldout, GUIContent label, Func<EditorGUIData, bool, EditorGUIData> foldoutGui, float directionSize = FOLD_HEIGHT)
        {
            var localFoldout = foldout;
            var newData = data.Slice(directionSize, (space) =>
            {
                localFoldout = UnityEditor.EditorGUI.Foldout(space, localFoldout, label);
            });
            foldout = localFoldout;
            newData = foldoutGui(newData, foldout);
            return newData;
        }

        public static EditorGUIData Space(this EditorGUIData data, float size)
        {
            return data.Slice(size, (space) => { });
        }

        public static EditorGUIData Toggle(this EditorGUIData data, ref bool toggle, float size)
        {
            var localToggle = toggle;
            data = data.Slice(size, (space) =>
            {
                localToggle = UnityEditor.EditorGUI.Toggle(space, localToggle);
            });
            toggle = localToggle;
            return data;
        }

        public static EditorGUIData SetGUIStatus(this EditorGUIData data, bool status, out bool resetableStatus)
        {
            resetableStatus = GUI.enabled;
            GUI.enabled = status;
            return data;
        }

        public static EditorGUIData SetColor(this EditorGUIData data, Color color, out Color resetableColor)
        {
            resetableColor = GUI.color;
            GUI.color = color;
            return data;
        }

        public static EditorGUIData DelayTextField(this EditorGUIData data, GUIContent label, string text, Action<string> onValueChanged, float directionSize = 16)
        {
            data = data.Slice(directionSize, (space) =>
            {
                var localText = UnityEditor.EditorGUI.DelayedTextField(space, label, text);
                if (localText != text)
                {
                    onValueChanged(localText);
                }
            });
            return data;
        }
    }
}