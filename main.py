import json

import pyautogui
import time
import os
import socket
import datetime
import win32gui
from mongoengine import *
from pynput.keyboard import Listener
import configparser
import requests
from collections import Counter
#Copy pythoncom25.dll & pywintypes25.dll to c:\windows\system32 folder (you might need to change to version&system path)


class MousePositions(Document):
    x_pos = IntField()
    y_pos = IntField()


class Record(Document):
    start_time = DateTimeField(default=datetime.datetime.utcnow(), required=True)
    end_time = DateTimeField(default=datetime.datetime.utcnow(), required=True)
    user_id = StringField()
    user_ip = StringField()
    screen_size = StringField()
    windows = DictField()
    inactivity_score = IntField()
    activity_score = IntField()


def check_active_window():
    if w.GetWindowText(w.GetForegroundWindow()) is not None:
        return True
    return False


def get_mouse_info():
    x, y = pyautogui.position()
    print(x, y, datetime.datetime.utcnow(), w.GetWindowText(w.GetForegroundWindow()), end="")
    if check_active_window():
        return MousePositions(x_pos=x, y_pos=y)
    return MousePositions(x_pos=x, y_pos=y)


input_inactivity_score = 0
activity_score = 0
def on_release(key):
    global input_inactivity_score

    if hasattr(key, 'char'):
        input_inactivity_score = 0
    else:
        input_inactivity_score = 0

path = os.path.expanduser('~') + "\\.rekapp-agent"

configFilePath = path + r'\rekapp.conf'

config = configparser.ConfigParser()
config.read(configFilePath)

if 'DEFAULT' in config:
    configs = config['DEFAULT']
    server_ip = configs['ReceiverIP']
    receiver_port = configs['ReceiverPort']
    package_capture_time = configs['PackageCaptureTime']
    package_capture_interval = configs['PackageCaptureInterval']
    inactivity_threshold = configs['InactivityThreshold']

    print(server_ip, receiver_port, package_capture_time, package_capture_interval)
else:
    print("Log fail")


def post_package(api, json):
    response = requests.post(api, json=json)
    if response.status_code == 200:
        print("sucessfully fetched the data with parameters provided")
        print(response.json())
    else:
        print(f"Hello person, there's a {response.status_code} error with your request")


w = win32gui

# Check whether the specified path exists or not
dir_exists = os.path.exists(path)
if not dir_exists:
    # Create a new directory because it does not exist
    os.makedirs(path)

while True:
    _start_time = datetime.datetime.utcnow()
    window_inactivity_score = 0
    total_inactivity_score = 0
    windows_object = {}
    #packet_object = Record(windows=windows_object)
    # Gets user login name
    _user_id = os.getlogin()
    # Gets ip address
    hostname = socket.gethostname()
    ip_address = socket.gethostbyname(hostname)

    pos_array = []
    for j in range(int(package_capture_time)):
        with Listener(on_release=on_release) as listener:

            previous_window_registered = w.GetWindowText(w.GetForegroundWindow())
            previous_mouse_position = get_mouse_info()
            print(str(j) + " previous_window_registered:" + previous_window_registered + " - previous_mouse_position:" + str(previous_mouse_position.x_pos) + " " + str(previous_mouse_position.y_pos))
            #packet_object.windows_object.name = previous_window_registered


            time.sleep(int(package_capture_interval)) #########################################
            pos_array.append(previous_window_registered)

            if w.GetWindowText(w.GetForegroundWindow()) != previous_window_registered:
                window_inactivity_score = 0
            window_inactivity_score += 1

            tmp_x_pos = get_mouse_info().x_pos
            tmp_y_pos = get_mouse_info().y_pos
            if tmp_x_pos != previous_mouse_position.x_pos and tmp_y_pos != previous_mouse_position.y_pos:
                input_inactivity_score = 0
                activity_score += 1
            input_inactivity_score += 1

            print(" window_inactivity_score:" + str(window_inactivity_score) + "\n- input_inactivity_score:" + str(input_inactivity_score) + " " + str(tmp_x_pos) + " " + str(tmp_y_pos))
            print()
            if input_inactivity_score >= int(inactivity_threshold) and window_inactivity_score >= int(inactivity_threshold):
                #if total_inactivity_score == 0:
                #    total_inactivity_score = inactivity_threshold
                total_inactivity_score += 1
            listener.stop()
            listener.join()
    windows_object = Counter(pos_array)
    #print(windows_object)
    _end_time = datetime.datetime.utcnow()
    packet_object = Record(start_time=_start_time, end_time=_end_time, user_id=_user_id, user_ip=ip_address,
                               screen_size=str(pyautogui.size()), inactivity_score=total_inactivity_score, windows=windows_object, activity_score=activity_score)
    input_inactivity_score = 0
    activity_score = 0
    pos_array = []

    post_object = dict(json.loads(packet_object.to_json()))
    packet_filepath = path + '\\' + str(time.mktime(datetime.datetime.now().timetuple()) * 1000)
    f = open(packet_filepath, "w")
    f.write(str(packet_object.to_json()))
    post_package("http://" + server_ip + ":" + receiver_port + "/write", post_object)
    f.close()

