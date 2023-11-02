using System;
using System.Threading

class Engine
{
    static void Main(string[] args)
    {
        Timer timer = new Timer(RunmainBlock,null,1000) // 1000 ms = 1 second
    
        // This line is used to prevent the program from exiting immediately.
        // You can use other synchronization mechanisms to control the program's lifecycle.
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();

    }

    static void Mainblock(object state){
        Console.WriteLine($"Main block executed at {DateTime.Now}");
    }
}

class Card
{
    public string Name {get; set;}

    public Card(string name){
        Name = name;
    }

    public override string ToString(){
        return $"{Name}"
    }
}

class Player
{
    public string Name {get; set;}
    public Deck[] Deck {get;set;}

    public Player(string name){
        Name = name;
    }

    public void DisplayDeck(){
        Console.Writeline($"{Name}'s Deck:");
        foreach (var card in Deck){
            if (card != null)
            {
                Console.Writeline(card.ToString());
            }
        }
    }

    public void AddCardToHand(Card card)
    {
        for (int i = 0; i < Deck.Length; i++){
            if (Deck[i] == null){
                Deck[i] = card;
                break;
            }
        }
    }

}