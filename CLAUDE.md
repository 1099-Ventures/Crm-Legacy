# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a legacy Dynamics 365 CRM integration project (circa 2015/2016) that provides helper libraries and services for integrating with Microsoft Dynamics CRM. The solution contains multiple integration components for SMS, Kaseya, N-able, and workflow automation.

## Build Commands

### Building the Solution
```bash
# Build entire solution (use Visual Studio or MSBuild)
msbuild Azuro.Crm.sln /p:Configuration=Release /p:Platform="Any CPU"

# Build for debug
msbuild Azuro.Crm.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

### Deployment Commands
```bash
# Deploy assemblies to cache (Release build by default)
./deployToAssemblyCache.bat

# Deploy specific build type
./deployToAssemblyCache.bat Debug

# Install assemblies to GAC
./GacInstall.bat

# Create NuGet packages
./nugetpush.bat

# Restart CRM services after deployment
./RestartCRMServices.bat
```

### Package Creation
```bash
# Create NuGet packages for core libraries
nuget pack Azuro.Crm.Entities.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
nuget pack Azuro.Crm.Integration.Common.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
nuget pack Azuro.Crm.Integration.v2011.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
```

## Architecture Overview

### Core Components

**Entity Layer:**
- `Azuro.Crm.Entities`: Core CRM entity definitions with early-bound classes
- `Azuro.Crm.Entities.EarlyBound`: Additional early-bound entity classes
- `Azuro.Crm.ExtractEntities`: Utility for extracting entity classes from CRM metadata

**Integration Foundation:**
- `Azuro.Crm.Integration.Common`: Common interfaces and base classes
- `Azuro.Crm.Integration.v2011`: CRM 2011 SDK implementations
- `Azuro.Crm.Integration.v2015`: CRM 2015/2016 SDK implementations
- `Azuro.Crm.Online.Common`: Online/cloud-specific components

**SMS Integration:**
- `Azuro.Crm.SmsProcessor`: Windows service for SMS processing
- `Azuro.Crm.SmsMessages`: Message definitions for SMS events
- `Azuro.Crm.SmsMessageHandlers`: Message handlers for SMS processing
- `Azuro.Crm.SmsPlugin.v2011/v2016`: CRM plugins for SMS functionality
- `Azuro.Sms.ProviderAck`: Web service for SMS provider acknowledgments

**External System Integrations:**
- `Azuro.Crm.KaseyaIntegration.v2011`: Kaseya integration logic
- `Azuro.Crm.KaseyaPlugin.v2011`: CRM plugin for Kaseya ticket sync
- `Azuro.Crm.Integration.Nable.*`: N-able integration components
- `Azuro.Crm.Workflow.*`: Workflow activities and support desk automation

### Key Patterns

**Plugin Architecture**: CRM plugins follow IPlugin interface with base abstract classes (APlugin, ACrmCodeActivity)

**Message Queue Pattern**: MSMQ for asynchronous processing with separate message handlers

**Factory Pattern**: CrmHelperFactory for service instances, SmsProviderFactory for SMS providers

**Repository Pattern**: CrmHelper classes abstract CRM SDK operations with strongly-typed interfaces

### Dependencies

**Microsoft Technologies:**
- .NET Framework 4.5.2/4.6.2
- Microsoft CRM SDK 9.0
- Entity Framework 6.0
- Windows Communication Foundation (WCF)
- Message Queuing (MSMQ)

**Third-Party Libraries:**
- NLog 4.4.12 for logging
- Azuro.Common utility library
- Azuro.MSMQ custom framework

### Deployment Architecture

**Assembly Deployment:**
- Strong-named assemblies with code signing
- Global Assembly Cache (GAC) deployment
- Automated deployment via batch scripts

**CRM Solution Deployment:**
- Managed CRM solutions in `CRM Solutions/` folder
- Plugin registration in CRM sandbox
- JavaScript customizations in `CRM JScript/` folder

**Service Deployment:**
- Windows services with automated installers
- Web applications for external callbacks
- SQL Server database for SMS logging (scripts in `SQL Scripts/`)

## Configuration

### Environment Configuration
- Multiple build configurations: Debug, Release, TrialBuild
- Environment-specific app.config files
- NLog.config for logging configuration

### Connection Strings
- CRM connection strings in app.config
- Database connections for SMS logging
- MSMQ queue configurations

### Security
- Strong-named assemblies with crmcode.azuro.co.za.pfx
- Windows Authentication for CRM services
- Service account-based authentication

## Testing

Look for test projects like `Azuro.Crm.Test` and `Azuro.Crm.Tester` for testing patterns and utilities.

## Key Files and Directories

- `Azuro.Crm.sln`: Main solution file
- `CRM JScript/`: JavaScript customizations for CRM forms
- `CRM Solutions/`: Managed CRM solution files
- `SQL Scripts/`: Database scripts for SMS logging
- `*.bat files`: Deployment and utility scripts
- `packages/`: NuGet packages directory

## Development Notes

- This is a legacy project targeting older CRM versions (2011/2015)
- Uses packages.config for NuGet (not PackageReference)
- Requires Visual Studio with CRM SDK tools
- Database scripts are for SMS logging functionality
- Strong naming required for GAC deployment