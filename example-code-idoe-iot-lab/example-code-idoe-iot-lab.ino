/*******************************************************************************
   Copyright (c) 2015 Thomas Telkamp and Matthijs Kooijman

   Permission is hereby granted, free of charge, to anyone
   obtaining a copy of this document and accompanying files,
   to do whatever they want with them without any restriction,
   including, but not limited to, copying, modification and redistribution.
   NO WARRANTY OF ANY KIND IS PROVIDED.

   This example sends a valid LoRaWAN packet with payload "Hello,
   world!", using frequency and encryption settings matching those of
   the (early prototype version of) The Things Network.

   Note: LoRaWAN per sub-band duty-cycle limitation is enforced (1% in g1,
    0.1% in g2).

   Change DEVADDR to a unique address!
   See http://thethingsnetwork.org/wiki/AddressSpace

   Do not forget to define the radio type correctly in config.h.

 *******************************************************************************/

#include <lmic.h>
#include <hal/hal.h>
#include <SPI.h>

#define LMIC_DEBUG_LEVEL 2

//Ports for ultrasonic
const int trigPin = 4;
const int echoPin = 5;

float duration, distance; 


// Change this!
// This should be in little endian format.
static const u1_t PROGMEM DEVEUI[8] = {0x79, 0x7F, 0x80, 0x83, 0xFD, 0x28, 0x64, 0x5B};
void os_getDevEui (u1_t* buf) {
  memcpy_P(buf, DEVEUI, 8);
}

// Change this!
static const u1_t PROGMEM APPKEY[16] = {0x94, 0xBC, 0xBE, 0x5A, 0xDC, 0x1D, 0x50, 0x7B, 0x19, 0xAB, 0x79, 0x35, 0xD1, 0xF5, 0x41, 0xEB};
void os_getDevKey (u1_t* buf) {
  memcpy_P(buf, APPKEY, 16);
}

static osjob_t sendjob;

// Schedule TX every this many seconds
const unsigned TX_INTERVAL = 7;

// Pin mapping
const lmic_pinmap lmic_pins = {
  .nss = 10,
  .rxtx = LMIC_UNUSED_PIN,
  .rst = 9,
  .dio = {2, 6, 7},
};

void onEvent (ev_t ev) {
  Serial.print(os_getTime());
  Serial.print(": ");
  switch (ev) {
    case EV_SCAN_TIMEOUT:
      Serial.println(F("EV_SCAN_TIMEOUT"));
      break;
    case EV_BEACON_FOUND:
      Serial.println(F("EV_BEACON_FOUND"));
      break;
    case EV_BEACON_MISSED:
      Serial.println(F("EV_BEACON_MISSED"));
      break;
    case EV_BEACON_TRACKED:
      Serial.println(F("EV_BEACON_TRACKED"));
      break;
    case EV_JOINING:
      Serial.println(F("EV_JOINING"));
      break;
    case EV_JOINED:
      Serial.println(F("EV_JOINED"));
      break;
    case EV_RFU1:
      Serial.println(F("EV_RFU1"));
      break;
    case EV_JOIN_FAILED:
      Serial.println(F("EV_JOIN_FAILED"));
      break;
    case EV_REJOIN_FAILED:
      Serial.println(F("EV_REJOIN_FAILED"));
      break;
      break;
    case EV_TXCOMPLETE:
      Serial.println(F("EV_TXCOMPLETE (includes waiting for RX windows)"));
      if (LMIC.dataLen) {
        // data received in rx slot after tx
        Serial.print(F("Data Received: "));
        Serial.write(LMIC.frame + LMIC.dataBeg, LMIC.dataLen);
        Serial.println();
      }
      // Schedule next transmission
      os_setTimedCallback(&sendjob, os_getTime() + sec2osticks(TX_INTERVAL), do_send);
      break;
    case EV_LOST_TSYNC:
      Serial.println(F("EV_LOST_TSYNC"));
      break;
    case EV_RESET:
      Serial.println(F("EV_RESET"));
      break;
    case EV_RXCOMPLETE:
      // data received in ping slot
      Serial.println(F("EV_RXCOMPLETE"));
      break;
    case EV_LINK_DEAD:
      Serial.println(F("EV_LINK_DEAD"));
      break;
    case EV_LINK_ALIVE:
      Serial.println(F("EV_LINK_ALIVE"));
      break;
    default:
      Serial.println(F("Unknown event"));
      break;
  }
}

byte buffer[52];

void do_send(osjob_t* j) {
  // Check if there is not a current TX/RX job running
  if (LMIC.opmode & OP_TXRXPEND) {
    Serial.println(F("OP_TXRXPEND, not sending"));
  } else {

   
    // Prepare upstream data transmission at the next possible time.
    //String message = "IoT is cool";
    getDistance();
    String message = String(distance);//"10";
    message.getBytes(buffer, message.length() + 1);
    Serial.println("Sending: " + message);
    LMIC_setTxData2(1, (uint8_t*) buffer, message.length() , 0);
    Serial.println(F("Packet queued"));
  }
  // Next TX is scheduled after TX_COMPLETE event.
}

// Leave this empty
static const u1_t PROGMEM APPEUI[8] = {};
void os_getArtEui (u1_t* buf) {
  memcpy_P(buf, APPEUI, 8);
}

void setup() {

  pinMode(trigPin, OUTPUT);  
	pinMode(echoPin, INPUT);  
  Serial.begin(115200);
  Serial.println(F("Starting"));

#ifdef VCC_ENABLE
  // For Pinoccio Scout boards
  pinMode(VCC_ENABLE, OUTPUT);
  digitalWrite(VCC_ENABLE, HIGH);
  delay(1000);
#endif

  // LMIC init
  os_init();
  // Reset the MAC state. Session and pending data transfers will be discarded.
  LMIC_reset();
  LMIC_setClockError(MAX_CLOCK_ERROR * 1 / 100);

  LMIC_setupChannel(0, 868100000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI);      // g-band
  LMIC_setupChannel(1, 868300000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);      // g-band
  LMIC_setupChannel(2, 868500000, DR_RANGE_MAP(DR_SF12, DR_SF7B),  BAND_CENTI);      // g-band
  LMIC_setupChannel(3, 867100000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI);      // g-band
  LMIC_setupChannel(4, 867300000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI);      // g-band
  LMIC_setupChannel(5, 867500000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI);      // g-band
  LMIC_setupChannel(6, 867700000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI);      // g-band
  LMIC_setupChannel(7, 867900000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI);      // g-band
  LMIC_setupChannel(8, 868800000, DR_RANGE_MAP(DR_FSK,  DR_FSK),  BAND_MILLI);      // g2-band

  // Disable link check validation
  LMIC_setLinkCheckMode(0);
//  LMIC_setDrTxpow(DR_SF7, 23);
//  LMIC_setDrTxpow(DR_SF8, 23);
//  LMIC_setDrTxpow(DR_SF9, 23);
//  LMIC_setDrTxpow(DR_SF10, 23);
//  LMIC_setDrTxpow(DR_SF11, 23);
//  LMIC_setDrTxpow(DR_SF12, 23);

  // Start job
  do_send(&sendjob);
}

void loop() {
  os_runloop_once();

}

void getDistance()
{
  digitalWrite(trigPin, LOW);  
	delayMicroseconds(2);  
	digitalWrite(trigPin, HIGH);  
	delayMicroseconds(10);  
	digitalWrite(trigPin, LOW); 
  duration = pulseIn(echoPin, HIGH); 
  distance = (duration*.0343)/2;
  Serial.print("Distance: ");  
	Serial.println(distance);  
	
}
