@echo off
echo Starting Winston Check-In App...
echo.
echo The app will be available at:
echo   Local: http://localhost:5000
echo   Network: http://YOUR_IP:5000
echo.
echo To access from internet, configure port forwarding on your router:
echo   - Forward external port 80 to internal port 5000
echo   - Forward external port 443 to internal port 5000 (for HTTPS)
echo.
echo Press Ctrl+C to stop the server
echo.

cd WinstonCheckIn
dotnet run --urls="http://0.0.0.0:5000"

