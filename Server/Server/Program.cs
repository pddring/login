using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.net.httplistener?view=net-7.0 
            Console.WriteLine("Welcome to the login server");

            // create a HTTP server that listens on port 80
            const int port = 8080;
            string prefix = $"http://localhost:{port}/";

            Console.WriteLine($"Listening on {prefix}");
            HttpListener server = new HttpListener();
            server.Prefixes.Add(prefix);

            server.Start();

            bool running = true;
 
            while(running)
            {
                HttpListenerContext context = server.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;



                Console.WriteLine($"{request.HttpMethod} Request '{request.RawUrl}'");

                if (request.HttpMethod == "POST")
                {
                    using (StreamReader r = new StreamReader(request.InputStream))
                    {
                        string query = r.ReadToEnd();
                        Match m = Regex.Match(query, "username=(.*)&password=(.*)");
                        if (m.Success)
                        {
                            string username = m.Groups[1].Value;
                            string password = m.Groups[2].Value;

                            string html = $"";
                            
                            if (username == "username" && password == "password")
                            {
                                html = "Login succeeded";
                            } else
                            {
                                html = "Login failed";
                            }
                            byte[] buffer = Encoding.UTF8.GetBytes(html);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            response.OutputStream.Close();
                            Console.WriteLine($"Attempting to log in with u:{username} and p:{password}");
                        }

                    }
                }
                else
                {

                    string html = $"";
                    byte[] buffer = Encoding.UTF8.GetBytes("");

                    switch (request.RawUrl)
                    {
                        case "/":
                            buffer = File.ReadAllBytes("../../static/index.html");
                            response.ContentType = "text/html";
                            break;
                        default:
                            string path = "../../static" + request.RawUrl;
                            if (File.Exists(path))
                            {
                                buffer = File.ReadAllBytes(path);
                            }
                            else
                            {
                                response.StatusCode = 404;
                                html = "Sorry - file not found";
                                buffer = Encoding.UTF8.GetBytes(html);
                                Console.WriteLine($"Unknown URL: {request.RawUrl}");
                            }

                            break;
                    }

                    Console.WriteLine($"Sending: {buffer.Length} bytes");
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
            }
            server.Stop();

        }
    }
}
