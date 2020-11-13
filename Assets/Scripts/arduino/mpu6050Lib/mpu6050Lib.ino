#include <MPU6050_tockn.h>
#include <Wire.h>

MPU6050 mpu6050(Wire);

long timer = 0;

void setup() {
  Serial.begin(9600);
  Wire.begin();
  mpu6050.begin();
  mpu6050.calcGyroOffsets(true);
}

void loop() {
  mpu6050.update();

  if(millis() - timer > 1000){
    //시리얼 모니터에 출력
    Serial.print("GyX="); Serial.print(mpu6050.getGyroX());
    Serial.print("|GyY="); Serial.print(mpu6050.getGyroY()Y);
    Serial.print("|GyZ="); Serial.print(mpu6050.getGyroZ()yZ);
    
    Serial.print("|AcX="); Serial.print(mpu6050.getAccX();
    Serial.print("|AcY="); Serial.print(mpu6050.getAccY();
    Serial.print("|AcZ="); Serial.print(mpu6050.getAccZ();
    
    Serial.print("|Tmp="); Serial.println(mpu6050.getTemp();
  
    Serial.print("|accAngleX=");Serial.print(mpu6050.getAccAngleX());
    Serial.print("|accAngleY=");Serial.println(mpu6050.getAccAngleY());
    
    Serial.print("|gyroAngleX=");Serial.print(mpu6050.getGyroAngleX());
    Serial.print("|gyroAngleY=");Serial.print(mpu6050.getGyroAngleY());
    Serial.print("|gyroAngleZ=");Serial.println(mpu6050.getGyroAngleZ());
    
    Serial.print("|angleX=");Serial.print(mpu6050.getAngleX());
    Serial.print("|angleY=");Serial.print(mpu6050.getAngleY());
    Serial.print("|angleZ=");Serial.println(mpu6050.getAngleZ());
    timer = millis();
    
  }

}
