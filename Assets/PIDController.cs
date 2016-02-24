using System.Collections;
using System;
using System.IO;

public interface ISaveData
{
	
	bool HasKey (string key);

	T GetValue<T> (string key);

	System.Object this [string key] { 
		set; 
	}
}

public abstract class PIDController
{
	public Int16[] AxisPID {
		get { return axisPID; }
	}

	public const int Angle = 0,
		Horizon = 1,
		Acro = 2;
	
	public void SetFlightMode (int flightmode)
	{
		this.flightMode = flightmode;
	}
	
	public string GetFlightMode ()
	{
		return flightMode.ToString ();
	}

	virtual public void Save (ISaveData data)
	{
		data ["FLIGHT_MODE"] = flightMode;
	}
	
	abstract public void GetPID (Int16[] axisPID, int[] inclination, int[] rcCommand, int[] gyroADC, int cycleTime);

	abstract public void SetPID (string value, int pidIndex, int axis);
	
	abstract public void SetRate (string value, short axis);
	
	abstract public void SetForce (string value, int axis);
	
	abstract public void SetLevelLimit (string value);
	
	abstract public string GetPID (int pidIndex, int axis);
	
	abstract public string GetRate (short axis);
	
	abstract public string GetForce (int axis);
	
	abstract public string GetLevelLimit ();

	virtual public void LoadSaved (ISaveData data)
	{
		if (data.HasKey ("FLIGHT_MODE")) {
			flightMode = data.GetValue<int> ("FLIGHT_MODE");
		}
	}
	
	public abstract string SaveFileName { get; }

	Int16[] axisPID = new Int16[3];
	public int flightMode = 0;//*
	public Int16 max_angle_inclination = 500;//*: Limits input for level modes

	const short GYRO_I_MAX = 256;
	const int PIDLEVEL = 3;

	public class pidMultiWiiClean : PIDController
	{
		public byte[] P8 = {0, 0, 0, 0};//0: P
		public byte[] I8 = {0, 0, 0, 0};//0: I
		public byte[] D8 = {0, 0, 0, 0};//0: D & level = Limits output for level modes
		int[] errorAngleI = { 0, 0, 0 };
		short[] lastGyro16 = { 0, 0, 0 };
		int[] errorGyroI = {0, 0, 0};
		int[] delta1_32 = {0, 0, 0}, delta2_32 = {0, 0, 0};

		public override string SaveFileName {
			get { return "PREF_PID_MULTIWII"; }
		}

		override public void GetPID (Int16[] axisPID, int[] inclination, int[] rcCommand, int[] gyroADC, int cycleTime)
		{
			int axis, prop;
			int error, errorAngle;
			int PTerm, ITerm, PTermACC = 0, ITermACC = 0, PTermGYRO = 0, ITermGYRO = 0, DTerm;
			int deltaSum;
			int delta;
			// **** PITCH & ROLL & YAW PID ****
			prop = Math.Min (Math.Max (Math.Abs (rcCommand [Constants.PITCH]), Math.Abs (rcCommand [Constants.ROLL])), 500); // range [0;500]
			
			for (axis = 0; axis < 3; axis++) {
				if ((flightMode == Angle || flightMode == Horizon) && (axis == Constants.ROLL || axis == Constants.PITCH)) { // MODE relying on ACC
					// observe max inclination
					errorAngle = Math.Min (Math.Max (2 * rcCommand [axis], -((int)max_angle_inclination)),
					                       +max_angle_inclination) - inclination [axis];
					
					PTermACC = errorAngle * P8 [PIDLEVEL] / 100; // 32 bits is needed for calculation: errorAngle*P8[PIDLEVEL] could exceed 32768   16 bits is ok for result
					PTermACC = Math.Min (Math.Max (PTermACC, -D8 [PIDLEVEL] * 5), +D8 [PIDLEVEL] * 5);
					
					errorAngleI [axis] = Math.Min (Math.Max (errorAngleI [axis] + errorAngle, -10000), +10000); // WindUp
					ITermACC = (errorAngleI [axis] * I8 [PIDLEVEL]) >> 12;
				}
				if (!(flightMode == Angle) || flightMode == Horizon || axis == Constants.YAW) { // MODE relying on GYRO or YAW axis
					error = (int)rcCommand [axis] * 10 * 8 / P8 [axis];
					error -= gyroADC [axis] / 4;
					
					PTermGYRO = rcCommand [axis];
					
					errorGyroI [axis] = Math.Min (Math.Max (errorGyroI [axis] + error, -16000), +16000); // WindUp
					if ((Math.Abs (gyroADC [axis]) > (640 * 4)) || (axis == Constants.YAW && Math.Abs (rcCommand [axis]) > 100))
						errorGyroI [axis] = 0;
					
					ITermGYRO = (errorGyroI [axis] / 125 * I8 [axis]) / 64;
				}
				if (flightMode == Horizon && (axis == Constants.ROLL || axis == Constants.PITCH)) {
					PTerm = (PTermACC * (500 - prop) + PTermGYRO * prop) / 500;
					ITerm = (ITermACC * (500 - prop) + ITermGYRO * prop) / 500;
				} else {
					if (flightMode == Angle && (axis == Constants.ROLL || axis == Constants.PITCH)) {
						PTerm = PTermACC;
						ITerm = ITermACC;
					} else {
						PTerm = PTermGYRO;
						ITerm = ITermGYRO;
					}
				}
				
				PTerm -= ((int)gyroADC [axis] / 4) * P8 [axis] / 10 / 8; // 32 bits is needed for calculation
				
				delta = (gyroADC [axis] - lastGyro16 [axis]) / 4;
				lastGyro16 [axis] = (short)gyroADC [axis];
				deltaSum = delta1_32 [axis] + delta2_32 [axis] + delta;
				delta2_32 [axis] = delta1_32 [axis];
				delta1_32 [axis] = delta;
				
				DTerm = (deltaSum * D8 [axis]) / 32;
				axisPID [axis] = (short)(PTerm + ITerm - DTerm);
			}
		}

		public override void SetPID (string value, int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				P8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 10f));
			} else if (pidIndex == 1) {
				I8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 1000f));
			} else if (pidIndex == 2) {
				D8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			}
		}

		public override void SetRate (string value, short axis)
		{

		}

		public override void SetForce (string value, int axis)
		{
			if (axis == 2) {
				P8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			} else if (axis == 3) {
				I8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			}
		}

		public override void SetLevelLimit (string value)
		{
			D8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
		}

		public override string GetPID (int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				return (P8 [axis] / 10f).ToString ();
			} else if (pidIndex == 1) {
				return (I8 [axis] / 1000f).ToString ();
			} else if (pidIndex == 2) {
				return D8 [axis].ToString ();
			} else
				return "N/A";
		}

		public override string GetRate (short axis)
		{
			return "N/A";
		}

		public override string GetForce (int axis)
		{
			if (axis == 2)
				return P8 [PIDLEVEL].ToString ();
			else if (axis == 3)
				return I8 [PIDLEVEL].ToString ();
			return "N/A";
		}

		public override string GetLevelLimit ()
		{
			return D8 [PIDLEVEL].ToString ();
		}

		public override void Save (ISaveData data)
		{
			for (int axis = 0; axis < 4; axis++) {
				data ["P8_" + axis] = P8 [axis];
				data ["I8_" + axis] = I8 [axis];
				data ["D8_" + axis] = D8 [axis];
			}
			base.Save (data);
		}

		public override void LoadSaved (ISaveData data)
		{
			base.LoadSaved (data);
			for (int n = 0; n < 4; n ++) {
				if (data.HasKey ("P8_" + n))
					P8 [n] = data.GetValue <byte> ("P8_" + n);
				if (data.HasKey ("I8_" + n))
					I8 [n] = data.GetValue <byte> ("I8_" + n);
				if (data.HasKey ("D8_" + n))
					D8 [n] = data.GetValue <byte> ("D8_" + n);
			}
		}
	}

	public class pidRewriteClean : PIDController
	{
		public override string SaveFileName {
			get { return "PREF_PID_MULTIWII_REWRITE"; }
		}

		public byte[] P8 = {0, 0, 0, 0};//1: P & Level = Force of Angle mode correction
		public byte[] I8 = {0, 0, 0, 0};//1: I & Level = Force of Horizon mode correction
		public byte[] D8 = {0, 0, 0, 0};//1: D & level = Rate at which level is reduced in horizon
		public byte[] rates = {0, 0, 0};//1: Scales input values
		int[] errorGyroI = {0, 0, 0};
		int[] lastError = { 0, 0, 0 };
		int[] delta1_32 = {0, 0, 0}, delta2_32 = {0, 0, 0};

		override public void GetPID (Int16[] axisPID, int[] inclination, int[] rcCommand, int[] gyroADC, int cycleTime)
		{
			Int32 errorAngle;
			Int16 axis;
			Int32 delta, deltaSum;
			Int32 PTerm, ITerm, DTerm;
			Int32 AngleRateTmp, RateError;
		
			byte horizonLevelStrength = 100;
			Int32 stickPosAil, stickPosEle, mostDeflectedPos;
		
			if (flightMode == Horizon) {
			
				// Figure out the raw stick positions
				stickPosAil = Math.Abs (rcCommand [Constants.ROLL]);
				stickPosEle = Math.Abs (rcCommand [Constants.PITCH]);
			
				mostDeflectedPos = Math.Max (stickPosAil, stickPosEle);
				mostDeflectedPos += (Math.Min (Math.Max (Math.Abs (inclination [Constants.PITCH]), Math.Abs (inclination [Constants.ROLL])), 1800) / 10);
				mostDeflectedPos = Math.Min (mostDeflectedPos, 500);
			
				// Progressively turn off the horizon self level strength as the stick is banged over
				horizonLevelStrength = (byte)((500 - mostDeflectedPos) / 5);  // 100 at centre stick, 0 = max stick deflection
			
				// Using Level D as a Sensitivity for Horizon. 0 more level to 255 more rate. Default value of 100 seems to work fine.
				// For more rate mode increase D and slower flips and rolls will be possible
				horizonLevelStrength = (byte)Math.Min (Math.Max ((10 * (horizonLevelStrength - 100) * (10 * D8 [PIDLEVEL] / 80) / 100) + 100, 0), 100);
			}
		
			// ----------PID controller----------
			for (axis = 0; axis < 3; axis++) {
				byte rate = rates [axis];
			
				// -----Get the desired angle rate depending on flight mode
				if (axis == Constants.YAW) { // YAW is always gyro-controlled (MAG correction is applied to rcCommand)
					AngleRateTmp = (((Int32)(rate + 27) * rcCommand [Constants.YAW]) >> 5);
				} else {
					// calculate error and limit the angle to max configured inclination
					errorAngle = Math.Min (Math.Max (2 * rcCommand [axis], -((int)max_angle_inclination)),
					                       +max_angle_inclination) - inclination [axis]; // 16 bits is ok here
				
					if (!(flightMode == Angle)) { //control is GYRO based (ACRO and HORIZON - direct sticks control is applied to rate PID
						AngleRateTmp = ((Int32)(rate + 27) * rcCommand [axis]) >> 4;
						if (flightMode == Horizon) {
							// mix up angle error to desired AngleRateTmp to add a little auto-level feel. horizonLevelStrength is scaled to the stick input
							AngleRateTmp += (errorAngle * I8 [PIDLEVEL] * horizonLevelStrength / 100) >> 4;
						}
					} else { // it's the ANGLE mode - control is angle based, so control loop is needed
						AngleRateTmp = (errorAngle * P8 [PIDLEVEL]) >> 4;
					}
				}
			
				// --------low-level gyro-based PID. ----------
				// Used in stand-alone mode for ACRO, controlled by higher level regulators in other modes
				// -----calculate scaled error.AngleRates
				// multiplication of rcCommand corresponds to changing the sticks scaling here
				RateError = AngleRateTmp - (gyroADC [axis] / 4);
			
				// -----calculate P component
				PTerm = (RateError * P8 [axis]) >> 7;
			
				// -----calculate I component
				// there should be no division before accumulating the error to integrator, because the precision would be reduced.
				// Precision is critical, as I prevents from long-time drift. Thus, 32 bits integrator is used.
				// Time correction (to avoid different I scaling for different builds based on average cycle time)
				// is normalized to cycle time = 2048.
				errorGyroI [axis] = errorGyroI [axis] + ((RateError * cycleTime) >> 11) * I8 [axis];
			
				// limit maximum integrator value to prevent WindUp - accumulating extreme values when system is saturated.
				// I coefficient (I8) moved before integration to make limiting independent from PID settings
				errorGyroI [axis] = Math.Min (Math.Max (errorGyroI [axis], (Int32)(- GYRO_I_MAX) << 13), (Int32)(+ GYRO_I_MAX) << 13);
				ITerm = errorGyroI [axis] >> 13;
			
				//-----calculate D-term
				delta = RateError - lastError [axis]; // 16 bits is ok here, the dif between 2 consecutive gyro reads is limited to 800
				lastError [axis] = RateError;
			
				// Correct difference by cycle time. Cycle time is jittery (can be different 2 times), so calculated difference
				// would be scaled by different dt each time. Division by dT fixes that.
				delta = (delta * ((UInt16)0xFFFF / (cycleTime >> 4))) >> 6;
				// add moving average here to reduce noise
				deltaSum = delta1_32 [axis] + delta2_32 [axis] + delta;
				delta2_32 [axis] = delta1_32 [axis];
				delta1_32 [axis] = delta;
			
				DTerm = (deltaSum * D8 [axis]) >> 8;
			
				// -----calculate total PID output
				axisPID [axis] = (Int16)(PTerm + ITerm + DTerm);
			}
		}

		public override void SetPID (string value, int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				P8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 10f));
			} else if (pidIndex == 1) {
				I8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 1000f));
			} else if (pidIndex == 2) {
				D8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			}
		}

		public override void SetRate (string value, short axis)
		{
			if (axis < 3) {
				rates [axis] = byte.Parse (value);
			} else if (axis == PIDLEVEL) {
				D8 [PIDLEVEL] = byte.Parse (value);
			}
		}

		public override void SetForce (string value, int axis)
		{
			switch (axis) {
			case 0:
				I8 [PIDLEVEL] = byte.Parse (value);
				break;
			case 1:
				P8 [PIDLEVEL] = byte.Parse (value);
				break;
			}
		}

		public override void SetLevelLimit (string value)
		{
		}
		
		public override string GetPID (int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				return (P8 [axis] / 10f).ToString ();
			} else if (pidIndex == 1) {
				return (I8 [axis] / 1000f).ToString ();
			} else if (pidIndex == 2) {
				return D8 [axis].ToString ();
			} else
				return "N/A";
		}
		
		public override string GetRate (short axis)
		{
			switch (axis) {
			case Constants.PITCH:
				return rates [Constants.PITCH].ToString ();
			case Constants.YAW:
				return rates [Constants.YAW].ToString ();
			case Constants.ROLL:
				return rates [Constants.ROLL].ToString ();
			case 3:
				return D8 [PIDLEVEL].ToString ();
			default:
				return "N/A";
			}
		}
		
		public override string GetForce (int axis)
		{
			switch (axis) {
			case 0:
				return I8 [PIDLEVEL].ToString ();
			case 1:
				return P8 [PIDLEVEL].ToString ();
			default:
				return "N/A";
			}
		}
		
		public override string GetLevelLimit ()
		{
			return "N/A";
		}

		public override void Save (ISaveData data)
		{
			for (int axis = 0; axis < 4; axis++) {
				data ["P8_" + axis] = P8 [axis];
				data ["I8_" + axis] = I8 [axis];
				data ["D8_" + axis] = D8 [axis];
			}
			for (int n = 0; n < 3; n ++) {
				data ["rates_" + n] = rates [n];
			}
			base.Save (data);
		}

		public override void LoadSaved (ISaveData data)
		{
			base.LoadSaved (data);
			for (int n = 0; n < 4; n ++) {
				if (data.HasKey ("P8_" + n))
					P8 [n] = data.GetValue <byte> ("P8_" + n);
				if (data.HasKey ("I8_" + n))
					I8 [n] = data.GetValue <byte> ("I8_" + n);
				if (data.HasKey ("D8_" + n))
					D8 [n] = data.GetValue <byte> ("D8_" + n);
			}
			for (int n = 0; n < 3; n ++) {
				if (data.HasKey ("rates_" + n))
					rates [n] = data.GetValue <byte> ("rates_" + n);
			}
		}

	}

	public class pidLuxFloat : PIDController
	{
		public float A_level;//2: Force of Angle mode correction
		public float H_level;//2: Force of Horizon mode correction
		public float H_sensitivity;//2: Rate at which level is reduced in horizon
		public float[] P_f = new float[3];//2: P
		public float[] I_f = new float[3];//2: I
		public float[] D_f = new float[3];//2: D
		public byte[] rates = {0, 0, 0};//2: Scales input values
		float[] lastGyroRate = new float[3];
		float[] errorGyroIf = new float[3];
		float[] delta1 = new float[3], delta2 = new float[3];
		
		public override string SaveFileName {
			get { return "PREF_PID_LUXFLOAT"; }
		}

		override public void GetPID (Int16[] axisPID, int[] inclination, int[] rcCommand, int[] gyroADC, int cycleTime)
		{
			float RateError, errorAngle, AngleRate, gyroRate;
			float ITerm, PTerm, DTerm;
			Int32 stickPosAil, stickPosEle, mostDeflectedPos;
			float delta, deltaSum;
			float dT;
			byte axis;
			float horizonLevelStrength = 1;

			dT = (float)cycleTime * 0.000001f;
		
			if (flightMode == Horizon) {
				// Figure out the raw stick positions
				stickPosAil = Math.Abs (rcCommand [Constants.ROLL]);
				stickPosEle = Math.Abs (rcCommand [Constants.PITCH]);
				mostDeflectedPos = Math.Max (stickPosAil, stickPosEle);
				mostDeflectedPos += (Math.Min (Math.Max (Math.Abs (inclination [Constants.PITCH]), Math.Abs (inclination [Constants.ROLL])), 1800) / 10);
				mostDeflectedPos = Math.Min (mostDeflectedPos, 500);
			
				// Progressively turn off the horizon self level strength as the stick is banged over
				horizonLevelStrength = (float)(500 - mostDeflectedPos) / 500;  // 1 at centre stick, 0 = max stick deflection
				if (H_sensitivity == 0) {
					horizonLevelStrength = 0;
				} else {
					horizonLevelStrength = Math.Min (Math.Max (((horizonLevelStrength - 1) * (100 / H_sensitivity)) + 1, 0), 1);
				}
			}
		
			// ----------PID controller----------
			for (axis = 0; axis < 3; axis++) {
				// -----Get the desired angle rate depending on flight mode
				byte rate = rates [axis];
			
				if (axis == Constants.YAW) {
					// YAW is always gyro-controlled (MAG correction is applied to rcCommand) 100dps to 1100dps max yaw rate
					AngleRate = (float)((rate + 10) * rcCommand [Constants.YAW]) / 50.0f;
				} else {
					// calculate error and limit the angle to the max inclination
					errorAngle = (Math.Min (Math.Max (rcCommand [axis], -((int)max_angle_inclination)),
					                        +max_angle_inclination) - inclination [axis]) / 10.0f; // 16 bits is ok here

					if (flightMode == Angle) {
						// it's the ANGLE mode - control is angle based, so control loop is needed
						AngleRate = errorAngle * A_level;
					} else {
						//control is GYRO based (ACRO and HORIZON - direct sticks control is applied to rate PID
						AngleRate = (float)((rate + 20) * rcCommand [axis]) / 50.0f; // 200dps to 1200dps max roll/pitch rate
						if (flightMode == Horizon) {
							// mix up angle error to desired AngleRate to add a little auto-level feel
							AngleRate += errorAngle * H_level * horizonLevelStrength;
						}
					}
				}
			
				gyroRate = gyroADC [axis] * 0.25f; // gyro output scaled to dps
			
				// --------low-level gyro-based PID. ----------
				// Used in stand-alone mode for ACRO, controlled by higher level regulators in other modes
				// -----calculate scaled error.AngleRates
				// multiplication of rcCommand corresponds to changing the sticks scaling here
				RateError = AngleRate - gyroRate;
			
				// -----calculate P component
				PTerm = RateError * P_f [axis];

				// -----calculate I component. Note that PIDweight is divided by 10, because it is simplified formule from the previous multiply by 10
				errorGyroIf [axis] = Math.Min (Math.Max (errorGyroIf [axis] + RateError * dT * I_f [axis] * 10, -250.0f), 250.0f);
			
				// limit maximum integrator value to prevent WindUp - accumulating extreme values when system is saturated.
				// I coefficient (I8) moved before integration to make limiting independent from PID settings
				ITerm = errorGyroIf [axis];
			
				//-----calculate D-term
				delta = gyroRate - lastGyroRate [axis];  // 16 bits is ok here, the dif between 2 consecutive gyro reads is limited to 800
				lastGyroRate [axis] = gyroRate;
			
				// Correct difference by cycle time. Cycle time is jittery (can be different 2 times), so calculated difference
				// would be scaled by different dt each time. Division by dT fixes that.
				delta *= (1.0f / dT);
				// add moving average here to reduce noise
				deltaSum = delta1 [axis] + delta2 [axis] + delta;
				delta2 [axis] = delta1 [axis];
				delta1 [axis] = delta;

				DTerm = Math.Min (Math.Max ((deltaSum / 3.0f) * D_f [axis], -300.0f), 300.0f);
			
				// -----calculate total PID output
				axisPID [axis] = Convert.ToInt16 (Math.Min (Math.Max (Math.Round (PTerm + ITerm - DTerm), -1000), 1000));

			}
		}

		public override void SetPID (string value, int pidIndex, int axis)
		{
			switch (pidIndex) {
			case 0:
				P_f [axis] = float.Parse (value);
				break;
			case 1:
				I_f [axis] = float.Parse (value);
				break;
			case 2:
				D_f [axis] = float.Parse (value);
				break;
			}
		}

		public override void SetRate (string value, short axis)
		{
			if (axis < 3) {
				rates [axis] = byte.Parse (value);
			} else if (axis == 3) {
				H_sensitivity = float.Parse (value);
			}
		}

		public override void SetForce (string value, int axis)
		{
			if (axis == 0) {
				H_level = float.Parse (value);
			} else if (axis == 1) {
				A_level = float.Parse (value);
			}
		}

		public override void SetLevelLimit (string value)
		{
		}
		
		public override string GetPID (int pidIndex, int axis)
		{
			if (pidIndex == 0)
				return (P_f [axis]).ToString ();
			else if (pidIndex == 1)
				return (I_f [axis]).ToString ();
			else if (pidIndex == 2)
				return (D_f [axis]).ToString ();
			else
				return "N/A";
		}
		
		public override string GetRate (short axis)
		{
			if (axis < 3) {
				return (rates [axis]).ToString ();
			} else if (axis == 3) {
				return (H_sensitivity).ToString ();
			} else
				return "N/A";
		}
		
		public override string GetForce (int axis)
		{
			if (axis == 0) {
				return H_level.ToString ();
			} else if (axis == 1) {
				return A_level.ToString ();
			} else 
				return "N/A";
		}
		
		public override string GetLevelLimit ()
		{
			return "N/A";
		}

		public override void Save (ISaveData data)
		{
			for (int n = 0; n < 3; n ++) {
				data ["P_f_" + n] = P_f [n];
				data ["I_f_" + n] = I_f [n];
				data ["D_f_" + n] = D_f [n];
				data ["rates_" + n] = rates [n];
			}
			data ["rates_3"] = H_sensitivity;
			data ["force_0"] = H_level;
			data ["force_1"] = A_level;
			base.Save (data);
		}

		public override void LoadSaved (ISaveData data)
		{
			base.LoadSaved (data);

			for (int n = 0; n < 3; n ++) {
				if (data.HasKey ("P_f_" + n))
					P_f [n] = data.GetValue <float> ("P_f_" + n);
				if (data.HasKey ("I_f_" + n))
					I_f [n] = data.GetValue <float> ("I_f_" + n);
				if (data.HasKey ("D_f_" + n))
					D_f [n] = data.GetValue <float> ("D_f_" + n);
				if (data.HasKey ("rates_" + n))
					rates [n] = data.GetValue <byte> ("rates_" + n);
			}
			if (data.HasKey ("rates_3"))
				H_sensitivity = data.GetValue <float> ("rates_3");
			
			if (data.HasKey ("force_0"))
				H_level = data.GetValue <float> ("force_0");
			
			if (data.HasKey ("force_1"))
				A_level = data.GetValue <float> ("force_1");
		}

	}

	public class pidMultiWii23 : PIDController
	{	
		public byte[] P8 = {0, 0, 0, 0};//3: P
		public byte[] I8 = {0, 0, 0, 0};//3: I
		public byte[] D8 = {0, 0, 0, 0};//3: D & level = Limits output for level modes
		public byte[] rates = {0, 0, 0};//3: Scales input values for Yaw
		int[] errorAngleI = { 0, 0, 0 };
		short[] lastGyro = { 0, 0, 0 };
		int[] errorGyroI = {0, 0, 0};
		int[] delta1 = {0, 0, 0}, delta2 = {0, 0, 0};
		
		public override string SaveFileName {
			get { return "PREF_PID_MULTIWII_23"; }
		}
		
		override public void GetPID (Int16[] axisPID, int[] inclination, int[] rcCommand, int[] gyroADC, int cycleTime)
		{
			int axis, prop = 0;
			int rc, error, errorAngle;
			int PTerm, ITerm, PTermACC = 0, ITermACC = 0, DTerm;
			int delta;
				
			if (flightMode == Horizon) {
				prop = Math.Min (Math.Max (Math.Abs (rcCommand [Constants.PITCH]), Math.Abs (rcCommand [Constants.ROLL])), 512);
			}
				
			// PITCH & ROLL
			for (axis = 0; axis < 3; axis+=2) {
					
				rc = rcCommand [axis] << 1;
					
				error = rc - (gyroADC [axis] / 4);
				errorGyroI [axis] = Math.Min (Math.Max (errorGyroI [axis] + error, -16000), +16000);   // WindUp   16 bits is ok here
					
				if (Math.Abs (gyroADC [axis]) > (640 * 4)) {
					errorGyroI [axis] = 0;
				}
					
				ITerm = (errorGyroI [axis] >> 7) * I8 [axis] >> 6;   // 16 bits is ok here 16000/125 = 128 ; 128*250 = 32000
					
				PTerm = (int)rc * P8 [axis] >> 6;
					
				if (flightMode == Angle || flightMode == Horizon) {   // axis relying on ACC
					// 50 degrees max inclination
					errorAngle = Math.Min (Math.Max (2 * rcCommand [axis], -((int)max_angle_inclination)),
						                       +max_angle_inclination) - inclination [axis];
						
					errorAngleI [axis] = Math.Min (Math.Max (errorAngleI [axis] + errorAngle, -10000), +10000);                                                // WindUp     //16 bits is ok here
						
					PTermACC = ((int)errorAngle * P8 [PIDLEVEL]) >> 7;   // 32 bits is needed for calculation: errorAngle*P8 could exceed 32768   16 bits is ok for result
						
					short limit = (short)(D8 [PIDLEVEL] * 5);
					PTermACC = Math.Min (Math.Max (PTermACC, -limit), +limit);
						
					ITermACC = ((int)errorAngleI [axis] * I8 [PIDLEVEL]) >> 12;  // 32 bits is needed for calculation:10000*I8 could exceed 32768   16 bits is ok for result
						
					ITerm = ITermACC + ((ITerm - ITermACC) * prop >> 9);
					PTerm = PTermACC + ((PTerm - PTermACC) * prop >> 9);
				}
					
				PTerm -= ((int)(gyroADC [axis] / 4) * P8 [axis]) >> 6;   // 32 bits is needed for calculation
					
				delta = (gyroADC [axis] - lastGyro [axis]) / 4;   // 16 bits is ok here, the dif between 2 consecutive gyro reads is limited to 800
				lastGyro [axis] = (short)gyroADC [axis];
				DTerm = delta1 [axis] + delta2 [axis] + delta;
				delta2 [axis] = delta1 [axis];
				delta1 [axis] = delta;
					
				DTerm = ((int)DTerm * D8 [axis]) >> 5;   // 32 bits is needed for calculation
					
				axisPID [axis] = (short)(PTerm + ITerm - DTerm);
			}
				
			//YAW
			rc = (int)rcCommand [Constants.YAW] * (2 * rates [Constants.YAW] + 30) >> 5;
			error = rc - (gyroADC [Constants.YAW] / 4);
			errorGyroI [Constants.YAW] += (int)error * I8 [Constants.YAW];
			errorGyroI [Constants.YAW] = Math.Min (Math.Max (errorGyroI [Constants.YAW], 2 - ((int)1 << 28)), -2 + ((int)1 << 28));
			if (Math.Abs (rc) > 50)
				errorGyroI [Constants.YAW] = 0;
				
			PTerm = (int)error * P8 [Constants.YAW] >> 6; // TODO: Bitwise shift on a signed integer is not recommended
				
			ITerm = Math.Min (Math.Max ((short)(errorGyroI [Constants.YAW] >> 13), (short)(-GYRO_I_MAX)), GYRO_I_MAX);
				
			axisPID [Constants.YAW] = (short)(PTerm + ITerm);
		}
		
		public override void SetPID (string value, int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				P8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 10f));
			} else if (pidIndex == 1) {
				I8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 1000f));
			} else if (pidIndex == 2) {
				D8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			}
		}
		
		public override void SetRate (string value, short axis)
		{
			
		}
		
		public override void SetForce (string value, int axis)
		{
			if (axis == 2) {
				P8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			} else if (axis == 3) {
				I8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			}
		}
		
		public override void SetLevelLimit (string value)
		{
			D8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
		}
		
		public override string GetPID (int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				return (P8 [axis] / 10f).ToString ();
			} else if (pidIndex == 1) {
				return (I8 [axis] / 1000f).ToString ();
			} else if (pidIndex == 2) {
				return D8 [axis].ToString ();
			} else
				return "N/A";
		}
		
		public override string GetRate (short axis)
		{
			return "N/A";
		}
		
		public override string GetForce (int axis)
		{
			if (axis == 2)
				return P8 [PIDLEVEL].ToString ();
			else if (axis == 3)
				return I8 [PIDLEVEL].ToString ();
			return "N/A";
		}
		
		public override string GetLevelLimit ()
		{
			return D8 [PIDLEVEL].ToString ();
		}
		
		public override void Save (ISaveData data)
		{
			for (int axis = 0; axis < 4; axis++) {
				data ["P8_" + axis] = P8 [axis];
				data ["I8_" + axis] = I8 [axis];
				data ["D8_" + axis] = D8 [axis];
			}
			base.Save (data);
		}
		
		public override void LoadSaved (ISaveData data)
		{
			base.LoadSaved (data);
			for (int n = 0; n < 4; n ++) {
				if (data.HasKey ("P8_" + n))
					P8 [n] = data.GetValue <byte> ("P8_" + n);
				if (data.HasKey ("I8_" + n))
					I8 [n] = data.GetValue <byte> ("I8_" + n);
				if (data.HasKey ("D8_" + n))
					D8 [n] = data.GetValue <byte> ("D8_" + n);
			}
		}
	}

	public class pidHarakiri : PIDController
	{
		public byte[] P8 = {0, 0, 0, 0};//5: P
		public byte[] I8 = {0, 0, 0, 0};//5: I
		public byte[] D8 = {0, 0, 0, 0};//5: D & level = Limits output for level modes
		public byte[] rates = {0, 0, 0};//5: Scales yaw input value
		float[] errorAngleIf = new float[3];
		float[] lastGyro = { 0.0f, 0.0f, 0.0f }, lastDTerm = { 0.0f, 0.0f, 0.0f };
		float[] errorGyroIf = new float[3];
		int[] errorGyroI = {0, 0, 0};
		
		public override string SaveFileName {
			get { return "PREF_PID_HARAKIRI"; }
		}

		override public void GetPID (Int16[] axisPID, int[] inclination, int[] rcCommand, int[] gyroADC, int cycleTime)
		{
			float delta, RCfactor, rcCommandAxis, MainDptCut, gyroADCQuant;
			float PTerm, ITerm, DTerm, PTermACC = 0.0f, ITermACC = 0.0f, ITermGYRO, error, prop = 0.0f;
			byte axis;
			float ACCDeltaTimeINS, FLOATcycleTime, Mwii3msTimescale;
		
			MainDptCut = (float)Math.PI / Math.Min (Math.Max (0, 0), 50);       // maincuthz (default 0 (disabled), Range 1-50Hz)
			FLOATcycleTime = (float)Math.Min (Math.Max (cycleTime, 1), 100000);                  // 1us - 100ms
			ACCDeltaTimeINS = FLOATcycleTime * 0.000001f;                              // ACCDeltaTimeINS is in seconds now
			RCfactor = ACCDeltaTimeINS / (MainDptCut + ACCDeltaTimeINS);               // used for pt1 element
		
			if (flightMode == Horizon) {
				prop = (float)Math.Min (Math.Max (Math.Abs (rcCommand [Constants.PITCH]), Math.Abs (rcCommand [Constants.ROLL])), 450) / 450.0f;
			}
		
			for (axis = 0; axis < 3; axis+=2) {
				Int32 tmp = (Int32)((float)gyroADC [axis] * 0.3125f);              // Multiwii masks out the last 2 bits, this has the same idea
				gyroADCQuant = (float)tmp * 3.2f;                                     // but delivers more accuracy and also reduces jittery flight
				rcCommandAxis = (float)rcCommand [axis];                                // Calculate common values for pid controllers
				if (flightMode == Angle || flightMode == Horizon) {
					error = Math.Min (Math.Max (2.0f * rcCommandAxis, -((int)max_angle_inclination)), +max_angle_inclination) - inclination [axis];
					PTermACC = error * (float)P8 [PIDLEVEL] * 0.008f;
					float limitf = (float)D8 [PIDLEVEL] * 5.0f;
					PTermACC = Math.Min (Math.Max (PTermACC, -limitf), +limitf);
					errorAngleIf [axis] = Math.Min (Math.Max (errorAngleIf [axis] + error * ACCDeltaTimeINS, -30.0f), +30.0f);
					ITermACC = errorAngleIf [axis] * (float)I8 [PIDLEVEL] * 0.08f;
				}
			
				if (!(flightMode == Angle)) {
					if (Math.Abs ((Int16)gyroADC [axis]) > 2560) {
						errorGyroIf [axis] = 0.0f;
					} else {
						error = (rcCommandAxis * 320.0f / (float)P8 [axis]) - gyroADCQuant;
						errorGyroIf [axis] = Math.Min (Math.Max (errorGyroIf [axis] + error * ACCDeltaTimeINS, -192.0f), +192.0f);
					}
				
					ITermGYRO = errorGyroIf [axis] * (float)I8 [axis] * 0.01f;
				
					if (flightMode == Horizon) {
						PTerm = PTermACC + prop * (rcCommandAxis - PTermACC);
						ITerm = ITermACC + prop * (ITermGYRO - ITermACC);
					} else {
						PTerm = rcCommandAxis;
						ITerm = ITermGYRO;
					}
				} else {
					PTerm = PTermACC;
					ITerm = ITermACC;
				}
			
				PTerm -= gyroADCQuant * P8 [axis] * 0.003f;
			
				delta = (gyroADCQuant - lastGyro [axis]) / ACCDeltaTimeINS;
			
				lastGyro [axis] = gyroADCQuant;
				lastDTerm [axis] += RCfactor * (delta - lastDTerm [axis]);
				DTerm = lastDTerm [axis] * D8 [axis] * 0.00007f;
			
				axisPID [axis] = (Int16)Convert.ToInt16 (Math.Round (PTerm + ITerm - DTerm));
			}
		
			Mwii3msTimescale = (Int32)FLOATcycleTime & (Int32)~3;                  // Filter last 2 bit jitter
			Mwii3msTimescale /= 3000.0f;
		
			if (false) { // [0/1] 0 = multiwii 2.3 yaw, 1 = older yaw. hardcoded for now
				PTerm = ((Int32)P8 [Constants.YAW] * (100 - (Int32)rates [Constants.YAW] * (Int32)Math.Abs (rcCommand [Constants.YAW]) / 500)) / 100;
				Int32 tmp = Convert.ToInt32 (Math.Round (gyroADC [Constants.YAW] * 0.25f));
				PTerm = rcCommand [Constants.YAW] - tmp * PTerm / 80;
				if ((Math.Abs (tmp) > 640) || (Math.Abs (rcCommand [Constants.YAW]) > 100)) {
					errorGyroI [Constants.YAW] = 0;
				} else {
					error = ((Int32)rcCommand [Constants.YAW] * 80 / (Int32)P8 [Constants.YAW]) - tmp;
					errorGyroI [Constants.YAW] = Math.Min (Math.Max (errorGyroI [Constants.YAW] + (Int32)(error * Mwii3msTimescale), -16000), +16000); // WindUp
					ITerm = (errorGyroI [Constants.YAW] / 125 * I8 [Constants.YAW]) >> 6;
				}
			} else {
				Int32 tmp = ((Int32)rcCommand [Constants.YAW] * (((Int32)rates [Constants.YAW] << 1) + 40)) >> 5;
				error = tmp - Convert.ToInt32 (Math.Round (gyroADC [Constants.YAW] * 0.25f));                       // Less Gyrojitter works actually better
			
				if (Math.Abs (tmp) > 50) {
					errorGyroI [Constants.YAW] = 0;
				} else {
					errorGyroI [Constants.YAW] = Math.Min (Math.Max (errorGyroI [Constants.YAW] + (Int32)(error * (float)I8 [Constants.YAW] * Mwii3msTimescale), -268435454), +268435454);
				}
			
				ITerm = Math.Min (Math.Max (errorGyroI [Constants.YAW] >> 13, -GYRO_I_MAX), +GYRO_I_MAX);
				PTerm = ((Int32)error * (Int32)P8 [Constants.YAW]) >> 6;
			}
		
			axisPID [Constants.YAW] = (Int16)Convert.ToInt16 (Math.Round (PTerm + ITerm));                                 // Round up result.
		}
		
		public override void SetPID (string value, int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				P8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 10f));
			} else if (pidIndex == 1) {
				I8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value) * 1000f));
			} else if (pidIndex == 2) {
				D8 [axis] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
			}
		}
		
		public override void SetRate (string value, short axis)
		{
			if (axis == Constants.YAW) {
				rates [Constants.YAW] = byte.Parse (value);
			}
		}
		
		public override void SetForce (string value, int axis)
		{
			switch (axis) {
			case 3:
				I8 [PIDLEVEL] = byte.Parse (value);
				break;
			case 2:
				P8 [PIDLEVEL] = byte.Parse (value);
				break;
			}
		}
		
		public override void SetLevelLimit (string value)
		{
			D8 [PIDLEVEL] = (byte)Convert.ToByte (Math.Round (float.Parse (value)));
		}
		
		public override string GetPID (int pidIndex, int axis)
		{
			if (pidIndex == 0) {
				return (P8 [axis] / 10f).ToString ();
			} else if (pidIndex == 1) {
				return (I8 [axis] / 1000f).ToString ();
			} else if (pidIndex == 2) {
				return D8 [axis].ToString ();
			} else
				return "N/A";
		}
		
		public override string GetRate (short axis)
		{
			if (axis == Constants.YAW) {
				return rates [Constants.YAW].ToString ();
			}
			return "N/A";
		}
		
		public override string GetForce (int axis)
		{
			switch (axis) {
			case 3:
				return I8 [PIDLEVEL].ToString ();
			case 2:
				return P8 [PIDLEVEL].ToString ();
			default:
				return "N/A";
			}
		}

		public override string GetLevelLimit ()
		{
			return D8 [PIDLEVEL].ToString ();
		}
		
		public override void Save (ISaveData data)
		{
			for (int axis = 0; axis < 4; axis++) {
				data ["P8_" + axis] = P8 [axis];
				data ["I8_" + axis] = I8 [axis];
				data ["D8_" + axis] = D8 [axis];
			}

			data ["rates_" + Constants.YAW] = rates [Constants.YAW];

			base.Save (data);
		}

		public override void LoadSaved (ISaveData data)
		{
			base.LoadSaved (data);
			for (int n = 0; n < 4; n ++) {
				if (data.HasKey ("P8_" + n))
					P8 [n] = data.GetValue <byte> ("P8_" + n);
				if (data.HasKey ("I8_" + n))
					I8 [n] = data.GetValue <byte> ("I8_" + n);
				if (data.HasKey ("D8_" + n))
					D8 [n] = data.GetValue <byte> ("D8_" + n);
			}
			if (data.HasKey ("rates_" + Constants.YAW))
				rates [Constants.YAW] = data.GetValue <byte> ("rates_" + Constants.YAW);
		}

	}
}
