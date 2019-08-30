using System;
using System.Threading.Tasks;

namespace BMS_Test_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                CallWebApi().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception: {ex.ToString()}");
            }
        }

        static ClientController _controller;

        static private async Task CallWebApi()
        {
            _controller = new ClientController("https://localhost:5001/api/");
            await _controller.Login("toba", "123123123");
            //await new AbsenceClient(_controller).Run();
            //await new CleaningTaskClient(_controller).Run();

            //await new WorkRegistrationClient(_controller).Run();
            // await new WorkClient(_controller).Run();
            //await new WorkContractClient(_controller).Run();
            await new ProjectClient(_controller).Run();
            //await new DocumentClient(_controller).Run();
            //await new CommentClient(_controller).Run();

            //await new WorkContractClient(_controller).Run();
            //await new InvitationClient(_controller).Run();
            //await new QualityReportClient(_controller).Run();
            //await new CleaningTaskClient(_controller).Run();
            //await new UserClient(_controller).Run();
        }
    }
}
