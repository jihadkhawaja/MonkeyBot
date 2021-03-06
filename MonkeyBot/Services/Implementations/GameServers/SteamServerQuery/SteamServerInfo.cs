﻿using System;

namespace MonkeyBot.Services
{
    public class SteamServerInfo
    {
        /// <summary>
        /// Socket address of server.
        /// </summary>
        public string Address { get; internal set; }

        /// <summary>
        /// Protocol version used by the server.
        /// </summary>
        public byte Protocol { get; internal set; }

        /// <summary>
        /// Name of the server.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Map the server has currently loaded.
        /// </summary>
        public string Map { get; internal set; }

        /// <summary>
        /// Name of the folder containing the game files.
        /// </summary>
        public string Directory { get; internal set; }

        /// <summary>
        /// Full name of the game.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Steam Application ID of game.
        /// </summary>
        public ushort Id { get; internal set; }

        /// <summary>
        /// Number of players on the server.
        /// </summary>
        public int Players { get; internal set; }

        /// <summary>
        /// Maximum number of players the server reports it can hold.
        /// </summary>
        public byte MaxPlayers { get; internal set; }

        /// <summary>
        /// Number of bots on the server.
        /// </summary>
        public byte Bots { get; internal set; }

        /// <summary>
        /// Indicates the type of server.(Dedicated/Non-dedicated/Proxy)
        /// </summary>

        public GameServertype ServerType { get; internal set; }

        /// <summary>
        /// Indicates the operating system of the server.(Linux/Windows/Mac)
        /// </summary>
        public GameEnvironment Environment { get; internal set; }

        /// <summary>
        /// Indicates whether the server requires a password.
        /// </summary>
        public bool IsPrivate { get; internal set; }

        /// <summary>
        /// Specifies whether the server uses VAC.
        /// </summary>
        public bool IsSecure { get; internal set; }

        /// <summary>
        /// Version of the game installed on the server.
        /// </summary>
        public string GameVersion { get; internal set; }

        /// <summary>
        /// Round-trip delay time.
        /// </summary>
        public long Ping { get; internal set; }

        //Additional Info
        /// <summary>
        /// The server's game port number.
        /// </summary>
        public ushort Port { get; internal set; }

        /// <summary>
        /// Server's SteamID.
        /// </summary>
        public ulong SteamId { get; internal set; }

        /// <summary>
        /// Spectator port number for SourceTV.
        /// </summary>
        public ushort SourceTVPort { get; internal set; }

        /// <summary>
        /// Name of the spectator server for SourceTV.
        /// </summary>
        public string SourceTVName { get; internal set; }

        /// <summary>
        /// Tags that describe the game according to the server.
        /// </summary>
        public string Keywords { get; internal set; }

        /// <summary>
        /// The server's 64-bit GameID.
        /// </summary>
        public ulong GameId { get; internal set; }

        public static SteamServerInfo Parse(byte[] data)
        {
            SteamServerInfo serverInfo = null;
            var parser = new Parser(data);
            if (parser.ReadByte() != (byte)ResponseMsgHeader.A2S_INFO)
            {
                throw new Exception("A2S_INFO message header is not valid");
            }

            serverInfo = new SteamServerInfo
            {
                Protocol = parser.ReadByte(),
                Name = parser.ReadString(),
                Map = parser.ReadString(),
                Directory = parser.ReadString(),
                Description = parser.ReadString(),
                Id = parser.ReadUShort(),
                Players = parser.ReadByte(),
                MaxPlayers = parser.ReadByte(),
                Bots = parser.ReadByte(),
                ServerType = new Func<GameServertype>(()
                    => ((char)parser.ReadByte()) switch
                    {
                        'l' => GameServertype.Listen,
                        'd' => GameServertype.Dedicated,
                        'p' => GameServertype.SourceTV,
                        _ => GameServertype.Invalid,
                    })(),
                Environment = new Func<GameEnvironment>(()
                    => ((char)parser.ReadByte()) switch
                    {
                        'l' => GameEnvironment.Linux,
                        'w' => GameEnvironment.Windows,
                        'm' => GameEnvironment.Mac,
                        'o' => GameEnvironment.Mac,
                        _ => GameEnvironment.Invalid,
                    })(),
                IsPrivate = parser.ReadByte() > 0,
                IsSecure = parser.ReadByte() > 0,

                GameVersion = parser.ReadString()
            };

            if (parser.HasUnParsedBytes)
            {
                byte edf = parser.ReadByte();
                serverInfo.Port = (edf & 0x80) > 0 ? parser.ReadUShort() : (ushort)0;
                serverInfo.SteamId = (edf & 0x10) > 0 ? parser.ReadULong() : 0;
                if ((edf & 0x40) > 0)
                {
                    serverInfo.SourceTVPort = parser.ReadUShort();
                    serverInfo.SourceTVName = parser.ReadString();
                }
                serverInfo.Keywords = (edf & 0x20) > 0 ? parser.ReadString() : string.Empty;
                serverInfo.GameId = (edf & 0x10) > 0 ? parser.ReadULong() : 0;
            }
            //serverInfo.Address = UdpSocket.Address.ToString();
            //serverInfo.Ping = Latency;

            return serverInfo;
        }

        /// <summary>
        /// Game Server's type.
        /// </summary>
        public enum GameServertype
        {
            /// <summary>
            /// Server returned an invalid value.
            /// </summary>
            Invalid,

            /// <summary>
            /// Dedicated.
            /// </summary>
            Dedicated,

            /// <summary>
            /// Non Dedicated.
            /// </summary>
            NonDedicated,

            /// <summary>
            /// Listen.
            /// </summary>
            Listen,

            /// <summary>
            /// Source TV.
            /// </summary>
            SourceTV,

            /// <summary>
            /// HLTV Server
            /// </summary>
            HLTVServer
        }

        /// <summary>
        /// Server's operating system.
        /// </summary>
        public enum GameEnvironment
        {
            /// <summary>
            /// Server returned an invalid value.
            /// </summary>
            Invalid,

            /// <summary>
            /// Linux.
            /// </summary>
            Linux,

            /// <summary>
            /// Windows.
            /// </summary>
            Windows,

            /// <summary>
            /// Mac.
            /// </summary>
            Mac
        }
    }
}