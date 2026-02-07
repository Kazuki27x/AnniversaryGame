using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class KeyItemManager
{
    public struct KeyItemBase
    {
        GameUtility.Stage m_stage;
        int m_memorieNumber;
        GameUtility.KeyItemKind m_kind;
        string m_resourcePath;
        public KeyItemBase(GameUtility.Stage stage, int memorieNumber, GameUtility.KeyItemKind kind, string resourcePath)
        {
            m_stage = stage;
            m_memorieNumber = memorieNumber;
            m_kind = kind;
            m_resourcePath = resourcePath;
        }
    }

    private List<KeyItemBase> m_keyItemList;

    // Start is called before the first frame update
    public async UniTask InitAsync(CancellationToken token)
    {
        // èâä˙âª
        m_keyItemList = new List<KeyItemBase>();

        m_keyItemList = await GameManager.Instance._CSVLoader.LoadCSVAsync("KeyItemList", token);
    }
}
