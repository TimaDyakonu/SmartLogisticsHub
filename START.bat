cd C:\Users\Timotei\RiderProjects\SmartLogisticsHub\src\SmartLogisticsHub.Frontend

start cmd /k "npm run dev"

REM wait ~2 seconds for server to start
timeout /t 2 > nul

start http://localhost:5173