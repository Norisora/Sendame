using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    Transform playerHand;
    [SerializeField]
    GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InstantCard(playerHand);
    }

    void InstantCard(Transform hand)
    {
        Instantiate(cardPrefab, hand, false);
    }
}
