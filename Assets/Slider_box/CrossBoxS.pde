import processing.serial.*;
import controlP5.*;

import cc.arduino.*;

Arduino arduino;

ControlP5 cp5;


float sensorData =100 ; 


int myColor = color(0,0,0);

int sensorVal; 
int mapVal; 
int trianPos; 

Table register;
int freq = 100;  // Frequency of recording = 10 Hz (once every 100 ms)

String pctimestamp; 
String timestamp ; 
boolean timeflag = false; 
int timer;

void setup() {
  size(800,800);
  noStroke();
 
  
  cp5 = new ControlP5(this);
  
  // create a toggle
  cp5.addButton("Record")
     .setPosition(100, 600)
     .setSize(80,20)
     ;  
  
  // create a toggle
  cp5.addButton("Save")
     .setPosition(200, 600)
     .setSize(80,20)
     ;  

  pctimestamp= str(year())+ '-' + str(month()) + '-' + str(day()) + '-' + str(hour()) + '-' + str(minute());
  
  register = new Table();
  
  register.addColumn("timestamp");
  register.addColumn("raw");
  register.addColumn("value");
  
  
   
  // Prints out the available serial ports.
  println(Arduino.list());
  
  // Modify this line, by changing the "0" to the index of the serial
  // port corresponding to your Arduino board (as it appears in the list
  // printed by the line above).
  arduino = new Arduino(this, Arduino.list()[4], 57600);
  arduino.pinMode(0, Arduino.OUTPUT);
}

void draw() {
  background(0);
  
  textSize(14);
  fill(255);
  text("SLIDER POSITION RECORDING", 100, 100); 
  
  fill(0,45,90);
  rect(100,200,500,25);
  
  
  sensorData = arduino.analogRead(0);
  mapVal= int(map(sensorData, 0, 1023, 0, 100));
  trianPos = int(map(sensorData, 0, 1023, 100, 600));
 
  fill(0,170,255);
  triangle(trianPos-5,225, trianPos+5, 225, trianPos, 200);
 
  println( "Normalized value: "+ mapVal + "  Raw value: " + sensorData);

  if(timeflag == true){
    if (millis() - timer >= freq) {
        TableRow newRow = register.addRow();
        timestamp= str(year())+'-'+str(month())+'-'+str(day())+'-'+ str(hour())+'-'+str(minute())+'-'+str(second())+'-'+str(millis()); 
        newRow.setString("timestamp", timestamp);
        newRow.setFloat("raw", sensorData);
        newRow.setFloat("value", mapVal);
        
        timer = millis();
        
        textSize(10);
        fill(255);
        text("RECORDING DATA", 100, 700);       
    }
  }
}

public void Record() {
  println("Recording Started");  
  timeflag = true; 
  timer = millis();
}

public void Save( ) {
  println("Save File");
  
  timeflag=false; 
  
  timestamp= str(year())+'-'+str(month())+'-'+str(day())+'-'+ str(hour())+'-'+str(minute())+'-'+str(second()); 

  saveTable(register, "data/" + timestamp + ".csv");
  
  
  textSize(10);
  fill(255);
  text("FILE SAVED", 100, 700); 
  
  register.clearRows();
  delay(4000);

}
