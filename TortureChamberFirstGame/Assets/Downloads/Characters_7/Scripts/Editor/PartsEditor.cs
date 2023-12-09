﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CharacterCustomization
{
    public class PartsEditor
    {
        private const float Width = 150;

        private Vector2 _scrollPosition;

        public void OnGUI(Rect rect, IEnumerable<Part> parts)
        {
            var partsArray = parts.ToArray();
            using (new GUI.GroupScope(rect))
            {
                using (var scrollViewScope = new GUILayout.ScrollViewScope(_scrollPosition))
                {
                    _scrollPosition = scrollViewScope.scrollPosition;
                    using (new GUILayout.HorizontalScope())
                    {
                        const int step = 3;
                        var index = 0;
                        while (index < partsArray.Length)
                        {
                            using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                            {
                                for (var i = index; i < index + step; i++)
                                {
                                    if (i < partsArray.Length)
                                    {
                                        RenderPart(partsArray[i]);
                                    }
                                }
                            }
                            index += step;
                        }
                    }
                }
            }
        }

        private static void RenderPart(Part part)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(Width)))
            {
                GUILayout.Label(part.Name);
                GUILayout.Box(AssetPreview.GetAssetPreview(part.SelectedVariant.PreviewObject));
                if (part.Name == CharacterCustomizationWindow.BodyPartName)
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        part.IsEnabled = EditorGUILayout.ToggleLeft("Enabled", part.IsEnabled, GUILayout.Width(100));
                    }
                }
                else
                {
                    part.IsEnabled = EditorGUILayout.ToggleLeft("Enabled", part.IsEnabled, GUILayout.Width(100));
                }

                using (new EditorGUI.DisabledScope(!part.IsEnabled))
                {
                    if (GUILayout.Button("Select"))
                    {
                        EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, $"t:prefab {part.Name}_", GUIUtility.GetControlID(FocusType.Passive));
                    }
                    var selectedMesh = EditorGUIUtility.GetObjectPickerObject();
                    if (selectedMesh && selectedMesh != null)
                    {
                        foreach (var variant in part.Variants.Where(variant => variant.Name == selectedMesh.name))
                        {
                            part.SelectedVariant = variant;
                        }
                    }

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.Width(Width)))
                    {
                        RenderLeftButton(part);
                        RenderCounter(part);
                        RenderRightButton(part);
                    }
                }
            }
        }

        private static void RenderLeftButton(Part part)
        {
            if (GUILayout.Button("<"))
            {
                var i = 0;
                foreach (var m in part.Variants.Where(v => v.Name == part.SelectedVariant.Name))
                {
                    i = part.Variants.IndexOf(m);
                }

                i--;
                if (i < 0)
                {
                    i = part.Variants.Count - 1;
                }
                part.SelectedVariant = part.Variants[i];
            }
        }

        private static void RenderRightButton(Part part)
        {
            if (GUILayout.Button(">"))
            {
                var i = 0;
                foreach (var m in part.Variants.Where(v => v.Name == part.SelectedVariant.Name))
                {
                    i = part.Variants.IndexOf(m);
                }

                i++;
                if (i >= part.Variants.Count)
                {
                    i = 0;
                }
                part.SelectedVariant = part.Variants[i];
            }
        }

        private static void RenderCounter(Part part)
        {
            var i = 0;
            foreach (var v in part.Variants.Where(v => v.Name == part.SelectedVariant.Name))
            {
                i = part.Variants.IndexOf(v);
            }
            var style = new GUIStyle
            {
                alignment = TextAnchor.LowerCenter,
                normal =
                {
                    textColor = Color.white
                }
            };
            GUILayout.Label($"{i + 1}/{part.Variants.Count}", style);
        }
    }
}