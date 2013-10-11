{\rtf1\ansi\ansicpg1252\cocoartf1187\cocoasubrtf390
{\fonttbl\f0\fmodern\fcharset0 CourierNewPSMT;}
{\colortbl;\red255\green255\blue255;}
\margl1440\margr1440\vieww16300\viewh19300\viewkind0
\deftab720
\pard\pardeftab720

\f0\fs26 \cf0 CREATE TABLE User\
(\
ID int,\
Name varchar(40),\
Points int,\
Powerups int,\
Customizations int,\
Longitude int,\
Latitude int,\
Tagged int,\
Events int,\
Friends int\
);\
\
CREATE TABLE Event\
(\
ID int,\
Name varchar(40),\
Price 
\fs28 Decimal (19,4)
\fs26 ,\
Time 
\fs24 DATETIME
\fs26 ,\
Reward varchar(255),\
Longitude int,\
Latitude int,\
Attended int \
);\
\
\
CREATE TABLE Powerup\
(\
ID int,\
Name varchar(40),\
Price 
\fs28 Decimal (19,4)
\fs26 ,\
ImgUrl 
\fs24 varchar(255)
\fs26 ,\
Reward varchar(255),\
Length int \
);\
\
\
CREATE TABLE Customization\
(\
ID int,\
Name varchar(40),\
Price 
\fs28 Decimal (19,4)
\fs26 ,\
ImgUrl 
\fs24 varchar(255)
\fs26 ,\
Reward varchar(255),\
Edition varchar(40) \
);}