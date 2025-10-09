using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QBankingCircularQueue
{
    internal class BankingCircularQueue
    {
        private string[] queue;
        private int rear, front, maxSize, size;

        public BankingCircularQueue(int n)
        {
            maxSize = n;
            size = 0;                    //size of queue is initially 0
            queue = new string[maxSize]; //allocate space to array of size n
            front = rear = -1;           // No elements in the queue initially
        }
        public bool isEmpty()
        {
            if (size==0)
                return true;
            else
                return false;
        }
        public bool isFull()
        {
            if (size==maxSize)
                return true;
            else
                return false;
        }
        public void enqueue(string callerID)
        {
            if (isFull())
                Console.WriteLine("Caller "+ callerID +" is rejected. Queue is Full");
            else
            {
                if (front == -1) //front is initialized to -1 
                    front = 0;
                rear = (rear + 1)%maxSize;
                queue[rear] = callerID; 
                size++;
                Console.WriteLine(callerID + " added to the Queue");

            }
        }
        public string dequeue()
        {          
            if (isEmpty())
            {
                Console.WriteLine("No callers in the queue. Sarah is idle.");
                return "";
            }
            else
            {
                string element = queue[front];               
                front = (front + 1)%maxSize;
                Console.WriteLine("Connecting to "+element);
                size--;
                return element;
            }
        }
        public void print()
        {
            if (isEmpty())
                Console.WriteLine("Queue is Empty!");
            else
            {               
                for (int i = front; i != rear; i=(i+1)%maxSize)
                    Console.Write(queue[i] + " ");
                Console.Write(queue[rear] + " ");
                Console.WriteLine();
            }
        }
    }
}
