﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class LoginVariables
    {
        public static bool fileTest = false;
        public static bool enableDraw = true;
        public static bool exiting = false;
        public static int connectionStarted = 0;
        public static bool ignoreDoWork = false;
        public static MainGame mainGame = null;
        public static string[] chatArray = new string[] { "" };

        public static string[] Player1_Current = new string[] { "" };
        public static string[] Player2_Current = new string[] { "" };

        public static int[] opponentCheckArray = new int[0];
        public static bool opponentCheckArrayLock = false;
        public static bool setCheckValues = false;
        public static string dataToSendToOpponent = "";

        public static int[] oppNums = { 0, 0, 0, 0, 0 };
        public static int[] nums = { 0, 0, 0, 0, 0 };

        public static int oppPreviousPos = 0;
        public static int oppCurrentPos = 0;

        public static string dataMoveInProgress = "0";
        public static string dataOppMoveInProgress = "0";

        public static List<string> receivedDataStore = new List<string>();
        public static string incompleteData = "";
        public static string savedData = "";

        public static UInt32[] imageBytes = new UInt32[6];
        public static List<byte[]> savedBytesList = new List<byte[]>();

        public static int[,] coloursArray = {
{171,109,7},
{31,248,240},
{141,51,84},
{99,224,7},
{28,203,252},
{208,229,194},
{34,32,58},
{244,108,170},
{193,168,63},
{112,68,7},
{158,239,62},
{64,90,55},
{49,94,237},
{93,200,34},
{206,178,65},
{174,5,171},
{149,30,118},
{22,180,177},
{199,183,1},
{93,94,79},
{177,29,239},
{76,196,241},
{200,219,112},
{197,159,135},
{213,197,88},
{24,37,185},
{104,18,162},
{147,218,38},
{145,136,59},
{140,120,163},
{54,211,255},
{157,172,171},
{51,130,238},
{80,229,41},
{58,208,127},
{238,65,238},
{89,175,146},
{71,244,76},
{172,29,66},
{201,186,60},
{181,60,99},
{205,142,203},
{135,55,25},
{173,228,162},
{203,24,44},
{121,89,9},
{166,162,71},
{76,51,200},
{16,238,223},
{179,240,142},
{104,106,199},
{177,154,169},
{198,109,65},
{179,102,9},
{213,172,203},
{239,124,220},
{31,51,196},
{56,34,24},
{245,105,9},
{154,188,160},
{236,212,71},
{138,68,122},
{219,25,125},
{131,178,235},
{199,243,59},
{61,119,134},
{253,32,114},
{2,147,189},
{202,47,71},
{175,188,49},
{159,44,176},
{161,199,145},
{194,74,103},
{80,20,98},
{94,156,128},
{216,113,47},
{220,183,241},
{130,156,155},
{45,233,65},
{137,192,21},
{50,43,92},
{76,227,134},
{58,37,43},
{169,123,45},
{31,173,12},
{15,164,246},
{60,241,190},
{24,210,5},
{63,29,144},
{21,115,222},
{80,56,137},
{81,30,232},
{6,17,185},
{250,157,75},
{3,50,60},
{196,98,239},
{143,30,68},
{75,77,207},
{87,26,89},
{87,13,197},
{246,37,177},
{77,89,66},
{163,205,142},
{35,8,146},
{60,65,83},
{115,20,160},
{200,123,186},
{161,133,85},
{153,4,235},
{186,250,4},
{143,175,4},
{243,4,119},
{153,182,216},
{249,32,149},
{96,108,21},
{177,14,11},
{123,227,5},
{184,142,145},
{11,85,160},
{92,179,85},
{87,233,211},
{138,147,89},
{142,73,61},
{219,217,93},
{199,207,90},
{37,64,25},
{64,231,246},
{223,62,22},
{254,215,32},
{115,232,175},
{215,60,3},
{131,162,55},
{193,213,216},
{187,35,197},
{205,160,9},
{167,240,191},
{54,229,20},
{135,28,203},
{95,119,170},
{188,139,188},
{57,27,96},
{142,3,157},
{90,68,44},
{35,99,107},
{64,132,196},
{48,17,158},
{12,87,230},
{97,60,2},
{154,80,32},
{55,35,42},
{140,5,73},
{28,130,8},
{100,63,156},
{108,76,172},
{146,19,130},
{168,70,152},
{7,46,101},
{12,175,52},
{214,216,222},
{94,243,179},
{124,35,33},
{1,150,93},
{33,35,1},
{187,203,210},
{2,160,10},
{57,206,41},
{127,241,9},
{136,48,244},
{196,128,86},
{21,70,170},
{41,6,76},
{232,239,41},
{139,144,124},
{36,135,34},
{150,75,98},
{147,10,15},
{41,156,9},
{181,222,1},
{169,244,243},
{44,60,21},
{7,12,221},
{87,145,197},
{133,25,176},
{225,174,61},
{172,91,106},
{230,242,228},
{146,49,118},
{70,235,170},
{160,116,39},
{74,62,251},
{54,95,83},
{103,29,122},
{69,62,186},
{157,13,240},
{95,72,237},
{204,9,131},
{150,195,2},
{14,154,11},
{158,192,33},
{51,182,29},
{157,58,218},
{180,170,79},
{233,68,62},
{240,161,179},
{41,244,35},
{133,231,162},
{188,212,106},
{164,180,123},
{248,39,196},
{72,200,92},
{89,18,136},
{156,255,210},
{22,170,157},
{157,226,196},
{69,166,33},
{78,229,165},
{232,25,216},
{126,118,226},
{178,248,134},
{94,187,131},
{138,161,124},
{225,150,143},
{43,141,245},
{80,111,61},
{5,30,241},
{165,95,13},
{89,170,66},
{31,96,175},
{131,219,155},
{166,234,6},
{76,183,238},
{26,19,65},
{72,99,15},
{146,35,87},
{175,151,169},
{154,120,88},
{240,12,153},
{121,5,17},
{64,107,65},
{204,93,187},
{244,133,163},
{238,4,184},
{79,234,11},
{176,93,252},
{254,198,134},
{234,111,200},
{5,232,98},
{67,182,221},
{109,211,177},
{220,217,168},
{0,124,5},
{83,158,158},
{180,192,146},
{241,75,141},
{76,63,175},
{156,123,62}
        };
    }
}
