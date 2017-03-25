using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenConfigLoader : MonoBehaviour
{

    [SerializeField]
    private System.Environment.SpecialFolder fileLocation;

    private string filePath;
    private string fileName = "/ConfigScreen.txt";

    private List<Pipe> pipes = new List<Pipe>();
    private List<Window> windows = new List<Window>();
    private List<Channel> channels = new List<Channel>();
    private List<CAVEScreen> screens = new List<CAVEScreen>();

    //variables
    private string host, separation, NumPipes, NumScreens, NumWindows;

    // Use this for initialization
    void Start()
    {

        Debug.Log("Begin Parse Configuration");
        Debug.Log("Begin Reading From Configuration Text File");

        //read the contents of ConfigScreen.txt file in Desktop folder
        string text = ReadFromFile();

        Debug.Log("Finished Retrieving Text From Configuration File");
        Debug.Log("Begin Parsing Information From Text");

        //parse to contents of the config file
        ParseInformation(text);

        Debug.Log("Finished Parsing Information From Text");
        Debug.Log("Begin Printing Out Data");

        //print all information
        PrintInformation();

        Debug.Log("Finished Printing Out Data");
        Debug.Log("Done");

    }

    public List<CAVEScreen> GetLoadedScreens()
    {
        return screens;
    }

    string ReadFromFile()
    {
        //grab the path to the folder to look for the config file
        filePath = System.Environment.GetFolderPath(fileLocation);
        Debug.Log("location: " + filePath);

        //get access to the contents of the config file
        string text = File.ReadAllText(filePath + fileName);
        Debug.Log("text is: " + text);

        return text;
    }

    void ParseInformation(string text)
    {

        text = text.Replace('>', ' ');

        //parsing the text by splitting versus the '<'
        string[] initialParse = text.Split(char.Parse("<"));
        for (int i = 0; i < initialParse.Length; i++)
        {

            //check if the line is not empty
            if (!(string.IsNullOrEmpty(initialParse[i])))
            {

                //create a depthParse string array to store more split arrays 
                string[] depthParse;

                //check if the line equals PipeConfig
                if (initialParse[i].Contains("PipeConfig"))
                {
                    Debug.Log("Parsing PipeConfig");
                    //increment i to start reading the next line
                    i++;
                    //while initialParse[i] does not equal PipeConfig because second PipeConfig signifies end of the Pipe section
                    while (!(initialParse[i].Contains("PipeConfig")))
                    {
                        //split the current line by " " and save into depthParse
                        depthParse = initialParse[i].Split(char.Parse(" "));
                        //declare Pipe pipeObj;
                        Pipe pipeObj = null;

                        if (!(depthParse[0].Contains("!"))) //don't make a channel on a comment
                        {
                            //declare Screen screenObj;
                            pipeObj = new Pipe();
                        }

                        //for (int j = 0; j < depthParse.Length; j++) loop through the line properties
                        for (int j = 0; j < depthParse.Length; j++)
                        {
                            if (depthParse[j] != "Pipe") // if depthParse[j] is a property element of Pipe
                            {
                                //split the current line by "=" and save into propertyParse
                                string[] propertyParse = depthParse[j].Split(char.Parse("="));

                                if (propertyParse[0] == "server") //check if property defines server
                                {
                                    pipeObj.server = propertyParse[1].Trim('"'); //e.g. server="0"
                                }
                                else if (propertyParse[0] == "screen") //check if property defines screen
                                {
                                    pipeObj.screen = propertyParse[1].Trim('"'); //e.g.screen="0"
                                }
                                else if (propertyParse[0] == "name") //check if property defines name
                                {
                                    pipeObj.name = propertyParse[1].Trim('"'); //e.g.name="aaa"
                                }

                            }
                        }

                        //add the pipeObj to a list of Pipes PipeList;
                        if (pipeObj != null)
                        {
                            pipes.Add(pipeObj);
                        }
                        //increment i to move along the file text
                        i++;

                    }
                }
                //check if the line equals WindowConfig
                else if (initialParse[i].Contains("WindowConfig"))
                {
                    Debug.Log("Parsing WindowConfig");
                    //increment i to start reading the next line
                    i++;
                    //while initialParse[i] does not equal WindowConfig because second WindowConfig signifies end of the Window section
                    while (!(initialParse[i].Contains("WindowConfig")))
                    {
                        //split the current line by " " and save into depthParse
                        depthParse = initialParse[i].Split(char.Parse(" "));
                        //declare Window windowObj;
                        Window windowObj = null;

                        if (!(depthParse[0].Contains("!"))) //don't make a channel on a comment
                        {
                            //declare Screen screenObj;
                            windowObj = new Window();
                        }

                        //for (int j = 0; j < depthParse.Length; j++) loop through the line properties
                        for (int j = 0; j < depthParse.Length; j++)
                        {
                            //if depthParse[j] != "Window"
                            if (depthParse[j] != "Window")
                            {
                                //split the current line by "=" and save into propertyParse
                                string[] propertyParse = depthParse[j].Split(char.Parse("="));

                                if (propertyParse[0] == "width")
                                {
                                    windowObj.width = propertyParse[1].Trim('"'); //e.g. width="3840"
                                }
                                else if (propertyParse[0] == "comment")
                                {
                                    windowObj.comment = propertyParse[1].Trim('"'); //e.g. comment="MAIN"
                                }
                                else if (propertyParse[0] == "pipeIndex")
                                {
                                    windowObj.pipeIndex = propertyParse[1].Trim('"'); //e.g. pipeIndex="0"
                                }
                                else if (propertyParse[0] == "height")
                                {
                                    windowObj.height = propertyParse[1].Trim('"'); //e.g. height="2160"
                                }
                                else if (propertyParse[0] == "left")
                                {
                                    windowObj.left = propertyParse[1].Trim('"'); //e.g left="0"
                                }
                                else if (propertyParse[0] == "bottom")
                                {
                                    windowObj.bottom = propertyParse[1].Trim('"'); //e.g. bottom="0"
                                }
                                else if (propertyParse[0] == "name")
                                {
                                    windowObj.name = propertyParse[1].Trim('"'); //e.g. name="0"
                                }
                                else if (propertyParse[0] == "decoration")
                                {
                                    windowObj.decoration = propertyParse[1].Trim('"'); //e.g. decoration="false"
                                }
                                else if (propertyParse[0] == "cudaDevice")
                                {
                                    windowObj.cudaDevice = propertyParse[1].Trim('"'); //e.g. cudaDevice="0"
                                }
                            }
                        }

                        //add the windowObj to a list of Windows WindowList;
                        if (windowObj != null)
                        {
                            windows.Add(windowObj);
                        }

                        //increment i to move along the file text
                        i++;
                    }
                }
                //check if the line equals ChannelConfig
                else if (initialParse[i].Contains("ChannelConfig"))
                {
                    Debug.Log("Parsing ChannelConfig");
                    //increment i to start reading the next line
                    i++;
                    //while initialParse[i] does not equal ChannelConfig because second ChannelConfig signifies end of the Channel section
                    while (!(initialParse[i].Contains("ChannelConfig")))
                    {
                        //split the current line by " " and save into depthParse
                        depthParse = initialParse[i].Split(char.Parse(" "));

                        //declare Channel channelObj;
                        Channel channelObj = null;

                        if (!(depthParse[0].Contains("!"))) //don't make a channel on a comment
                        {
                            //declare Screen screenObj;
                            channelObj = new Channel();
                        }

                        //for (int j = 0; j < depthParse.Length; j++) loop through the line properties
                        for (int j = 0; j < depthParse.Length; j++)
                        {
                            //if depthParse[j] != "Channel"
                            if (depthParse[j] != "Channel")
                            {
                                //split the current line by "=" and save into propertyParse
                                string[] propertyParse = depthParse[j].Split(char.Parse("="));

                                if (propertyParse[0] == "left")
                                {
                                    channelObj.left = propertyParse[1].Trim('"'); //e.g. left="0"
                                }
                                else if (propertyParse[0] == "width")
                                {
                                    channelObj.width = propertyParse[1].Trim('"'); //e.g. width="3840"
                                }
                                else if (propertyParse[0] == "bottom")
                                {
                                    channelObj.bottom = propertyParse[1].Trim('"'); //e.g. bottom="0"
                                }
                                else if (propertyParse[0] == "height")
                                {
                                    channelObj.height = propertyParse[1].Trim('"'); //e.g. height="2160"
                                }
                                else if (propertyParse[0] == "stereoMode")
                                {
                                    channelObj.stereoMode = propertyParse[1].Trim('"'); //e.g stereoMode="VERTICAL_SPLIT"
                                }
                                else if (propertyParse[0] == "windowIndex")
                                {
                                    channelObj.windowIndex = propertyParse[1].Trim('"'); //e.g. windowIndex="0"
                                }
                                else if (propertyParse[0] == "name")
                                {
                                    channelObj.name = propertyParse[1].Trim('"'); //e.g. name="0"
                                }
                                else if (propertyParse[0] == "comment")
                                {
                                    channelObj.comment = propertyParse[1].Trim('"'); //e.g. comment="LEFT"
                                }

                            }
                        }
                        //add the channelObj to a list of Channels ChannelList;
                        if (channelObj != null)
                        {
                            channels.Add(channelObj);
                        }
                        //increment i to move along the file text
                        i++;
                    }
                }
                //check if the line equals ScreenConfig
                else if (initialParse[i].Contains("ScreenConfig"))
                {
                    Debug.Log("Parsing ScreenConfig");
                    //increment i to start reading the next line
                    i++;
                    //while initialParse[i] does not equal ScreenConfig because second ScreenConfig signifies end of the screen section
                    while (!(initialParse[i].Contains("ScreenConfig")))
                    {
                        //split the current line by " " and save into depthParse
                        depthParse = initialParse[i].Split(char.Parse(" "));

                        //declare a new screen object
                        CAVEScreen screenObj = null;

                        if (!(depthParse[0].Contains("!"))) //don't make a screen on a comment
                        {
                            //declare Screen screenObj;
                            screenObj = new CAVEScreen();
                        }

                        //for (int j = 0; j < depthParse.Length; j++) loop through the line properties
                        for (int j = 0; j < depthParse.Length; j++)
                        {
                            //if depthParse[j] != "Screen"
                            if (depthParse[j] != "Screen")
                            {
                                //split the current line by "=" and save into propertyParse
                                string[] propertyParse = depthParse[j].Split(char.Parse("="));

                                if (propertyParse[0] == "height")
                                {
                                    screenObj.height = propertyParse[1].Trim('"'); //e.g. height="978"
                                }
                                else if (propertyParse[0] == "h")
                                {
                                    screenObj.h = propertyParse[1].Trim('"'); //e.g. h="44.6"
                                }
                                else if (propertyParse[0] == "width")
                                {
                                    screenObj.width = propertyParse[1].Trim('"'); //e.g. width="1727"
                                }
                                else if (propertyParse[0] == "p")
                                {
                                    screenObj.p = propertyParse[1].Trim('"'); //e.g. p="0"
                                }
                                else if (propertyParse[0] == "originX")
                                {
                                    screenObj.originX = propertyParse[1].Trim('"'); //e.g originX="-835.6"
                                }
                                else if (propertyParse[0] == "comment")
                                {
                                    screenObj.comment = propertyParse[1].Trim('"'); //e.g. comment="S_A"
                                }
                                else if (propertyParse[0] == "originY")
                                {
                                    screenObj.originY = propertyParse[1].Trim('"'); //e.g. originY="848.9"
                                }
                                else if (propertyParse[0] == "r")
                                {
                                    screenObj.r = propertyParse[1].Trim('"'); //e.g. r="90.0"
                                }
                                else if (propertyParse[0] == "name")
                                {
                                    screenObj.name = propertyParse[1].Trim('"'); //e.g name="0"
                                }
                                else if (propertyParse[0] == "originZ")
                                {
                                    screenObj.originZ = propertyParse[1].Trim('"'); //e.g. originZ="0"
                                }
                                else if (propertyParse[0] == "screen")
                                {
                                    screenObj.screen = propertyParse[1].Trim('"'); //e.g. screen="0"
                                }

                            }
                        }
                        //add the screenObj to a list of Screens ScreenList;
                        if (screenObj != null)
                        {
                            screens.Add(screenObj);
                        }
                        //increment i to move along the file text
                        i++;
                    }
                }
                //else
                else
                {
                    Debug.Log("Parsing Variables");
                    //split the current line by " " and save into depthparse
                    depthParse = initialParse[i].Split(char.Parse(" "));

                    //check if depthParse[0] is LOCAL
                    if (depthParse[0] == "LOCAL")
                    {

                        //split depthParse[1] by "=" and save into string[] tempArray
                        string[] tempArray = depthParse[1].Split(char.Parse("="));
                        //if tempArray[0] == "host"
                        if (tempArray[0] == "host")
                        {
                            //set host = tempArray[1]
                            host = tempArray[1].Trim('"');
                        }
                        //else 
                        else
                        {
                            Debug.Log("Unknown Property in Local");
                        }
                    }
                    //check if depthParse[0] is Stereo
                    else if (depthParse[0] == "Stereo")
                    {
                        //split depthParse[1] by "=" and save into string[] tempArray
                        string[] tempArray = depthParse[1].Split(char.Parse("="));
                        //if tempArray[0] == "seperation"
                        if (tempArray[0] == "seperation" || tempArray[0] == "separation")
                        {
                            //set separation = tempArray[1]
                            separation = tempArray[1].Trim('"');
                        }
                        //else
                        else
                        {
                            Debug.Log("Unknown Property in Stereo");
                        }
                    }
                    //check if depthParse[0] is NumPipes
                    else if (depthParse[0] == "NumPipes")
                    {
                        //split depthParse[1] by "=" and save into string[] tempArray
                        string[] tempArray = depthParse[1].Split(char.Parse("="));
                        //if tempArray[0] == "value"
                        if (tempArray[0] == "value")
                        {
                            //set NumPipes = tempArray[1]
                            NumPipes = tempArray[1].Trim('"');
                        }
                        //else
                        else
                        {
                            Debug.Log("Unknown Property in NumPipes");
                        }
                    }
                    //check if depthParse[0] is NumScreens
                    else if (depthParse[0] == "NumScreens")
                    {
                        //split depthParse[1] by "=" and save into string[] tempArray
                        string[] tempArray = depthParse[1].Split(char.Parse("="));
                        //if tempArray[0] == "value"
                        if (tempArray[0] == "value")
                        {
                            //set NumScreens = tempArray[1]
                            NumScreens = tempArray[1].Trim('"');
                        }
                        //else
                        else
                        {
                            Debug.Log("Unknown Property in NumScreens");
                        }
                    }
                    //check if depthParse[0] is NumWindows
                    else if (depthParse[0] == "NumWindows")
                    {
                        //split depthParse[1] by "=" and save into string[] tempArray
                        string[] tempArray = depthParse[1].Split(char.Parse("="));
                        //if tempArray[0] == "value"
                        if (tempArray[0] == "value")
                        {
                            //set NumWindows = tempArray[1]
                            NumWindows = tempArray[1].Trim('"');
                        }
                        //else
                        else
                        {
                            Debug.Log("Unknown Property in NumWindows");
                        }
                    }
                }
            }
        }
    }

    void PrintInformation()
    {
        //print the variables
        Debug.Log("Reading Variables: ");
        Debug.Log("Host: " + host);
        Debug.Log("Separation: " + separation);
        Debug.Log("NumPipes: " + NumPipes);
        Debug.Log("NumScreens: " + NumScreens);
        Debug.Log("NumWindows: " + NumWindows);

        //loop through PipeList
        Debug.Log("Reading Pipes: ");
        for (int i = 0; i < pipes.Count; i++)
        {
            //print server
            Debug.Log("Pipe #" + i + " Server: " + pipes[i].server);
            //print screen
            Debug.Log("Pipe #" + i + " Screen: " + pipes[i].screen);
            //print name
            Debug.Log("Pipe #" + i + " Name: " + pipes[i].name);
        }

        //loop through WindowList
        Debug.Log("Reading Windows: ");
        for (int i = 0; i < windows.Count; i++)
        {
            //print width
            Debug.Log("Window #" + i + " Width: " + windows[i].width);
            //print comment
            Debug.Log("Window #" + i + " Comment: " + windows[i].comment);
            //print pipeIndex
            Debug.Log("Window #" + i + " PipeIndex: " + windows[i].pipeIndex);
            //print height
            Debug.Log("Window #" + i + " Height: " + windows[i].height);
            //print left
            Debug.Log("Window #" + i + " Left: " + windows[i].left);
            //print bottom
            Debug.Log("Window #" + i + " Bottom: " + windows[i].bottom);
            //print name
            Debug.Log("Window #" + i + " Name: " + windows[i].name);
            //print decoration
            Debug.Log("Window #" + i + " Decoration: " + windows[i].decoration);
            //check if cudaDevice is null because config's are different. if not, print
            if (windows[i].cudaDevice != null)
            {
                Debug.Log("Window #" + i + " CudaDevice: " + windows[i].cudaDevice);
            }
        }

        //loop through ChannelList
        Debug.Log("Reading Channels: ");
        for (int i = 0; i < channels.Count; i++)
        {
            //print left
            Debug.Log("Channel #" + i + " Left: " + channels[i].left);
            //print width
            Debug.Log("Channel #" + i + " Width: " + channels[i].width);
            //print bottom
            Debug.Log("Channel #" + i + " Bottom: " + channels[i].bottom);
            //print height
            Debug.Log("Channel #" + i + " Height: " + channels[i].height);
            //print stereoMode
            Debug.Log("Channel #" + i + " StereoMode: " + channels[i].stereoMode);
            //print windowIndex
            Debug.Log("Channel #" + i + " windowIndex: " + channels[i].windowIndex);
            //print name
            Debug.Log("Channel #" + i + " Name: " + channels[i].name);
            //check if comment is null because config's are different. if not, print
            if (channels[i].comment != null)
            {
                Debug.Log("Channel #" + i + " Comment: " + channels[i].comment);
            }
        }

        //loop through ScreenList
        Debug.Log("Reading Screens: ");
        for (int i = 0; i < screens.Count; i++)
        {
            //print height
            Debug.Log("Screen #" + i + " Height: " + screens[i].height);
            //print h
            Debug.Log("Screen #" + i + " H: " + screens[i].h);
            //print width
            Debug.Log("Screen #" + i + " Width: " + screens[i].width);
            //print p
            Debug.Log("Screen #" + i + " P: " + screens[i].p);
            //print originX
            Debug.Log("Screen #" + i + " OriginX: " + screens[i].originX);
            //print comment
            Debug.Log("Screen #" + i + " Comment: " + screens[i].comment);
            //print originY
            Debug.Log("Screen #" + i + " originY: " + screens[i].originY);
            //print r
            Debug.Log("Screen #" + i + " r: " + screens[i].r);
            //print name
            Debug.Log("Screen #" + i + " Name: " + screens[i].name);
            //print originZ
            Debug.Log("Screen #" + i + " originZ: " + screens[i].originZ);
            //check if screen is null because config's are different. if not, print
            if (screens[i].screen != null)
            {
                Debug.Log("Screen #" + i + " Screen: " + screens[i].screen);
            }
        }
    }
}

public class Pipe
{
    public string server;
    public string screen;
    public string name;
}

public class Window
{
    public string width;
    public string comment;
    public string pipeIndex;
    public string height;
    public string left;
    public string bottom;
    public string name;
    public string decoration;
    public string cudaDevice; //found in the UCSD Config
}

public class Channel
{
    public string left;
    public string width;
    public string bottom;
    public string height;
    public string stereoMode;
    public string windowIndex;
    public string name;
    public string comment; //found in the UCSD config
}

public class CAVEScreen
{
    public string height;
    public string h;
    public string width;
    public string p;
    public string originX;
    public string comment;
    public string originY;
    public string r;
    public string name;
    public string originZ;
    public string screen; //NOT found in the UCSD Config
}