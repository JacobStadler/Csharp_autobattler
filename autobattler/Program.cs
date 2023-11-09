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
            return $"{Name} ASM: {AttackSpeedModifier} DSM: {DrawSpeedModifier} HM: {HealthModifier} BM: {BlockModifier}";
        }

        public virtual string Play(Player player)
        {
            ApplyEffects(player);
            return $"{player} played {Name}";
        }

        public virtual void ApplyEffects(Player player)
        {
            player.AlterAttackSpeed(AttackSpeedModifier);
            player.AlterDrawSpeed(DrawSpeedModifier);
            player.AlterBlock(BlockModifier);
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
        public Weapon Weapon { get; set;}
        public Card[] Deck { get; set;}
        public List<Card> DiscardPile { get; private set;}
        public int DrawSpeed { get; set;} // Draw speed in milliseconds
        public int AttackSpeed { get; set;} // Attack speed in milliseconds
        public int Health { get; set;}
        public int MaxHealth { get; set;}
        public int Block { get; set;}
        private Random random = new Random();
        private List<Timer> activeTimers = new List<Timer>();

        public Player(string name, int drawspeed, int attackspeed, int maxhealth, int block, Weapon weapon)
        {
            Name = name;
            Deck = new Card[10];
            DiscardPile = new List<Card>();
            DrawSpeed = drawspeed;
            AttackSpeed = attackspeed;
            Health = maxhealth;
            MaxHealth = maxhealth;
            Block = block;
            Weapon = weapon ?? new IronSword();
        }

        public void StartCombat(Player target)
        {
            StartAttacking(target);
            StartDrawingCards();
        }

        public void CancelAllTimers() 
        {
            foreach(var timer in activeTimers)
            {
                timer.Dispose();
            }
            activeTimers.Clear();
        }

        public void AlterHealth(int health)
        {
            Health += health;
            if (Health < 0)
            {
                Health = 0;
            }
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


        public void DisplayDiscard()
        {
            Console.WriteLine($"{Name}'s Discard:");
            foreach (var card in DiscardPile)
            {
                if (card != null)
                {
                    Console.WriteLine(card.ToString());
                }
            }
        }

        public void StartDrawingCards()
        {
           Timer drawTimer = new Timer(DrawCard, null, 0, DrawSpeed);
            activeTimers.Add(drawTimer);
        }

        private void DrawCard(object state)
        {
            if (Health <= 0)
            {
                CancelAllTimers();
            }
            else {
                if (Deck[0] != null)
                {
                    Card drawnCard = Deck[0];
                    Console.WriteLine($"{Name} drew {drawnCard.Name} at {DateTime.Now}");
                    PlayCard(drawnCard);

                    for (int i = 0; i < Deck.Length - 1; i++)
                    {
                        Deck[i] = Deck[i + 1];
                    }
                    Deck[Deck.Length - 1] = null;
                }
                else
                {
                    ShuffleDiscardIntoDeck();
                }
            }
        }

        public void ShuffleDiscardIntoDeck()
        {
            int discardCount = DiscardPile.Count;

            for (int i = 0;i < discardCount; i++)
            {
                int randomIndex = random.Next(0, DiscardPile.Count);
                Card cardToMove = DiscardPile[randomIndex];

                for (int j = 0;j<Deck.Length;j++)
                {
                    if (Deck[j] == null)
                    {
                        Deck[j] = cardToMove;
                        break;
                    }
                }

                // Remove card from discard pile
                DiscardPile.RemoveAt(randomIndex);
            }

            Console.WriteLine($"{Name} Reshuffled");
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

        public void StartAttacking(Player target)
        {
            Timer attackTimer = new Timer(state => Attack(state, target), null, 0, AttackSpeed);
            activeTimers.Add(attackTimer);
        }

        private void Attack(object state, Player target)
        {
            if (Health <= 0)
            {
                CancelAllTimers();
            }
            else {
                target.AlterHealth(Weapon.Damage);
                Console.WriteLine($"{Name} attacked {target.Name} dealing {Weapon.Damage}.");
            }
        }

        public void AlterAttackSpeed(int speed)
        {
            AttackSpeed += speed;
        }

        public void AlterBlock(int block)
        {
            Block += block;
        }

        public Card MakeTotalRandomCard()
        {
            //Random random = new Random();
            return new Card("Random Card", random.Next(-1000,1000), random.Next(-1000,1000), random.Next(-10,10), random.Next(0,10));
        }

        public Card MakeSemiRandomCard()
        {
            int total = -10;
            int attackMod = 0;
            int drawMod = 0;
            int healthMod = 0;
            int blockMod = 0;
            while (total != 0)
            {
                attackMod = random.Next(-10, 10);
                drawMod = random.Next(-10, 10);
                healthMod = random.Next(-10, 10);
                blockMod = random.Next(0, 10);
                total = healthMod + drawMod + attackMod + blockMod;
            }
            return new Card("SRandom Card", attackMod * 100, drawMod * 100, healthMod, blockMod);
            
        }

        public Card ChooseCard(List<Card> cards)
        {
            for (int i = 0;i < cards.Count;i++)
            {
                Console.WriteLine($"{i} {cards[i].Name} Attack Speed Mod: {cards[i].AttackSpeedModifier} Draw Speed Mod: {cards[i].DrawSpeedModifier} Health Mod: {cards[i].HealthModifier} Block Modifier: {cards[i].BlockModifier}");
            }
            Console.WriteLine("Which Card do you want? : ");
            string usrInput = Console.ReadLine();
            int intusrInput = Int32.Parse(usrInput);
            AddCardToDeck(cards[intusrInput]);
            return cards[intusrInput];
        }

        public void PlayCard(Card card) 
        { 
            Console.WriteLine($"{Name} played {card.Name}");

            DiscardPile.Add(card);

        }

        public void ThreeRandomCards()
        {
            List<Card> Choices = new List<Card>();
            Choices.Add(MakeSemiRandomCard());
            Choices.Add(MakeSemiRandomCard());
            Choices.Add(MakeSemiRandomCard());
            Card chosenCard = ChooseCard(Choices);
            Console.WriteLine($"{chosenCard.Name} Attack Speed Mod: {chosenCard.AttackSpeedModifier} Draw Speed Mod: {chosenCard.DrawSpeedModifier} Health Mod: {chosenCard.HealthModifier} Block Modifier: {chosenCard.BlockModifier}");
        }

        public void ThreeRandom_RandomCards()
        {
            AddCardToDeck(MakeSemiRandomCard());
            AddCardToDeck(MakeSemiRandomCard());
            AddCardToDeck(MakeSemiRandomCard());
        }
    }

    class Weapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Damage { get; set; }

        public Weapon(string weaponName, string weaponDescription, int weaponDamange) 
        {
            Name = weaponName; Description = weaponDescription; Damage = weaponDamange;
        }
    }

    class IronSword : Weapon
    {
        public IronSword(string name="Iron Sword",string description = "A simple iron sword",int damage=2)
            :base(name, description, damage)
        {
            // additinal initialization
        }
    }

    class Program
    {
       static void Main(string[] args)
        {
            Player player1 = new Player("Sus", 2000, 2000, 20, 0, null);
            player1.ThreeRandomCards();
            player1.ThreeRandomCards();
            player1.ThreeRandomCards();
            //player1.AddCardToDeck(new Card("Silent Visage"));
            //player1.AddCardToDeck(new Card("Invigorate"));
            Player player2 = new Player("Usu", 2000, 2000, 20, 0, null);
            //player2.AddCardToDeck(new Card("Silent Visage"));
            player2.ThreeRandom_RandomCards();
            player1.DisplayDeck();

            player1.StartCombat(player2);
            player2.StartCombat(player1);

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