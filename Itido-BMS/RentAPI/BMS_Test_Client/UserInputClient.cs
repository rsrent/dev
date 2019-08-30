using System;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class UserInputClient
    {
        ClientController _clientController;

        public UserInputClient(ClientController clientController)
        {
            _clientController = clientController;
        }

        public async Task Start()
        {
            while (true)
            {
                Console.WriteLine("\nWould you like to make a:");
                Console.WriteLine("Get/GetMany/Put/Post request?");
                string callType = "";
                string path = "";
                string content = "";



                var input1 = Console.ReadLine();
                var inputs = input1.Trim().Split(" ");
                if (inputs.Length == 1)
                {
                    callType = inputs[0];
                    Console.WriteLine("You selected: " + callType + ", please type in the paht..");
                    path = Console.ReadLine();
                }
                else if (inputs.Length == 2)
                {
                    callType = inputs[0];
                    path = inputs[1];
                }

                if (callType == "Get")
                {
                    try
                    {
                        var result = await _clientController.Get(path);
                        if (result != null) result.Print();
                        else Console.WriteLine("Error, try again");
                    }
                    catch
                    {
                        Console.WriteLine("Error, try again");
                    }
                }
                if (callType == "GetMany")
                {
                    try
                    {
                        var result = await _clientController.GetMany(path);
                        if (result != null) result.Print();
                        else Console.WriteLine("Error, try again");
                    }
                    catch
                    {
                        Console.WriteLine("Error, try again");
                    }
                }
                if (callType == "Put")
                {
                    try
                    {
                        var result = await _clientController.PutNoContent(path, content);
                        if (result) Console.WriteLine("Success");
                        else Console.WriteLine("Error, try again");
                    }
                    catch
                    {
                        Console.WriteLine("Error, try again");
                    }
                }
                if (callType == "Post")
                {
                    try
                    {
                        var result = await _clientController.PostNoContent(path, content);
                        if (result) Console.WriteLine("Success");
                        else Console.WriteLine("Error, try again");
                    }
                    catch
                    {
                        Console.WriteLine("Error, try again");
                    }
                }
            }
        }
    }
}
