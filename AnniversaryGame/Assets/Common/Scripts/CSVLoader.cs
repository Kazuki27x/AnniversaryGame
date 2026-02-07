using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CSVLoader
{
	public async UniTask<List<TextContentData>> LoadStoryCSVAsync(string address, CancellationToken token)
	{
		TextAsset textAsset = await AddressableAssetLoadUtility.LoadAssetAsync<TextAsset>(address, token);
		var lines = textAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

		List<TextContentData> tmpList = new List<TextContentData>();
		foreach (var line in lines)
		{
			string[] LineSplit = line.Split(',');
			TextContentData tmpData = new TextContentData(
				(GameUtility.SpeakPlayer)int.Parse(LineSplit[0]),
				(GameUtility.Emotion)int.Parse(LineSplit[1]),
				LineSplit[2],
				LineSplit[3]);
			tmpList.Add(tmpData);
		}

		return tmpList;
	}

	public async UniTask<List<KeyItemManager.KeyItemBase>> LoadCSVAsync(string address, CancellationToken token)
	{
		TextAsset textAsset = await AddressableAssetLoadUtility.LoadAssetAsync<TextAsset>(address, token);
		var lines = textAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

		List<KeyItemManager.KeyItemBase> tmpList = new List<KeyItemManager.KeyItemBase>();
		foreach (var line in lines)
		{
			string[] LineSplit = line.Split(',');
			KeyItemManager.KeyItemBase tmpData = new KeyItemManager.KeyItemBase(
				(GameUtility.Stage)int.Parse(LineSplit[0]),
				int.Parse(LineSplit[1]),
				(GameUtility.KeyItemKind)int.Parse(LineSplit[2]),
				LineSplit[3]);
			tmpList.Add(tmpData);
		}

		return tmpList;
	}
}
