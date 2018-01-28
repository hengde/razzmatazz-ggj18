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

    public static string[] GetKeywordsForPartProblem(int row, int col, bool b0, bool b1, bool b2){
        string[] retStrings = new string[3];

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

        retStrings[0] = brokenPartSolutions[indexInChart];
        retStrings[1] = brokenPartSolutions[indexInChart+1];
        retStrings[2] = brokenPartSolutions[indexInChart+2];

        return retStrings;
    }


    public static string[] AllKeywords(){
        return brokenPartSolutions;
    }
}