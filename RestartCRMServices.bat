net stop MSCRMSandboxService
net stop MSCRMAsyncService$maintenance
net stop MSCRMAsyncService

net start MSCRMSandboxService
net start MSCRMAsyncService$maintenance
net start MSCRMAsyncService

pause