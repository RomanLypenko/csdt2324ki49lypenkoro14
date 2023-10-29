String mySt="";
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  if(Serial.available()){
    mySt= Serial.readString();

    mySt.toUpperCase();
    Serial.println("Output from arduino:");
    Serial.println(mySt);
  }

  mySt=""; 
}