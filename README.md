# Automated Desktop Screenshot Capture and WhatsApp Sender

## 1. Project Overview

The Automated Desktop Screenshot Capture and WhatsApp Sender is a C# Windows application that automatically captures desktop screenshots, detects duplicate images, and sends unique screenshots to a registered WhatsApp number using the Meta WhatsApp Cloud API.

The main objective of this project is to automate the process of capturing desktop screenshots and sharing them through WhatsApp.

## 2. Key Features

- Automatically captures desktop screenshots.
- Captures 6 screenshots in one cycle.
- Saves screenshots in a local folder.
- Compares images pixel by pixel to identify exact duplicates.
- Sends only unique screenshots through WhatsApp.
- Deletes processed screenshots after processing.
- Maintains an error log for troubleshooting.
- Automatically starts when Windows is logged in using a shortcut in the Windows Startup folder.

## 3. Technologies Used

- **Programming Language:** C#
- **Framework:** .NET Framework
- **Image Processing:** System.Drawing
- **HTTP Communication:** HttpClient
- **API Integration:** Meta WhatsApp Cloud API
- **Data Handling:** JSON
- **Pattern Matching:** Regular Expressions (Regex)
- **Development Environment:** Visual Studio

## 4. How Does the Project Work?

1. The application creates a local folder named `Screenshots`.
2. It captures 6 desktop screenshots at one-minute intervals.
3. All captured images are saved as PNG files.
4. The ImageComparer class compares the screenshots pixel by pixel.
5. Duplicate images are identified and excluded.
6. Unique screenshots are uploaded to the WhatsApp Cloud API.
7. The application sends the images to the configured WhatsApp recipient.
8. Processed screenshot files are deleted from the local folder.
9. Errors are recorded in a log file for troubleshooting.

## 5. Automatic Startup Configuration

The application is configured to start automatically when the user logs in to Windows.

This is achieved by creating a shortcut of the application executable and placing it inside the Windows Startup folder.

As a result, the application starts automatically after Windows login without requiring the user to launch it manually.

## 6. Project Structure

```text
WhatsApp-Screenshot-Sender/
│
├── Properties/
│   └── AssemblyInfo.cs
│
├── .gitignore
├── App.config
├── ImageCompare.cs
├── Program.cs
├── ScreenshotHelp.csproj
├── ScreenshotHelp.slnx
└── WhatsAppSender.cs
```

## 7. Configuration Requirements

The application uses the following settings in `App.config`:

- `AccessToken` – WhatsApp Cloud API access token.
- `PhoneNumberId` – WhatsApp Phone Number ID.
- `ReceiverNumbers` – Recipient phone number.

Valid WhatsApp Cloud API credentials are required to send screenshots.

**Security Note:** Never upload real API access tokens or private credentials to a public GitHub repository.

## 8. How to Run the Project

1. Clone or download this repository.
2. Open `ScreenshotHelp.slnx` in Visual Studio.
3. Configure the required WhatsApp Cloud API credentials in `App.config`.
4. Build the project.
5. Run the application.
6. To enable automatic startup, create a shortcut of the application executable and place it in the Windows Startup folder.

## 9. Learning Outcomes

Through this project, I gained practical experience in:

- C# programming and application development.
- Working with images using System.Drawing.
- File and folder handling.
- Comparing images pixel by pixel.
- Sending HTTP requests using HttpClient.
- Integrating external APIs.
- Handling JSON responses.
- Exception handling and error logging.
- Configuring applications to start automatically with Windows.

## 10. Developer

**Chandani Kumari**

GitHub Profile: https://github.com/chandanikumari2-hue

Project Repository: https://github.com/chandanikumari2-hue/WhatsApp-Screenshot-Sender

---

## Security and Privacy Notice

This application should only be used on computers you own or have explicit permission to monitor.

Users should be informed before their desktop screenshots are captured or shared through WhatsApp. Keep API credentials private and secure.
