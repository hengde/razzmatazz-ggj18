public static class CarProblems{
    public static string[] brokenPartSolutions = new string[] { 
        "Twist", "rotary", "calibrator",
        "Crank", "forearm", "twister",
        "Engage", "reverse", "throttle",
        "Install", "microwave", "quencher",
        "Respect", "hypno", "battery",
        "Loosen", "aquatic", "belt",

        "Update", "neuro", "box", 
        "Download", "quantum", "shapeshifter", 
        "Shrink", "macro", "engine",
        "Stretch", "gear", "plugs",
        "reroute", "phase", "converters",
        "Mount", "prop", "cylinder",

        "Belittle", "micro", "calibrator",
        "Freeze", "heating", "valve",
        "Rewire", "electrical", "smasher",
        "Destroy", "astral", "generator",
        "Shift", "stereo", "pump",
        "Activate", "spark", "rocket",
    };

    public static string[] funnySoundSolutions = new string[] {
        "Supercharge", "subatomic", "synthesizer",
        "Bedazzle", "loadbearing", "mixer",
        "Prime", "overworked", "motor", 
        "Reheat", "antigravity", "gate",
        "Tighten", "spinning", "pedal", 
        "Reinstall", "oriented", "pulley",
        "Defrost", "manual", "lever",
        "Grind", "functional", "port",
        "Magnify", "nuclear", "wheel",
        "Appreciate", "organic", "cable",
        "Unscramble", "negative", "positron",
        "Resize", "derivative", "transporter",
        "Configure", "supersonic", "lockbox", 
        "Cool", "crank", "pipe",
        "Beautify", "ultra", "bolt",
        "Diversify", "mega", "cord", 
        "Enchant", "audiovisual", "lobe",
        "Encrypt", "optic", "switch"
    };

    public static string[] somethingLeakingSolutions = new string[] {
        "Enlarge", "engine", "shaft",
        "Decorate", "missing", "decontaminant",
        "Remodel", "neat", "magic",
        "Contemplate", "automatic", "thing",
        "Jiggle", "Miniscule", "Arm",
        "Tweak", "Overbearing", "Radio",
        "Dangle", "postindustrial", "hand",
        "Equip", "object", "communicator",
        "Modify", "muscle", "bar",
        "Understand", "Global", "Handle",
        "Integrate", "Universal", "Dongle",
        "Acclimate", "Local", "Bearing",
        "Downplay", "positional", "tank",
        "Pinch", "miserable", "wingtip",
        "Press", "enthusiastic", "spoiler",
        "Sideswipe", "Vibrating", "System",
        "Domesticate", "Strobing", "Cone",
        "Empty", "Hydraulic", "Organ",
    };

    public static string[] warningLightSolutions = new string[]{        
        "Reboot", "hydro", "flim",
        "Amplify", "bone", "Piston",
        "Replace", "cooling", "pads",
        "Eject", "spare", "leg",
        "Translate", "Zero", "handle",
        "Rotate", "Imported", "Pole",
        "TurboCharge", "hyper", "grinder",
        "Restart", "mono", "battery",
        "Render", "Torque", "Fluid",
        "Boost", "leaky", "keypad",
        "Override", "prehistoric", "locks",
        "Refill", "unpaid", "Axle",
        "Calibrate", "fuel", "calibrator",
        "Uninstall", "spring", "magic",
        "Undermine", "glove", "box",
        "Coordinate", "heavy", "chain",
        "Change", "Fire", "Output",
        "Flip", "messy", "cog",
    };

    public static string[] GetKeywordsForPartProblem(int row, int col, bool b0, bool b1, bool b2){
        string[] retStrings = new string[3];
        int indexInChart = getIndexInChart(row, col, b0, b1, b2);
        retStrings[0] = brokenPartSolutions[indexInChart];
        retStrings[1] = brokenPartSolutions[indexInChart+1];
        retStrings[2] = brokenPartSolutions[indexInChart+2];
        return retStrings;
    }

    public static string[] GetKeywordsForSoundProblem(int row, int col, bool b0, bool b1, bool b2){
        string[] retStrings = new string[3];
        int indexInChart = getIndexInChart(row, col, b0, b1, b2);
        retStrings[0] = funnySoundSolutions[indexInChart];
        retStrings[1] = funnySoundSolutions[indexInChart+1];
        retStrings[2] = funnySoundSolutions[indexInChart+2];
        return retStrings;
    }

    public static string[] GetKeywordsForLeakingProblem(int row, int col, bool b0, bool b1, bool b2){
        string[] retStrings = new string[3];
        int indexInChart = getIndexInChart(row, col, b0, b1, b2);
        retStrings[0] = somethingLeakingSolutions[indexInChart];
        retStrings[1] = somethingLeakingSolutions[indexInChart+1];
        retStrings[2] = somethingLeakingSolutions[indexInChart+2];
        return retStrings;
    }

    public static string[] GetKeywordsForLightProblem(int row, int col, bool b0, bool b1, bool b2){
        string[] retStrings = new string[3];
        int indexInChart = getIndexInChart(row, col, b0, b1, b2);
        retStrings[0] = warningLightSolutions[indexInChart];
        retStrings[1] = warningLightSolutions[indexInChart+1];
        retStrings[2] = warningLightSolutions[indexInChart+2];
        return retStrings;
    }

    public static string[] AllKeywords(){
        string[] allStrings = new string[
            brokenPartSolutions.Length + 
            funnySoundSolutions.Length + 
            somethingLeakingSolutions.Length + 
            warningLightSolutions.Length
        ];

        int j = 0;
        for(int i = 0; i < brokenPartSolutions.Length; i++){
            allStrings[i+j] = brokenPartSolutions[i];
        };
        j += brokenPartSolutions.Length;

        for(int i = 0; i < funnySoundSolutions.Length; i++){
            allStrings[i+j] = funnySoundSolutions[i];
        };
        j += funnySoundSolutions.Length;

        for(int i = 0; i < somethingLeakingSolutions.Length; i++){
            allStrings[i+j] = somethingLeakingSolutions[i];
        };
        j += somethingLeakingSolutions.Length;

        for(int i = 0; i < warningLightSolutions.Length; i++){
            allStrings[i+j] = warningLightSolutions[i];
        };
        j += warningLightSolutions.Length;

        return allStrings;
    }

    static int getIndexInChart(int row, int col, bool b0, bool b1, bool b2){
        bool boolForColumn = false;
        if (col == 0){
            boolForColumn = b0;
        }
        else if (col == 1){
            boolForColumn = b1;
        }
        else if (col == 2){
            boolForColumn = b2;
        }

        int boolOffset = boolForColumn ? 0 : 1;
        int indexInChart = (row*6) + (col*2) + boolOffset;
        indexInChart *= 3;
        return indexInChart;
    }
}