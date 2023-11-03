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
        public int AttackSpeedModifier { get; set; }
        public int DrawSpeedModifier { get; set; }
        public int HealthModifier { get; set; }
        public int BlockModifier { get; set; }

        public Card(string name, int attackSpeedModifier = 0, int drawSpeedModifier = 0, int healthModifier = 0, int blockModfier = 0)
        {
            Name = name;
            AttackSpeedModifier = attackSpeedModifier;
            DrawSpeedModifier = drawSpeedModifier;
            HealthModifier = healthModifier;
            BlockModifier = blockModfier;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual string Play(Player player)
        {
            return $"{player} played {Name}";
        }

    }

    class QuickDraw : Card
    {
        public QuickDraw(string name)
            :base(name)
        {
            // additinal initialization
        }

        public override string Play(Player player)
        {
            string res = base.Play(player);
            player.AlterDrawSpeed(-1000);
            return $"{res} and lowered their attack speed";
        }


    }

    class QuickAttack : Card
    {
        public QuickAttack(string name)
            :base(name)
        {
            // add additinal inititialization here
        }

        public override string Play(Player player)
        {
            string res = base.Play(player);
            player.AlterAttackSpeed(-1000);
            return $"{res} and lowered their draw speed.";
            
        }

    }

    class Block : Card
    {
        public Block(string name)
            :base(name)
        {
            // additinal initialization
        }

        public override string Play(Player player)
        {
            string res =  base.Play(player);
            player.AlterBlock(10);
            return $"{res} and added block";
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
        }

        public void StartCombat()
        {
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

        public void AlterAttackSpeed(int speed)
        {
            AttackSpeed += speed;
        }

        public void AlterBlock(int block)
        {
            Block += block;
        }

        private Card MakeRandomCard()
        {
            Random random = new Random();
            return new Card("Random Card",random.Next(-1000,1000),random.Next(-1000,1000),random.Next(-10,10),random.Next(0,10));
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