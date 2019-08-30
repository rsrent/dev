using Foundation;
using System;
using UIKit;
using AVFoundation;
using System.Threading.Tasks;
using System.Diagnostics;
using CoreMedia;
using CoreVideo;
using CoreFoundation;
using System.IO;

using System.Collections.Generic;

namespace ModuleLibraryiOS.Camera
{
    public partial class CameraController : UIViewController
    {
		AVCaptureSession session;
		AVCaptureDeviceInput deviceInput;
		AVCaptureVideoPreviewLayer videoPreviewLayer;
		NSUrl fileUrl;
		AVCaptureMovieFileOutput movieFileOutput;
		MyRecordingDelegate recordingDelegate;
		AVCaptureVideoDataOutput videoDataOutput;
		public AVCaptureAudioChannel audioChannel;
		AVCaptureAudioDataOutput audioOutput;
        AVCaptureStillImageOutput stillImageOutput;
		AVCaptureDevicePosition devicePosition = AVCaptureDevicePosition.Back;

		public bool front = false;
		bool recording = false;
		bool configured = false;

        Action<NSUrl> finishedRecordingAction;
        Action<NSData> pictureTakenAction;

        public static Dictionary<CameraAction, Action> StartCamera(UIView container, UIViewController viewController, UINavigationController NavigationController, Action<NSUrl> finishedRecording, Action<NSData> pictureTaken) {
			var chatStoryboard = UIStoryboard.FromName("Camera", null);
			var newView = chatStoryboard.InstantiateViewController("CameraController") as CameraController;

            if(container != null && viewController != null) {
				newView.View.Frame = container.Bounds;
				newView.WillMoveToParentViewController(viewController);

				container.AddSubview(newView.View);
				viewController.AddChildViewController(newView);
				newView.DidMoveToParentViewController(viewController);
			}
            else if (NavigationController != null) NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
			newView.parseInfo(finishedRecording, pictureTaken);

            var cameraActionDictionary = new Dictionary<CameraAction, Action>();
            cameraActionDictionary.Add(CameraAction.StartRecording, newView.StartRecording);
            cameraActionDictionary.Add(CameraAction.StopRecording, newView.StopRecording);
            cameraActionDictionary.Add(CameraAction.TakePicture, newView.TakePicture);
            cameraActionDictionary.Add(CameraAction.SwitchCamera, newView.SwitchCamera);
            return cameraActionDictionary;
        }

        public enum CameraAction {
            StartRecording, StopRecording, TakePicture, SwitchCamera
        }

        private void parseInfo(Action<NSUrl> finishedRecording, Action<NSData> pictureTaken) {
            this.finishedRecordingAction = finishedRecording;
            this.pictureTakenAction = pictureTaken;
        }

		public CameraController(IntPtr handle) : base(handle)
		{
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
            return UIInterfaceOrientationMask.Portrait;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            if(NavigationController != null) NavigationController.NavigationBar.Hidden = true;
            StartCamera();
		}

        async void StartCamera() {
            await AuthorizeCameraUse();
            SetupLiveCameraStream(devicePosition);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (NavigationController != null) NavigationController.NavigationBar.Hidden = false;
        }

		private async Task SetupLiveCameraStream(AVCaptureDevicePosition cameraPosition)
		{
			await Task.Delay(50);

			session = new AVCaptureSession();
			var device = GetCameraForOrientation(cameraPosition);
			if (!configured) ConfigureCameraForDevice(device);
			var camera = device;
			var mic = AVCaptureDevice.GetDefaultDevice(AVMediaType.Audio);
			if (camera == null || mic == null) throw new Exception("Can't find devices");

			deviceInput = AVCaptureDeviceInput.FromDevice(camera);
			var micInput = AVCaptureDeviceInput.FromDevice(mic);

			if (session.CanAddInput(deviceInput)) session.AddInput(deviceInput);
			if (session.CanAddInput(micInput)) session.AddInput(micInput);

			try { videoPreviewLayer.RemoveFromSuperLayer(); }
			catch (Exception exc) { Debug.WriteLine(exc.Message); }

			//var viewLayer = liveCameraStream.Layer;
			videoPreviewLayer = new AVCaptureVideoPreviewLayer(session) { Frame = this.View.Frame };
			liveCameraStream.Layer.AddSublayer(videoPreviewLayer);
            videoPreviewLayer.Orientation = AVCaptureVideoOrientation.Portrait;

			var filePath = Path.Combine(Path.GetTempPath(), "HEYYYYY :D " + DateTime.Now.ToString() + ".mov");
			fileUrl = NSUrl.FromFilename(filePath);





			/*
			var settings = new CVPixelBufferAttributes { PixelFormatType = CVPixelFormatType.CV32BGRA };
			using (videoDataOutput = new AVCaptureVideoDataOutput { WeakVideoSettings = settings.Dictionary })
			{
				SetSampleBufferDelegate();
				session.AddOutput(videoDataOutput);
			} 




			audioOutput = new AVCaptureAudioDataOutput();
			audioOutput.SetSampleBufferDelegateQueue(new audioDelegate(this), new DispatchQueue("myOtherQueue"));
			session.AddOutput(audioOutput); */

			movieFileOutput = new AVCaptureMovieFileOutput();
			session.AddOutput(movieFileOutput);

			stillImageOutput = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};
			session.AddOutput(stillImageOutput);
			session.StartRunning();
		}

		private void SetSampleBufferDelegate()
		{
			var queue = new DispatchQueue("myQueue");
			var outputRecorder = new videoDelegate(this);
			videoDataOutput.SetSampleBufferDelegate(outputRecorder, queue);
		}

		private void SwitchCamera()
		{
			devicePosition = deviceInput.Device.Position;
			session.StopRunning();

			if (front)
			{
				devicePosition = AVCaptureDevicePosition.Back;
				front = false;
			}
			else
			{
				devicePosition = AVCaptureDevicePosition.Front;
				front = true;
			}
			SetupLiveCameraStream(devicePosition);
		}

		private AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
			foreach (var device in devices)
			{
				if (device.Position == orientation)
				{
					return device;
				}
			}
			return null;
		}

		void ConfigureCameraForDevice(AVCaptureDevice device)
		{
			configured = true;
			var error = new NSError();
			if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
			{
				device.LockForConfiguration(out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration();
			}
			else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
			else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
		}

        private void StartRecording(){
            if (recording) return;
            recording = true;


			//session.RemoveOutput(videoDataOutput);
			recordingDelegate = new MyRecordingDelegate(this);
			//movieFileOutput = new AVCaptureMovieFileOutput();
			//session.AddOutput(movieFileOutput);

			//AVCaptureConnection connection = (AVCaptureConnection)movieFileOutput.Connections[0];
			//connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeRight;
			movieFileOutput.StartRecordingToOutputFile(fileUrl, recordingDelegate);
        }

        private void StopRecording() {
			if (!recording) return;
            recording = false;
			movieFileOutput.StopRecording();

            /*
			session.RemoveOutput(movieFileOutput);
			session.StopRunning();
			SetupLiveCameraStream(devicePosition);
			*/
        }

        private async void TakePicture() {
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
			pictureTakenAction.Invoke(jpegImageAsNsData);
        }

		public static async Task<bool> AuthorizeCameraUse()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            if (authorizationStatus == AVAuthorizationStatus.Denied)
                return false;

            if (authorizationStatus != AVAuthorizationStatus.Authorized) 
                return await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            
            return true;
		}

		private class MyRecordingDelegate : AVCaptureFileOutputRecordingDelegate
		{
			CameraController cameraController;
			public MyRecordingDelegate(CameraController cameraController)
			{
				this.cameraController = cameraController;
			}

			public override void DidStartRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections)
			{
				foreach (AVCaptureConnection connection in captureOutput.Connections)
				{
					foreach (AVCaptureAudioChannel au in connection.AvailableAudioChannels)
					{
						cameraController.audioChannel = au;
					}
				}
			}

			public override void FinishedRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections, NSError error)
			{
				if (UIVideo.IsCompatibleWithSavedPhotosAlbum(outputFileUrl.Path))
				{
                    cameraController.finishedRecordingAction.Invoke(outputFileUrl);
				}
				else new UIAlertView("Incompatible", "Incompatible", null, "OK", null).Show();
			}
		}

		private class videoDelegate : AVCaptureVideoDataOutputSampleBufferDelegate
		{
			CameraController cameraController;

			public videoDelegate(CameraController cameraController)
			{
				this.cameraController = cameraController;
			}

			public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
			{
				sampleBuffer.Dispose();
				return;
			}

			void TryDispose(IDisposable obj)
			{
				if (obj != null) obj.Dispose();
			}
		}

		private class audioDelegate : AVCaptureAudioDataOutputSampleBufferDelegate
		{
			CameraController cameraController;
			public audioDelegate(CameraController cameraController) { this.cameraController = cameraController; }
			public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
			{
				foreach (AVCaptureAudioChannel au in connection.AvailableAudioChannels) cameraController.audioChannel = au;
			}
		}
	}
}
