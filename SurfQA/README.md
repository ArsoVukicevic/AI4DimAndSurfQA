# SurfQA - surface inspection module

Client.py - a python script containing demos for calling SurfQA as a service and in the stand-alone python app

Flask_server.py - a python script containing the flask demo code for calling surface detection as a web-service

SurfQA.py - the class containing the code for surface defects detection




Notes: 
* model weights "best.pt" are available here: https://drive.google.com/file/d/1vlunbov-4zywAdXJzEwJnUspf0sr9Y-9/view?usp=sharing 
* we trained the ultralytics yolov8 model - documentaion is available on the official repository https://github.com/ultralytics/ultralytics

# Shor guide how to run it on your side

Step #1 - Download this repository
Step #2 - Run Client.py
Step #3 - Download "best.pt" weights of the pretrained model
Step #4a - To run Client.py as a web service - run also Flask_server.py 
Step #4b - To run Client.py as a standalone app, change flag variable into it
