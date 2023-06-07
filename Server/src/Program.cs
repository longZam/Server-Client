using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CommandLine;
using YamlDotNet;
using log4net;
using log4net.Config;
using System.Reflection;
using System.Security.Cryptography;
using LiteNetLib;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

#region Commandline Verbs

[Verb("run")]
public class RunServerOptions
{

}

[Verb("keygen")]
public class KeygenOptions
{

}

#endregion


public class Program
{
    private static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<RunServerOptions, KeygenOptions>(args)
                                .WithParsed<RunServerOptions>(RunServer)
                                .WithParsed<KeygenOptions>(Keygen);
    }

    private static void RunServer(RunServerOptions options)
    {
        ServerSettings settings = new ServerSettings();

        MainServer server = new MainServer(settings, LogManager.GetLogger("MainServer"));
        server.Run();
    }

    private static void Keygen(KeygenOptions options)
    {

    }
}