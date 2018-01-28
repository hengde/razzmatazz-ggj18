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
        "Hug", "drive", "bulb",
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

    public static string[] AllKeywords(){
        string[] allStrings = new string[
            brokenPartSolutions.Length + 
            funnySoundSolutions.Length
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