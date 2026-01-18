using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AddressableAssetLoadUtility
{

    // 指定したアドレスのオブジェクトを取得
    // where T : ？ はTに入れることができる対象（Object = UnityEngine.Object(GameObjectやTexture2Dなど)）
    public static async UniTask<T> LoadAssetAsync<T>(string _address, CancellationToken token) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(_address);
        var asset = await handle.ToUniTask(cancellationToken: token);
        return (T)asset;
    }

    // 指定したラベル対象のオブジェクトを取得
    public static List<T> LoadAssetListAsync<T>(string _labelName) where T : Object
    {
        var handle = Addressables.LoadAssetsAsync<T>(_labelName, null);
        var asset = handle.WaitForCompletion();
        return (List<T>)asset;
    }
}
