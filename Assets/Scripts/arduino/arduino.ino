#include<Wire.h>
#include<Arduino.h>

const int MPU=0x68;  //Default I2C memory address for MPU 6050
int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;

void setup(){
  Wire.begin();      //Initialize Wire library with no address parameter, making arduino as master of I2C communication. (MPU6050 as slave device)
  Wire.beginTransmission(MPU); //Start transmitting data to MPU
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     //MPU-6050 시작 모드로
  Wire.endTransmission(true); 
  Serial.begin(9600);
}

void loop(){
  Wire.beginTransmission(MPU);    //데이터 전송시작
  Wire.write(0x3B);               // register 0x3B (ACCEL_XOUT_H), 큐에 데이터 기록
  Wire.endTransmission(false);    //연결유지
  Wire.requestFrom(MPU,14,true);  //MPU에 데이터 요청
  //데이터 한 바이트 씩 읽어서 반환
  AcX=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  AcY=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  Tmp=Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  GyX=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  GyY=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  GyZ=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
  
  //시리얼 모니터에 출력
  Serial.print("GyX="); Serial.print(GyX);
  Serial.print("|GyY="); Serial.print(GyY);
  Serial.print("|GyZ="); Serial.print(GyZ);
  Serial.print("|AcX="); Serial.print(AcX);
  Serial.print("|AcY="); Serial.print(AcY);
  Serial.print("|AcZ="); Serial.print(AcZ);
  Serial.print("|Tmp="); Serial.println(Tmp/340.00+36.53);  
  delay(333);
}
