namespace QBankingCircularQueue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, Welcome to FinTech Bank Online Call Service..");
            BankingCircularQueue q=new BankingCircularQueue(6);
            string[] events = {"C Alice", "C Bob", "S", "C Charlie","C David","C Eve","S","C Frank","C Grace","C Henry","C Isabella",
            "S", "S", "C Jack", "S", "S","S","S","S","S"};
            string callerId;           
            Console.WriteLine("*** Online Banking Call System Simulation ***\n");
            foreach (string s in events)
            {
                if (s.StartsWith("C"))
                {
                    callerId = s.Substring(2);
                    q.enqueue(callerId);
                    Console.WriteLine("Call Queue: ");
                    q.print();
                }
                else if (s == "S")
                {
                    q.dequeue();
                    Console.WriteLine("Call Queue: ");
                    q.print();
                }
            }             
        }
    }
}