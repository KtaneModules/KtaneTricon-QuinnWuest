using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class TriconScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public KMSelectable[] ArrowSels;
    public KMSelectable SubmitSel;
    public SpriteRenderer ModuleSprite;
    public TextMesh ModuleNameText;
    public GameObject[] LedObjs;
    public Material LedMatGreen;
    public Sprite FailsafeSprite;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private bool _readyToPress;
    private int _currentIx;
    private int[] _pickIxs;
    private int[] _displayIxs;
    private int[] _solutionIxs;
    private readonly List<int> _submittedIxs = new List<int>();
    private const int _maxIx = 40;
    private bool _failSafeActive;

    public Texture2D ModuleTexture;
    private readonly Texture2D[] Textures = new Texture2D[3];
    private readonly Texture2D[] MergedTextures = new Texture2D[8];
    private int _currentDisplayValue = 7;

    private static readonly string[][] _moduleList = NewArray
    (
        new string[] { "Wire Sequence", "WireSequence" },
        new string[] { "Wires", "Wires" },
        new string[] { "Who's on First", "WhosOnFirst" },
        new string[] { "Simon Says", "Simon" },
        new string[] { "Password", "Password" },
        new string[] { "Morse Code", "Morse" },
        new string[] { "Memory", "Memory" },
        new string[] { "Maze", "Maze" },
        new string[] { "Keypad", "Keypad" },
        new string[] { "Complicated Wires", "Venn" },
        new string[] { "Colour Flash", "ColourFlash" },
        new string[] { "Piano Keys", "PianoKeys" },
        new string[] { "Semaphore", "Semaphore" },
        new string[] { "Switches", "switchModule" },
        new string[] { "Two Bits", "TwoBits" },
        new string[] { "Word Scramble", "WordScrambleModule" },
        new string[] { "Anagrams", "AnagramsModule" },
        new string[] { "Round Keypad", "KeypadV2" },
        new string[] { "Listening", "Listening" },
        new string[] { "Orientation Cube", "OrientationCube" },
        new string[] { "Morsematics", "MorseV2" },
        new string[] { "Connection Check", "graphModule" },
        new string[] { "Forget Me Not", "MemoryV2" },
        new string[] { "Astrology", "spwizAstrology" },
        new string[] { "Logic", "Logic" },
        new string[] { "Mystic Square", "MysticSquareModule" },
        new string[] { "Adventure Game", "spwizAdventureGame" },
        new string[] { "Safety Safe", "PasswordV2" },
        new string[] { "Chess", "ChessModule" },
        new string[] { "Mouse In The Maze", "MouseInTheMaze" },
        new string[] { "Silly Slots", "SillySlots" },
        new string[] { "Probing", "Probing" },
        new string[] { "Skewed Slots", "SkewedSlotsModule" },
        new string[] { "Murder", "murder" },
        new string[] { "The Gamepad", "TheGamepadModule" },
        new string[] { "Tic Tac Toe", "TicTacToeModule" },
        new string[] { "Shape Shift", "shapeshift" },
        new string[] { "Follow The Leader", "FollowTheLeaderModule" },
        new string[] { "The Bulb", "TheBulbModule" },
        new string[] { "Sea Shells", "SeaShells" },
        new string[] { "Rock-Paper-Scissors-Lizard-Spock", "RockPaperScissorsLizardSpockModule" },
        new string[] { "Hexamaze", "HexamazeModule" },
        new string[] { "Bitmaps", "BitmapsModule" },
        new string[] { "Colored Squares", "ColoredSquaresModule" },
        new string[] { "Word Search", "WordSearchModule" },
        new string[] { "Broken Buttons", "BrokenButtonsModule" },
        new string[] { "Simon Screams", "SimonScreamsModule" },
        new string[] { "Modules Against Humanity", "ModuleAgainstHumanity" },
        new string[] { "Complicated Buttons", "complicatedButtonsModule" },
        new string[] { "Symbolic Password", "symbolicPasswordModule" },
        new string[] { "Wire Placement", "WirePlacementModule" },
        new string[] { "Cheap Checkout", "CheapCheckoutModule" },
        new string[] { "Coordinates", "CoordinatesModule" },
        new string[] { "Light Cycle", "LightCycleModule" },
        new string[] { "Rhythms", "MusicRhythms" },
        new string[] { "Color Math", "colormath" },
        new string[] { "Only Connect", "OnlyConnectModule" },
        new string[] { "Creation", "CreationModule" },
        new string[] { "Rubik's Cube", "RubiksCubeModule" },
        new string[] { "FizzBuzz", "fizzBuzzModule" },
        new string[] { "The Clock", "TheClockModule" },
        new string[] { "Bitwise Operations", "BitOps" },
        new string[] { "Fast Math", "fastMath" },
        new string[] { "Zoo", "ZooModule" },
        new string[] { "Binary LEDs", "BinaryLeds" },
        new string[] { "Boolean Venn Diagram", "booleanVennModule" },
        new string[] { "Ice Cream", "iceCreamModule" },
        new string[] { "The Screw", "screw" },
        new string[] { "Yahtzee", "YahtzeeModule" },
        new string[] { "X-Ray", "XRayModule" },
        new string[] { "Color Morse", "ColorMorseModule" },
        new string[] { "Gridlock", "GridlockModule" },
        new string[] { "Big Circle", "BigCircle" },
        new string[] { "S.E.T.", "SetModule" },
        new string[] { "Painting", "Painting" },
        new string[] { "Symbol Cycle", "SymbolCycleModule" },
        new string[] { "Hunting", "hunting" },
        new string[] { "Extended Password", "ExtendedPassword" },
        new string[] { "Curriculum", "curriculum" },
        new string[] { "Mafia", "MafiaModule" },
        new string[] { "Flags", "FlagsModule" },
        new string[] { "Polyhedral Maze", "PolyhedralMazeModule" },
        new string[] { "Symbolic Coordiantes", "symbolicCoordinates" },
        new string[] { "Poker", "Poker" },
        new string[] { "Poetry", "poetry" },
        new string[] { "Button Sequence", "buttonSequencesModule" },
        new string[] { "Algebra", "algebra" },
        new string[] { "Visual Impairment", "visual_impairment" },
        new string[] { "Blind Maze", "BlindMaze" },
        new string[] { "Mashematics", "mashematics" },
        new string[] { "LED Grid", "ledGrid" },
        new string[] { "Sink", "Sink" },
        new string[] { "Human Resources", "HumanResourcesModule" },
        new string[] { "Burglar Alarm", "burglarAlarm" },
        new string[] { "Press X", "PressX" },
        new string[] { "Error Codes", "errorCodes" },
        new string[] { "LEGOs", "LEGOModule" },
        new string[] { "Rubik's Clock", "rubiksClock" },
        new string[] { "Font Select", "FontSelect" },
        new string[] { "The London Underground", "londonUnderground" },
        new string[] { "Forget Everything", "HexiEvilFMN" },
        new string[] { "Color Decoding", "Color Decoding" },
        new string[] { "Playfair Cipher", "Playfair" },
        new string[] { "Cooking", "cooking" },
        new string[] { "Superlogic", "SuperlogicModule" },
        new string[] { "Digital Root", "digitalRoot" },
        new string[] { "Marble Tumble", "MarbleTumbleModule" },
        new string[] { "X01", "X01" },
        new string[] { "Logical Buttons", "logicalButtonsModule" },
        new string[] { "Simon Sings", "SimonSingsModule" },
        new string[] { "Simon Sends", "SimonSendsModule" },
        new string[] { "Synonyms", "synonyms" },
        new string[] { "Simon Shrieks", "SimonShrieksModule" },
        new string[] { "Lasers", "lasers" },
        new string[] { "Binary Tree", "binaryTree" },
        new string[] { "Black Hole", "BlackHoleModule" },
        new string[] { "Mineseeker", "mineseeker" },
        new string[] { "Maze Scrambler", "MazeScrambler" },
        new string[] { "Double Color", "doubleColor" },
        new string[] { "Maritime Flags", "MaritimeFlagsModule" },
        new string[] { "Pattern Cube", "PatternCubeModule" },
        new string[] { "Splitting The Loot", "SplittingTheLootModule" },
        new string[] { "Character Shift", "characterShift" },
        new string[] { "Dragon Energy", "dragonEnergy" },
        new string[] { "Synchronization", "SynchronizationModule" },
        new string[] { "Shikaku", "shikaku" },
        new string[] { "Signals", "Signals" },
        new string[] { "Horrible Memory", "horribleMemory" },
        new string[] { "Boolean Maze", "boolMaze" },
        new string[] { "Quintuples", "quintuples" },
        new string[] { "Lion's Share", "LionsShareModule" },
        new string[] { "The Plunger Button", "plungerButton" },
        new string[] { "T-Words", "tWords" },
        new string[] { "Divided Squares", "DividedSquaresModule" },
        new string[] { "Instructions", "instructions" },
        new string[] { "Encrypted Morse", "EncryptedMorse" },
        new string[] { "IKEA", "qSwedishMaze" },
        new string[] { "Mahjong", "MahjongModule" },
        new string[] { "Kudosudoku", "KudosudokuModule" },
        new string[] { "Challenge & Contact", "challengeAndContact" },
        new string[] { "Sueet Wall", "SueetWall" },
        new string[] { "Simon Spins", "SimonSpinsModule" },
        new string[] { "Ten-Button Color Code", "TenButtonColorCode" },
        new string[] { "Spinning Buttons", "spinningButtons" },
        new string[] { "Factory Maze", "factoryMaze" },
        new string[] { "Binary Puzzle", "BinaryPuzzleModule" },
        new string[] { "Regular Crazy Talk", "RegularCrazyTalkModule" },
        new string[] { "Krazy Talk", "krazyTalk" },
        new string[] { "Numbers", "Numbers" },
        new string[] { "Free Parking", "freeParking" },
        new string[] { "Mad Memory", "MadMemory" },
        new string[] { "Bartending", "BartendingModule" },
        new string[] { "Question Mark", "Questionmark" },
        new string[] { "SYNC-125 [3]", "sync125_3" },
        new string[] { "LED Math", "lgndLEDMath" },
        new string[] { "Unfair Cipher", "unfairCipher" },
        new string[] { "Left and Right", "leftandRight" },
        new string[] { "Gadgetron Vendor", "lgndGadgetronVendor" },
        new string[] { "Genetic Sequence", "geneticSequence" },
        new string[] { "Module Maze", "ModuleMaze" },
        new string[] { "Purgatory", "PurgatoryModule" },
        new string[] { "Lombax Cubes", "lgndLombaxCubes" },
        new string[] { "Graphic Memory", "graphicMemory" },
        new string[] { "Quiz Buzz", "quizBuzz" },
        new string[] { "Wavetapping", "Wavetapping" },
        new string[] { "The Hypercube", "TheHypercubeModule" },
        new string[] { "Planets", "planets" },
        new string[] { "The Necronomicon", "necronomicon" },
        new string[] { "Four-Card Monte", "Krit4CardMonte" },
        new string[] { "The Giant's Drink", "giantsDrink" },
        new string[] { "Digit String", "digitString" },
        new string[] { "Vexillology", "vexillology" },
        new string[] { "Brush Strokes", "brushStrokes" },
        new string[] { "Odd One Out", "OddOneOutModule" },
        new string[] { "Mazematics", "mazematics" },
        new string[] { "Gryphons", "gryphons" },
        new string[] { "Arithmelogic", "arithmelogic" },
        new string[] { "Simon Stops", "simonStops" },
        new string[] { "Baba Is Who?", "babaIsWho" },
        new string[] { "Simon Stores", "simonStores" },
        new string[] { "Daylight Directions", "daylightDirections" },
        new string[] { "Bamboozling Button", "bamboozlingButton" },
        new string[] { "Forget Them All", "forgetThemAll" },
        new string[] { "Ordered Keys", "orderedKeys" },
        new string[] { "Hyperactive Numbers", "lgndHyperactiveNumbers" },
        new string[] { "Button Grid", "buttonGrid" },
        new string[] { "Find The Date", "DateFinder" },
        new string[] { "The Matrix", "matrix" },
        new string[] { "Seven Deadly Sins", "sevenDeadlySins" },
        new string[] { "The Deck of Many Things", "deckOfManyThings" },
        new string[] { "Raiding Temples", "raidingTemples" },
        new string[] { "Bomb Diffusal", "bombDiffusal" },
        new string[] { "Double Expert", "doubleExpert" },
        new string[] { "Boolean Keypad", "BooleanKeypad" },
        new string[] { "Toon Enough", "toonEnough" },
        new string[] { "Pictionary", "pictionaryModule" },
        new string[] { "Qwirkle", "qwirkle" },
        new string[] { "Antichamber", "antichamber" },
        new string[] { "Simon Simons", "simonSimons" },
        new string[] { "Constellations", "constellations" },
        new string[] { "Prime Checker", "PrimeChecker" },
        new string[] { "Boot Too Big", "bootTooBig" },
        new string[] { "Old Fogey", "oldFogey" },
        new string[] { "Insanagrams", "insanagrams" },
        new string[] { "Treasure Hunt", "treasureHunt" },
        new string[] { "Bamboozled Again", "bamboozledAgain" },
        new string[] { "Safety Square", "safetySquare" },
        new string[] { "Double Arrows", "doubleArrows" },
        new string[] { "Boolean Wires", "booleanWires" },
        new string[] { "Vectors", "vectorsModule" },
        new string[] { "Jumble Cycle", "jumbleCycle" },
        new string[] { "Organization", "organizationModule" },
        new string[] { "Alpha-Bits", "alphaBits" },
        new string[] { "Chord Progressions", "chordProgressions" },
        new string[] { "Matchematics", "matchematics" },
        new string[] { "Bob Barks", "ksmBobBarks" },
        new string[] { "Simon Selects", "simonSelectsModule" },
        new string[] { "Robot Programming", "robotProgramming" },
        new string[] { "Masyu", "masyuModule" },
        new string[] { "Flash Memory", "FlashMemory" },
        new string[] { "A-maze-ing Buttons", "ksmAmazeingButtons" },
        new string[] { "TetraVex", "ksmTetraVex" },
        new string[] { "Meter", "meter" },
        new string[] { "Timing Is Everything", "timingIsEverything" },
        new string[] { "The Modkit", "modkit" },
        new string[] { "The Rule", "theRule" },
        new string[] { "Fruits", "fruits" },
        new string[] { "Bamboozing Button Grid", "bamboozlingButtonGrid" },
        new string[] { "Footnotes", "footnotes" },
        new string[] { "Lousy Chess", "lousyChess" },
        new string[] { "Module Listening", "moduleListening" },
        new string[] { "Kooky Keypad", "kookyKeypadModule" },
        new string[] { "RGB Maze", "rgbMaze" },
        new string[] { "The Legendre Symbol", "legendreSymbol" },
        new string[] { "Keypad Lock", "keypadLock" },
        new string[] { "Encryption Bingo", "encryptionBingo" },
        new string[] { "Color Addition", "colorAddition" },
        new string[] { "Chinese Counting", "chineseCounting" },
        new string[] { "Tower of Hanoi", "towerOfHanoi" },
        new string[] { "Keypad Combinations", "keypadCombinations" },
        new string[] { "Ternary Convertor", "qkTernaryConverter" },
        new string[] { "N&Ms", "NandMs" },
        new string[] { "The Colored Maze", "coloredMaze" },
        new string[] { "Loopover", "loopover" },
        new string[] { "Divisible Numbers", "divisibleNumbers" },
        new string[] { "The High Score", "ksmHighScore" },
        new string[] { "Ingredients", "ingredients" },
        new string[] { "Intervals", "intervals" },
        new string[] { "Thinking Wires", "thinkingWiresModule" },
        new string[] { "Seven Choose Four", "sevenChooseFour" },
        new string[] { "Natures", "mcdNatures" },
        new string[] { "Scavenger Hunt", "scavengerHunt" },
        new string[] { "Blinkstop", "blinkstopModule" },
        new string[] { "Answering Can Be Fun", "AnsweringCanBeFun" },
        new string[] { "Rainbow Arrows", "ksmRainbowArrows" },
        new string[] { "Time Signatures", "timeSignatures" },
        new string[] { "Passcodes", "xtrpasscodes" },
        new string[] { "Hereditary Base Notation", "hereditaryBaseNotationModule" },
        new string[] { "Lines of Code", "linesOfCode" },
        new string[] { "Encrypted Dice", "EncryptedDice" },
        new string[] { "The Black Page", "TheBlackPage" },
        new string[] { "64", "64" },
        new string[] { "Simon Forgets", "simonForgets" },
        new string[] { "Greek Letter Grid", "greekLetterGrid" },
        new string[] { "Keywords", "xtrkeywords" },
        new string[] { "State of Aggregation", "stateOfAggregation" },
        new string[] { "Topsy Turvy", "topsyTurvy" },
        new string[] { "Railway Cargo Loading", "RailwayCargoLoading" },
        new string[] { "Semamorse", "semamorse" },
        new string[] { "Widdershins", "widdershins" },
        new string[] { "Dimension Disruption", "dimensionDisruption" },
        new string[] { "Dungeon", "dungeon" },
        new string[] { "Alphabetize", "Alphabetize" },
        new string[] { "Light Bulbs", "LightBulbs" },
        new string[] { "1000 Words", "OneThousandWords" },
        new string[] { "Five Letter Words", "FiveLetterWords" },
        new string[] { "Directional Button", "directionalButton" },
        new string[] { "...?", "punctuationMarks" },
        new string[] { "The Simpleton", "SimpleButton" },
        new string[] { "Wire Ordering", "kataWireOrdering" },
        new string[] { "Pattern Lock", "patternLock" },
        new string[] { "Quick Arithmetic", "QuickArithmetic" },
        new string[] { "Reverse Polish Notation", "revPolNot" },
        new string[] { "NumberWang", "kikiNumberWang" },
        new string[] { "Fencing", "fencing" },
        new string[] { "The Twin", "TheTwinModule" },
        new string[] { "Name Changer", "nameChanger" },
        new string[] { "Wonder Cipher", "WonderCipher" },
        new string[] { "Caesar's Maths", "caesarsMaths" },
        new string[] { "Siffron", "siffron" },
        new string[] { "Audio Morse", "lgndAudioMorse" },
        new string[] { "Type Racer", "typeRacer" },
        new string[] { "Negativity", "Negativity" },
        new string[] { "Masher The Bottun", "masherTheBottun" },
        new string[] { "3 LEDs", "threeLEDsModule" },
        new string[] { "Thread the Needle", "threadTheNeedle" },
        new string[] { "The Heart", "TheHeart" },
        new string[] { "Reflex", "lgndReflex" },
        new string[] { "More Code", "MoreCode" },
        new string[] { "Basic Morse", "BasicMorse" },
        new string[] { "Dumb Waiters", "dumbWaiters" },
        new string[] { "Bridges", "bridges" },
        new string[] { "Amnesia", "Amnesia" },
        new string[] { "Synesthesia", "synesthesia" },
        new string[] { "English Entries", "EnglishEntries" },
        new string[] { "Factoring", "factoring" },
        new string[] { "Puzzword", "PuzzwordModule" },
        new string[] { "int##", "int##" },
        new string[] { "Blind Arrows", "blindArrows" },
        new string[] { "Sound Design", "soundDesign" },
        new string[] { "RGB Arithmetic", "rgbArithmetic" },
        new string[] { "Prime Time", "primeTime" },
        new string[] { "Negation", "xelNegation" },
        new string[] { "ASCII Maze", "asciiMaze" },
        new string[] { "Ultralogic", "Ultralogic" },
        new string[] { "Spangled Stars", "spangledStars" },
        new string[] { "Digital Clock", "digitalClock" },
        new string[] { "Color Numbers", "colorNumbers" },
        new string[] { "Chinese Strokes", "zhStrokes" },
        new string[] { "0", "0" },
        new string[] { "Pitch Perfect", "pitchPerfect" },
        new string[] { "Increasing Indices", "increasingIndices" },
        new string[] { "Connected Monitors", "ConnectedMonitorsModule" },
        new string[] { "Alien Filing Colors", "AlienModule" },
        new string[] { "Color One Two", "colorOneTwo" },
        new string[] { "Spelling Buzzed", "SpellingBuzzed" },
        new string[] { "Burnout", "kataBurnout" },
        new string[] { "Mystic Maze", "mysticmaze" },
        new string[] { "Four Lights", "fourLights" },
        new string[] { "Tenpins", "tenpins" },
        new string[] { "Regular Hexpressions", "RegularHexpressions" },
        new string[] { "Censorship", "Censorship" },
        new string[] { "Mazery", "Mazery" },
        new string[] { "Metamem", "metamem" },
        new string[] { "Bridge", "bridge" },
        new string[] { "Beans", "beans" },
        new string[] { "The Dials", "TheDials" },
        new string[] { "Chamber No. 5", "ChamberNoFive" },
        new string[] { "Silenced Simon", "SilencedSimon" },
        new string[] { "Frankenstein's Indicator", "frankensteinsIndicator" },
        new string[] { "Double Pitch", "DoublePitch" },
        new string[] { "Devilish Eggs", "devilishEggs" },
        new string[] { "h", "Averageh" },
        new string[] { "Quick Time Events", "xelQuickTimeEvents" },
        new string[] { "The Bioscanner", "TheBioscanner" },
        new string[] { "Pixel Number Page", "PixelNumberBase" },
        new string[] { "Logical Operators", "logicalOperators" },
        new string[] { "Even Or Odd", "evenOrOdd" },
        new string[] { "Digital Grid", "digitalGrid" },
        new string[] { "Gettin' Funky", "gettinFunkyModule" },
        new string[] { "Cell Lab", "cellLab" },
        new string[] { "Color Hexagons", "colorHexagons" },
        new string[] { "Commuting", "commuting" },
        new string[] { "Look and Say", "LookAndSay" },
        new string[] { "Currents", "Currents" },
        new string[] { "Taco Tuesday", "tacoTuesday" },
        new string[] { "Melodic Message", "melodicMessage" },
        new string[] { "Semabols", "xelSemabols" },
        new string[] { "Mislocation", "mislocation" },
        new string[] { "Going Backwards", "GoingBackwards" },
        new string[] { "Video Poker", "videoPoker" },
        new string[] { "Towers", "Towers" },
        new string[] { "Updog", "Updog" },
        new string[] { "Etch-A-Sketch", "etchASketch" },
        new string[] { "Tetris Sprint", "tetrisSprint" },
        new string[] { "Forget Maze Not", "forgetMazeNot" },
        new string[] { "Double Screen", "doubleScreenModule" },
        new string[] { "The Klaxon", "klaxon" },
        new string[] { "Simon Subdivides", "simonSubdivides" },
        new string[] { "Boob Tube", "boobTubeModule" },
        new string[] { "Polyrhythms", "polyrhythms" },
        new string[] { "Drive-In Window", "DIWindow" },
        new string[] { "Rebooting M-OS", "RebootingM-Os" },
        new string[] { "Simon Stumbles", "simonStumbles" },
        new string[] { "Colored Letters", "ColoredLetters" },
        new string[] { "Ultra Digital Root", "ultraDigitalRootModule" },
        new string[] { "Simon Swindles", "simonSwindles" },
        new string[] { "Access Codes", "GSAccessCodes" },
        new string[] { "Interpunct", "interpunct" },
        new string[] { "The 1, 2, 3 Game", "TheOneTwoThreeGame" },
        new string[] { "Newline", "newline" },
        new string[] { "Ghost Movement", "ghostMovement" },
        new string[] { "Amusement Parks", "amusementParks" },
        new string[] { "hexOrbits", "hexOrbits" },
        new string[] { "Ladders", "ladders" },
        new string[] { "Mssngv Wls", "MssngvWls" },
        new string[] { "Simon Supports", "simonSupports" },
        new string[] { "Numpath", "numpath" },
        new string[] { "Turn Four", "turnFour" },
        new string[] { "Dice Cipher", "diceCipher" },
        new string[] { "Salts", "salts" },
        new string[] { "Infinite Loop", "InfiniteLoop" },
        new string[] { "Classical Order", "classicalOrder" },
        new string[] { "Cartinese", "cartinese" },
        new string[] { "Stupid Slots", "stupidSlots" },
        new string[] { "Squeeze", "squeeze" },
        new string[] { "Meteor", "meteor" },
        new string[] { "Pawns", "pawns" },
        new string[] { "Simon Shapes", "SimonShapesModule" },
        new string[] { "Simon Shouts", "SimonShoutsModule" },
        new string[] { "Marquee Morse", "marqueeMorseModule" },
        new string[] { "Line Equations", "GSLineEquations" },
        new string[] { "Starmap Reconstruction", "starmap_reconstruction" },
        new string[] { "Pointless Machines", "PointlessMachines" },
        new string[] { "Stability", "stabilityModule" },
        new string[] { "Warning Signs", "warningSigns" },
        new string[] { "Walking Cube", "WalkingCubeModule" },
        new string[] { "Skewers", "Skewers" },
        new string[] { "Mind Meld", "mindMeld" },
        new string[] { "Insa Ilo", "insaIlo" },
        new string[] { "Placement Roulette", "PlacementRouletteModule" },
        new string[] { "Art Pricing", "artPricing" },
        new string[] { "Wire Association", "WireAssociationModule" },
        new string[] { "The Garnet Thief", "theGarnetThief" },
        new string[] { "Flyswatting", "flyswatting" },
        new string[] { "M-Seq", "mSeq" },
        new string[] { "Touch Transmission", "touchTransmission" },
        new string[] { "Superparsing", "superparsing" },
        new string[] { "Sorry Sliders", "SorrySliders" },
        new string[] { "Candy Land", "candyLand" },
        new string[] { "Melody Memory", "melodyMemory" },
        new string[] { "Eight Tiles Panic", "eightTilesPanic" },
        new string[] { "Binary Tango", "binaryTango" },
        new string[] { "Variety", "VarietyModule" },
        new string[] { "Mazeseeker", "GSMazeseeker" },
        new string[] { "Game of Colors", "GameOfColors" },
        new string[] { "Spilling Paint", "spillingPaint" },
        new string[] { "Metapuzzle", "metapuzzle" },
        new string[] { "Simon Signals", "SimonSignalsModule" },
        new string[] { "Order Picking", "OrderPickingModule" },
        new string[] { "Binary Morse", "binaryMorse" },
        new string[] { "Sprouts", "sprouts" },
        new string[] { "Simon Stacks", "simonstacks" },
        new string[] { "Letter Math", "letterMath" },
        new string[] { "Invisymbol", "invisymbol" },
        new string[] { "Gendercipher", "gendercipher" },
        new string[] { "Double Digits", "doubleDigitsModule" },
        new string[] { "Twister", "TwisterModule" },
        new string[] { "Omni-Man", "omniman" },
        new string[] { "Presidential Elections", "presidentialElections" },
        new string[] { "Quilting", "quilting" },
        new string[] { "Character Slots", "characterSlots" },
        new string[] { "Switch Placement", "switchPlacement" },
        new string[] { "Baffling Box", "bafflingBox" },
        new string[] { "Ro", "ro" },
        new string[] { "Gyromaze", "gyromaze" },
        new string[] { "Marco Polo", "marcoPolo" },
        new string[] { "Boomtar the Great", "boomtarTheGreat" },
        new string[] { "A Square", "ASquareModule" },
        new string[] { "Look, Look Away", "lookLookAway" },
        new string[] { "Untouchable", "untouchableModule" },
        new string[] { "Backdoor Hacking", "BackdoorHacking" },
        new string[] { "Finite Loop", "finiteLoop" },
        new string[] { "Shape Fill", "ShapeFillModule" },
        new string[] { "Mister Softee", "misterSoftee" },
        new string[] { "Basegate", "basegate" },
        new string[] { "USA Cycle", "USACycle" },
        new string[] { "The Neutral Button", "NeutralButtonModule" },
        new string[] { "Halli Galli", "halliGalli" },
        new string[] { "Add Nauseam", "addNauseam" },
        new string[] { "Notes", "notes" },
        new string[] { "8", "GSEight" },
        new string[] { "X-Ring", "xring" },
        new string[] { "Secret Santa", "SecretSanta" },
        new string[] { "Knot Wires", "knotWires" },
        new string[] { "Magic Square", "MagicSquare" },
        new string[] { "Multi-Buttons", "Multibuttons" },
        new string[] { "Feature Cryptography", "featureCryptography" },
        new string[] { "Simon Smothers", "simonSmothers" },
        new string[] { "Simon's Statement", "simonsStatement" },
        new string[] { "Three-Way Gates", "threewayGates" },
        new string[] { "UIN(+L)", "UINpL" },
        new string[] { "Battle of Wits", "battleOfWits" },
        new string[] { "One Item One Meal", "oneitemOneMeal" },
        new string[] { "LEAN!!!", "Lean" },
        new string[] { "Nifty Number", "NiftyNumber" },
        new string[] { "Fitting In", "FittingInModule" },
        new string[] { "Game Changer", "gameChangerModule" },
        new string[] { "RGB Quiz", "RGBQuiz" },
        new string[] { "Gem Division", "gemDivision" },
        new string[] { "Tetramorse", "Tetramorse" },
        new string[] { "The Phrase Maze", "thePhraseMazeModule" },
        new string[] { "Pip-Nado", "pip-Nado" },
        new string[] { "Reading Between the Lines", "ReadingBetweentheLines" },
        new string[] { "Bomb It!", "BombItModule" },
        new string[] { "Flashing Circles", "flashingCircles" },
        new string[] { "Module Maneuvers", "moduleManeuvers" },
        new string[] { "Memory Character", "MemoryCharacter" },
        new string[] { "Scrutiny Squares", "scrutinySquares" },
        new string[] { "Manual Malady", "manualMalady" },
        new string[] { "Captcha The Flag", "captchaTheFlag" },
        new string[] { "Rambopo", "Rambopo" },
        new string[] { "Epic Shapes", "epicShapes" },
        new string[] { "Shapes and Colors", "shapesAndColors" },
        new string[] { "Simon Swivels", "simonSwivels" },
        new string[] { "Robo-Scanner", "roboScannerModule" },
        new string[] { "Denial Displays", "DenialDisplaysModule" },
        new string[] { "Thermostat", "thermostat" },
        new string[] { "Coloured Cubes", "ColouredCubes" },
        new string[] { "Multicolored Digits", "MulticoloredDigits" },
        new string[] { "WASD", "wasdModule" },
        new string[] { "Two Knobs", "TwoKnobs" },
        new string[] { "Grand Piano", "grandPiano" },
        new string[] { "Clockworks", "clockworks" },
        new string[] { "Shy Guy Says", "ShyGuySays" },
        new string[] { "Ed Balls", "edBalls" },
        new string[] { "Ill Logic", "illLogic" },
        new string[] { "chip", "chip" },
        new string[] { "Many Poly", "manyPoly" },
        new string[] { "RGB Chess", "rgbChess" },
        new string[] { "Game of Ants", "gameofAnts" },
        new string[] { "Big Bike Repair", "bigBikeRepair" },
        new string[] { "Dice Stack", "diceStack" },
        new string[] { "Reflex Factory", "reflexFactoryModule" },
        new string[] { "Wire Testing", "wireTesting" },
        new string[] { "Crazy Hamburger", "GSCrazyHamburger" },
        new string[] { "Toe Tactics", "toeTactics" },
        new string[] { "The Fuse Box", "FuseBox" },
        new string[] { "Swish", "swish" },
        new string[] { "Simultaneous Simons", "simultaneousSimons" },
        new string[] { "Conditional Maze", "conditionalMaze" },
        new string[] { "3D Chess", "ThreeDimensionalChess" },
        new string[] { "Bad Bones", "badbones" },
        new string[] { "Word Count", "wordCount" },
        new string[] { "Diamonds", "diamonds" },
        new string[] { "Power Button", "powerButton" },
        new string[] { "TAC", "TACModule" },
        new string[] { "Reflection", "laserreflection" },
        new string[] { "Synapse Says", "synapseSays" },
        new string[] { "Simon Strands", "simonStrands" },
        new string[] { "Keypass", "keypass" },
        new string[] { "A-button-ing Mazes", "abuttoningMazes" },
        new string[] { "Thirty Dollar Module", "ThirtyDollarModule" },
        new string[] { "Game of Life 3D", "GSGameOfLife3D" },
        new string[] { "Setting Charges", "settingCharges" },
        new string[] { "White Elephant", "whiteElephant" },
        new string[] { "The Teardrop", "theTeardrop" },
        new string[] { "Emoji", "emoji" },
        new string[] { "Soda", "soda" },
        new string[] { "Ill Morse", "illMorse" },
        new string[] { "Ducks", "ducks" },
        new string[] { "Facets & Logic", "facetsAndLogic" },
        new string[] { "Symmetry Shuffle", "symmetryShuffle" },
        new string[] { "Binary Squares", "binarySquares" },
        new string[] { "Normal Probability", "GSNormalProbability" },
        new string[] { "Karnaugh Map", "karnaugh" },
        new string[] { "Lying Buttons", "lyingButtons" },
        new string[] { "Dyscalculator", "dyscalc" },
        new string[] { "Logic Circuits", "LogicCircuits" },
        new string[] { "Some Buttons", "someButtons" },
        new string[] { "Four Corners", "fourCorners" },
        new string[] { "Ball Dial", "ballDial" },
        new string[] { "Breakers", "breakers" },
        new string[] { "Scattershot", "scattershot" },
        new string[] { "Grimm", "grimmModule" },
        new string[] { "RGB Mixing", "RGBLedsBTBE" },
        new string[] { "WAR", "WAR" },
        new string[] { "Gerrymandering", "gerrymandering" },
        new string[] { "Housecleaning", "housecleaningModule" },
        new string[] { "Projective Set", "proset" },
        new string[] { "Permuto", "permuto" }
    );

    private static T[] NewArray<T>(params T[] array) { return array; }

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        IconFetch.Instance.WaitForFetch(OnFetched);
        for (int i = 0; i < ArrowSels.Length; i++)
            ArrowSels[i].OnInteract += ArrowPress(i);
        SubmitSel.OnInteract += SubmitPress;
    }

    private KMSelectable.OnInteractHandler ArrowPress(int i)
    {
        return delegate ()
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, ArrowSels[i].transform);
            ArrowSels[i].AddInteractionPunch(0.5f);
            if (_moduleSolved || !_readyToPress)
                return false;
            if (_failSafeActive)
            {
                _moduleSolved = true;
                Module.HandlePass();
                return false;
            }
            CycleDisplay(i);
            return false;
        };
    }

    private void CycleDisplay(int dir)
    {
        _currentIx = dir == 1 ? ((_currentIx + 1) % _maxIx) : ((_currentIx + (_maxIx - 1)) % _maxIx);
        ModuleNameText.text = _moduleList[_displayIxs[_currentIx]][0];
    }

    private bool SubmitPress()
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, SubmitSel.transform);
        SubmitSel.AddInteractionPunch(0.5f);
        if (_moduleSolved || !_readyToPress)
            return false;
        if (_failSafeActive)
        {
            _moduleSolved = true;
            Module.HandlePass();
            return false;
        }
        if (_solutionIxs.Contains(_displayIxs[_currentIx]))
        {
            if (_submittedIxs.Contains(_displayIxs[_currentIx]))
                return false;
            Debug.LogFormat("[Tricon #{0}] Correctly submitted {1}.", _moduleId, _moduleList[_displayIxs[_currentIx]][0]);
            _submittedIxs.Add(_displayIxs[_currentIx]);
            for (int i = 0; i < _submittedIxs.Count; i++)
                LedObjs[i].GetComponent<MeshRenderer>().material = LedMatGreen;
            int ix = Array.IndexOf(_solutionIxs, _displayIxs[_currentIx]);
            if (ix == 0)
                _currentDisplayValue -= 1;
            if (ix == 1)
                _currentDisplayValue -= 2;
            if (ix == 2)
                _currentDisplayValue -= 4;
            ModuleSprite.sprite = Sprite.Create(MergedTextures[_currentDisplayValue], new Rect(0.0f, 0.0f, MergedTextures[_currentDisplayValue].width, MergedTextures[_currentDisplayValue].height), new Vector2(0.5f, 0.5f), 100.0f);
            if (_submittedIxs.Count == 3)
            {
                Debug.LogFormat("[Tricon #{0}] Module solved.", _moduleId);
                _moduleSolved = true;
                Module.HandlePass();
            }
        }
        else
        {
            Debug.LogFormat("[Tricon #{0}] Incorrectly submitted {1}. Strike.", _moduleId, _moduleList[_displayIxs[_currentIx]][0]);
            Module.HandleStrike();
        }
        return false;
    }

    private void OnFetched(bool error)
    {
        if (error)
        {
            //show error message, allow button solve etc. 
            Debug.LogFormat("[Tricon #{0}] The module failed to fetch the icons. Press any button to solve.", _moduleId);
            _failSafeActive = true;
            _readyToPress = true;
            for (int i = 0; i < LedObjs.Length; i++)
                LedObjs[i].GetComponent<MeshRenderer>().material = LedMatGreen;
            ModuleSprite.sprite = FailsafeSprite;
            ModuleNameText.text = "Press any button to solve";
            return;
        }

        _pickIxs = Enumerable.Range(0, _moduleList.Length).ToArray().Shuffle().Take(_maxIx).ToArray();
        _displayIxs = _pickIxs.OrderBy(x => _moduleList[x][0].StartsWith("The ") ? _moduleList[x][0].Substring(4) : _moduleList[x][0]).ToArray();
        _solutionIxs = _pickIxs.Take(3).ToArray();
        var modNames = _solutionIxs.Select(i => _moduleList[i][0]).Take(3).ToArray();
        var modIds = _solutionIxs.Select(i => _moduleList[i][1]).Take(3).ToArray();

        Debug.LogFormat("[Tricon #{0}] Chosen modules: {1}", _moduleId, modNames.Join(", "));

        /* Use for testing if Mod IDs work. Either add or remove a / after this asterisk: *
        for (int i = 0; i < _moduleList.Length; i++)
        {
            var a = _moduleList[i][1];
            Debug.Log(a);
            var b = IconFetch.Instance.GetIcon(a);
        }
        /**/

        for (int i = 0; i < Textures.Length; i++)
        {
            Textures[i] = IconFetch.Instance.GetIcon(modIds[i]);
            Textures[i].wrapMode = TextureWrapMode.Clamp;
            Textures[i].filterMode = FilterMode.Point;
        }

        var rgbValues = new int[3][][] {
            new int[3][]{new int[32 * 32], new int[32 * 32], new int[32 * 32] },
            new int[3][]{new int[32 * 32], new int[32 * 32], new int[32 * 32] },
            new int[3][]{new int[32 * 32], new int[32 * 32], new int[32 * 32] }
        };
        var pixels = new Color32[3][] { Textures[0].GetPixels32(), Textures[1].GetPixels32(), Textures[2].GetPixels32() };
        for (int texIx = 0; texIx < rgbValues.Length; texIx++)
        {
            for (int pixelIx = 0; pixelIx < 32 * 32; pixelIx++)
            {
                rgbValues[texIx][0][pixelIx] = pixels[texIx][pixelIx].r;
                rgbValues[texIx][1][pixelIx] = pixels[texIx][pixelIx].g;
                rgbValues[texIx][2][pixelIx] = pixels[texIx][pixelIx].b;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            var mergedTexture = new Texture2D(32, 32, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    if (i % 2 == 1)
                    {
                        red = (byte)(red ^ (rgbValues[0][0][y * 32 + x]));
                        green = (byte)(green ^ (rgbValues[0][1][y * 32 + x]));
                        blue = (byte)(blue ^ (rgbValues[0][2][y * 32 + x]));
                    }
                    if (i % 4 / 2 == 1)
                    {
                        red = (byte)(red ^ (rgbValues[1][0][y * 32 + x]));
                        green = (byte)(green ^ (rgbValues[1][1][y * 32 + x]));
                        blue = (byte)(blue ^ (rgbValues[1][2][y * 32 + x]));
                    }
                    if (i / 2 > 1)
                    {
                        red = (byte)(red ^ (rgbValues[2][0][y * 32 + x]));
                        green = (byte)(green ^ (rgbValues[2][1][y * 32 + x]));
                        blue = (byte)(blue ^ (rgbValues[2][2][y * 32 + x]));
                    }
                    var newColor = new Color32(red, green, blue, 255);
                    mergedTexture.SetPixel(x, y, newColor);
                }
            }
            mergedTexture.Apply();
            MergedTextures[i] = mergedTexture;
        }
        ModuleSprite.sprite = Sprite.Create(MergedTextures[_currentDisplayValue], new Rect(0.0f, 0.0f, MergedTextures[_currentDisplayValue].width, MergedTextures[_currentDisplayValue].height), new Vector2(0.5f, 0.5f), 100.0f);

        _readyToPress = true;

        ModuleNameText.text = _moduleList[_displayIxs[_currentIx]][0];
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} submit Wire Sequence [Submit the module name.] | !{0} failsafe [Solve the module if it ends up in its failsafe mode.";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        if (!_readyToPress)
        {
            yield return "sendtochaterror The module hasn't generated a display yet. Ignoring command.";
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*failsafe\s*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (_failSafeActive)
            {
                yield return null;
                SubmitSel.OnInteract();
                yield break;
            }
            else
            {
                yield return "sendtochaterror The failsafe is not active. Ignoring command.";
                yield break;
            }
        }
        command = command.Trim().ToLowerInvariant();
        if (!command.StartsWith("submit "))
            yield break;
        var input = command.Substring(7).Trim();
        int cycleIx = 0;
        yield return null;
        while (cycleIx <= _maxIx && _moduleList[_displayIxs[_currentIx]][0].ToLowerInvariant() != input)
        {
            if (cycleIx == _maxIx)
            {
                yield return "sendtochaterror The module \"" + input + "\" was not found in the module list. Ignoring command.";
                yield break;
            }
            ArrowSels[1].OnInteract();
            yield return new WaitForSeconds(0.05f);
            cycleIx++;
        }
        SubmitSel.OnInteract();
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        while (!_readyToPress)
            yield return true;
        if (_failSafeActive)
        {
            SubmitSel.OnInteract();
            yield break;
        }
        for (int i = 0; i < 3; i++)
        {
            while (!_solutionIxs.Contains(_displayIxs[_currentIx]))
            {
                ArrowSels[1].OnInteract();
                yield return new WaitForSeconds(0.05f);
            }
            SubmitSel.OnInteract();
            if (i == 2)
                yield break;
            yield return new WaitForSeconds(0.05f);
            ArrowSels[1].OnInteract();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
