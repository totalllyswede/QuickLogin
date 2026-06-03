# QuickLogin

**QuickLogin** is a lightweight Windows utility that streamlines logging into **World of Warcraft** by securely storing account credentials and automating the login process.

It is designed for convenience and simplicity, with a modern interface and no background processes.

---

## Features

* **Secure Credential Storage** - Uses Windows Credential Manager to safely store passwords
* **Multiple Account Management** - Add and remove unlimited accounts
* **Auto-Launch Game** - Optionally launch World of Warcraft automatically before logging in
* **Automated Login** - Focuses the WoW window, types credentials, and submits login automatically
* **Clean WDB** - Deletes wow cache folder located in the same directory as your wow.exe. This helps private servers run better, use this every few times you login.

QuickLogin does **not** interact with the game client beyond sending standard keyboard input.

<img width="633" height="506" alt="D" src="https://github.com/user-attachments/assets/fb8134b4-df15-4ea5-825e-78f7648735aa" />


---

## Requirements

* Windows 7 or newer
* .NET Desktop Runtime 8
* World of Warcraft installed

---

## Installation 

1. Download **QuickLogin.zip**
2. Extract to a folder of your choice
3. Run **QuickLogin.exe**

---

## Security

* Credentials are stored **only** in Windows Credential Manager
* No plaintext credential files
* No network communication
* No telemetry or background services
* All credentials are tied to the current Windows user account

---

## Usage

### Basic Setup
1. Launch **QuickLogin**
2. Click **"+ Add Account"** to create a new account entry
3. Enter a display name, username, and password
4. Click **"Save"**
5. Select your account from the dropdown

### Login Options

**Option 1: Manual Launch**
1. Start World of Warcraft and wait at the login screen
2. Select your account in QuickLogin
3. Click **"Login"** (keeps QuickLogin open) or **"Login & Close"** (closes after login)

**Option 2: Auto-Launch** *(Recommended)*
1. Click **"Browse..."** and select your **Wow.exe** file
2. Check **"Auto-launch game when logging in"**
3. Select your account from the dropdown
4. Click **"Login & Close"** or **"Login"**
5. QuickLogin will launch WoW, wait 10 seconds, then automatically log you in

### Managing Accounts
* **Add Account** - Create a new account entry
* **Remove Account** - Delete the selected account (credentials removed from Credential Manager)

---

## Important Notes

* The **"Remember Account Name"** option must be **unchecked** on the WoW login screen
* This tool relies on simulated keyboard input
* If not using auto-launch, WoW must be open and at the login screen before clicking Login
* QuickLogin is intended for personal convenience only

---

## FAQ

**Does QuickLogin automate gameplay?**
* No. It only types credentials at the login screen.

**Does it modify game files or memory?**
* No. It only sends keyboard input to the login screen.

**Can I use this with multiple WoW accounts?**
* Yes! You can add unlimited accounts and switch between them.

**Is the auto-launch feature required?**
* No, it's optional. You can manually start WoW and use QuickLogin to login.

**How do I change the auto-launch wait time?**
* Currently set to 10 seconds. Future versions may include customization.

**Is this affiliated with Blizzard?**
* No. This is an independent, community-made utility.

**Is this against WoW's Terms of Service?**
* QuickLogin only automates the login process using standard keyboard input, similar to a password manager. It does not interact with gameplay. Use at your own discretion.

---

## License

* This project is provided as-is for personal use
* No warranties are expressed or implied
* Use at your own risk

---

## 🔧 Technical Details

* **Framework**: .NET 8 (WPF)
* **Dependencies**: 
  - Windows Credential Manager (CredentialManagement)
  - InputSimulatorStandard (keyboard simulation)



