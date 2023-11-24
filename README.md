# A.L.A.N
**A OpenAI Chatbot for discord, allows for character customization and NRAF bypass**

A simple bot designed to handle communications from discord to openai language models and generate responses, open to customization.


**ALAN running on Windows 10:**

![image](https://github.com/injectionmethod/A.L.A.N/assets/80434330/314471df-0f0d-4739-b93c-c62b6f824b2e)

**Example of an ALAN:**

![image](https://github.com/injectionmethod/A.L.A.N/assets/80434330/7196d1ed-3251-478d-acae-8158e3305e6d)



Note:

**The [source code](https://github.com/injectionmethod/A.L.A.N/blob/main/Raw%20Module%20Code/ALAN.vb) requires you to add the Discord.Net NuGet package [Seen here](https://discordnet.dev/guides/getting_started/installing.html?tabs=vs-install%2Ccore2-1)**


Requirements:

- A discord account with access to developer portal (https://discord.com/developers/)

- A open-ai account with access to a api key, paid or free it doesn't matter [https://help.openai.com/getting-an-api-key](https://help.openai.com/en/articles/4936850-where-do-i-find-my-api-key)



Features:

- NRAF Character File (write character traits into the file named "character.cfg" to personalize the bot to your needs)

- Auto-Wipe (auto wiping options so that the bot can handle multiple queries without repeating the chat history and responding to all, also good to find more bypasses)

- Base64 (send/decode messages in Base64 to try a basic bypass)
  
- Temperature Adjustment (increase the randomness/accuracy in answers on the fly)
  
- Model Changing (change the language model from openai to adjust to your needs)
  
- Sleep Functions

- Name Changing (your characters name does not need to be A.L.A.N, this can be changed from config)
  
- General Security Features (heartbeat, ping, endpoint checking and geoIP commands)
