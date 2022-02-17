/*
 * This ESP32 code is created by esp32io.com
 *
 * This ESP32 code is released in the public domain
 *
 * For more detail (instruction and wiring diagram), visit https://esp32io.com/tutorials/esp32-button-toggle-led
 */

#include <Arduino.h>
#include <ezButton.h>

#define BUTTON_PIN 16  // ESP32 pin GIOP16, which connected to button
#define LED_PIN    32  // ESP32 pin GIOP32, which connected to led

ezButton button(BUTTON_PIN);

// The below are variables, which can be changed
int led_state = LOW;    // the current state of LED
int button_state;       // the current state of button
int last_button_state;  // the previous state of button
int incomingByte = 48;    // byte 0

void setup() {
  Serial.begin(9600);                // initialize serial
//  pinMode(BUTTON_PIN, INPUT_PULLUP); // set ESP32 pin to input pull-up mode
  pinMode(LED_PIN, OUTPUT);          // set ESP32 pin to output mode

  button.setDebounceTime(50);
  button_state = digitalRead(BUTTON_PIN);
}

void loop() {
    button.loop(); // MUST call the loop() function first

    if (button.isPressed()) {
      Serial.println("The button is pressed");
    }
  //   // toggle state of LED
  //   led_state = !led_state;

  //   // control LED arccoding to the toggled state
  //   digitalWrite(LED_PIN, led_state);

  if (Serial.available() > 0) {
      // read the incoming byte:
      incomingByte = Serial.read();

      // say what you got:
      if (incomingByte == 49) {
        led_state = HIGH;
      } else {
        led_state = LOW;
      }
      digitalWrite(LED_PIN, led_state);
  }
}

