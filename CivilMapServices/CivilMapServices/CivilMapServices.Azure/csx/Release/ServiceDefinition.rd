<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CivilMapServices.Azure" generation="1" functional="0" release="0" Id="da5c9331-a2cf-424f-bc5f-b2793fc966ac" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CivilMapServices.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="CivilMapServices:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/LB:CivilMapServices:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
        <inPort name="CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCertificate|CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:CloudToolsDiagnosticAgentVersion" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:CloudToolsDiagnosticAgentVersion" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Profiling.ProfilingConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServices:Profiling.ProfilingConnectionString" />
          </maps>
        </aCS>
        <aCS name="CivilMapServicesInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/MapCivilMapServicesInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:CivilMapServices:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint">
          <toPorts>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCivilMapServices:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapCivilMapServices:CloudToolsDiagnosticAgentVersion" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/CloudToolsDiagnosticAgentVersion" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Profiling.ProfilingConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Profiling.ProfilingConnectionString" />
          </setting>
        </map>
        <map name="MapCivilMapServicesInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServicesInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CivilMapServices" generation="1" functional="0" release="0" software="C:\Users\Jing\Documents\Visual Studio 2015\Projects\CivilMapServices\CivilMapServices.Azure\csx\Release\roles\CivilMapServices" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp" portRanges="8172" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/SW:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="CloudToolsDiagnosticAgentVersion" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="Profiling.ProfilingConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CivilMapServices&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CivilMapServices&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServicesInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServicesUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServicesFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="CivilMapServicesUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="CivilMapServicesFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="CivilMapServicesInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e4d71019-3a9b-469e-b20a-1c43e63b216c" ref="Microsoft.RedDog.Contract\ServiceContract\CivilMapServices.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="b4647ec6-7de3-4c5e-8cbd-68256ade6969" ref="Microsoft.RedDog.Contract\Interface\CivilMapServices:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="01a098dd-a632-492d-85fd-d66ca6e31f23" ref="Microsoft.RedDog.Contract\Interface\CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="5cf2f7d5-a10e-4f75-beb6-d17eff82ea09" ref="Microsoft.RedDog.Contract\Interface\CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CivilMapServices.Azure/CivilMapServices.AzureGroup/CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>