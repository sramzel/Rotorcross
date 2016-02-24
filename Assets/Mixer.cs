using System.Collections;

public class Mixer {

	private static Mixer mixer;

	public static float[,] Mixers(int i) { return GetInstance.mixers[i]; }

	static Mixer GetInstance { 
		get { 
			if (mixer == null) {
				mixer = new Mixer ();
			}
			return mixer;
		}
	}

	readonly float[][,] mixers = new float[][,]{
		mixerQuadX,
		mixerQuadP,
		mixerY4,
		mixerHex6X,
		mixerHex6P,
		mixerHex6H,
		mixerY6,
		mixerOctoX8,
		mixerOctoFlatP,
		mixerOctoFlatX
	};

	public static float[,] mixerQuadX = new float[,]{
		{ 1.0f, -1.0f,  1.0f, -1.0f },          // REAR_R
		{ 1.0f, -1.0f, -1.0f,  1.0f },          // FRONT_R
		{ 1.0f,  1.0f,  1.0f,  1.0f },          // REAR_L
		{ 1.0f,  1.0f, -1.0f, -1.0f },          // FRONT_L
	};
	
	public static float[,] mixerQuadP = new float[,]{
		{ 1.0f,  0.0f,  1.0f, -1.0f },          // REAR
		{ 1.0f, -1.0f,  0.0f,  1.0f },          // RIGHT
		{ 1.0f,  1.0f,  0.0f,  1.0f },          // LEFT
		{ 1.0f,  0.0f, -1.0f, -1.0f },          // FRONT
	};
	
	public static float[,] mixerY6 = new float[,]{
		{ 1.0f,  0.0f,  1.333333f,  1.0f },     // REAR
		{ 1.0f, -1.0f, -0.666667f, -1.0f },     // RIGHT
		{ 1.0f,  1.0f, -0.666667f, -1.0f },     // LEFT
		{ 1.0f,  0.0f,  1.333333f, -1.0f },     // UNDER_REAR
		{ 1.0f, -1.0f, -0.666667f,  1.0f },     // UNDER_RIGHT
		{ 1.0f,  1.0f, -0.666667f,  1.0f },     // UNDER_LEFT
	};
	
	public static float[,] mixerHex6P = new float[,]{
		{ 1.0f, -0.866025f,  0.5f,  1.0f },     // REAR_R
		{ 1.0f, -0.866025f, -0.5f, -1.0f },     // FRONT_R
		{ 1.0f,  0.866025f,  0.5f,  1.0f },     // REAR_L
		{ 1.0f,  0.866025f, -0.5f, -1.0f },     // FRONT_L
		{ 1.0f,  0.0f,      -1.0f,  1.0f },     // FRONT
		{ 1.0f,  0.0f,       1.0f, -1.0f },     // REAR
	};
	
	public static float[,] mixerY4 = new float[,]{
		{ 1.0f,  0.0f,  1.0f, -1.0f },          // REAR_TOP CW
		{ 1.0f, -1.0f, -1.0f,  0.0f },          // FRONT_R CCW
		{ 1.0f,  0.0f,  1.0f,  1.0f },          // REAR_BOTTOM CCW
		{ 1.0f,  1.0f, -1.0f,  0.0f },          // FRONT_L CW
	};
	
	public static float[,] mixerHex6X = new float[,]{
		{ 1.0f, -0.5f,  0.866025f,  1.0f },     // REAR_R
		{ 1.0f, -0.5f, -0.866025f,  1.0f },     // FRONT_R
		{ 1.0f,  0.5f,  0.866025f, -1.0f },     // REAR_L
		{ 1.0f,  0.5f, -0.866025f, -1.0f },     // FRONT_L
		{ 1.0f, -1.0f,  0.0f,      -1.0f },     // RIGHT
		{ 1.0f,  1.0f,  0.0f,       1.0f },     // LEFT
	};
	
	public static float[,] mixerOctoX8 = new float[,]{
		{ 1.0f, -1.0f,  1.0f, -1.0f },          // REAR_R
		{ 1.0f, -1.0f, -1.0f,  1.0f },          // FRONT_R
		{ 1.0f,  1.0f,  1.0f,  1.0f },          // REAR_L
		{ 1.0f,  1.0f, -1.0f, -1.0f },          // FRONT_L
		{ 1.0f, -1.0f,  1.0f,  1.0f },          // UNDER_REAR_R
		{ 1.0f, -1.0f, -1.0f, -1.0f },          // UNDER_FRONT_R
		{ 1.0f,  1.0f,  1.0f, -1.0f },          // UNDER_REAR_L
		{ 1.0f,  1.0f, -1.0f,  1.0f },          // UNDER_FRONT_L
	};
	
	public static float[,] mixerOctoFlatP = new float[,]{
		{ 1.0f,  0.707107f, -0.707107f,  1.0f },    // FRONT_L
		{ 1.0f, -0.707107f, -0.707107f,  1.0f },    // FRONT_R
		{ 1.0f, -0.707107f,  0.707107f,  1.0f },    // REAR_R
		{ 1.0f,  0.707107f,  0.707107f,  1.0f },    // REAR_L
		{ 1.0f,  0.0f, -1.0f, -1.0f },              // FRONT
		{ 1.0f, -1.0f,  0.0f, -1.0f },              // RIGHT
		{ 1.0f,  0.0f,  1.0f, -1.0f },              // REAR
		{ 1.0f,  1.0f,  0.0f, -1.0f },              // LEFT
	};
	
	public static float[,] mixerOctoFlatX = new float[,]{
		{ 1.0f,  1.0f, -0.414178f,  1.0f },      // MIDFRONT_L
		{ 1.0f, -0.414178f, -1.0f,  1.0f },      // FRONT_R
		{ 1.0f, -1.0f,  0.414178f,  1.0f },      // MIDREAR_R
		{ 1.0f,  0.414178f,  1.0f,  1.0f },      // REAR_L
		{ 1.0f,  0.414178f, -1.0f, -1.0f },      // FRONT_L
		{ 1.0f, -1.0f, -0.414178f, -1.0f },      // MIDFRONT_R
		{ 1.0f, -0.414178f,  1.0f, -1.0f },      // REAR_R
		{ 1.0f,  1.0f,  0.414178f, -1.0f },      // MIDREAR_L
	};
	
	public static float[,] mixerHex6H = new float[,]{
		{ 1.0f, -1.0f,  1.0f, -1.0f },     // REAR_R
		{ 1.0f, -1.0f, -1.0f,  1.0f },     // FRONT_R
		{ 1.0f,  1.0f,  1.0f,  1.0f },     // REAR_L
		{ 1.0f,  1.0f, -1.0f, -1.0f },     // FRONT_L
		{ 1.0f,  0.0f,  0.0f,  0.0f },     // RIGHT
		{ 1.0f,  0.0f,  0.0f,  0.0f },     // LEFT
	};
	
	public static float[,] mixerVtail4 = new float[,]{
		{ 1.0f,  -0.58f,  0.58f, 1.0f },        // REAR_R
		{ 1.0f,  -0.46f, -0.39f, -0.5f },       // FRONT_R
		{ 1.0f,  0.58f,  0.58f, -1.0f },        // REAR_L
		{ 1.0f,  0.46f, -0.39f, 0.5f },         // FRONT_L
	};
	
	public static float[,] mixerAtail4 = new float[,]{
		{ 1.0f,  0.0f,  1.0f,  1.0f },          // REAR_R
		{ 1.0f, -1.0f, -1.0f,  0.0f },          // FRONT_R
		{ 1.0f,  0.0f,  1.0f, -1.0f },          // REAR_L
		{ 1.0f,  1.0f, -1.0f, -0.0f },          // FRONT_L
	};
}
