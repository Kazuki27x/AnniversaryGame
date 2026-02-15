// Feelcerca Game Lib - Editor.AssetUtil.AddressableNameReplacer
// Copyright (c) 2023 Feelcerca Inc.
// support@feelcerca.com

using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Feelcerca.Editor.AssetUtil
{
	public class ChangeAddressableName : EditorWindow
	{
		private static AddressableAssetSettings Settings
		{
			get
			{
				var r = AddressableAssetSettingsDefaultObject.Settings;
				if (r == null) Debug.LogError("Not found addressableAssetSettings!");
				return r;
			}
		}

		private string pattern = null;
		private string newString = null;

		private string[] groupNames = null;
		private int groupIndex = 0;

		private void ReplaceNames()
		{
			var settings = Settings;
			if (settings == null) return;

			if (pattern == null)
			{
				Debug.LogWarning("The input for 'Pattern' is required.");
				return;
			}

			var reg = new Regex(pattern);
			var ns = newString != null ? newString : "";

			var gn = groupNames != null ? groupNames[groupIndex] : "";

			foreach (var group in settings.groups)
				if (group.Name == gn)
					foreach (var entry in group.entries)
					{
						if (Regex.IsMatch(entry.address, pattern))
						{
							var beforAddress = entry.address;
							var address = reg.Replace(entry.address, ns);
							entry.SetAddress(address);
							Debug.Log($"{beforAddress} Å® {address}");
						}
					}

			settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, null, true);
		}


		private void OnGUI()
		{
			EditorGUILayout.BeginVertical();

			if (groupNames != null && 0 < groupNames.Length)
			{
				var options = new GUIContent[groupNames.Length];

				var i = 0;
				foreach (var s in groupNames) options[i++] = new GUIContent(s);

				groupIndex = EditorGUILayout.Popup(new GUIContent("Group Name"), groupIndex, options);
			}

			pattern = EditorGUILayout.TextField(new GUIContent("Pattern"), pattern, new GUILayoutOption[] { GUILayout.Width(480) });
			newString = EditorGUILayout.TextField(new GUIContent("New String"), newString, new GUILayoutOption[] { GUILayout.Width(480) });

			if (GUILayout.Button("Replace", new GUILayoutOption[] { GUILayout.Width(240) })) ReplaceNames();

			EditorGUILayout.EndVertical();
		}

		private void Init()
		{
			var settings = Settings;
			if (settings != null && 0 < settings.groups.Count)
			{
				groupNames = new string[settings.groups.Count];

				var i = 0;
				foreach (var group in Settings.groups) groupNames[i++] = group.Name;
			}
		}

		[MenuItem("MyEditor/Replace Group Names")]
		private static void ShowWindow()
		{
			var replacer = EditorWindow.GetWindow(typeof(ChangeAddressableName)) as ChangeAddressableName;
			replacer.Init();
		}
	}
}