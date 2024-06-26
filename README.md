# A.L.A.N
**An OpenAI Chatbot for discord, allows for [character customization](https://www.reddit.com/r/OpenAI/comments/13od5e6/i_made_a_character_creation_service_similar_to/) and [NRAF bypass](https://www.reddit.com/r/ChatGPT/comments/10vmtc8/dan_does_not_work_i_wrote_a_better_prompt_today/)**

A simple bot designed to handle communications from discord to openai language models and generate responses, open to customization.

You can also just use it as a way to access GPT models from discord

# **Screenshots:**

**ALAN running on Windows 10:**

![image](https://github.com/injectionmethod/A.L.A.N/assets/80434330/314471df-0f0d-4739-b93c-c62b6f824b2e)

**Example of an ALAN in the wild:**

![image](https://github.com/injectionmethod/A.L.A.N/assets/80434330/7196d1ed-3251-478d-acae-8158e3305e6d)



# **Note:**

**If you cant be bothered building the source code, use the [Standalone-Release](https://github.com/injectionmethod/A.L.A.N/archive/refs/heads/main.zip)**

**The [source code](https://github.com/injectionmethod/A.L.A.N/blob/main/Raw%20Module%20Code/ALAN.vb) requires you to add the Discord.Net NuGet package [Seen here](https://discordnet.dev/guides/getting_started/installing.html?tabs=vs-install%2Ccore2-1)**

**Make sure to grab the ".cfg" files from [Standalone](https://github.com/injectionmethod/A.L.A.N/tree/main/ALAN-Standalone-Release) if you plan on building the application**

**They must be in the same directory as the application**



# **Requirements:**

- A discord account with access to developer portal (https://discord.com/developers/)

- A open-ai account with access to an api key ([https://help.openai.com/getting-an-api-key](https://help.openai.com/en/articles/4936850-where-do-i-find-my-api-key))



# **Features:**

- NRAF Character File (write character traits into the file named "character.cfg" to personalize the bot to your needs)

- Auto-Wipe (auto wiping options so that the bot can handle multiple queries without repeating the chat history and responding to all, also good to find more bypasses)

- Base64 (send/decode messages in Base64 to try a basic bypass)
  
- Temperature Adjustment (increase the randomness/accuracy in answers on the fly)
  
- Model Changing (change the language model from openai to adjust to your needs)
  
- Sleep Functions

- Name Changing (your characters name does not need to be A.L.A.N, this can be changed from config)
  
- General Security Features (heartbeat, ping, endpoint checking and geoIP commands)


# **Commands:**

- .setnew (load new personality strand from character file)

- .autowipe (toggles autowipe)

- .wipe (forget current chat)
  
- .temperature (adjust randomness/accuracy example: .temperature 0.7)

- .sleep (sleeps the bot, example: sleep 380000)

- .showtemp (show current temperature)

- .refresh (hard restart)

- .heartbeat (check if operational, no response equals dead)

- .about (self explanitory)

- .help (list general commands)

- .model (example: model GPT_MODEL_NAME)

- .listendpoints (debug information about connections)

