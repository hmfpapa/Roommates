﻿using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        //  This is the address of the database.
        //  We define it here as a constant since it will never change.

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);


            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreid = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreid);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string chorename = Console.ReadLine();


                        Chore choretoadd = new Chore()
                        {
                            Name = chorename
                        };

                        choreRepo.Insert(choretoadd);

                        Console.WriteLine($"{choretoadd.Name} has been added and assigned an Id of {choretoadd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateid = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateid);

                        Console.WriteLine($"{roommate.Id} - {roommate.FirstName} pays {roommate.RentPortion} for room {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("View unassigned chores"):
                        Console.WriteLine("Unassigned Chores: ");
                        List<Chore> uchores = choreRepo.GetUnassignedChores();

                        foreach (Chore c in uchores)
                        {
                            Console.WriteLine($"{c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        List<Chore> allChores = choreRepo.GetAll();
                        List<Roommate> allRoommates = roommateRepo.GetAll();

                        // Show list of all chores
                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"{c.Id}: {c.Name}");
                        }
                        Console.Write("Please select a chore: ");
                        int selectedChoreId = int.Parse(Console.ReadLine());

                        // Show list of all roommates
                        foreach (Roommate r in allRoommates)
                        {
                            Console.WriteLine($"{r.Id}: {r.FirstName}");
                        }
                        Console.Write("Please enter the number of roommate to assign the chore: ");
                        int selectedRoommateId = int.Parse(Console.ReadLine());

                        // Add assigned chore to RoommateChore data table
                        choreRepo.AssignChore(selectedRoommateId, selectedChoreId);
                        Console.WriteLine("Success! Chore has been assigned.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> theseRooms = roomRepo.GetAll();
                        foreach (Room r in theseRooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.Write("Which room would you like to delete? ");
                        int selectedId = int.Parse(Console.ReadLine());
                        try
                        {
                            roomRepo.Delete(selectedId);
                            Console.WriteLine("Room has been successfully deleted");
                        }
                        catch
                        {
                            Console.WriteLine("Somebody lives here. You need to reassign them to another room first. Which room should they move to?");
                            foreach (Room r in theseRooms)
                            {
                                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                            }
                            int newId = Int32.Parse(Console.ReadLine());
                            roomRepo.UpdateRoomId(selectedId, newId);
                            roomRepo.Delete(selectedId);
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int thisChoreId = int.Parse(Console.ReadLine());
                        Chore thisChore = choreOptions.FirstOrDefault(r => r.Id == thisChoreId);

                        Console.Write("New Name: ");
                        thisChore.Name = Console.ReadLine();

                        choreRepo.Update(thisChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> theseChores = choreRepo.GetAll();
                        foreach (Chore c in theseChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Which chore would you like to delete? ");
                        int chosenId = int.Parse(Console.ReadLine());
                        try
                        {
                            choreRepo.Delete(chosenId);
                            Console.WriteLine("Chore has been successfully deleted");
                        }
                        catch {
                            choreRepo.DeleteChoreAssignment(chosenId);
                            choreRepo.Delete(chosenId);
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Search for roommate",
                "View unassigned chores",
                "Assign chore to roommate",
                "Update a room",
                "Delete a room",
                "Update a chore",
                "Delete a chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}