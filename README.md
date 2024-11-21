# Aqua Alert

## Description
**Aqua Alert** is an intuitive plant monitoring application designed to help users maintain optimal moisture levels for their plants.  
It integrates sensor data with a user-friendly interface to display water levels and plant health, ensuring your greenery thrives.

---

## Directory Structure
1. **AquaAlert Folder**:  
   This directory contains all the essential `.cs`, `.xaml`, and JSON files required to run the application.
2. **AquaAlertTest Folder**:  
   Reserved for Unit Tests to ensure application stability and correctness.

---

## Essential Files
To ensure the application functions correctly, make sure the following files are present:

1. **AquaAlert > Datasources > SensorData**  
   Stores simulated or real-time sensor data received from your hardware device.

2. **AquaAlert > Resources > Images**  
   Contains plant images (`plant_sad.png`, `plant_neutral.png`, `plant_happy.png`) for visual updates based on water levels.

3. **AquaAlert > NetworkConfig**  
   Stores the network security configuration required for communication with external devices, such as Arduino sensors.

---

## Launching the Application
To start Aqua Alert, open the **AquaAlert.sln** solution file in your IDE.  
Ensure the hardware (Arduino or sensor device) is configured to transmit data to the IP address `http://192.168.137.63`.

---

## Default Configuration
### 1. Default Sensor Setup:
- The app is pre-configured to connect to a local device on `http://192.168.137.63`.
- Ensure the device is active and broadcasting JSON data.

### 2. Default Plant Images:
- Plant images (`plant_sad.png`, `plant_neutral.png`, `plant_happy.png`) dynamically update based on water levels.

### 3. Water Level Scaling:
- The default water level height is set to **300 pixels**, adjustable in the settings or via the `maxHeight` property in the code.

---

## Network Configuration
### 1. Access Network State:
The application requires the following permissions to function:
- `android.permission.ACCESS_NETWORK_STATE`
- `android.permission.INTERNET`

### 2. Cleartext Traffic:
The `network-security-config` file enables HTTP communication with local devices.  
It is pre-configured for the IP address `192.168.137.63`.

---

## Admin Functionality
### 1. Admin Access:
The application is controlled by default settings for water levels and thresholds.  
You can modify these values directly in the app or through the configuration files.

### 2. Adjusting Max Height:
Navigate to the settings menu or change the `maxHeight` property in **MainPage.xaml.cs**.

---

## Additional Information
### 1. UI Compatibility:
- Zoom adjustments may be necessary for optimal UI experience on certain devices.  
  Use `Ctrl + -` to zoom out if elements appear cut off.

### 2. Data Location:
- **Sensor Data**:  
  `AquaAlert\bin\Debug\net7.0\Datasources\SensorData`
- **Logs**:  
  `AquaAlert\bin\Debug\net7.0\Logs`

### 3. Debugging Notes:
- Ensure the sensor hardware is connected and broadcasting data to avoid errors during runtime.
- Use the admin interface to verify sensor connectivity and adjust thresholds.

---
# Aqua Alert - Arduino Code

## Description
This is the Arduino-based firmware for the **Aqua Alert** project.  
It collects real-time sensor data (e.g., soil moisture, temperature) and transmits it to the **Aqua Alert** mobile app.  
The firmware is designed to work seamlessly with the [Aqua Alert mobile app](https://github.com/mootjso/Aqua-Alert).

---

## Features
- Real-time data collection from sensors (e.g., soil moisture, temperature).
- Transmits data over Wi-Fi to the Aqua Alert app.
- Compatible with the Arduino Uno WiFi Rev2 or similar boards.

---

## Hardware Requirements
- **Arduino Board**: Arduino Uno WiFi Rev2 (or equivalent).
- **Sensors**:
  - Soil moisture sensor.
  - Optional: Temperature sensor (e.g., DS18B20).
- **Other Components**:
  - Breadboard.
  - Jumper wires.

---

## Directory Structure
1. **AquaAlert.ino**:  
   Main Arduino sketch file for running the Aqua Alert firmware.

---

## Configuration
Before uploading the code to your Arduino, ensure the following settings are correct in the **AquaAlert.ino** file:

### Wi-Fi Configuration
Update the Wi-Fi credentials in the code:

**Aqua Alert simplifies plant care by keeping you informed in real-time about the health of your greenery! 🌱**

```c++
#define WIFI_SSID "YourNetworkName"
#define WIFI_PASSWORD "YourNetworkPassword"

