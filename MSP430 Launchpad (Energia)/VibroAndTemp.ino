#include <dht.h>
/*
 * Библиотека DTH находится https://github.com/adafruit/DHT-sensor-library
 * В файле dth.cpp нужно истравить 50, 74 строчки на return 0
 * Удалить вызод функций на 110 и 140 строчках
 *
 * Напряжение питания датчиков +5В, но при +3,3 тоже работают
 */

#define DHTPIN    P1_4    // Датчик температуры
#define VIBROPIN  P1_7    // Датчик вибрации

#define DHTTYPE   DHT11   // DHT 11 
//#define DHTTYPE DHT22   // DHT 22  (AM2302)
//#define DHTTYPE DHT21   // DHT 21  (AM2301)

// Инициализируем структуру
DHT dht(DHTPIN, DHTTYPE);

float h,t;          // Значения влажности (h) и температуры (t)
int val;            // Оцифрованные значения с датчика вибрации

void setup() 
{
    Serial.begin(9600); 
    dht.begin();

    pinMode(RED_LED, OUTPUT);   
    pinMode(GREEN_LED, OUTPUT);  
    pinMode(PUSH2, INPUT_PULLUP);
}

void loop() 
{ 
    // Если нажата кнопка User, зажигам зеленый светодиод, считываем температуру и влажность
    if(digitalRead(PUSH2) == HIGH)    
    {
        digitalWrite(RED_LED, LOW);
        digitalWrite(GREEN_LED, HIGH);
        h = dht.readHumidity();
        t = dht.readTemperature();
        Serial.print("TEMP");
        Serial.print(h);
        Serial.println(t); 
    }
    // Иначе зажигаем красный светодиод и считываем значения с порта А7 (P1_7)
    else
    {
        digitalWrite(RED_LED, HIGH);
        digitalWrite(GREEN_LED, LOW);
        val=analogRead(VIBROPIN);
        
        // Для более качественных результатов, берем серднеарифметическое значение 4 замеров
        for(int i = 0; i < 4; i++)
        {
           val+=analogRead(VIBROPIN);
        }

        val = val / 4;
        Serial.println(val,DEC);
    }  
}

