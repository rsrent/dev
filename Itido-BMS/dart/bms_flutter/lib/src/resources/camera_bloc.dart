import 'package:camera/camera.dart';

class CameraBloc {
  List<CameraDescription> cameras;
  Future<bool> cameraLoaded;
  CameraController primaryCamera;
  CameraController secondaryCamera;

  CameraBloc() {
    cameraLoaded = availableCameras().then<bool>((_cameras) async {
      cameras = _cameras;
      primaryCamera = CameraController(cameras[0], ResolutionPreset.medium);
      await primaryCamera.initialize();
      // if (cameras.length == 2) {
      //   secondaryCamera = CameraController(cameras[1], ResolutionPreset.medium);
      //   await secondaryCamera.initialize();
      // }
      return true;
    });
  }

  void dispose() {
    primaryCamera?.dispose();
    secondaryCamera?.dispose();
  }
}
