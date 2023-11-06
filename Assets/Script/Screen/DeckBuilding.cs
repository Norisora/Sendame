using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DeckBuilding : MonoBehaviour
{
    static int[] CardIDs = { 1, 2, 3, 4, 5, 6, 7};
    DeckData DeckData { get; set; }
    [SerializeField]
    CardController cardPrefab;
    [SerializeField]
    RectTransform parent;
    CardController SelectCardObject { get; set; }

    CardController[] ViewPort { get; set; }


    private void Awake()
    {
        ViewPort = new CardController[CardIDs.Length];
        DeckData = new DeckData();

    }
    private void Start()
    {
        DeckData.CreateData(CardIDs);
        Viewing();
    }
    void Viewing()
    {

        for (int i = 0; i < CardIDs.Length; i++)
        {
            var cardData = DeckData.PassCard();

            ViewPort[i] = Instantiate(cardPrefab, parent);
            ViewPort[i].InitCard(cardData, SelectCard);
            
        }
        //DistVert();
    }
    public void SelectCard(CardController selected)
    {
        if (selected == null) return;
        SelectCardObject = selected;
    }
    void DistVert()
    {
        int count = CardIDs.Length;

        if (count < 3)
        {
            return;
        }

        Transform[] transforms = Selection.transforms.OrderBy(a => a.position.x).ToArray();


        float min = transforms[0].position.x;
        float max = transforms[count - 1].position.x;
        float d = (max - min) / (count - 1);

        for (int i = 1; i < count - 1; i++)
        {
            var t = transforms[i];
            Vector3 p = t.position;
            p.x = d * i + min;
            t.position = p;
        }
    }
}
