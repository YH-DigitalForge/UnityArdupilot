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

  if(millis() - timer > 5){
    /*
    0 ~ 2 : Angle X ~ Z
    3 ~ 5 : Acc X ~ Z
    6 ~ 8 : Gyro X ~ Z
    9 : Tmp
    10 ~ 11 : Acc Angle X ~ Y
    12 ~ 14 : Gyro Angle X ~ Z
    */
    Serial.print("[UnityArdupilot]");
    Serial.print("AngleX=");Serial.print(mpu6050.getAngleX());
    Serial.print("|AngleY=");Serial.print(mpu6050.getAngleY());
    Serial.print("|AngleZ=");Serial.print(mpu6050.getAngleZ());
    
    Serial.print("|AccX="); Serial.print(mpu6050.getAccX());
    Serial.print("|AccY="); Serial.print(mpu6050.getAccY());
    Serial.print("|AccZ="); Serial.print(mpu6050.getAccZ());
    
    Serial.print("|GyroX="); Serial.print(mpu6050.getGyroX());
    Serial.print("|GyroY="); Serial.print(mpu6050.getGyroY());
    Serial.print("|GyroZ="); Serial.print(mpu6050.getGyroZ());
    
    Serial.print("|Tmp="); Serial.print(mpu6050.getTemp());
    
    Serial.print("|AccAngleX=");Serial.print(mpu6050.getAccAngleX());
    Serial.print("|AccAngleY=");Serial.print(mpu6050.getAccAngleY());
    
    Serial.print("|GyroAngleX=");Serial.print(mpu6050.getGyroAngleX());
    Serial.print("|GyroAngleY=");Serial.print(mpu6050.getGyroAngleY());
    Serial.print("|GyroAngleZ=");Serial.println(mpu6050.getGyroAngleZ());
    timer = millis();
    
  }

}
