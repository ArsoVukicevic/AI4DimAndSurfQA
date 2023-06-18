# SurfQA - surface inspection module

Client.py - a python script containing demos for calling SurfQA as a service and in the stand-alone python app

Flask_server.py - a python script containing the flask demo code for calling surface detection as a web-service

SurfQA.py - the class containing the code for surface defects detection




Notes: 
* model weights "best.pt" are available here: https://drive.google.com/file/d/1vlunbov-4zywAdXJzEwJnUspf0sr9Y-9/view?usp=sharing 
* we trained the ultralytics yolov8 model - documentaion is available on the official repository https://github.com/ultralytics/ultralytics
* test_img.jpg - test image containing various types of surface defects

# Short guide how to run it on your side

Step #1 - Download this repository

Step #2 - Download "best.pt" weights of the pretrained model

Step #3 - Run Client.py

```Python
# read cv2 image
cv2_image = cv2.imread("test_img.jpg")

# testing on client side
if True:
    # load model
    from SurfQA import SurfQA
    model_type    = "YOLOv8"
    model_weights = "best.pt"
    model         = SurfQA(model_type=model_type, model_weights=model_weights)
    # inference 
    model.inference(cv2_image)
    # get annoated image
    img = model.get_image_with_annotated_resutls()
    cv2.imwrite('img_annotated.jpg', img)

# testing on server-side
if True:    
    # prepare image 
    cv2_image_base64  = base64.b64encode(cv2.imencode('.jpg', cv2_image)[1]).decode()
    img_decoded       = base64.b64decode(cv2_image_base64)
    img_decoded_as_np = np.frombuffer(img_decoded , dtype=np.uint8)
    img_decoded_cv2   = cv2.imdecode(img_decoded_as_np, flags=1)
    # send on web service
    end_point         = "http://localhost:3000//SurfQA_module"
    response          = requests.post(end_point, data=cv2_image_base64)
    # convert reponse to python dict
    response_dict     = json.loads(response.text)
    print(response_dict[0])
```
Step #4a - To run Client.py as a web service - run also Flask_server.py 

Step #4b - To run Client.py as a standalone app, change flag variable into it

# Requirements
install the following list of packages
- requests==2.24.0
- json5==0.8.5
- jsonpickle==1.4.1
- opencv-python==4.1.2.30
- numpy==1.19.1
- Flask==1.1.4
- ultralytics==8.0.117
