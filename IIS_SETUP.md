# IIS Hosting Setup Guide for Blazor WebAssembly

## Prerequisites
1. **Windows 10/11 Pro** or **Windows Server**
2. **IIS enabled** in Windows Features
3. **ASP.NET Core Hosting Bundle** installed
4. **.NET 9.0 Runtime** installed

## Step 1: Enable IIS
1. Open **Control Panel** → **Programs** → **Turn Windows features on/off**
2. Check **Internet Information Services (IIS)**
3. Expand **World Wide Web Services** → **Application Development Features**
4. Check **ASP.NET Core** (if available)
5. Expand **Common HTTP Features** and check:
   - **Default Document**
   - **Directory Browsing**
   - **HTTP Errors**
   - **Static Content**
6. Click **OK** and restart if prompted

## Step 2: Install Required Components
1. Download **ASP.NET Core Hosting Bundle** from: https://dotnet.microsoft.com/download/dotnet/9.0
2. Install **ASP.NET Core Hosting Bundle** (includes runtime + hosting)
3. Restart IIS: `iisreset` in Command Prompt as Administrator

## Step 3: Publish the Blazor WebAssembly App
```bash
cd WinstonCheckIn
dotnet publish -c Release -o ./publish
```

## Step 4: Create web.config for Blazor WASM
Create a `web.config` file in the publish folder with proper Blazor WASM configuration:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <staticContent>
      <mimeMap fileExtension=".wasm" mimeType="application/wasm" />
      <mimeMap fileExtension=".blat" mimeType="application/octet-stream" />
      <mimeMap fileExtension=".dll" mimeType="application/octet-stream" />
      <mimeMap fileExtension=".json" mimeType="application/json" />
      <mimeMap fileExtension=".woff" mimeType="application/font-woff" />
      <mimeMap fileExtension=".woff2" mimeType="application/font-woff2" />
    </staticContent>
    <httpCompression>
      <dynamicTypes>
        <add mimeType="application/octet-stream" enabled="true" />
        <add mimeType="application/wasm" enabled="true" />
      </dynamicTypes>
    </httpCompression>
    <rewrite>
      <rules>
        <rule name="Blazor WASM" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="index.html" />
        </rule>
      </rules>
    </rewrite>
    <defaultDocument>
      <files>
        <clear />
        <add value="index.html" />
      </files>
    </defaultDocument>
  </system.webServer>
</configuration>
```

## Step 5: Configure IIS Site
1. Open **IIS Manager**
2. Right-click **Sites** → **Add Website**
3. **Site name**: WinstonCheckIn
4. **Physical path**: `C:\Users\georg\RiderProjects\WinstonCheckIn\WinstonCheckIn\publish`
5. **Port**: 80 (or 443 for HTTPS)
6. **Host name**: Leave empty for local access, or add your domain
7. Click **OK**

## Step 6: Configure Application Pool
1. In IIS Manager, go to **Application Pools**
2. Find your site's app pool (usually named after your site)
3. Right-click → **Advanced Settings**
4. Set **.NET CLR Version** to **No Managed Code**
5. Set **Managed Pipeline Mode** to **Integrated**
6. Click **OK**

## Step 7: Set Permissions
1. Right-click the publish folder → **Properties** → **Security**
2. Add **IIS_IUSRS** with **Read & Execute** permissions
3. Add **IUSR** with **Read & Execute** permissions
4. Click **OK**

## Step 8: Test Local Access
1. Open browser to `http://localhost` (or your server IP)
2. The Blazor WebAssembly app should load and work properly

## Step 9: Configure for Internet Access

### Option A: Port Forwarding (Simple)
1. **Router Admin Panel** (usually 192.168.1.1 or 192.168.0.1)
2. **Port Forwarding** → **Virtual Server** or **Port Forwarding**
3. **External Port**: 80
4. **Internal IP**: Your PC's IP address (find with `ipconfig`)
5. **Internal Port**: 80
6. **Protocol**: TCP
7. **Enable**: Yes

### Option B: Dynamic DNS (Recommended)
1. Sign up for **No-IP**, **DuckDNS**, or **DynDNS**
2. Get a free subdomain like `winstoncheckin.ddns.net`
3. Configure your router's DDNS settings
4. Users can access via `http://winstoncheckin.ddns.net`

### Option C: Cloudflare Tunnel (Most Secure)
1. Install **Cloudflare Tunnel** (cloudflared)
2. Create tunnel: `cloudflared tunnel create winstoncheckin`
3. Configure tunnel to point to localhost:80
4. Run tunnel: `cloudflared tunnel run winstoncheckin`
5. Get public URL from Cloudflare dashboard

## Step 10: Security Configuration

### Windows Firewall
1. Open **Windows Defender Firewall**
2. **Inbound Rules** → **New Rule**
3. **Port** → **TCP** → **Specific local ports**: 80
4. **Allow the connection**
5. **Domain, Private, Public** (choose as needed)

### SSL/HTTPS (Recommended)
1. Get SSL certificate (Let's Encrypt, Cloudflare, or self-signed)
2. In IIS Manager, select your site
3. **Bindings** → **Add** → **HTTPS**
4. **Port**: 443
5. **SSL certificate**: Select your certificate

## Troubleshooting

### Common Issues:
- **App won't load**: Check if web.config is in the root publish folder
- **404 errors**: Verify MIME types and URL rewrite rules
- **Can't access from internet**: Check firewall, port forwarding, and router settings
- **Database issues**: Ensure app has write permissions to the folder
- **Blazor WASM not loading**: Check browser console for errors, verify .wasm MIME type

### Debug Steps:
1. Check **Windows Event Viewer** → **Windows Logs** → **Application**
2. Check **IIS Manager** → **Logging** for access logs
3. Test with different browsers
4. Check browser developer tools console for errors

## Security Best Practices
- Use HTTPS with valid SSL certificates
- Set up proper firewall rules
- Regular backups of the database files
- Monitor access logs
- Consider using Cloudflare for DDoS protection
- Keep Windows and IIS updated
- Use strong passwords for router admin panel

## Performance Optimization
- Enable **HTTP/2** in IIS
- Configure **Gzip compression** for static files
- Use **CDN** for static assets (optional)
- Monitor **Application Pool** memory usage

