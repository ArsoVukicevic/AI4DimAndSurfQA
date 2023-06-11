import cv2

class SurfQA:
    # Deep learning model for defects detection, segmentation or instance segmentation
    model         = None 
    model_type    = None
    model_weights = None
    # image info
    img        = None
    img_path   = None
    # infernece results
    results    = None
    # class constructor
    def __init__(self, model_type=None, model_weights=None):
        if model_type is not None:
            self.set_model_type( model_type)
        if model_weights is not None:
            self.set_model_weights(model_weights)

    def set_model_type(self, model_type='YOLOv8'):
        if model_type == 'YOLOv8':            
            self.model_type = str(model_type)
        if model_type == 'Your model':
            print("add code for your model here")
    
    def set_model_weights(self, model_weights):
        if self.model_type == 'YOLOv8':
            from ultralytics import YOLO
            self.model_weights = model_weights
            self.model         = YOLO(model_weights)

    # set image for inference
    def set_inference_image(self, input_image):
        # if image path (string) is passed - read it
        if type(input_image) is type(str):
            self.img = cv2.imread(input_image)
        # else cv2 image is passed 
        else:
            self.img = input_image.copy()

    def inference(self, input_image=None):
        # if None = image is already read 
        if input_image is not None:
            self.set_inference_image(input_image)
        self.results = self.model.predict(source=self.img)
    
    def get_bounding_boxes(self):
        return self.results[0].boxes.cpu().numpy().xyxy.astype(int)
    
    def get_bounding_boxes_as_list(self):
        rez_bounding_box = self.get_bounding_boxes()
        bboxes = []
        for i in range(len(rez_bounding_box)):
            bboxes.append([int(rez_bounding_box[i][0]), int(rez_bounding_box[i][1]), int(rez_bounding_box[i][2]-rez_bounding_box[i][0]), int(rez_bounding_box[i][3]-rez_bounding_box[i][1])])
        return bboxes

    def get_class_names(self):
        return self.results[0].boxes.cpu().numpy().cls.astype(int)

    def get_class_names_as_list(self):
        rez_bounding_box = self.get_class_names()
        classes = []
        for i in range(len(rez_bounding_box)):
            classes.append([int(rez_bounding_box[i])])
        return classes

    def get_image_with_annotated_resutls(self):
        rez_bounding_box = self.get_bounding_boxes()
        rez_class_names  = self.get_class_names()
        cv2_image = self.img.copy()
        for i in range(len(rez_class_names)):
            # bbox
            start_point = (int(rez_bounding_box[i][0]), int(rez_bounding_box[i][1]))
            end_point   = (int(rez_bounding_box[i][2]), int(rez_bounding_box[i][3]))
            color       = (255, 0, 0)
            thickness   = 2
            cv2_image   = cv2.rectangle(cv2_image, start_point, end_point, color, thickness)
            # text
            fontScale   = 1
            txt         = str(rez_class_names[i])
            org         = (int(rez_bounding_box[i][0]), int(rez_bounding_box[i][3]))
            font        = cv2.FONT_HERSHEY_SIMPLEX
            cv2_image   = cv2.putText(cv2_image, txt, org, font,  fontScale, color, thickness, cv2.LINE_AA)
        return cv2_image