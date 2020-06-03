using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System;
using Card = System.ValueTuple<CoMonadTest.Rank, CoMonadTest.Suit>;//Simplifies labelling parameter types

namespace CoMonadTest
{
    public enum Suit { Heart, Spade, Diamond, Club }//? 1
    public enum Rank { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }//? 2
    [TestClass]
    public class Domain_Modelling_CardGame_Scott_Wlaschin
    {


        [TestMethod]
        public void Runnable_Card_Game_Model()  //# Scott Wlaschin Domain modelling https://youtu.be/PLFl95c-IiU?t=716
        {
            List<Card> deck = OpenNewDeckOfCards();//? 3

            ImmutableStack<Card> shuffledDeck = Shuffle(deck);//? 4 

            var player = (name: "Hard Luck", hand: ImmutableList<Card>.Empty);//? 5

            for (int i = 0; i < 5; i++)//? 6
            {
                //deal
                shuffledDeck = shuffledDeck.Pop(out Card card);//? 7
                //pickup
                player.hand = player.hand.Add(card);//? 8
            }

            //? 8 LINES Of actual CODE and a couple of filled in helper methods and we are have a rudimentary running design!
            // For fun 
            // Helper methods are to get it actually running
            //If you got here u must realise that a couple of helper methods and variables with appropriate names can transmit the idea of the domain - and it runs!
            // These could be left as throw  in body but I thought more fun to complete the task

            ShowHand(player);




            var dropped = player.hand.Min();
            Debug.WriteLine($"SWAP OUT LOWEST CARD :  {dropped}");
            player.hand = player.hand.Remove(dropped);

            shuffledDeck = shuffledDeck.Pop(out Card c);
            player.hand = player.hand.Add(c);
            ShowHand(player);


   
        }












        static List<Card> OpenNewDeckOfCards()
        {
            var deck = new List<Card>();
            //Fill Deck
            for (Suit i = Suit.Heart; i <= Suit.Club; i++)
            {
                for (Rank j = Rank.Two; j <= Rank.Ace; j++)
                {
                    deck.Add((j,i));
                }
            }
            return deck;
        }
        static ImmutableStack<Card> Shuffle(List<Card> deck)
        {
            var array = deck.ToArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n);
                n--;
                Card temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            var stack = ImmutableStack<Card>.Empty;
            foreach (var item in array)
            {
                stack = stack.Push(item);
            }
            return stack;
        }
        static void ShowHand((string name, ImmutableList<Card> hand) player)
        {
            Debug.WriteLine($"Player '{player.name}' has these cards.");
            foreach (var item in player.hand.OrderBy(cc => cc.Item1).ThenBy(c2 => c2.Item2))
            {
                Debug.WriteLine(item);
            }
            foreach (var g in player.hand.GroupBy(crd => crd.Item1).Where(z => z.Count() > 1))
            {

                Debug.WriteLine($"Player '{player.name}' has {g.Count()} {g.Key}'s. What a WINNER!");

            }
        }
    }
  

}
