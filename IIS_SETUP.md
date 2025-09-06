# IIS Hosting Setup Guide

## Prerequisites
1. **Windows 10/11 Pro** or **Windows Server**
2. **IIS enabled** in Windows Features
3. **ASP.NET Core Hosting Bundle** installed

## Step 1: Enable IIS
1. Open **Control Panel** → **Programs** → **Turn Windows features on/off**
2. Check **Internet Information Services (IIS)**
3. Expand **World Wide Web Services** → **Application Development Features**
4. Check **ASP.NET Core** (if available)
5. Click **OK** and restart if prompted

## Step 2: Install ASP.NET Core Hosting Bundle
1. Download from: https://dotnet.microsoft.com/download/dotnet-core
2. Install **ASP.NET Core Hosting Bundle** (not just runtime)
3. Restart IIS: `iisreset` in Command Prompt as Administrator

## Step 3: Publish the App
```bash
cd WinstonCheckIn
dotnet publish -c Release -o ./publish
```

## Step 4: Configure IIS
1. Open **IIS Manager**
2. Right-click **Sites** → **Add Website**
3. **Site name**: WinstonCheckIn
4. **Physical path**: `C:\Users\georg\RiderProjects\WinstonCheckIn\WinstonCheckIn\publish\wwwroot`
5. **Port**: 80 (or 443 for HTTPS)
6. Click **OK**

## Step 5: Configure Static Files
1. In IIS Manager, select your site
2. Double-click **MIME Types**
3. Add new MIME type:
   - **File name extension**: `.wasm`
   - **MIME type**: `application/wasm`
4. Add another MIME type:
   - **File name extension**: `.blat`
   - **MIME type**: `application/octet-stream`

## Step 6: Test
1. Open browser to `http://localhost` (or your server IP)
2. The app should load and work properly

## Step 7: Port Forwarding (for Internet Access)
1. **Router Admin Panel** (usually 192.168.1.1)
2. **Port Forwarding** → **Virtual Server**
3. **External Port**: 80
4. **Internal IP**: Your PC's IP address
5. **Internal Port**: 80
6. **Protocol**: TCP
7. **Enable**: Yes

## Step 8: Dynamic DNS (Optional)
1. Sign up for **No-IP**, **DuckDNS**, or **DynDNS**
2. Get a free subdomain like `yourname.ddns.net`
3. Configure your router's DDNS settings
4. Users can access via `http://yourname.ddns.net`

## Troubleshooting
- **App won't start**: Check Windows Event Viewer for errors
- **Can't access from internet**: Check firewall and port forwarding
- **Static files not loading**: Check MIME types and file permissions
- **Database issues**: Ensure app has write permissions to the folder

## Security Notes
- Consider using HTTPS with SSL certificates
- Set up proper firewall rules
- Regular backups of the database files
- Monitor access logs

