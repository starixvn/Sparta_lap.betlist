add to spname

<Function Name="LRF">
    <Setting Key="Las_GetLicenseeDomain" SpName="Las_GetLicenseeDomain" DbName="DBLAS.LASDB.bodbLicenseeNet" Plain="0" />
  </Function>


add connectionString:

<add name="DBLAS.LASDB.bodbLicenseeNet" connectionString="Data Source=10.18.200.231;Initial Catalog=LASDB;integrated security=false; User ID=bodbLicenseeNet;Password=Uqeq4MmLwveHZzqok3kHGf7fqd4SEfMq0vtQ5l2TQ50==" providerName="System.Data.SqlClient" />



new seting - 08/25/2014


    Spnames:
    <Setting Key="LAS_InsertInternalAuthority" SpName="LAS_InsertInternalAuthority" DbName="DBLAS.LASDB.bodbLicenseeNet" Plain="0" TimeOut="600" />

    AppSettings:
    Local:
    <add key="LegendEnvironment" value="UAT" />
    <add key="LegendAdminSite" value="http://10.18.200.209:7980/V1" />

    UAT:
    <add key="LegendEnvironment" value="UAT" />
    <add key="LegendAdminSite" value="http://qa-las-admin.starific.net/" />

    PRO:
    <add key="LegendEnvironment" value="PRO" />
    <add key="LegendAdminSite" value="http://ag.006660.net//" />
