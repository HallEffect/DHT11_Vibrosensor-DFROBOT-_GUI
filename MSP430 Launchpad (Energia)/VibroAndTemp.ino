// Example testing sketch for various DHT humidity/temperature sensors
// Written by ladyada, public domain

#include <dht.h>

#define DHTPIN P1_4     // what pin we're connected to

// Uncomment whatever type you're using!
#define DHTTYPE DHT11   // DHT 11 
//#define DHTTYPE DHT22   // DHT 22  (AM2302)
//#define DHTTYPE DHT21   // DHT 21 (AM2301)

// Connect pin 1 (on the left) of the sensor to +5V
// Connect pin 2 of the sensor to whatever your DHTPIN is
// Connect pin 4 (on the right) of the sensor to GROUND
// Connect a 10K resistor from pin 2 (data) to pin 1 (power) of the sensor

DHT dht(DHTPIN, DHTTYPE);
//DHT sensor = DHT();
int x=0;
float h,t,flag=1;
int val;
uint8_t volatile edge;

void setup() {
  Serial.begin(9600); 
  dht.begin();
// pinMode(PUSH2, INPUT);    
pinMode(RED_LED, OUTPUT);   
pinMode(GREEN_LED, OUTPUT);  
pinMode(PUSH2, INPUT_PULLUP);
  //digitalWrite(RED_LED, LOW);
  
  //attachInterrupt(PUSH2, InterFlag,FALLING);
}

/*
void InterFlag()
{ 
if(edge == FALLING) {
    attachInterrupt(PUSH2, InterFlag, RISING); 
    edge = RISING;
  } else {
    attachInterrupt(PUSH2, InterFlag, FALLING);
    edge = FALLING;
  }  
  
flag=!flag;
Serial.println("11111111");
}
*/
void loop() 
{ 
  

  if(digitalRead(PUSH2)==!HIGH)
  {
                digitalWrite(RED_LED, LOW);
                digitalWrite(GREEN_LED, HIGH);
              /*  if (Serial.available() > 0) 
                {     //если есть доступные данные
                      // считываем байт
                         x = Serial.read();                         
                         if (x==49)
                         {
                                h = dht.readHumidity();
                                Serial.println(h);                              
                                x=0;  
                        }
                        else if (x==50)
                        {
                                t = dht.readTemperature();  
                                if (isnan(h)) 
                                {
                                  Serial.println("Failed to read from DHT");
                                } 
                                else 
                                {
                                  Serial.println(t);
                                }
                                x=0;  
                        }
               }*/
               
              h = dht.readHumidity();
              t = dht.readTemperature();
          Serial.print("TEMP");
          Serial.print(h);
          Serial.println(t); 
    }
  else
  {
      digitalWrite(RED_LED, HIGH);
       digitalWrite(GREEN_LED, LOW);
      val=analogRead(A7);//Connect the sensor to analog pin 0

      for(int i=0;i<4;i++)
      {
       val+=analogRead(A7);
      }
      val=val/4;

      Serial.println(val,DEC);//
  }  
  
  /*
h = dht.readHumidity();
t = dht.readTemperature();
 val=analogRead(A7);
   Serial.print(val,DEC);//
Serial.print(h);
  Serial.println(t);
*/
}
