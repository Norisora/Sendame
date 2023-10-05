using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView cardView;
    public CardModel cardModel;

    private void Awake()
    {
        cardView = GetComponent<CardView>();
    }

    public void Init(int CardID)    //�J�[�h�������ɌĂ΂��֐�
    {
        cardModel = new CardModel(CardID);  //�J�[�h�f�[�^���擾
        cardView.Show(cardModel);
    }
}
