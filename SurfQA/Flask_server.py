from flask import Flask, request, Response, make_response
from flask import send_from_directory

import jsonpickle, json
import os, io, cv2, base64
import numpy as np

from SurfQA import SurfQA

# Deep learning model for defects detection
model_type    = "YOLOv8"
model_weights  ="best.pt"
model = SurfQA(model_type=model_type, model_weights=model_weights)

# Initialize the Flask application
app = Flask(__name__)

@app.route('/SurfQA_module', methods=['GET', 'POST'])
def SurfQA_module():
    # decode from base64 to cv2
    cv2_image_base64 = np.fromstring(request.data, dtype=np.uint8)
    img_decoded       = base64.b64decode(cv2_image_base64)
    img_decoded_as_np = np.frombuffer(img_decoded , dtype=np.uint8)
    img_decoded_cv2   = cv2.imdecode(img_decoded_as_np, flags=1)
    # inference 
    model.inference(img_decoded_cv2)
    # annotated img
    img = model.get_image_with_annotated_resutls()
    cv2.imwrite('img_annotated.jpg', img)
    # return predictions in json format
    classes  = model.get_class_names_as_list()
    bboxes   = model.get_bounding_boxes_as_list()
    rez_dict = {
        "bboxes"  : bboxes,
        "classes" : classes
    }
    json_object = json.dumps([rez_dict]) 
    print(json_object)
    return json_object

app.run(debug = True, host = "localhost", port = 3000)