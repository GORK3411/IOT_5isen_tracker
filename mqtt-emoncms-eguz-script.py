import datetime
import urllib2
import time
import paho.mqtt.client as mqtt
import json
import base64
from datetime import datetime
import csv
from matplotlib.dates import DateFormatter
import matplotlib.pyplot as plt

mqtt_borker_address = "X.Y.Z.W"
mqtt_port = 1883
device_eui_list = []
device_name_list = ['eguz']

emonApiKey=" " # Remplacer par votre API code

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))

    # Subscribing in on_connect() means that if we lose the connection and
    # reconnect then subscriptions will be renewed.
    client.subscribe("application/2/node/XXXXXX/rx") # remplacer par votre node-id

def on_message(client, userdata, msg):
    print(msg.topic+" "+str(msg.payload))
    data = json.loads(msg.payload)
    devEUI = data['devEUI']
    nodeName = data['deviceName']

    if 'data' not in data.keys():
        return 0

    json_payload = base64.b64decode(data['data'])
    
    print json_payload
    # Prepare the URL 
    #url  ="http://emoncms.org/" +  "input/post.json?node="+ str(devEUI) + "&apikey=" + emonApiKey + "&json=" + urllib2.quote(json_txt)
    
    # Send the data to emoncms
    try:
        urllib2.urlopen(url)
        #print url
    except urllib2.URLError, e:
        print "Oops, timed out?"
    except socket.timeout:
        print "Timed out!"

client = mqtt.Client()
#client.username_pw_set("username", "password")
client.on_connect = on_connect
client.on_message = on_message

client.connect(mqtt_borker_address, mqtt_port, 60)

# Blocking call that processes network traffic, dispatches callbacks and
# handles reconnecting.
# Other loop*() functions are available that give a threaded interface and a
# manual interface.
client.loop_forever()
