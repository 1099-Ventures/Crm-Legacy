# Azuro CRM Legacy Integration Libraries

> **Educational Open Source Project**  
> A comprehensive Microsoft Dynamics 365 CRM integration framework (circa 2015-2016) demonstrating enterprise-grade patterns for SMS, ticketing system, and workflow automation.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Key Features](#key-features)
- [Quick Start](#quick-start)
- [Project Structure](#project-structure)
- [Integration Components](#integration-components)
- [Configuration](#configuration)
- [Building & Deployment](#building--deployment)
- [Educational Value](#educational-value)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

This repository contains a legacy Microsoft Dynamics CRM integration platform built during the 2015-2016 era. It demonstrates enterprise-grade patterns for integrating CRM systems with external services including SMS providers, ticketing systems (Kaseya), monitoring platforms (N-able), and custom workflow automation.

**⚠️ Legacy Notice**: This codebase targets older CRM versions (2011/2015) and .NET Framework 4.5.2. It's provided for educational purposes to demonstrate enterprise integration patterns and architectural decisions from that era.

## 🏗️ Architecture

### Core Architectural Patterns

- **Plugin Architecture**: Extensible CRM plugins with version-specific implementations
- **Message Queue Pattern**: MSMQ for reliable asynchronous processing
- **Service-Oriented Architecture**: Web services for external system integration
- **Factory Pattern**: Configurable service instantiation
- **Repository Pattern**: Abstracted data access with strongly-typed interfaces

### System Components

```
┌─────────────────────┐    ┌─────────────────────┐    ┌─────────────────────┐
│   External Systems  │    │   CRM Integration   │    │   Background       │
│                     │    │     Platform        │    │   Services         │
│ • SMS Providers     │◄──►│                     │◄──►│ • SMS Processor    │
│ • Kaseya PSA        │    │ • Plugins           │    │ • Ticket Processor │
│ • N-able RMM        │    │ • Workflows         │    │ • Message Handlers │
│ • SharePoint        │    │ • Web Services      │    │ • Queue Listeners  │
└─────────────────────┘    └─────────────────────┘    └─────────────────────┘
```

## ✨ Key Features

### 📱 SMS Integration
- **Multi-Provider Support**: Clickatell and extensible provider architecture
- **Queue-Based Processing**: Reliable message delivery with retry logic
- **Audit Trail**: Comprehensive logging to separate SMS database
- **CRM Integration**: Automatic activity creation and case updates

### 🎫 Kaseya PSA Integration
- **Bidirectional Sync**: Tickets, time entries, and customer data
- **Session Management**: Robust authentication with automatic renewal
- **Field Mapping**: Configurable attribute mapping between systems
- **Error Recovery**: Handles API failures and session timeouts

### 🔧 N-able RMM Integration
- **Notification Processing**: XML-based alert handling
- **Automatic Case Creation**: Monitoring alerts become CRM cases
- **User Assignment**: Intelligent technician assignment logic
- **Configuration-Driven**: External configuration for default values

### 🔄 Workflow Automation
- **Business Hours Calculation**: Complex calendar-based scheduling
- **Escalation Engine**: Sophisticated rule-based escalation
- **Activity Chaining**: Support for complex workflow scenarios
- **SLA Management**: Service level agreement tracking

## 🚀 Quick Start

### Prerequisites
- Visual Studio 2015/2017 with CRM SDK tools
- Microsoft Dynamics CRM 2011/2015/2016
- .NET Framework 4.5.2+
- SQL Server (for SMS logging)
- MSMQ (for message processing)

### Building the Solution

```bash
# Clone the repository
git clone https://github.com/yourusername/azuro-crm-legacy.git
cd azuro-crm-legacy

# Build the entire solution
msbuild Azuro.Crm.sln /p:Configuration=Release /p:Platform="Any CPU"

# Deploy to assembly cache
./deployToAssemblyCache.bat

# Install to GAC (requires admin privileges)
./GacInstall.bat
```

### Basic Configuration

1. **Configure CRM Connection**:
   ```xml
   <connectionStrings>
     <add name="CRM" connectionString="Server=https://yourorg.crm.dynamics.com; Username=user@domain.com; Password=password;" />
   </connectionStrings>
   ```

2. **Setup SMS Provider**:
   ```xml
   <smsConfiguration>
     <provider name="Clickatell" apiKey="your-api-key" />
   </smsConfiguration>
   ```

3. **Configure Message Queues**:
   ```xml
   <msmq>
     <queues>
       <add name="SmsQueue" path=".\private$\sms" />
     </queues>
   </msmq>
   ```

## 📁 Project Structure

### Core Libraries
- **`Azuro.Crm.Entities`**: Early-bound CRM entity definitions
- **`Azuro.Crm.Integration.Common`**: Common interfaces and base classes
- **`Azuro.Crm.Integration.v2011/v2015`**: Version-specific implementations

### Integration Components
- **`Azuro.Crm.SmsProcessor`**: SMS message processing service
- **`Azuro.Crm.KaseyaIntegration`**: PSA ticketing system integration
- **`Azuro.Crm.Integration.Nable`**: RMM monitoring integration
- **`Azuro.Crm.Workflow`**: Custom workflow activities

### Supporting Infrastructure
- **`Azuro.Crm.SmsData`**: Entity Framework data layer
- **`Azuro.Crm.SmsMessages`**: Message queue definitions
- **`Azuro.Crm.Test`**: Unit and integration tests

## 🔧 Integration Components

### SMS Integration Deep Dive

The SMS integration demonstrates several enterprise patterns:

```csharp
// Message queue processing
public class SmsMessageHandler : IMessageHandler<SmsMessage>
{
    public async Task Handle(SmsMessage message)
    {
        // Process SMS with retry logic
        await ProcessWithRetry(message);
        
        // Update CRM activity
        await UpdateCrmActivity(message);
        
        // Log to audit database
        await AuditSmsActivity(message);
    }
}

// Provider abstraction
public interface ISmsProvider
{
    Task<SmsResult> SendSms(string to, string message);
    Task<SmsStatus> GetStatus(string messageId);
}
```

### Plugin Architecture

Version-specific plugin implementations:

```csharp
// v2011 Implementation
public class SmsPlugin : APlugin
{
    public override void Execute(IServiceProvider serviceProvider)
    {
        var context = GetExecutionContext(serviceProvider);
        var service = GetOrganizationService(serviceProvider);
        
        // Plugin logic here
    }
}

// v2015+ Implementation with improved error handling
public class SmsPlugin : APlugin
{
    protected override void Execute(IServiceProvider serviceProvider, 
        IOrganizationService service, IPluginExecutionContext context)
    {
        try
        {
            // Enhanced plugin logic
        }
        catch (Exception ex)
        {
            LogError(ex, context.CorrelationId);
            throw new InvalidPluginExecutionException(ex.Message);
        }
    }
}
```

## ⚙️ Configuration

### Environment-Specific Settings

The solution supports multiple deployment configurations:

- **Debug**: Development environment with verbose logging
- **Release**: Production deployment with optimized performance
- **TrialBuild**: Demonstration environment with limited features

### Configuration Sections

```xml
<configSections>
  <section name="smsConfiguration" type="Azuro.Crm.Configuration.SmsConfigurationSection" />
  <section name="kaseyaConfiguration" type="Azuro.Crm.Configuration.KaseyaConfigurationSection" />
  <section name="nableConfiguration" type="Azuro.Crm.Configuration.NableConfigurationSection" />
</configSections>
```

### Logging Configuration

Comprehensive NLog configuration with multiple targets:

```xml
<nlog>
  <targets>
    <target name="file" xsi:type="File" fileName="logs/crm-${shortdate}.log" />
    <target name="eventlog" xsi:type="EventLog" source="Azuro.Crm" />
    <target name="database" xsi:type="Database" connectionString="..." />
  </targets>
</nlog>
```

## 🏗️ Building & Deployment

### Build Process

```bash
# Standard build
msbuild Azuro.Crm.sln /p:Configuration=Release

# Create NuGet packages
./nugetpush.bat

# Deploy to development environment
./deployToAssemblyCache.bat Debug

# Deploy to production
./deployToAssemblyCache.bat Release
```

### Deployment Components

1. **CRM Solutions**: Managed solutions in `CRM Solutions/`
2. **JavaScript Customizations**: Form scripts in `CRM JScript/`
3. **Database Scripts**: SMS logging schema in `SQL Scripts/`
4. **Windows Services**: Background processing services
5. **Web Applications**: External callback handlers

### Strong Naming & Security

All assemblies are strong-named and signed with `crmcode.azuro.co.za.pfx` for GAC deployment and security compliance.

## 📚 Educational Value

This codebase demonstrates several valuable concepts for students and developers:

### Enterprise Integration Patterns
- **Message Queue Processing**: Reliable async communication
- **Service Abstraction**: Clean separation of concerns
- **Configuration Management**: Environment-specific deployments
- **Error Handling**: Robust exception management and logging

### Microsoft Dynamics CRM Development
- **Plugin Development**: Event-driven processing
- **Workflow Activities**: Custom business logic
- **Early-Bound Entities**: Strongly-typed data access
- **SDK Evolution**: Handling multiple CRM versions

### Legacy System Integration
- **SOAP Web Services**: XML-based system communication
- **Session Management**: Authentication and state handling
- **Data Mapping**: Transforming between system formats
- **Backward Compatibility**: Supporting multiple versions

### Code Quality Practices
- **Logging Strategy**: Comprehensive audit trails
- **Testing Approach**: Unit and integration testing
- **Documentation**: XML documentation and inline comments
- **Version Control**: Branching strategies for multiple versions

## 🤝 Contributing

While this is primarily an educational project, contributions are welcome:

1. **Bug Reports**: Issues with legacy compatibility
2. **Documentation**: Improvements to code documentation
3. **Educational Examples**: Additional usage examples
4. **Modernization**: Porting patterns to newer frameworks

## 📜 License

This project is released under the MIT License for educational purposes. See [LICENSE](LICENSE) for details.

---

## 🔍 Code Quality Assessment

**Strengths:**
- ✅ Consistent architectural patterns across components
- ✅ Comprehensive logging and error handling
- ✅ Version-specific implementations showing evolution
- ✅ Modular design allowing easy extension

**Areas for Learning:**
- 📚 Legacy patterns that have evolved in modern development
- 📚 Enterprise integration challenges and solutions
- 📚 Configuration management in complex systems
- 📚 Multi-version support strategies

**Modern Alternatives:**
- 🔄 Consider Azure Service Bus instead of MSMQ
- 🔄 Use dependency injection containers instead of factory pattern
- 🔄 Implement async/await patterns throughout
- 🔄 Adopt newer authentication methods (OAuth, JWT)

---

*This project represents a snapshot of enterprise CRM integration circa 2015-2016. While the technologies are dated, the architectural patterns and integration strategies remain relevant for understanding enterprise system design.*