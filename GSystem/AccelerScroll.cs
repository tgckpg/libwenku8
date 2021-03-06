﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Devices.Sensors;
using Windows.System.Display;

using Net.Astropenguin.Helpers;
using Net.Astropenguin.Logging;

namespace GR.GSystem
{
	public class AccelerScroll
	{
		private static readonly string ID = typeof( AccelerScroll ).Name; 

		// Display Requests are accumulative, so we need static bool store the state
		public static bool StateActive { get; private set; } = false;

		private Accelerometer Meter;
		private DisplayRequest DispRequest;

		private bool ReadingStarted = false;
		public Action<float> Delta;

		public bool Available => Meter != null;
		public bool TrackAutoAnchor = false;

		public float AccelerMultiplier;
		public float TerminalVelocity;
		public float BrakeOffset;
		public float Brake;
		public float BrakingForce;

		public bool ForceBrake = false;
		public bool ProgramBrake = false;

		private float _X;
		private float HitBound;

		public AccelerScroll()
		{
			Meter = Accelerometer.GetDefault( AccelerometerReadingType.Standard );
			if( Meter == null ) return;

			Meter.ReportInterval = 300;

			DispRequest = new DisplayRequest();
			ReleaseActive();
		}

		public void UpdateOrientation( DisplayOrientations Ori )
		{
			if ( Meter == null ) return;
			Meter.ReadingTransform = Ori;
		}

		public void StartCallibrate()
		{
			Logger.Log( ID, "Start callibration", LogType.DEBUG );
			if ( Meter == null ) return;

			Meter.ReadingChanged -= Meter_ReadingChanged;
			Meter.ReadingChanged += Meter_CallibrateChanged;
		}

		public void EndCallibration()
		{
			UpdateHitBound();
			Logger.Log( ID, "End callibration", LogType.DEBUG );
			if ( Meter == null ) return;

			Meter.ReadingChanged -= Meter_CallibrateChanged;
			Meter.ReadingChanged += Meter_ReadingChanged;
		}

		public void StartReading()
		{
			UpdateHitBound();

			if ( Meter == null || ReadingStarted )
				return;

			ReadingStarted = true;

			Meter.ReadingChanged += Meter_ReadingChanged;
		}

		public void StopReading()
		{
			if ( Meter == null ) return;

			ReleaseActive();

			Meter.ReadingChanged -= Meter_ReadingChanged;
			ReadingStarted = false;
		}

		private void Meter_CallibrateChanged( Accelerometer sender, AccelerometerReadingChangedEventArgs args )
		{
			// Stabilize the value
			// Easings.ParamTween( ref _X, ( float ) args.Reading.AccelerationX, 0.90f, 0.10f );
			// Delta( _X );
			Delta( _X = ( float ) args.Reading.AccelerationX );
		}

		private void Meter_ReadingChanged( Accelerometer sender, AccelerometerReadingChangedEventArgs args )
		{
			// Easings.ParamTween( ref _X, ( float ) args.Reading.AccelerationX, 0.90f, 0.10f );
			_X = ( float ) args.Reading.AccelerationX;

			float PosX = 0.5f * ( 1 + _X );

			bool InRange = ( PosX < HitBound || HitBound + Brake < PosX );

			if ( InRange )
			{
				Delta?.Invoke( _X - BrakeOffset );
				RequestActive();
			}
			else
			{
				Delta?.Invoke( 0 );
				ReleaseActive();
			}
		}

		private void RequestActive()
		{
			if ( !StateActive )
			{
				StateActive = true;
				Worker.UIInvoke( DispRequest.RequestActive );
			}
		}

		private void ReleaseActive()
		{
			if ( StateActive )
			{
				StateActive = false;
				Worker.UIInvoke( DispRequest.RequestRelease );
			}
		}

		private void UpdateHitBound()
		{
			HitBound = ( BrakeOffset + 1 ) * ( 0.5f * ( 1 - Brake ) );
		}

	}
}