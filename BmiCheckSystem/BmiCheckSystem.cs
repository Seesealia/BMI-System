using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BMICalculator;

class Program
{
    // File path for JSON storage
    static string filePath = "users.json";

    static void Main(string[] args)
    {
        while (true)
        {
            // Display menu options
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Log In");
            Console.WriteLine("3. Exit");
            Console.Write("Please choose an option: ");
            string choice = Console.ReadLine();

            // Handle user choice
            switch (choice)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    LogIn();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    // Method to register a new user
    static void Register()
    {
        // Load users from JSON file
        var users = LoadUsers();

        Console.Write("Enter your username: ");
        string username = Console.ReadLine();
        if (users.ContainsKey(username))
        {
            Console.WriteLine("Username already exists. Please choose another.");
            return;
        }

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Add new user to the dictionary
        users[username] = new User { Username = username, Password = password };
        // Save updated users to JSON file
        SaveUsers(users);
        Console.WriteLine("Registration successful!");
    }

    // Method to log in an existing user
    static void LogIn()
    {
        // Load users from JSON file
        var users = LoadUsers();

        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Checks if the username and password are correct
        if (users.ContainsKey(username) && users[username].Password == password)
        {
            Console.WriteLine("Login successful!");
            // Calculate BMI for the logged-in user
            CalculateBMI(users[username]);
            // Save updated users to JSON file
            SaveUsers(users);
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
        }
    }

    // Method to calculate BMI for a user
    static void CalculateBMI(User user)
    {
        Console.Write("Enter height in centimeters: ");
        double heightCm = double.Parse(Console.ReadLine());
        Console.Write("Enter weight in kilograms: ");
        double weight = double.Parse(Console.ReadLine());

        // Convert height from centimeters to meters
        double heightMeters = heightCm / 100;

        // Calculate BMI
        double bmi = weight / (heightMeters * heightMeters);
        user.BMI = bmi;

        // Determine BMI category
        string category = DetermineBMICategory(bmi);

        Console.WriteLine($"Your BMI is {bmi:F2}, which is considered {category}.");
    }

    // Method to determine BMI category
    static string DetermineBMICategory(double bmi)
    {
        if (bmi < 18.5)
        {
            return "underweight";
        }
        else if (bmi >= 18.5 && bmi < 24.9)
        {
            return "normal weight";
        }
        else if (bmi >= 25 && bmi < 29.9)
        {
            return "overweight";
        }
        else
        {
            return "obese";
        }
    }

    // Method to load users from a JSON file
    static Dictionary<string, User> LoadUsers()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, User>>(json);
        }
        return new Dictionary<string, User>();
    }

    // Method to save users to a JSON file
    static void SaveUsers(Dictionary<string, User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}

// User class to store user information
class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public double BMI { get; set; }
}



