#include <WiFi.h>

const char* ssid = "MO";
const char* password = "ArabBlackDay";

// Static IP settings
IPAddress local_IP(192, 168, 137, 63);    // Desired IP address
IPAddress gateway(192, 168, 137, 1);      // Your router's IP address
IPAddress subnet(255, 255, 255, 0);       // Subnet mask

WiFiServer server(80);

const int sensorPin = A0;
int sensorValue = 0;
const int mosfetPin = 7;
const int threshold = 500;
bool sensorStatus = true;

void setup() {
    Serial.begin(115200);

    // Hier geef ik de arduino een static IP
    WiFi.config(local_IP, gateway, subnet);

    WiFi.begin(ssid, password);

    // Hier connect hij naar mijn hotspot
    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.print(".");
    }
    Serial.println("\nConnected to WiFi");

    // Om te testen laat de arduino ook nog de IP laten zien.
    Serial.print("IP Address: ");
    Serial.println(WiFi.localIP());

    pinMode(mosfetPin, OUTPUT);

    server.begin();
    Serial.println("Server started");
}

void loop() {
    WiFiClient client = server.available();
    // Hier leest hij de sensor pin
    sensorValue = analogRead(sensorPin);
    // Hier kijkt hij als de vochtigheid in de plant lager is dan de threshold en als de motor geactiveerd is
    if (sensorStatus) {
      if (sensorValue < threshold) {
          digitalWrite(mosfetPin, HIGH);  // Pomp aan
      } else {
          digitalWrite(mosfetPin, LOW); // Pomp uit
      }
    }

    // Als er iemand geconnect is voert hij dit verder uit.
    if (client) {
        Serial.println("Client connected.");

        // Hier wacht hij totdat de client wat data stuurt.
        while (client.connected() && !client.available()) {
            delay(1);
        }

        // Hier leest hij de request van de client (de app in onze geval)
        String request = client.readStringUntil('\r');
        Serial.println("Client request: " + request);
        client.flush();

        // Hier zet hij de sensor in een json format
        String jsonResponse = "{ \"sensor\": " + String(sensorValue) + " }";

        // Hier wordt de JSON data gepushed naar de server
        client.println("HTTP/1.1 200 OK");
        client.println("Content-Type: application/json");
        client.println("Connection: close");
        client.println();
        client.println(jsonResponse);

        Serial.println("Sent sensor data: " + jsonResponse);

        delay(1);
        
        // Hier stopt hij de connectie en begin hij opnieuw
        client.stop();
        Serial.println("Client disconnected.");
    }

    int userInput = Serial.parseInt();
    Menu(userInput);
}

void Menu(int userInput)
{
    // Hier regelt hij de aan- en uitfunctie van de motor
    switch (userInput) {
        case 1:
            if (sensorStatus) {
                sensorStatus = false; // Motorstatus uitzetten
                digitalWrite(mosfetPin, LOW); // Schakel de motor uit
                Serial.println("The water motor has been turned off!");
            } else {
                Serial.println("The motor is already off!");
            }
            break;

        case 2:
            if (!sensorStatus) {
                sensorStatus = true; // Motorstatus aanzetten
                if (sensorValue < threshold) {
                    digitalWrite(mosfetPin, HIGH); // Schakel de motor aan
                }
                Serial.println("The water motor has been turned on!");
            } else {
                Serial.println("The motor is already on!");
            }
            break;
    }
}
