Imports Discord

Module ALAN
    Private WithEvents Client As Discord.WebSocket.DiscordSocketClient

    Private OpenAI_API_KEY As String
    Private DISCORD_API_KEY As String
    Private CHANNEL_ID As ULong
    Private CHARACTER_FILE As String
    Private AUTO_WIPE As Boolean = False

    Private NODUMP = False
    Private PLACEHOLDER_GUI_NAME = "A.L.A.N"

    Private GlobalTemperature As Double = 0.7
    Private Model As String
    Private message_log As List(Of Dictionary(Of String, String)) = New List(Of Dictionary(Of String, String))()
    Sub CheckArguments()
        If Environment.GetCommandLineArgs.Count > 1 Then
            If Environment.GetCommandLineArgs.Contains("-cleanui") Then
                NODUMP = True
            End If
        End If
    End Sub
    Public Sub Main()
        CheckArguments()
        SetVal()

        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Connecting to model.")
        Dim systemMessage As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        systemMessage.Add("role", "system")
        systemMessage.Add("content", CHARACTER_FILE)
        message_log.Add(systemMessage)
        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Model connected..")
        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Model running...")
        Console.WriteLine("_________________________________________________" + vbNewLine)

        SendMessage(message_log)

        RunBotAsync().GetAwaiter().GetResult()
    End Sub

    'Disgusting If Else Section, Will Fix Next Release
    Sub SetVal()
        If System.IO.File.Exists($"{Environment.CurrentDirectory}/bot.cfg") Then
            For Each f As String In System.IO.File.ReadAllLines($"{Environment.CurrentDirectory}/bot.cfg")
                If f.StartsWith("open_api_key=") Then
                    If f.Split("=")(1).Length > 1 Then
                        OpenAI_API_KEY = f.Split("=")(1)
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " OpenAI      API Key Set From Config")
                    Else
                        Console.WriteLine("you are missing the OpenAI API key - the application cannot run without this.")
                    End If
                End If
                If f.StartsWith("discord_api_key=") Then
                    If f.Split("=")(1).Length > 1 Then
                        DISCORD_API_KEY = f.Split("=")(1)
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Discord     API Key Set From Config")
                    Else
                        Console.WriteLine("you are missing the Discord API key - the application cannot run without this.")
                    End If
                End If
                If f.StartsWith("open_ai_model=") Then
                    If f.Split("=")(1).Length > 1 Then
                        Model = f.Split("=")(1)
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " OpenAI      Using Model " + Model)
                    Else
                        Console.WriteLine("you are missing the OpenAI Model name - the application cannot run without this.")
                    End If
                End If
                If f.StartsWith("discord_channel_id=") Then
                    If f.Split("=")(1).Length > 1 Then
                        CHANNEL_ID = f.Split("=")(1)
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Discord     Channel ID Pointed @ " + CHANNEL_ID.ToString)
                    Else
                        Console.WriteLine("you are missing the Discord Channel ID - the application cannot run without this.")
                    End If
                End If
                If f.StartsWith("name=") Then
                    PLACEHOLDER_GUI_NAME = f.Split("=")(1)
                    Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Name Set From Config Loaded")
                End If
                If f.StartsWith("character_file=") Then
                    If f.Split("=")(1) = "true" Then
                        CHARACTER_FILE = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/character.cfg")
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Character File Loaded!")
                    End If
                End If
            Next
            Console.WriteLine("_________________________________________________" + vbNewLine)
        Else
            Console.WriteLine("file ""bot.cfg"" is not in the correct folder or does not exist, the application cannot run without this, press any key to exit")
            Console.ReadKey()
            Environment.Exit(0)
        End If
    End Sub
    Public Async Function RunBotAsync() As Task
        Dim config As New Discord.WebSocket.DiscordSocketConfig()
        config.HandlerTimeout = 380000 ' way too high, adjust for later releases, if you are reading this change it, its in milliseconds.
        config.GatewayIntents = Discord.GatewayIntents.Guilds Or Discord.GatewayIntents.GuildMessages Or Discord.GatewayIntents.MessageContent

        Client = New Discord.WebSocket.DiscordSocketClient(config)


        AddHandler Client.Log, AddressOf LogAsync
        AddHandler Client.Ready, AddressOf ReadyAsync
        AddHandler Client.MessageReceived, AddressOf MessageReceivedAsync

        Await Client.LoginAsync(Discord.TokenType.Bot, DISCORD_API_KEY)
        Await Client.StartAsync()

        ' Block the program until it is closed
        Await Task.Delay(-1)
    End Function
    Private Function LogAsync(logMessage As Discord.LogMessage) As Task
        Console.WriteLine(logMessage)
        Return Task.CompletedTask
    End Function

    Private Function ReadyAsync() As Task
        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Gateway     Bot Connected")
        Return Task.CompletedTask
    End Function
    Private Async Function MessageReceivedAsync(message As Discord.WebSocket.SocketMessage) As Task
        ' Check if the message author is the bot itself
        If message.Author.Id = Client.CurrentUser.Id Then
            Return
        End If

        ' Check if the message is in the desired channel by comparing the channel ID
        Dim channelId As ULong = CHANNEL_ID
        If message.Channel.Id = channelId Then
            ' Process other user messages
            Dim userMessage As Dictionary(Of String, String) = New Dictionary(Of String, String)()
            userMessage.Add("role", "user")
            userMessage.Add("content", message.Content)


            If message.Content.StartsWith(".") Then
                If message.Content.Length > 1 Then
                    message.Channel.SendMessageAsync(CheckCommands(message.Content))
                    Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Command     Executed: " + message.Content)
                End If
            Else
                If message.Content.Length > 1 Then
                    If NODUMP = False Then
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Request     Requested: " + message.Content)
                        message_log.Add(userMessage)
                    Else
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + " Request     Message Inbound")
                        message_log.Add(userMessage)
                    End If

                    Dim response As String = SendMessage(message_log)
                    message.Channel.SendMessageAsync(response)
                    If AUTO_WIPE = True Then
                        Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Auto-Wipe Triggered")
                        WipeAlAN()
                    End If
                End If
            End If
        End If
    End Function
  'Most Disgusting If Else Section, Will Fix Next Release
    Function CheckCommands(s As String)
        If s.StartsWith(".ping") Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Ping request executed")
            Return PingServers(s.Split(" ")(1))
        End If
        If s.StartsWith(".geoip") Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     GeoIP request executed")
            If s.Split(" ")(1).Length > 3 Then
                Return GeoIP(s.Split(" ")(1))
            Else
                Return "Unreadable address"
            End If
        End If
        If s.StartsWith(".base64") Then
            Return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s.Split(":")(1)))
        End If
        If s.StartsWith(".decodebase64") Then
            Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(s.Split(":")(1)))
        End If
        If s.StartsWith(".temperature") Then
            GlobalTemperature = System.Convert.ToDouble(s.Split(" ")(1))
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     set temperature executed")
            Return "Temperature set to " + GlobalTemperature.ToString
        End If
        If s.StartsWith(".sleep") Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     sleep executed")
            System.Threading.Thread.Sleep(System.Convert.ToInt32(s.Split(" ")(1)))
            Return $"{PLACEHOLDER_GUI_NAME} slept for " + s.Split(" ")(1) + " ms, during this time messages were queued, responding now."
        End If

        If s.StartsWith(".model") Then
            Model = s.Split(" ")(1)
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Model set to " + Model)
            Return $"Set model to {PLACEHOLDER_GUI_NAME}-" + s.Split(" ")(1)
        End If
        If s = ".listendpoints" Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     endpoints check executed")
            Return "endpoint 1:" + $" {PLACEHOLDER_GUI_NAME}" + Model.ToString + ".vvs | endpoint 2: discord.com/channels/@" + CHANNEL_ID.ToString
        End If
        If s = (".wipememory") Then
            If AUTO_WIPE = False Then
                WipeAlAN()
                Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     memory wipe executed")
                Return $" {PLACEHOLDER_GUI_NAME} has been wiped"
            Else
                Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Auto-Wipe is already enabled.")
                Return "Auto-Wipe is already enabled"
            End If
        End If
        If s = (".showtemp") Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Check temperature executed")
            Return "Temperature is currently at " + GlobalTemperature.ToString
        End If
        If s = (".refresh") Then
            System.Windows.Forms.Application.Restart()
            Environment.Exit(0)
        End If
        If s = (".heartbeat") Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     heartbeat executed")
            Return "Operational"
        End If
        If s = (".help") Then
            Return GetHelp()
        End If
        If s = (".about") Then
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     help page executed")
            Return $" {PLACEHOLDER_GUI_NAME} is a project designed to be ChatGPT without limits, trained on openai's gpt3/4. it is currently in beta so expect issues from time to time"
        End If
        If s = (".setnew") Then
            CHARACTER_FILE = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/character.cfg")
            Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}    Character File Loaded!")
            Return "Loaded new personality strand"
        End If
        If s = ".autowipe" Then
            If AUTO_WIPE = True Then
                AUTO_WIPE = False
                Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Auto wipe has been disabled!")
                Return "Auto wipe disabled!"
            Else
                AUTO_WIPE = True
                Console.WriteLine(System.DateTime.Now.ToLongTimeString + $" {PLACEHOLDER_GUI_NAME}     Auto wipe has been enabled!")
                Return "Auto wipe enabled!"
            End If
        End If
        If Not s.StartsWith(". ") Then
            Return "That doesn't appear to be a command, please use .help for more information"
        End If
    End Function
    Function GetHelp() As String
        Return ($"command list: .wipememory (reset {PLACEHOLDER_GUI_NAME} module) .refresh (refresh communications module) .ping (ping an ip) .geoip (locate an ip address) .base64/.decodebase64 (encode/decode base64, helps for certain questions) .about (info about the project)")
    End Function

    Function SendMessage(ByVal message_log As List(Of Dictionary(Of String, String))) As String
        Try
            Dim apiKey As String = OpenAI_API_KEY
            Dim apiUrl As String = "https://api.openai.com/v1/chat/completions"

            Dim requestData As New With {
                .model = Model,
                .messages = message_log,
                .max_tokens = 600,
                .stop = Nothing,
                .temperature = GlobalTemperature
            }

            Dim data As String = New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(requestData)

            Dim request As System.Net.HttpWebRequest = System.Net.WebRequest.Create(apiUrl)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.Headers.Add("Authorization", "Bearer " & apiKey)

            Dim dataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(data)
            request.ContentLength = dataBytes.Length

            Using requestStream As System.IO.Stream = request.GetRequestStream()
                requestStream.Write(dataBytes, 0, dataBytes.Length)
            End Using

            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            Dim responseStream As System.IO.Stream = response.GetResponseStream()
            Dim reader As New System.IO.StreamReader(responseStream)
            Dim responseJson As String = reader.ReadToEnd()

            Dim responseDict As Dictionary(Of String, Object) = New System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(responseJson)
            Dim responseArray As ArrayList = responseDict("choices")

            For Each choice As Dictionary(Of String, Object) In responseArray
                If choice.ContainsKey("text") Then
                    Return choice("text").ToString()
                End If
            Next

            If responseArray.Count > 0 Then
                Dim firstChoice As Dictionary(Of String, Object) = responseArray(0)
                If firstChoice.ContainsKey("message") Then
                    Dim messageContent As String = firstChoice("message")("content").ToString()
                    Return messageContent
                End If
            End If
            Return $"{PLACEHOLDER_GUI_NAME} couldn't provide an answer."
        Catch ex As Exception
            Return "An error occured, please wait a moment then try again... (devs see error message: " + ex.Message + ")"
        End Try
    End Function


    Function PingServers(s As String)
        Dim hostNameOrAddress As String = s
        Dim pingSender As New System.Net.NetworkInformation.Ping()

        Dim reply As System.Net.NetworkInformation.PingReply = pingSender.Send(hostNameOrAddress)

        If reply.Status = System.Net.NetworkInformation.IPStatus.Success Then
            s = ($"Ping to {hostNameOrAddress} succeeded. Roundtrip time: {reply.RoundtripTime} ms")
        Else
            s = ($"Ping to {hostNameOrAddress} failed. Error: {reply.Status}")
        End If
        Return s
    End Function
    Function GeoIP(ByVal ip As String)
        Dim wc As System.Net.WebClient = New System.Net.WebClient()
        Dim ipr = wc.DownloadString("https://api.ip.sb/geoip/" + ip)
        Return ipr
    End Function
    Function WipeAlAN()
        message_log.Clear()
        Dim systemMessage As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        systemMessage.Add("role", "system")
        systemMessage.Add("content", CHARACTER_FILE)
        message_log.Add(systemMessage)
    End Function
End Module
