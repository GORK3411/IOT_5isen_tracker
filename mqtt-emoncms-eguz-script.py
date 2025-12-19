import datetime
import urllib.request
import urllib.error
import urllib.parse
import time
import paho.mqtt.client as mqtt
import json
import base64
from datetime import datetime
import csv
from matplotlib.dates import DateFormatter
import matplotlib.pyplot as plt
import socket
import psycopg2

# -------------------------------
# PostgreSQL connection
# -------------------------------
conn = psycopg2.connect(
    host="localhost",     # ou l'IP du serveur PostgreSQL
    database="IoT",
    user="postgres",
    password="NoelNoel"
)
cursor = conn.cursor()


mqtt_borker_address = "212.98.137.194"
mqtt_port = 1883
device_eui_list = []
device_name_list = ['eguz']

#0x94, 0xBC, 0xBE, 0x5A, 0xDC, 0x1D, 0x50, 0x7B, 0x19, 0xAB, 0x79, 0x35, 0xD1, 0xF5, 0x41, 0xEB
emonApiKey="94bcbe5adc1d507b19ab7935d1f541eb" # Remplacer par votre API code

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))

    # Subscribing in on_connect() means that if we lose the connection and
    # reconnect then subscriptions will be renewed.
    #0x79, 0x7F, 0x80, 0x83, 0xFD, 0x28, 0x64, 0x5B
    client.subscribe("application/24/device/5b6428fd83807f79/rx") # remplacer par votre node-id

def on_message(client, userdata, msg):
    print(msg.topic+" "+str(msg.payload))
    data = json.loads(msg.payload)
    devEUI = data['devEUI']
    nodeName = data['deviceName']

    if 'data' not in data.keys() or data['data'] is None:
        return 0

    decoded = base64.b64decode(data['data']).decode('utf-8', errors='ignore')
    print(decoded)
    cursor.execute(
    "INSERT INTO sensor_data(payload) VALUES(%s)",
    (decoded,)
    )

        
    conn.commit()
    print("✅ Data inserted into PostgreSQL")
    """
    """
    #Partie pas obligatoire a faire , elle sert uniquement a envoyer les donne a EmonCMS un site web qui permet de visualier les données
    """
    #json_payload = base64.b64decode(data['data'])
    print(json_payload)
    json_payload = base64.b64decode(data['data'])
    json_txt = json_payload.decode("utf-8")
    # Prepare the URL 
    #url  ="http://emoncms.org/" +  "input/post.json?node="+ str(devEUI) + "&apikey=" + emonApiKey + "&json=" + urllib2.quote(json_txt) 
    url  ="http://emoncms.org/" +  "input/post.json?node="+ str(devEUI) + "&apikey=" + emonApiKey + "&json=" + urllib.parse.quote(json_txt)
    
    # Send the data to emoncms
    # Send the data to emoncms
    try:
        urllib.request.urlopen(url)
    except urllib.error.URLError as e:
        print("Oops, timed out?", e)

    except socket.timeout:
        print("Timed out!")

    """

    #Insert into database


client = mqtt.Client()
#client.username_pw_set("username", "password")
client.username_pw_set("user", "bonjour")
client.on_connect = on_connect
client.on_message = on_message

client.connect(mqtt_borker_address, mqtt_port, 60)

# Blocking call that processes network traffic, dispatches callbacks and
# handles reconnecting.
# Other loop*() functions are available that give a threaded interface and a
# manual interface.
client.loop_forever()
