#include <ArduinoJson.h>
#include <TimeLib.h>

const int fanPin = 9;
bool fanState = false;
unsigned long lastFanToggleTime = 0;
StaticJsonDocument<200> jsonDoc;

void setup() {
  Serial.begin(9600);
  pinMode(fanPin, OUTPUT);
}

///
/// Toggles the state of the fan based on the provided external boolean value 
/// and updates the last fan toggle time.
///
void toggleFanState(bool externalBool) {
    fanState = externalBool;
    lastFanToggleTime = millis();
}

///
/// Checks if the current time falls within the specified fan schedule.
///
bool isFanScheduledTime() {
    int currentHour = hour();
    int currentMinute = minute();
    int currentSecond = second();

    String startTime = jsonDoc["ventilatorControl"]["fanSchedule"]["startTime"].as<String>();
    int startHour = startTime.substring(0, 2).toInt();
    int startMinute = startTime.substring(3, 5).toInt();
    int startSecond = startTime.substring(6).toInt();

    String endTime = jsonDoc["ventilatorControl"]["fanSchedule"]["endTime"].as<String>();
    int endHour = endTime.substring(0, 2).toInt();
    int endMinute = endTime.substring(3, 5).toInt();
    int endSecond = endTime.substring(6).toInt();

    return (currentHour == startHour && currentMinute == startMinute && currentSecond == startSecond) ||
           (currentHour == endHour && currentMinute == endMinute && currentSecond == endSecond);
}
///
/// Checks if the fan timer has expired based on the configured duration.
///
bool isFanTimerExpired() {
    bool timerEnabled = jsonDoc["ventilatorControl"]["fanTimer"]["enabled"];
    int durationSeconds = jsonDoc["ventilatorControl"]["fanTimer"]["durationSeconds"];

    return timerEnabled && (millis() - lastFanToggleTime >= durationSeconds * 1000);
}
///
/// Processes a JSON string, updating the fan state based on schedule and timer configurations.
///
void processJsonString(const String& jsonStr) {
    deserializeJson(jsonDoc, jsonStr);

    // Check fan schedule and timer
    if (isFanScheduledTime() || isFanTimerExpired()) {
        toggleFanState(!fanState);
    }

    fanState = jsonDoc["ventilatorControl"]["fanState"];
    int fanPower = jsonDoc["ventilatorControl"]["fanPower"];

    // Applying fan state and power
    if (fanState) {
      analogWrite(fanPin, fanPower);
    } else {
      analogWrite(fanPin, 0);
    }
}

void loop() {
  if (Serial.available() > 0) {
    String jsonStr = Serial.readString();

    Serial.print("Parse JSON str: ");
    Serial.println(jsonStr);
    Serial.print("Parse JSON doc: ");
    Serial.println(jsonDoc.as<String>());

    // Process JSON string
    processJsonString(jsonStr);
  }
}
