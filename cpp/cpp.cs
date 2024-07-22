#include <opencv2/opencv.hpp>

using namespace cv;

void setup() {
    Serial.begin(9600);

    VideoCapture cap(0);
    if (!cap.isOpened()) {
        Serial.println("Erro ao abrir a câmera!");
        while (true);
    }

//carrega o classificador em cascata para detecção (alocar dados)
    CascadeClassifier plateCascade;
    plateCascade.load("haarcascade_plate.xml");

    Mat frame;
    while (true) {
        cap.read(frame);

        if (!frame.empty()) {
            //escala de cinza para o rastreamento
            Mat gray;
            cvtColor(frame, gray, COLOR_BGR2GRAY);

            // detecta placas na imagem
            std::vector<Rect> plates;
            plateCascade.detectMultiScale(gray, plates, 1.1, 2, 0 | CASCADE_SCALE_IMAGE, Size(30, 30));

            if (plates.size() > 0) {
                // desenha um retângulo ao redor da primeira placa detectada
                Rect plateRect = plates[0];
                rectangle(frame, plateRect, Scalar(255, 0, 0), 2);
                // envia a identificação da placa para o monitor serial
                String plateID = "Placa: " + std::to_string(plateRect.x) + ", " + std::to_string(plateRect.y);
                Serial.println(plateID);
            }
            // mostra o frame com retângulo da placa
            imshow("Reconhecimento de Placa", frame);
        }

        if (waitKey(1) == 27) break; 
        }
}

void loop() {
}
