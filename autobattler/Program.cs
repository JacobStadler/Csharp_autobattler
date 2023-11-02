using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace autobattler
{
    class Card
    {
        public string Name { get; set; }

        public Card(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    class Player
    {
        public string Name { get; set;}
        public Card[] Deck { get; set;}
        public int DrawSpeed { get; set;} // Draw speed in milliseconds
        public int AttackSpeed { get; set;} // Attack speed in milliseconds
        public int Health { get; set;}
        public int MaxHealth { get; set;}
        public int Block { get; set;}

        public Player(string name,int drawspeed,int attackspeed,int maxhealth,int block)
        {
            Name = name;
            Deck = new Card[5];
            DrawSpeed = drawspeed;
            AttackSpeed = attackspeed;
            Health = maxhealth;
            MaxHealth = maxhealth;
            Block = block;

            // initialize timers when character created
            StartAttacking();
            StartDrawingCards();
        }

        public void DisplayDeck()
        {
            Console.WriteLine($"{Name}'s Deck:");
            foreach (var card in Deck)
            {
                if (card != null)
                {
                    Console.WriteLine(card.ToString());
                }
            }
        }

        public void StartDrawingCards()
        {
           Timer timer = new Timer(DrawCard, null, 0, DrawSpeed);
        }

        private void DrawCard(object state)
        {
            Console.WriteLine($"{Name} drew a card at {DateTime.Now}");
        }

        public void AlterDrawSpeed(int speed)
        {
            DrawSpeed += speed;
        }

        public void AddCardToDeck(Card card)
        {
            for (int i = 0; i < Deck.Length; i++)
            {
                if (Deck[i] == null)
                {
                    Deck[i] = card;
                    break;
                }
            }
        }

        public void StartAttacking()
        {
            Timer attackTimer = new Timer(Attack,null, 0, AttackSpeed);
        }

        private void Attack(object state)
        {
            Console.WriteLine($"{Name} attacked");
        }
    }

    class Program
    {
       static void Main(string[] args)
        {
            Player player1 = new Player("Sus", 2000, 1000, 20, 0);
            player1.AddCardToDeck(new Card("Silent Visage"));
            player1.AddCardToDeck(new Card("Invigorate"));
            Player player2 = new Player("Usu", 1500, 1500, 20, 0);
            player2.AddCardToDeck(new Card("Silent Visage"));
            player1.DisplayDeck();

            //Timer timer = new Timer(Mainblock, null, 0, 1000); // 1000 ms = 1 second

            // This line is used to prevent the program from exiting immediately.
            // You can use other synchronization mechanisms to control the program's lifecycle.
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
        static void Mainblock(object state)
        {
            Console.WriteLine($"Main block executed at {DateTime.Now}");
        }
    }
}