/* Docker essential steps to run this program
 * In Developer PowerShell
 * 1) cd C:\Users\maamr\source\repos\ConestogaCodingCompetition
 Step 1 : Skipped in the video presentation
 * 2) docker build -t conestoga-coding-competition .    
 Step 2: Building conestoga-coding-competition (i.e. any image name)
 * 3) docker run --rm -v "${PWD}/animals.txt:/app/animals.txt" -it conestoga-coding-competition
 Step 3: Running the image in the container iteractively (Point the Present Working Directory) 
 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

// Initial Declaration and populating Adoption Fee dynamically
class Animal
{
	public string ID { get; set; }
	public string Species { get; set; }
	public string Name { get; set; }
	public char Gender { get; set; }
	public bool Spayed { get; set; }
	public string Breed { get; set; }
	public string Colour { get; set; }
	public DateTime Birthday { get; set; }
	public string VaccineStatus { get; set; }
	public string Identification { get; set; }
	public decimal AdoptionFee => CalculateAdoptionFee();
	// Dynamic calculation
	public decimal CalculateAdoptionFee()
	{
		int age = DateTime.Now.Year - Birthday.Year;
		if (age < 1) return 300; // Kittens, puppies, and all young animals
		if (age > 10) return 100; // Seniors
		return 200; // others
	}
	// Default printing format
	public override string ToString()
	{
		return $"-------------------------------\n" +
			   $" ID:            {ID}\n" +
			   $" Species:       {Species}\n" +
			   $" Name:          {Name}\n" +
			   $" Gender:        {Gender}\n" +
			   $" Spayed:        {Spayed}\n" +
			   $" Breed:         {Breed}\n" +
			   $" Colour:        {Colour}\n" +
			   $" Birthday:      {Birthday:dd/MM/yyyy}\n" +
			   $" Vaccine Status:{VaccineStatus}\n" +
			   $" Identification:{Identification}\n" +
			   $" Adoption Fee:  ${AdoptionFee}\n" +
			   $"-------------------------------";
	}
}

// Create, Read, Update, Delete implemented
class Program
{
	//static string filePath = "C:\\Users\\maamr\\source\\repos\\ConestogaCodingCompetition\\animals.txt";
	static string filePath = "animals.txt"; // this file shares directory with .csproj, .sln and

	static List<Animal> animals = new List<Animal>();

	static void Main(string[] args)
	{
		Console.WriteLine("Welcome to the Non-Profit Animal Shelter !");

		LoadAnimals();
		ShowMenu();
	}

	static void LoadAnimals()
	{
		if (File.Exists(filePath))
		{
			foreach (var line in File.ReadAllLines(filePath))
			{
				// Reading inputs
				var parts = line.Split(',');
				animals.Add(new Animal
				{
					// 9 variables
					ID = parts[0],
					Species = parts[1],
					Name = parts[2],
					Gender = parts[3][0], // even if F or Female both gets the same answer
					Spayed = bool.Parse(parts[4]),
					Breed = parts[5],
					Colour = parts[6],
					Birthday = DateTime.ParseExact(parts[7], "dd/MM/yyyy", null),
					VaccineStatus = parts[8],
					Identification = parts[9]
					// Adoption fee gets calculated automatically until here
				});
			}
		}
	}

	static void SaveAnimals()
	{
		File.WriteAllLines(filePath, animals.Select(a => a.ToString()));
	}

	static void ShowMenu()
	{
		while (true)
		{
			Console.WriteLine("1. Add Animal");
			Console.WriteLine("2. Remove Animal by ID");
			Console.WriteLine("3. Search by Name or Species");
			Console.WriteLine("4. Display Sorted Animals by species");
			Console.WriteLine("5. Display 3 Oldest Animals for each species");
			Console.WriteLine("6. Exit");
			Console.Write("Choose an option: ");
		
			switch (Console.ReadLine())
			{
				case "1": AddAnimal(); break;
				case "2": RemoveAnimal(); break;
				case "3": SearchAnimal(); break;
				case "4": DisplaySortedAnimals(); break;
				case "5": DisplayOldestAnimals(); break;
				case "6": Console.WriteLine("Exiting..."); return;
				default: // HELP instructions for the user(any confusions!)
					Console.WriteLine("Invalid option. I'm opening 'help' for a list of commands.");
					ShowHelp();
					break;
			}
		}
	}

	static void ShowHelp()
	{
		Console.WriteLine("\n=== Non-Profit Animal Shelter Help ===");
		Console.WriteLine("Commands:");
		Console.WriteLine("  1     - Add a new animal to the system.");
		Console.WriteLine("  2     - Remove animal by ID in the shelter.");
		Console.WriteLine("  3     - Search for an animal by name or species."); 
		Console.WriteLine("  4     - Display animals sorted by species.");
		Console.WriteLine("  5     - Display the three oldest animals for each species. ");
		Console.WriteLine("  6     - Exit the application.");
		Console.WriteLine("Any other - Redirect to help menu.");
		Console.WriteLine("  Run the program and type commands interactively.");
		Console.WriteLine("============================================\n");

	}

	// C (Create)
	static void AddAnimal()
	{
		// 9 variables
		Console.Write("Species: "); string species = Console.ReadLine();
		Console.Write("Name: "); string name = Console.ReadLine();
		Console.Write("Gender (M/F): "); char gender = Console.ReadLine()[0];
		Console.Write("Spayed (true/false): "); bool spayed = bool.Parse(Console.ReadLine());
		Console.Write("Breed: "); string breed = Console.ReadLine();
		Console.Write("Colour: "); string colour = Console.ReadLine();
		Console.Write("Birthday (dd/MM/yyyy): "); DateTime birthday = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
		Console.Write("Vaccine Status(Up to date, late, unknown): "); string vaccineStatus = Console.ReadLine();
		Console.Write("Identification: "); string identification = Console.ReadLine();

		// ID generation
		string id = (animals.Count + 1).ToString("D8"); // 0 masking
		// Creating a new object everytime -> Adoption fee gets calculated automatically
		Animal newAnimal = new Animal { ID = id, Species = species, Name = name, Gender = gender, Spayed = spayed, Breed = breed, Colour = colour, Birthday = birthday, VaccineStatus = vaccineStatus, Identification = identification };
		animals.Add(newAnimal);
		SaveAnimals();
		Console.WriteLine($"Animal added successfully with an adoption fee of ${newAnimal.AdoptionFee}.");

	}

	// D (Delete By Id)
	static void RemoveAnimal()
	{
		Console.Write("Enter Animal ID to remove: ");
		string id = Console.ReadLine();
		animals.RemoveAll(a => a.ID == id);
		SaveAnimals();
		Console.WriteLine("Animal removed successfully.");
	}
	// Search by name / species *Case In-sensitive
	static void SearchAnimal()
	{
		// Case in-sensitive search 
		Console.Write("Enter Name or Species: ");
		string search = Console.ReadLine().ToLower();
		var results = animals.Where(a => a.Name.ToLower().Contains(search) || a.Species.ToLower().Contains(search)).ToList();
		results.ForEach(Console.WriteLine);
	}
	// Sort animals
	static void DisplaySortedAnimals()
	{
		var sortedAnimals = animals.OrderBy(a => a.Species).ToList();
		sortedAnimals.ForEach(Console.WriteLine);
	}
	// Group by top 3 oldest animals in each species
	static void DisplayOldestAnimals()
	{
		var groupedBySpecies = animals.GroupBy(a => a.Species);
		foreach (var group in groupedBySpecies)
		{
			Console.WriteLine($"\nSpecies: {group.Key}");
			var oldest = group.OrderBy(a => a.Birthday).Take(3);
			foreach (var animal in oldest) Console.WriteLine(animal);
		}
	}
}
