 
const int MPUADDR=0x68; // MPU6050 I2C주소
int AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;
 
void setup()
{
  Serial.begin(9600);
  
  Wire.begin();
  Wire.beginTransmission(MPUADDR);
  Wire.write(0x6B); // PWR_MGMT_1 레지스터
  Wire.write(0); // Wakes Up
  Wire.endTransmission(true);
}
 
void loop()
{
  Wire.beginTransmission(MPUADDR);
  Wire.write(0x3B); // ACCEL_XOUT_H 레지스터
  Wire.endTransmission(false);
  Wire.requestFrom(MPUADDR,14,true);// 0x3B로부터 14byte의 데이터를 요청
  // High 8비트와 Low 8비트를 합쳐 16비트 값으로 각각의 값 저장
  AcX = Wire.read() << 8 | Wire.read(); // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  AcY = Wire.read() << 8 | Wire.read(); // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ = Wire.read() << 8 | Wire.read(); // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  Tmp = Wire.read() << 8 | Wire.read(); // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  GyX = Wire.read() << 8 | Wire.read(); // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  GyY = Wire.read() << 8 | Wire.read(); // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  GyZ = Wire.read() << 8 | Wire.read(); // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
 
  //시리얼 센서값 출력
  Serial.print(AcX);
  Serial.print("");
  Serial.print(AcY);
  Serial.print("");
  Serial.print(AcZ);
  Serial.println();
  delay(300);
}
