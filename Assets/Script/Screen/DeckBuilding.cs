using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilding : MonoBehaviour
{
    static int[] CardIDs = { 1, 2, 3, 4, 5, 6, 7};
    DeckData CardList { get; set; }
    DeckData NewDeckData { get; set; }
    [SerializeField]
    CardController cardPrefab;
    [SerializeField]
    RectTransform cardListViewer;
    [SerializeField]
    Button saveButton;
    [SerializeField]
    Button setDeckButton;
    [SerializeField]
    DropPlace dropPlace;
    CardController SelectCardObject { get; set; }

    CardController[] CardListContents { get; set; }

    int deckIndex;

    private void Awake()
    {
        CardListContents = new CardController[CardIDs.Length];
        CardList = new DeckData();  //所持カード
        NewDeckData = new DeckData();
    }
    private void Start()
    {
        CardList.CreateData(CardIDs);
        Viewing();  //CardIDs.Lengthを並べる
        Load();
        Save();     //セーブボタン押下時の関数
        SetNewDeck();   //プリセットデッキ作成
    }
    void Viewing()
    {
        for (int i = 0; i < CardIDs.Length; i++)
        {
            var cardData = CardList.PassCard();

            CardListContents[i] = Instantiate(cardPrefab, cardListViewer);
            CardListContents[i].InitCard(cardData, SelectCard);
        }
    }
    public void SelectCard(CardController selected)
    {
        if (selected == null) return;
        SelectCardObject = selected;
    }

    /// <summary>
    ///  選択したカードをデッキへ加える
    /// </summary>
    /// <param name="cardID"></param>
    public void GetID(int cardID)
    {
        NewDeckData.Choice(cardID);
    }
    public void RemoveID(int cardID)
    {
        NewDeckData.Remove(cardID);
    }
   
    public void Load()
    {
        if (SaveDataManager.Instance.HasSaveData(SaveDataManager.SaveType.Deck, deckIndex))
        {
            NewDeckData = SaveDataManager.Instance.Load<DeckData>(SaveDataManager.SaveType.Deck, deckIndex);
            foreach (var cardID in NewDeckData.deck)
            {
                var card = CardListContents.FirstOrDefault(v => v.Data.CardModel.cardID == cardID);
                if (card == null)
                {
                    // TODO エラー　入るはずない　チートなど
                }
                dropPlace.MoveCard(card.transform);
            }
        }
    }
    public void Save()
    {
        saveButton.onClick.AddListener(() => {
            Debug.Log("NewDeckDataのデッキ枚数" + NewDeckData.deck.Count());
            NewDeckData.Building();

            SaveDataManager.Instance.Save(NewDeckData, SaveDataManager.SaveType.Deck, deckIndex);
            SoundManager.instance.PlayOneShot(SoundManager.OneShotType.Save);
            //SoundManager.instance.PlaySE(SoundManager.SEType.Save);
            GameDirector.Instance.TransitionManager.TransitionScreen(ConstScreenList.ScreenType.Main);
        });
    }
    public void SetNewDeck()
    {
        setDeckButton.onClick.AddListener(() => {
            if (NewDeckData.deck != null) NewDeckData.deck.Clear();
            setDeckButton.enabled = false;
            int count = 0;
            for (int i = 1; i <= 3; i++)
            {
                while (count <= 15)
                {
                    GetID(i);
                    count++;
                }
                count = 0;
            }
        });
    }
}
