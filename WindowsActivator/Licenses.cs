namespace WindowsActivator.Licenses;

public abstract class License
{
    // Special instance to represent no selection
    public static readonly License NoLicense = new GenericLicense("No License Selected", "None", "");

    protected License(string name, string category, string productKey, string? targetEdition = null)
    {
        Name = name;
        Category = category;
        ProductKey = productKey;
        TargetEdition = targetEdition;
    }

    public string Name { get; }
    public string Category { get; }
    public string ProductKey { get; }
    public string? TargetEdition { get; } // Optional target edition for DISM upgrades

    public override string ToString()
    {
        return Name;
    }

    // Internal GenericLicense class for creating specific licenses
    internal class GenericLicense : License
    {
        public GenericLicense(string name, string category, string productKey, string? targetEdition = null)
            : base(name, category, productKey, targetEdition)
        {
        }
    }
}

public static class WindowsLicense
{
    private const string Category = "Windows";

    public static readonly License WinPro = new License.GenericLicense("Windows 10/11 Pro", Category,
        "W269N-WFGWX-YVC9B-4J6C9-T83GX", "Professional");

    public static readonly License WinEnterprise = new License.GenericLicense("Windows 10/11 Enterprise", Category,
        "NPPR9-FWDCX-D2C8J-H872K-2YT43", "Enterprise");

    public static readonly License Server2022Standard = new License.GenericLicense("Windows Server 2022 Standard",
        Category, "VDYBN-27WPP-V4HQT-9VMD4-VMK7H", "ServerStandard");

    public static readonly License Server2022Datacenter = new License.GenericLicense("Windows Server 2022 Datacenter",
        Category, "WX4NM-KYWYW-QJJR4-XV3QB-6VM33", "ServerDatacenter");

    public static readonly License Server2022Azure = new License.GenericLicense(
        "Windows Server 2022 Datacenter: Azure Edition", Category, "NPTYM-C746H-B6BGC-P238P-RHK3Y",
        "ServerAzureEdition");

    public static readonly License Server2019Standard = new License.GenericLicense("Windows Server 2019 Standard",
        Category, "N69G4-B89J2-4G8F4-WWYCC-J464C", "ServerStandard");

    public static readonly License Server2019Datacenter = new License.GenericLicense("Windows Server 2019 Datacenter",
        Category, "WMDGN-G9PQG-XVVXX-R3X43-63DFG", "ServerDatacenter");

    public static readonly License Server2019Essentials = new License.GenericLicense("Windows Server 2019 Essentials",
        Category, "WVDHN-86M7X-466P6-VHXV7-YY726", "ServerEssentials");

    public static readonly License Server2016Standard = new License.GenericLicense("Windows Server 2016 Standard",
        Category, "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY", "ServerStandard");

    public static readonly License Server2016Datacenter = new License.GenericLicense("Windows Server 2016 Datacenter",
        Category, "CB7KF-BWN84-R7R2Y-793K2-8XDDG", "ServerDatacenter");

    public static readonly License Server2016Essentials = new License.GenericLicense("Windows Server 2016 Essentials",
        Category, "JCKRF-N37P4-C2D82-9YXRT-4M63B", "ServerEssentials");

    public static readonly License EnterpriseLTSC2024 = new License.GenericLicense("Windows 10 Enterprise LTSC 2024",
        Category, "8BCDC-KFD4V-9RH2C-X3V7H-C2C9T", "EnterpriseS");

    public static readonly License EnterpriseLTSC2021 = new License.GenericLicense("Windows 10 Enterprise LTSC 2021",
        Category, "M7XTQ-FN8P6-TTKYV-9D4CC-J462D", "EnterpriseS");

    public static readonly License EnterpriseLTSC2019 = new License.GenericLicense("Windows 10 Enterprise LTSC 2019",
        Category, "M7XTQ-FN8P6-TTKYV-9D4CC-J462D", "EnterpriseS");

    public static readonly License EnterpriseLTSC2016 = new License.GenericLicense("Windows 10 Enterprise LTSC 2016",
        Category, "DCPHK-NFMTC-H88MJ-PFHPY-QJ4BJ", "EnterpriseS");

    public static readonly License EnterpriseLTSC2015 = new License.GenericLicense("Windows 10 Enterprise LTSC 2015",
        Category, "WNMTR-4C88C-JK8YV-HQ7T2-76DF9", "EnterpriseS");
}

public static class OfficeLicense
{
    private const string Category = "Office";

    public static readonly License Office2024Standard =
        new License.GenericLicense("Office LTSC Standard 2024", Category, "TMRTB-K4QC3-M3TJT-4RF92-HH8T4");

    public static readonly License Office2024ProPlus = new License.GenericLicense("Office LTSC Professional Plus 2024",
        Category, "XJ2XN-FW8RK-P4HMP-DKDBV-GCVGB");

    public static readonly License Office2021Standard =
        new License.GenericLicense("Office LTSC Standard 2021", Category, "TMRTB-K4QC3-M3TJT-4RF92-HH8T4");

    public static readonly License Office2021ProPlus = new License.GenericLicense("Office LTSC Professional Plus 2021",
        Category, "FXYTK-NJJ8C-GB6DW-3DYQT-6F7TH");

    public static readonly License Office2019Standard =
        new License.GenericLicense("Office Standard 2019", Category, "6NWWJ-YQWMR-QKGCB-6TMB3-9D9HK");

    public static readonly License Office2019ProPlus =
        new License.GenericLicense("Office Professional Plus 2019", Category, "NMMKJ-6RK4F-KMJVX-8D9MJ-6MWKP");

    public static readonly License Office2016Standard =
        new License.GenericLicense("Office Standard 2016", Category, "JNRGM-WHDWX-FJJG3-K47QV-DRTFM");

    public static readonly License Office2016ProPlus =
        new License.GenericLicense("Office Professional Plus 2016", Category, "XQNVK-8JYDB-WJ9W3-YJ8YR-WFG99");
}
