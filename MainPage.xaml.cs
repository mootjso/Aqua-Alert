using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;

namespace Aqua_Alert
{
    public partial class MainPage : ContentPage
    {
        private double progressValue = 0;
        private readonly HttpClient client;
        private readonly int maxHeight = 300;

        public MainPage()
        {
            // Dit stond er al standaard
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            // Client gemaakt die door blijft kijken
            client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite) };

            StartSensorValueUpdates();
        }

        private async void StartSensorValueUpdates()
        {
            // Hier hebben wij een try method gebruikt zodat de app niet crashed maar in de background wel een error code terug stuurt indien het niet werkt.
            try
            {
                // Hier zet hij de IP-adres als string format in de url string variable
                string url = "http://192.168.137.63";
                // Hier wacht het totdat de site bereikbaar is en een connectie maakt.
                var responseStream = await client.GetStreamAsync(url);

                // Hier gebruikt hij de data in de url om het te lezen van een netwerkstream en dit in reader zet.
                // using is zodat we dat de geheugen van de apparaat vrij wordt gemaakt zodra we er mee klaar zijn.
                using (var reader = new StreamReader(responseStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var jsonLine = await reader.ReadLineAsync();
                        if (!string.IsNullOrEmpty(jsonLine))
                        {
                            // Parse JSON
                            // Dit gedeelte had ik met een beetje hulp van een vriend die Informatica studeert gemaakt.

                            // De JObject.Parse() zorgt er voor om een JSON-string om te zetten naar een Object waar je direct de sleutel waarde (hier is het sensor) uit te halen.
                            // Dit komt uit de bibliotheek Newtonsoft.JSON.
                            var json = JObject.Parse(jsonLine);
                            int sensorValue = (int)json["sensor"];

                            // Hier kijkt hij naar de progressValue en zorgt hij er voor dat het altijd tussen de 1.0 en 0.0 is.
                            // De sensorValue wordt verdeeld door de maximale waarden (1023)
                            progressValue = Math.Min(1.0, Math.Max(0.0, sensorValue / 1023.0));
                            // Door de Dispatcher.Dispatch(() zorgt er voor dat ik de UI interface kan bijwerken vanuit een achtergrond taak
                            // Hiermee heb ik ook hulp gekregen van mijn vriend.
                            Dispatcher.Dispatch(() =>
                            {
                                // Hier zet ik de int sensorValue naar een string en toon ik dit op het scherm
                                SensorValueLabel.Text = sensorValue.ToString();

                                UpdateWaterLevel(maxHeight);
                                // Hier gaat hij de plant image update aan de hoeveelheid water is.
                                // Dit replaced hij dan in de MainPage.xaml voor uiterlijk.
                                UpdatePlantImage();
                            });
                        }
                    }
                }
            }
            // Hier pakt hij de error en doet hij apart zetten.
            catch (Exception ex)
            {
                // Hier laat hij de error code zien waarom hij niet wilde connecten.
                Console.WriteLine("Error fetching sensor data: " + ex.Message);
                // Hier probeert hij opnieuw de site te benaderen door een reconnect.
                StartSensorValueUpdates();
            }
        }

        private void UpdateWaterLevel(int max)
        {
            // Hier zetten wij een maxHeight en dit wordt vervolgens gemenigvuldigd door de progressValue om aan te tonen hoe vochtig het is.
            // 300 * 0.5 = 150 bijvoorbeeld
            // Dus dan worden de pixels van de bar tot de helft gevuld
            var newHeight = max * progressValue;
            WaterLevel.HeightRequest = newHeight;
        }

        // Hier update hij de plant png met mate hoe hoog de water niveau is.
        private void UpdatePlantImage()
        {
            if (progressValue < 0.3)
            {
                PlantImage.Source = "plant_sad.png";
            }
            else if (progressValue >= 0.3 && progressValue < 0.5)
            {
                PlantImage.Source = "plant_neutral.png";
            }
            else
            {
                PlantImage.Source = "plant_happy.png";
            }
        }
    }
}
