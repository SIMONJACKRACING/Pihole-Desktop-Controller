# Pi-hole Desktop Controller (Partial Open Source Release)
Download Latest Build:
https://github.com/WL-Chan/Pihole-Desktop-Controller/releases/download/latest/PiholeDesktopController.zip

<details>
<summary>Screenshots</summary>

### Login Page
![Login Page](screenshots/Login-Page.png)

### Main Dashboard 1
![Main Dashboard 1](screenshots/No-Info.png)

### Main Dashboard 2 
![Main Dashboard 2](screenshots/Main-Window-Enable.png)

### Main Dashboard 3
![Main Dashboard 3](screenshots/Main-Window-Disable.png)

### Domain Management
![Domain Management](screenshots/Domains-Management.png)

### Gravity Update
![Gravity Update](screenshots/Gravity-Update.png)

### Pause Ad Blocking
![Pause Blocking](screenshots/Pause-Ad-Blocking.png)
</details>

This repository contains the connection and command logic used by the Pi-hole Desktop Controller application.  
Only the SSH and command-related components are provided here.  
The complete user interface, settings system, statistics logic, and other internal features are not included.  
You can download the fully compiled application from the Releases section.

---

## Preparing Your Pi-hole System

The desktop controller requires a few things to be configured on your Pi-hole device.  
Depending on whether Pi-hole runs on Raspberry Pi OS or a virtual machine, the steps may differ.

---

### 1. Install SQLite3

SQLite3 is required for Pi-hole statistics and database queries.

First connect to your Pi-hole via SSH:

```
ssh username@your-pihole-ip
```

Then install SQLite3:

```
sudo apt update
sudo apt install sqlite3
```

---

### 2. Configure sudo permissions (if your OS requires it)

Raspberry Pi OS usually does not require any sudo configuration changes.  
Other Linux distributions (Ubuntu, Debian server, VM environments) may require passwordless access for Pi-hole commands.

Open the sudoers editor:

```
sudo visudo
```

Scroll to the bottom and locate:

```
#includedir /etc/sudoers.d
```

You must place the following two lines **directly above** the includedir entry:

```
username ALL=(ALL) NOPASSWD: /usr/local/bin/pihole
username ALL=(ALL) NOPASSWD: /usr/bin/sqlite3
```

Replace `username` with your actual Linux username.

After adding the two permission lines, your sudoers file should resemble the example below:

```
username ALL=(ALL) NOPASSWD: /usr/local/bin/pihole
username ALL=(ALL) NOPASSWD: /usr/bin/sqlite3
#includedir /etc/sudoers.d

@includedir /etc/sudoers.d
```

If these rules are missing or placed incorrectly, the controller will not be able to read Pi-hole status or statistics.


---

## Purpose of This Repository

This repository provides the communication layer of the application, allowing developers to understand how Pi-hole can be controlled through SSH.

The source code here covers:

- Creating and managing SSH connections  
- Running Pi-hole commands  
- Enabling and disabling Pi-hole  
- Executing custom Linux commands  

All other parts of the application remain private.

---

## Included Source Code

### 1. SSH Connection Manager  
**Location:**  
```
Source/Connection/SshConnectionManager.cs
```

This class manages connection settings, opens and closes SSH sessions, and executes commands.

### 2. Pi-hole Command Helper  
**Location:**  
```
Source/Commands/SshCommands.cs
```

This class provides simple methods for:

- Enabling Pi-hole  
- Disabling Pi-hole for a chosen duration  
- Running custom commands  

---

## Example Usage

Full example located in:

```
Examples/ExampleUsage.cs
```

Short sample:

```csharp
var manager = new SshConnectionManager();
manager.Configure("192.168.1.100", 22, "pi", "password");
manager.Connect();

string result = manager.RunCommand("pihole status");
Console.WriteLine(result);

manager.Disconnect();

var cmd = new SshCommands();
cmd.SetCredentials("192.168.1.100", 22, "pi", "password");
await cmd.EnablePiHoleAsync();
```

---

## What Is Not Included

The following components are not part of this public repository:

- All WPF user interface files  
- Live log streaming logic  
- Smart scroll helpers  
- Settings manager and encrypted password system  
- Statistics logic (lifetime/today queries)  
- UI converters, helpers, and interface code  

These remain private as part of the full application.

---

## Downloading the Application

A prebuilt version of Pi-hole Desktop Controller is available in the **Releases** section.  
No compilation is required to use the application.

---

## License

The code in this repository is released under the MIT License.  
This applies only to the files published here.  
All remaining application components are closed-source.

---

## Notes

- SSH access must be enabled on your Pi-hole device.  
- SQLite3 is required for statistics-related features.  
- Some systems require modifying sudoers; Raspberry Pi OS generally does not.  
- The code provided here is intended only as a reference for SSH and command execution.

