import requests
import json
import cv2, base64
import numpy as np

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