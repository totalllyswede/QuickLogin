# QuickLogin

**QuickLogin** is a small Windows utility that streamlines logging into **World of Warcraft** by securely storing account credentials and typing them into the login screen on demand.

It is designed for convenience and simplicity, with a clean interface and no background processes.

### FYI - This program does not actually launch the wow client, only logs in to the game when wow is open, might add launcher functionality in the future just isn't what I needed the tool for.

---

## ✨ What QuickLogin Does

* Securely stores login credentials using **Windows Credential Manager**
* Brings **World of Warcraft** to the foreground
* Types the selected account’s username and password
* Submits the login automatically
* Exits immediately after completing the action

QuickLogin does **not** interact with the game client beyond sending standard keyboard input.

---

## 📸 Screenshots

### Main Window & Add / Edit Account

<img width="600" height="680" alt="a" src="https://github.com/user-attachments/assets/50d2852a-a6fc-4693-b203-d81e4dfdbfb8" />

---

## 🖥️ Requirements

* Windows 7 or newer
* .NET Desktop Runtime 8
* World of Warcraft running and on the login screen

## 📁 Installation 

* Download - QuickLogin.zip
* Extract
* Run QuickLogin.exe
* Right Click > Send to Desktop to make a shortcut

---

## 🔒 Security

* Credentials are stored **only** in Windows Credential Manager
* No plaintext credential files
* No network communication
* No telemetry or background services

All credentials are tied to the current Windows user account.

---

## 🚀 Usage

1. Start **World of Warcraft** and wait at the login screen
2. Launch **QuickLogin**
3. Add one or more accounts
4. Select an account from the dropdown
5. Click **Log In** to login and keep QuickLogin Open
**OR**
6. Click **Log In & Close** to login and close QuickLogin

QuickLogin will focus the WoW window, type the credentials, submit the login, and then close automatically.

---

## ⚠️ Important Notes

* The **“Remember Account Name”** option must be **unchecked** on the WoW login screen
* This tool relies on simulated keyboard input and requires the login screen to be active
* QuickLogin is intended for personal convenience only


## ❓ FAQ

Does QuickLogin automate gameplay?
* No. It only types credentials at the login screen.

Does it modify game files or memory?
* No.

Is this affiliated with Blizzard?
* No. This is an independent, community-made utility.

## 📄 License

* This project is provided as-is for personal use.
* No warranties are expressed or implied.
