document.addEventListener("DOMContentLoaded", function() {
    let video = document.getElementById('videoElement');
    let canvas = document.getElementById('canvasElement');
    let ctx = canvas.getContext('2d');
    
    navigator.mediaDevices.getUserMedia({ video: true })
        .then(function(stream) {
            video.srcObject = stream;
            video.play();
        })
        .catch(function(err) {
            console.log("Erro ao acessar a câmera: " + err);
        });

    video.addEventListener('canplay', function() {
        let tracker = new tracking.ObjectTracker('license_plate');
        tracker.setInitialScale(4);
        tracker.setStepSize(2);
        tracker.setEdgesDensity(0.1);

        tracking.track('#videoElement', tracker, { camera: true });

        tracker.on('track', function(event) {
            if (event.data.length === 0) {
               
                document.getElementById('plateText').textContent = 'Nenhuma placa identificada';
            } else {
                let plate = event.data[0];
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                ctx.strokeStyle = '#f00';
                ctx.lineWidth = 2;
                ctx.strokeRect(plate.x, plate.y, plate.width, plate.height);

                document.getElementById('plateText').textContent = plate.identifier;
            }
        });
    });
});
