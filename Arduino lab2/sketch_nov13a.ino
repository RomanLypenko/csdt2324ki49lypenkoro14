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

void loop() {
  if (Serial.available() > 0) {
    String jsonStr = Serial.readString();
    deserializeJson(jsonDoc, jsonStr);

    Serial.print("Parse JSON str: ");
    Serial.println(jsonStr);
    Serial.print("Parse JSON doc: ");
    Serial.println(jsonDoc.as<String>());

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

    bool timerEnabled = jsonDoc["ventilatorControl"]["fanTimer"]["enabled"];
    int durationSeconds = jsonDoc["ventilatorControl"]["fanTimer"]["durationSeconds"];


    if (timerEnabled) {
      if (millis() - lastFanToggleTime >= durationSeconds * 1000) {
        fanState = !fanState;  // Змінюємо стан вентилятора
        lastFanToggleTime = millis();
      }
    } else {
      if (currentHour == startHour && currentMinute == startMinute && currentSecond == startSecond) {
        fanState = true;
        lastFanToggleTime = millis();
      } else if (currentHour == endHour && currentMinute == endMinute && currentSecond == endSecond) {
        fanState = false;
        lastFanToggleTime = millis();
      }
    }

    fanState = jsonDoc["ventilatorControl"]["fanState"];


    int fanPower = jsonDoc["ventilatorControl"]["fanPower"];


    if (fanState) {
      analogWrite(fanPin, fanPower);
      Serial.println(fanPin);
      Serial.println(fanPower);
    } else {
      analogWrite(fanPin, 0);
    }
  }
}