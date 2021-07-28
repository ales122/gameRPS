using System;
using System.Security.Cryptography;
using System.Text;

namespace gameRPS
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length % 2 == 0 || args.Length == 0 || args.Length == 1) { ShowRightExample(); }
            else
            {
                bool flag = true;
                foreach (string i in args)
                {
                    var list = Array.FindAll(args, x => x == i);

                    if (list.Length > 1) { flag = false; break; }
                }
                if (flag)
                {
                    int computerMove = GenerateComputerMove(args); string key = GenarateHMACkey();
                    GenerateComputerMoveHMAC(key, args[computerMove]);
                    int playerMove = ShowMenu(args) - 1;
                    if (playerMove >= 0)
                    {
                        Console.WriteLine($"You move:{args[playerMove]}");
                        Console.WriteLine($"Computer move:{args[computerMove]}");
                        Console.WriteLine(DefineWinner(playerMove, computerMove, args));
                        Console.WriteLine($"HMAC key:{key}");
                    }
                }
                else { ShowRightExample(); }
            }
        }
        static int ShowMenu(string[] args)
        {
            int choose;
            do
            {
                Console.WriteLine("Avaible moves:");
                for (int i = 0; i < args.Length; i++) { Console.WriteLine($"{i + 1}-{args[i]}"); }
                Console.WriteLine("0-exit");
                Console.Write("Enter your move:");
                choose = Convert.ToInt32(Console.ReadLine());
            } while (choose > args.Length || choose < 0);
            return choose;

        }

        static string DefineWinner(int playerMove, int computerMove, string[] args)
        {
            string message = "";
            if (playerMove != computerMove)
            {
                int half = 1;
                for (int i = playerMove; i < args.Length; i++)
                {
                    if (i == args.Length - 1) { i = -2; }
                    else
                    {
                        int m = i + 1;
                        if (m == computerMove && half <= (args.Length - 1) / 2)
                        {

                            message = "You lose!"; break;
                        }
                        else if (m != computerMove && half <= (args.Length - 1) / 2) { half++; }
                        else if (half > (args.Length - 1) / 2) { message = "You win!"; break; }
                    }
                }
            }
            else message = "Draw!";
            return message;
        }
        static int GenerateComputerMove(string[] args)
        {
            Random random = new Random();
            return random.Next(0, args.Length);
        }
        static string GenarateHMACkey()
        {
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[16];
            random.GetBytes(bytes);
            string key = BitConverter.ToString(bytes).Replace("-", "");
            return key;
        }
        static void GenerateComputerMoveHMAC(string key, string move)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.Default.GetBytes(key));
            byte[] sha256Bytes = hmac.ComputeHash(Encoding.Default.GetBytes(move));
            Console.WriteLine($"HMAC:{Convert.ToBase64String(sha256Bytes)}");
        }
        static void ShowRightExample()
        {
            Console.WriteLine("Input something like this:rock paper scissors lizard spock");
        }

    }
}
