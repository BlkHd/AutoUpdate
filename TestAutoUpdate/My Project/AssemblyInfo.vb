Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' Le informazioni generali relative a un assembly sono controllate dal seguente
' insieme di attributi. Per modificare le informazioni associate a un assembly
' occorre quindi modificare i valori di questi attributi.

' Rivedere i valori degli attributi dell'assembly

<Assembly: AssemblyTitle("TestAutoUpdate")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("")>
<Assembly: AssemblyProduct("TestAutoUpdate")>
<Assembly: AssemblyCopyright("Copyright ©  2016")>
<Assembly: AssemblyTrademark("")>

<Assembly: CLSCompliant(True)>

<Assembly: ComVisible(False)>

'Se il progetto viene esposto a COM, il GUID che segue verrà utilizzato per creare l'ID della libreria dei tipi
<Assembly: Guid("0e5f44d0-1364-4aa1-bdba-c846334691cc")>

' Le informazioni sulla versione di un assembly sono costituite dai seguenti quattro valori:
'
'      Numero di versione principale
'      Numero di versione secondario
'      Numero build
'      Revisione
'
' È possibile specificare tutti i valori oppure impostare valori predefiniti per i numeri relativi alla revisione e alla build
' utilizzando l'asterisco (*) come descritto di seguito:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")> 

'L'attributo seguente impedisce la visualizzazione dell'avviso FxCop "CA2232: Microsoft.Usage: Aggiungere STAThreadAttribute all'assembly"
' poiché l'applicazione dispositivo non supporta thread STA.
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")>
