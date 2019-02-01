using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherOWM
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "nothing";
            async Task Here()
            {
                Console.WriteLine("Введите название города на англйском языке, примеры:");
                Console.WriteLine("Москва - Moscow, Санкт-Петербург - Saint Petersburg");
                Console.Write("Город:");
                string Name = Console.ReadLine();
                try
                {
                    WebRequest request = WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?q=" + Name + "&mode=xml&APPID=2aff4d5deb2951c1a017e9261a1b00c2");
                    WebResponse response = await request.GetResponseAsync();
                    string answer = string.Empty;
                    using (Stream s = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(s))
                        {
                            answer = await reader.ReadToEndAsync();
                        }
                    }
                    response.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(answer);
                    string temp = doc.DocumentElement["temperature"].Attributes[0].Value;
                    double tempd = double.Parse(temp.Split('.')[0]) + double.Parse(temp.Split('.')[1]) / 100 - 273;
                    string hum = doc.DocumentElement["humidity"].Attributes[0].Value + "%";
                    string sunrise = doc.DocumentElement["city"]["sun"].Attributes[0].Value;
                    string sunset = doc.DocumentElement["city"]["sun"].Attributes[1].Value;

                    

                    Console.WriteLine("Время отображается в формате GMT 0");
                    Console.Write("Температура: ");
                    tempd = Math.Round(tempd, 2);
                    Console.Write(tempd);
                    Console.WriteLine("°C");
                    Console.Write("Влажность: ");
                    Console.WriteLine(hum);
                    Console.Write("Восход в: ");
                    sunrise = sunrise.Split('T')[1];
                    Console.WriteLine(sunrise);
                    Console.Write("Закат в: ");
                    sunset = sunset.Split('T')[1];
                    Console.WriteLine(sunset);

                    string date = DateTime.Today.Day + "," + DateTime.Today.Month + "," +
                    DateTime.Today.Year;
                    StreamWriter sr = new StreamWriter("C:/"+Name + date + ".txt");
                    sr.WriteLine("Время отображается в формате GMT 0");
                    sr.Write("Температура: ");
                    sr.Write(tempd);
                    sr.WriteLine("°C");
                    sr.Write("Влажность: ");
                    sr.WriteLine(hum);
                    sr.Write("Восход в: ");
                    sr.WriteLine(sunrise);
                    sr.Write("Закат в: ");
                    sr.WriteLine(sunset);
                    sr.Close();

                }
                 catch (WebException e)
                {
                    if (e.Message == "Удаленный сервер возвратил ошибку: (404) Не найден.")
                    {
                        Console.WriteLine("Операция не удалась, возможно, вы неправильно ввели название города.");
                        
                    }
                }


            }
            while (true)
            {
                Task GetWeather = Task.Run(() => Here());
                Task.WaitAll(GetWeather);
                
                Console.WriteLine("Хотите попробовать еще? y/n");
                if (Console.ReadLine() != "y")
                {
                    break;
                }
            }

        }
    }
}
