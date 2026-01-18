using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CSVLoader : MonoBehaviour
{
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


		/*
			FileStream DataFile = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader DataReader = new StreamReader(DataFile);
		var data = await LoadCsvAsync(path);
		//
		while (DataReader.Peek() != -1)                                                                     // ■ファイルの最後まで繰返
		{                                                                                                   // │
			string LineData = DataReader.ReadLine();                                                        // ├【変数定義】武器データ１行だけ（１行だけ読込）
			string[] LineSplit = LineData.Split(',');                                                       // ├【   〃   】武器データを，記号で分割して配列化
																											// │
			bName[ix] = LineSplit[0];                                                                       // ├ 配列に代入 武器名称
			bKouka[ix] = int.Parse(LineSplit[1]);                                                           // ├ 　　〃　　 武器効果(数字化)
			bKaine[ix] = int.Parse(LineSplit[2]);                                                           // ├ 　　〃　　 買値　　(　〃　)
			bUrine[ix] = int.Parse(LineSplit[3]);                                                           // ├ 　　〃　　 売値　　(　〃　)
			bSoubi1[ix] = int.Parse(LineSplit[4]);                                                          // ├ 　　〃　　 ローレ装(　〃　)
			bSoubi2[ix] = int.Parse(LineSplit[5]);                                                          // ├ 　　〃　　 サマル装(　〃　)
			bSoubi3[ix] = int.Parse(LineSplit[6]);                                                          // ├ 　　〃　　 ムーン装(　〃　)
			bBikou[ix] = LineSplit[7];                                                                      // ├ 　　〃　　 備考
			ix++;                                                                                           // └ 配列用添字 +1
		}                                                                                                   //
		DataReader.Close();
		*/
	}
}
