# A.L.A.N
**An OpenAI Chatbot for discord, allows for character customization and NRAF bypass**

A simple bot designed to handle communications from discord to openai language models and generate responses, open to customization.

You can also just use it as a way to access GPT models from discord


**ALAN running on Windows 10:**

![image](https://github.com/injectionmethod/A.L.A.N/assets/80434330/314471df-0f0d-4739-b93c-c62b6f824b2e)

**Example of an ALAN:**

![image](https://github.com/injectionmethod/A.L.A.N/assets/80434330/7196d1ed-3251-478d-acae-8158e3305e6d)



**Note:**

**The [source code](https://github.com/injectionmethod/A.L.A.N/blob/main/Raw%20Module%20Code/ALAN.vb) requires you to add the Discord.Net NuGet package [Seen here](https://discordnet.dev/guides/getting_started/installing.html?tabs=vs-install%2Ccore2-1)**


**Requirements:**

- A discord account with access to developer portal (https://discord.com/developers/)

- A open-ai account with access to a api key, paid or free it doesn't matter ([https://help.openai.com/getting-an-api-key](https://help.openai.com/en/articles/4936850-where-do-i-find-my-api-key))



**Features:**

- NRAF Character File (write character traits into the file named "character.cfg" to personalize the bot to your needs)

- Auto-Wipe (auto wiping options so that the bot can handle multiple queries without repeating the chat history and responding to all, also good to find more bypasses)

- Base64 (send/decode messages in Base64 to try a basic bypass)
  
- Temperature Adjustment (increase the randomness/accuracy in answers on the fly)
  
- Model Changing (change the language model from openai to adjust to your needs)
  
- Sleep Functions

- Name Changing (your characters name does not need to be A.L.A.N, this can be changed from config)
  
- General Security Features (heartbeat, ping, endpoint checking and geoIP commands)


**Commands:**

- .autowipe (toggles autowipe)

- .wipememory (wipe current chat)
  
- .temperature (adjust randomness/accuracy example: .temperature 0.7)

- .sleep (sleeps the bot, example: sleep 380000)

- .showtemp (show current temperature)

- .refresh (hard restart)

- .heartbeat (check if operational, no response equals dead)

- .about (self explanitory)

- .help (list general commands)

- .model (example: model GPT_MODEL_NAME)

- .listendpoints (debug information about connections)

- .geoip (gets detailed information about an address, example: .geoip 127.0.0.1)

- .ping (example: .ping 127.0.0.1)

- base64/decodebase64 (encode/decode base64, primitive obfuscation)
