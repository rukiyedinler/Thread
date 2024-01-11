using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
class Program
{
    static ArrayList allNumbers = new ArrayList();
    static ArrayList evenNumbers = new ArrayList();
    static ArrayList oddNumbers = new ArrayList();
    static ArrayList primeNumbers = new ArrayList();

    static object lockObject = new object();

    static void Main()
    {
        // 1'den 1.000.000'e kadar olan sayıları ArrayList'e ekle
        for (int i = 1; i <= 1000000; i++)
        {
            allNumbers.Add(i);
        }

        // 4 ayrı Thread oluştur
        Thread t1 = new Thread(new ThreadStart(FindEvenNumbers));
        Thread t2 = new Thread(new ThreadStart(FindOddNumbers));
        Thread t3 = new Thread(new ThreadStart(FindPrimeNumbers));
        Thread t4 = new Thread(new ThreadStart(FindPrimeNumbers)); // İki ayrı Thread asal sayıları bulacak

        // Thread'leri başlat
        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        // Thread'lerin tamamlanmasını bekle
        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();

        // Sonuçları ekrana yazdır
        Console.WriteLine("Even Numbers: " + string.Join(", ", evenNumbers.ToArray()));
        Console.WriteLine("Odd Numbers: " + string.Join(", ", oddNumbers.ToArray()));
        Console.WriteLine("Prime Numbers: " + string.Join(", ", primeNumbers.ToArray()));
    }

    static void FindEvenNumbers()
    {
        foreach (int number in allNumbers)
        {
            if (number % 2 == 0)
            {
                lock (lockObject)
                {
                    evenNumbers.Add(number);
                }
            }
        }
    }

    static void FindOddNumbers()
    {
        foreach (int number in allNumbers)
        {
            if (number % 2 != 0)
            {
                lock (lockObject)
                {
                    oddNumbers.Add(number);
                }
            }
        }
    }

    static void FindPrimeNumbers()
    {
        foreach (int number in allNumbers)
        {
            if (IsPrime(number))
            {
                lock (lockObject)
                {
                    primeNumbers.Add(number);
                }
            }
        }
    }

    static bool IsPrime(int n)
    {
        if (n <= 1)
            return false;

        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
                return false;
        }

        return true;
    }
}