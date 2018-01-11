<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CivilMapCloudServices" generation="1" functional="0" release="0" Id="b3637aa5-06b8-4c14-89f6-5f3bb37eb356" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CivilMapCloudServicesGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="CivilMapServices:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/LB:CivilMapServices:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
        <inPort name="CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCertificate|CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="CivilMapServicesInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/MapCivilMapServicesInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:CivilMapServices:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint">
          <toPorts>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCivilMapServices:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapCivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapCivilMapServicesInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServicesInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CivilMapServices" generation="1" functional="0" release="0" software="C:\Users\Jing\Documents\Visual Studio 2015\Projects\CivilMapServices\CivilMapCloudServices\csx\Release\roles\CivilMapServices" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp" portRanges="8172" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/SW:CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CivilMapServices&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CivilMapServices&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServicesInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServicesUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServicesFaultDomains" />
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
    <implementation Id="2b19dd53-1d99-49bb-9c6f-788f57184d21" ref="Microsoft.RedDog.Contract\ServiceContract\CivilMapCloudServicesContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="dae30a9c-ba67-4061-aebc-df85c101ca99" ref="Microsoft.RedDog.Contract\Interface\CivilMapServices:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="a83505e6-1f99-4c02-8459-5f039ec4d761" ref="Microsoft.RedDog.Contract\Interface\CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="5729f3c3-26f2-4958-b122-0f714339585e" ref="Microsoft.RedDog.Contract\Interface\CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CivilMapCloudServices/CivilMapCloudServicesGroup/CivilMapServices:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>