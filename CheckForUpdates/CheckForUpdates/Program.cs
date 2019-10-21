using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;

namespace CheckURLforUpdates //admis pls
{
    class Program
    {
        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // citeste asta: https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter URL: ");
            string url = Console.ReadLine(); //citim url ul
            int lungime = 0, L;
            int ok = 0;
            string htmlCode; //Codul sursa
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                htmlCode = client.DownloadString(url);
                L = htmlCode.Length; //calculam cate caractere contine codul
            }
            while (true)//loop
            {
                Program p = new Program();
                // p.OpenUrl(url);//deschide site ul 
                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {
                    // client.DownloadFile(url, @"C:\Users\Asus\source\repos\CheckURLforUpdates\CheckURLforUpdates\sursahtml.txt");//scrie codul in fisier

                    // Or you can get the file content without saving it
                    htmlCode = client.DownloadString(url);
                    lungime = htmlCode.Length;
                    //Console.WriteLine(lungime);
                    if (L != lungime) ok = 1; //daca s-a schimbat lungimea, a aparut o modificare

                }
                if (ok == 1)
                {
                    Console.WriteLine("NEW CONTENT!");
                    Console.Beep();
                }
                else Console.WriteLine("No updates yet...");

                //Thread.Sleep(1500);
            }
        }
    }
}
