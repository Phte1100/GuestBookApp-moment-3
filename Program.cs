using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // Används för JSON-serialisering
#nullable disable // Tar bort varningarna

class Program
{
    static void Main()
    {
        // Skapa en instans av GuestBook (detta laddar också in tidigare inlägg)
        GuestBook guestBook = new GuestBook();

bool running = true;
while (running)
{
    // Rensa konsolen och visa menyn efter att ett val är hanterat
    Console.Clear();
    Console.WriteLine("P H I L I P ' S  G U E S T B O O K");
    Console.WriteLine("\n\n1. Lägg till ett inlägg \n2. Ta bort ett inlägg\n\nX. Avsluta \n");

    guestBook.DisplayEntries();

    Console.WriteLine("\nGör ditt val: ");
    string choice = Console.ReadLine();

    switch (choice.ToLower()) // Gör val hantering case-insensitive
    {
                case "1": // Lägg till ett inlägg
                    string owner, text;

                    // Upprepa tills både ägare och text är giltiga (ej tomma eller whitespace)
                    do
                    {
                        Console.Write("Ange ägare: ");
                        owner = Console.ReadLine();

                        Console.Write("Ange inläggstext: ");
                        text = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(text))
                        {
                            Console.WriteLine("Ägare och inlägg får inte vara tomma. Försök igen.");
                        }

                    } while (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(text)); // Fortsätt tills båda fält är ifyllda

                    // Lägg till inlägg till gästboken när valideringen är klar
                    guestBook.AddEntry(owner, text);

                    Console.WriteLine("Inlägget har lagts till.");
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    break;


                case "2": // Ta bort ett inlägg
            Console.Write("Ange index för inlägget du vill ta bort: ");
            int index;
            if (int.TryParse(Console.ReadLine(), out index))
            {
                // Ta bort inlägget med det angivna indexet
                guestBook.RemoveEntry(index);
            }
            else
            {
                Console.WriteLine("Du kan enbart välja en siffra ur listan.");
            }

            // Vänta på att användaren trycker på en tangent innan rensning
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
            break;

        case "x": // Avsluta
            running = false;
            break;

        default:
                    if (!string.IsNullOrEmpty(choice))
                    {
                        Console.WriteLine("Du kan endast välja 1, 2 eller x.");
                        // Vänta på att användaren trycker på en tangent innan rensning
                        Console.WriteLine("\nTryck på valfri tangent för att försöka igen...");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Fältet får inte vara tomt.");
                        // Vänta på att användaren trycker på en tangent innan rensning
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                        Console.ReadKey();
                        break;
                    }
    }
}


        // Spara alla inlägg till fil
        guestBook.SaveEntries();

        Console.WriteLine("\nProgrammet avslutas. Tack för att du använde gästboken!");
    }
}

public class Entry
{
    // Egenskap för ägaren av inlägget
    public string Owner { get; set; }

    // Egenskap för texten i inlägget
    public string Text { get; set; }

    // Konstruktor för att skapa ett nytt inlägg
    public Entry(string owner, string text)
    {
        Owner = owner;
        Text = text;
    }

    // Standardkonstruktor för deserialisering
    public Entry() { }
}

public class GuestBook
{
    // Lista för att hålla alla inlägg
    private List<Entry> entries;

    // Filnamn där gästboken sparas
    private const string FileName = "guestbook.json";

    // Konstruktor som initialiserar listan och laddar tidigare sparade inlägg från filen
    public GuestBook()
    {
        entries = new List<Entry>();
        LoadEntries(); // Ladda inlägg från fil om de finns
    }

    // Metod för att lägga till ett nytt inlägg
    public void AddEntry(string owner, string text)
    {

            entries.Add(new Entry { Owner = owner, Text = text });

    }

    // Metod för att ta bort ett inlägg baserat på index
    public void RemoveEntry(int index)
    {
        if (index >= 0 && index < entries.Count)
        {
            entries.RemoveAt(index);
            Console.WriteLine("Inlägget har tagits bort.");
        }
        else
        {
            Console.WriteLine("Ogiltigt index.");
        }
    }

    // Metod för att visa alla inlägg
    public void DisplayEntries()
    {
        if (entries.Count > 0)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine($"[{i}]: {entries[i].Owner} - {entries[i].Text}");
            }
        }
        else
        {
            Console.WriteLine("Inga inlägg finns i gästboken.");
        }
    }

    // Metod för att spara inlägg till en JSON-fil
    public void SaveEntries()
    {
        try // try-catch
        {
            string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText(FileName, json);
            Console.WriteLine("Inläggen har sparats.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fel vid sparande av inlägg: {ex.Message}");
        }
    }

    // Metod för att ladda inlägg från en JSON-fil
    public void LoadEntries()
    {
        if (File.Exists(FileName))
        {
            try
            {
                string json = File.ReadAllText(FileName);
                entries = JsonConvert.DeserializeObject<List<Entry>>(json) ?? new List<Entry>();
                Console.WriteLine($"Inlägg har laddats: {entries.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid inläsning av inlägg: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ingen befintlig fil hittades. Skapar en ny lista.");
        }
    }
}
