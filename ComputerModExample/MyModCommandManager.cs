using System;
using BepInEx;
using BepInEx.Configuration;
using ComputerInterface;
using UnityEngine;
using Zenject;

namespace ComputerModExample
{
    internal class MyModCommandManager : IInitializable
    {
        private readonly CommandHandler _commandHandler;

        // Request the CommandHandler
        // This gets resolved by zenject since we bind MyModCommandManager in the container
        public MyModCommandManager(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Initialize()
        {
            // Add a command
            // You can pass null in argumentsType if you aren't expecting any
            _commandHandler.AddCommand(new Command(name: "whoami", argumentTypes: null, args =>
            {
                // args is an array of arguments (string) passed when entering the command
                // the command handler already checks if the correct amount of arguments is passed

                // the string you return is going to be shown in the terminal as a return message
                // you can break up the message into multiple lines by using \n
                return "MONKE";
            }));

            // Pass null in the argumentTypes array for any type that doesn't need to be converted (i.e. it's a string)
            _commandHandler.AddCommand(new Command(name: "echo", argumentTypes: new Type[]{null}, args =>
            {
                return (string) args[0];
            }));

            // Pass the types of the arguments you are expecting and the command handler will try to find an converter
            // and convert them to for you one the command is executed
            // the types are checked during adding and if the command handler doesn't find an converter for the type it will log an error
            _commandHandler.AddCommand(new Command(name: "add", argumentTypes: new [] { typeof(int), typeof(int) }, args =>
            {
                return ((int) args[0] + (int) args[1]).ToString();
            }));

            // you can add your own converters like this
            // NOTE: this will add the converter in BepInEx globally
            // that means you can use the type as a config entry too.
            // only use this if you need to convert the type often
            // in your program or if you are using it in configs too.

            // already handled types:
            // all c# predefined data types like int, float, bool and so on
            // all unity vector lengths (vector2, vector3, ...)
            // unity color
            // unity quaternion
            var converter = new TypeConverter()
            {
                ConvertToString = (obj, type) =>
                {
                    var color = (MyOwnColorClass) obj;
                    return ColorUtility.ToHtmlStringRGB(new Color(color.R, color.G, color.B));
                },
                ConvertToObject = (str, type) =>
                {
                    ColorUtility.TryParseHtmlString((str.StartsWith("#")?"":"#")+str, out var unityColor);
                    return new MyOwnColorClass((int)(unityColor.r*255), (int)(unityColor.g * 255), (int)(unityColor.b * 255));
                }
            };

            TomlTypeConverter.AddConverter(typeof(MyOwnColorClass), converter);

            _commandHandler.AddCommand(new Command(name: "hextorgb", new []{typeof(MyOwnColorClass)}, args =>
            {
                var color = (MyOwnColorClass) args[0];
                return $"{color.R}, {color.G}, {color.B}";
            }));
        }
    }

    public class MyOwnColorClass
    {
        public int R;
        public int G;
        public int B;

        public MyOwnColorClass(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}