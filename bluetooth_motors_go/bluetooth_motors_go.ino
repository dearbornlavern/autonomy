int vertical = 0;
int horizontal = 1;     
int motor1Speed = 10;
int motor2Speed = 11;
int motorLeft1 = 7;
int motorLeft2 = 8;
int motorRight1 = 12;
int motorRight2 = 13;
int MAX_SPD = 1023;
int MIN_T = 8;
int MAX_T = 900;
String rx;
int ps;
int att;
int med;
long int delta;

void setup() 
{
  pinMode(motor1Speed, OUTPUT);
  pinMode(motor2Speed, OUTPUT);
  pinMode(motorLeft1, OUTPUT);
  pinMode(motorLeft2, OUTPUT);
  pinMode(motorRight1, OUTPUT);
  pinMode(motorRight2, OUTPUT);
  Serial.begin(115200);
  ps = 0;
  att = 0;
  med = 0;
  delta = 0;
  //while (! Serial);
  //Serial.println("Speed 0 to 255");
}

void loop() 
{
   int pos_x;
   int pos_y;
   int spd; 
   int dir;   
   int headset = parseHeadset();
   
   if (ps == 200)
   {
       Serial.println("Mindwave skin contact not detected.");
   }
   if(ps != 200 && att >=45)// && ((pos_x >= MIN_T && pos_x <= MAX_T) && (pos_y >= MIN_T && pos_y <= MAX_T)))
   {
       dir = setDirection(500,5);
       spd = setSpd(dir,500,10);
       //Serial.print("dir=");Serial.println(dir);
       //Serial.println("Mindwave activity detected.");
       //delay(2000);
   }
   else
   {
       pos_x = analogRead(horizontal);
       pos_y = analogRead(vertical);
       dir = setDirection(pos_x, pos_y);
       spd = setSpd(dir, pos_x, pos_y);
       //Serial.println("Joystick Mode.");
    }
    //Serial.print("dir=");Serial.println(dir);

    analogWrite(motor1Speed,(spd / 4));
    analogWrite(motor2Speed,(spd / 4));
    delay(10);   
}

int parseHeadset()
{
  int val = 0;
  int pos[5];
  
  while(Serial.available())
  {
     rx += (char)Serial.read();    
  }
  if(rx.length())
  {
    //Serial.print("rx = ");Serial.println(rx);  //For debugging!
    String local;
    pos[0] = 0;
    pos[1] = rx.indexOf("%");
    pos[2] = rx.indexOf("^");
    pos[3] = rx.indexOf("&");
    pos[4] = rx.indexOf("$");
    
    //Parse out the poor signal parameter
    //Serial.print("pos[1] = ");Serial.println(pos[1]);
    if (pos[1] != -1)
    {
      for (int i = pos[1] + 1; i > 0 && rx[i] >= '0' && rx[i] <= '9';i++)
      {
         local += rx[i];
      }
      ps = local.toInt();
      local = "";  
    }
    Serial.print("signal quality = ");Serial.println(ps);
    
    if (ps == 200)
    {
       val = -1;
       //Serial.println("Mindwave skin contact not detected.");
    }
    
    //Parse out the attention section
    for (int i = pos[2] + 1; i > 0 && rx[i] >= '0' && rx[i] <= '9';i++)
    {
       local += rx[i];
    }
    att = local.toInt();
    Serial.print("attention      = ");Serial.println(att);
    local = "";
    
    //Parse out the meditation section
    for (int i = pos[3] + 1; i > 0 && rx[i] >= '0' && rx[i] <= '9';i++)
    {
       local += rx[i];
    }
    med = local.toInt();
    Serial.print("meditation     = ");Serial.println(med);
    local = "";
    
    //Parse out the delta section
    for (int i = pos[4] + 1; i > 0 && rx[i] >= '0' && rx[i] <= '9';i++)
    {
       local += rx[i];
    }
    delta = local.toInt();
    Serial.print("delta          = ");Serial.println(delta);
    
    local = "";
    Serial.println("");
    rx = "";
  }
  return val;
}


int setDirection(int x, int y)
{
  int dir;
  //Zero position
  if ((x >= MIN_T && x <= MAX_T) && (y >= MIN_T && y <= MAX_T))
  {
    //Serial.println("zero");
    digitalWrite(motorLeft1,LOW);
    digitalWrite(motorLeft2,LOW);
    digitalWrite(motorRight1,LOW);
    digitalWrite(motorRight2,LOW);  
    dir = 0;
  }
  //Joystick Up
  else if (y <= MIN_T && (x >= MIN_T && x <= MAX_T))
  {
    //Serial.println("forward");
    digitalWrite(motorLeft1,LOW);
    digitalWrite(motorLeft2,HIGH);
    digitalWrite(motorRight1,LOW);
    digitalWrite(motorRight2,HIGH);  
    dir = 1;
  }
  //Joystick Up-Right
  else if ((x >= MAX_T && y <= MIN_T))
  {
    //Serial.println("forward right");
    digitalWrite(motorLeft1,LOW);
    digitalWrite(motorLeft2,HIGH);
    digitalWrite(motorRight1,HIGH);
    digitalWrite(motorRight2,LOW);  
    dir = 2;
  }
  //Joystick Right
  else if ((x >= MAX_T && (y <= MAX_T && y >= MIN_T)))
  {
    //Serial.println("right");
    digitalWrite(motorLeft1,LOW);
    digitalWrite(motorLeft2,HIGH);
    digitalWrite(motorRight1,HIGH);
    digitalWrite(motorRight2,LOW);  
    dir = 3;
  }
  //Joystick Down-Right
  else if ((x >= MAX_T && y >= MAX_T))
  {
    //Serial.println("rev right");
    digitalWrite(motorLeft1,LOW);
    digitalWrite(motorLeft2,HIGH);
    digitalWrite(motorRight1,HIGH);
    digitalWrite(motorRight2,LOW);  
    dir = 4;
  }
  //Joystick Down
  else if (y >= MAX_T && (x >= MIN_T && x <= MAX_T))
  {
    //Serial.println("reverse");
    digitalWrite(motorLeft1,HIGH);
    digitalWrite(motorLeft2,LOW);
    digitalWrite(motorRight1,HIGH);
    digitalWrite(motorRight2,LOW);  
    dir = 5;
  }
  //Joystick Down-Left
  else if ((x <= MIN_T && y >= MAX_T))
  {
    //Serial.println("rev left");
    digitalWrite(motorLeft1,HIGH);
    digitalWrite(motorLeft2,LOW);
    digitalWrite(motorRight1,LOW);
    digitalWrite(motorRight2,HIGH); 
    dir = 6;
  }
  //Joystick Left
  else if ((x <= MIN_T && (y >= MIN_T && y <= MAX_T)))
  {
    //Serial.println("left");
    digitalWrite(motorLeft1,HIGH);
    digitalWrite(motorLeft2,LOW);
    digitalWrite(motorRight1,LOW);
    digitalWrite(motorRight2,HIGH);  
    dir = 7;
  }
  //Joystick Up-Left
  else if ((x <= MIN_T && y <= MIN_T))
  {
    //Serial.println("forward left");
    digitalWrite(motorLeft1,HIGH);
    digitalWrite(motorLeft2,LOW);
    digitalWrite(motorRight1,LOW);
    digitalWrite(motorRight2,HIGH);  
    dir = 8;
  }
  return dir;
}

int setSpd(int dir, int x, int y)
{
  int spd;
  switch(dir)
  {
    case 1:
    case 2:
    case 8:
      spd = MAX_SPD - y;
      break;
    case 4:
    case 5:
    case 6:
      spd = y;
      break;
    case 3:
      spd = x;
      break;
    case 7:
      spd = MAX_SPD - x;
      break;
    default:
      spd = 0;
      break;
  }
  return spd;
}
