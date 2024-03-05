using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public Suit suit;
    public int rank; // Ace is 1, J is 11, Q is 12, K is 13
    public Sprite image;
}
