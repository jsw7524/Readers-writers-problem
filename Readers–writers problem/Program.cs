using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Readers_writers_problem
{
    class ReadersWritersProblem
    {
        Semaphore resource = new Semaphore(1, 1);
        Semaphore rmutex = new Semaphore(1, 1);
        Semaphore readTry = new Semaphore(1, 1);
        int data = 0;
        int readcount = 0;
        Random rnd = new Random();

        void Writer()
        {
            readTry.WaitOne();
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            resource.WaitOne();
            data = (data + 1) % 1000;
            //Thread.Sleep(rnd.Next(100));
            resource.Release();
            readTry.Release();
        }

        void Reader()
        {
            readTry.WaitOne();
            rmutex.WaitOne();
            readcount++;
            if (readcount == 1)
            {
                resource.WaitOne();
            }
            rmutex.Release();
            readTry.Release();
            ////////////////////////////////
            Console.WriteLine("Reader " + Thread.CurrentThread.ManagedThreadId + " : " + data);
            ////////////////////////////////
            rmutex.WaitOne();
            readcount--;
            if (readcount == 0)
            {
                resource.Release();
            }
            rmutex.Release();
        }


        public async Task ExecuteWriter()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Writer();
                }
            });
        }

        public async Task ExecuteReader()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Reader();
                }
            });
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var rwp = new ReadersWritersProblem();

            rwp.ExecuteWriter();

            rwp.ExecuteReader();
            rwp.ExecuteReader();
            rwp.ExecuteReader();

            Console.ReadLine();
        }
    }
}
